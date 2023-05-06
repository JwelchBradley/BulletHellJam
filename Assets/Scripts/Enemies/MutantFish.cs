using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantFish : EnemyBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public override void MoveEnemy()
    {
        float distance = Vector2.Distance(gameObject.transform.position, player.transform.position);

        if(distance <= 5f)
        {

        }
        else if(distance > 5f)
        {

        }
    }

    public override void ShootProjectile()
    {
        base.ShootProjectile();
    }
}
