using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public GameObject ghost;
    public int damage;
    public int level;
    int ghostFrames;
    int betweenGhostFrames;

    public bool homing;
    [Tooltip("The Bullet's movement (forward) per frame")]
    public float moveSpeed;
    [Tooltip("The Bullet's turn speed (right) per frame [BECOMES POSITIVE LERP IF HOMING]")]
    public float turnSpeed;

    private float timeOfSpawn = 0;
    [SerializeField] private float lifeTime = -1;
    [SerializeField] private bool ShouldBounce = false;
    [SerializeField] private bool isPlayerOwned = false;

    private void Awake()
    {
        timeOfSpawn = GameManager.CurrentFrameCount;
    }

    private void Start()
    {
        AddToLevelList();
    }

    private void AddToLevelList()
    {
        if (isPlayerOwned)
        {
            GameManager.instance.PlayerBullets.Add(this);
        }
        else
        {
            if (level <= 0 || level >= GameManager.instance.ListOfBulletLists.Count) return;
            else
            {
                GameManager.instance.ListOfBulletLists[level - 1].Add(this);
            }
        }
    }

    public void Move(bool _ghost = false)
    {
        if (!_ghost && isPlayerOwned)
        {
            print("Move");
        }

        GameObject obj = gameObject;
        if (_ghost)
            obj = ghost;

        obj.transform.position = obj.transform.position + (obj.transform.up * moveSpeed);

        if (homing)
        {
            Vector3 dir = PlayerMovement.instance.transform.position - obj.transform.position;
            obj.transform.up = Vector3.Lerp(obj.transform.up, dir, turnSpeed);
        }
        else
        {
            obj.transform.Rotate(obj.transform.forward * turnSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (ghost)
        {
            if (GameManager.instance.runningFrames)
            {
                ghost.SetActive(false);
                ResetGhost();
                betweenGhostFrames = 0;
            }
            else
            {
                betweenGhostFrames++;
                if (betweenGhostFrames >= GameManager.framesBetweenGhostFrame)
                {
                    betweenGhostFrames = 0;
                    ghost.SetActive(true);
                    ghostFrames++;
                    Move(true);

                    if (ghostFrames >= GameManager.totalGhostFrames)
                    {
                        ResetGhost();
                    }
                }
            }
        }
        else if(lifeTime > 0 && timeOfSpawn+lifeTime < GameManager.CurrentFrameCount)
        {
            Destroy(gameObject);
        }
    }

    private void ResetGhost()
    {
        ghostFrames = 0;
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("Reflect: " + Vector3.Reflect(transform.up, collision.contacts[0].normal));
            transform.up = Vector3.Reflect(transform.up, collision.contacts[0].normal);
        }

        if (collision.gameObject.CompareTag("Coral"))
        {
            print("Reflect: " + Vector3.Reflect(transform.up, collision.contacts[0].normal));
            transform.up = Vector3.Reflect(transform.up, collision.contacts[0].normal);
            if (gameObject.tag != "Ghost")
            {
                collision.gameObject.GetComponent<Coral>().coralHealth--;
            }
        }

        if (collision.gameObject.tag == "Bubble")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            //transform.forward = Vector3.Reflect(transform.up, o);
        }

        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (isPlayerOwned)
        {
            GameManager.instance.PlayerBullets.Remove(this);
        }
        else
        {
            GameManager.instance.ListOfBulletLists[level - 1].Remove(this);
        }
    }
}
