using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    private const string ghostTag = "Ghost";
    private const string coralTag = "Coral";
    private const string wallTag = "Wall";
    private const string bulletTag = "Bullet";
    private const string bubbleTag = "Bubble";

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
    [SerializeField] private bool isExplosive = false;
    [SerializeField] private GameObject explodeObject;
    [SerializeField] private int amoundOfExplodeObjects;
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
            if (lifeTime > 0 && timeOfSpawn + lifeTime < GameManager.CurrentFrameCount)
            {
                Destroy(gameObject);
            }

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
    }

    private void ResetGhost()
    {
        ghostFrames = 0;
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            if (ShouldBounce)
            {
                BounceObject(collision.contacts[0].normal);
            }

            if (isExplosive)
            {
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag(coralTag))
        {
            if (ShouldBounce)
            {
                BounceObject(collision.contacts[0].normal);
            }

            if (isExplosive)
            {
                Destroy(gameObject);
            }

            if (gameObject.tag != ghostTag)
            {
                collision.gameObject.GetComponent<Coral>().coralHealth--;
            }
        }

        if (collision.gameObject.tag == bubbleTag && !gameObject.CompareTag(bubbleTag))
        {
            Destroy(collision.gameObject);
        }
    }

    private void BounceObject(Vector3 normal)
    {
        transform.up = Vector3.Reflect(transform.up, normal);
    }

    public void TailWhip()
    {
        transform.rotation = Quaternion.Inverse(transform.rotation);
    }

    private void Explode()
    {
        for (int i = 0; i < amoundOfExplodeObjects; i++)
        {
            var explodeBubble = Instantiate(explodeObject, transform.position, Quaternion.identity * Quaternion.Euler(new Vector3(0, 0, (360.0f / amoundOfExplodeObjects) * i)));

            if (gameObject.CompareTag(ghostTag))
            {
                Destroy(explodeBubble, 0.25f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isExplosive)
        {
            if (other.gameObject.tag == bulletTag)
            {
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.tag == bulletTag && ghost != null && other.gameObject.TryGetComponent(out Bullet bullet) && isPlayerOwned != bullet.isPlayerOwned)
        {
            Destroy(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this == null) return;

        if (isExplosive)
        {
            Explode();
        }

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
