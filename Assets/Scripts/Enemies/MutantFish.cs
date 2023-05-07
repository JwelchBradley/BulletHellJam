using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantFish : EnemyBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeFields();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        gameObject.transform.LookAt(player.transform.position);

        if(GameManager.instance.runningFrames)
        {
            MoveEnemy();
            ShootProjectile();
        }
    }

    public override void MoveEnemy()
    {
        float distance = Vector2.Distance(gameObject.transform.position, player.transform.position);

        if(distance <= 5f)
        {
            ShootProjectile();
        }
        else
        {
            gameObject.transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

    }

    public override void ShootProjectile()
    {
        StartCoroutine(InstantiateBullet());
    }

    IEnumerator InstantiateBullet()
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate(projectile, transform.parent.position, Quaternion.identity);

    }
}
