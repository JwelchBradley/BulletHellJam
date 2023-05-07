using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHook : EnemyBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeFields();

        if(moveSpeed == 0)
        {
            moveSpeed = 1;
        }
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
        }
    }

    public override void MoveEnemy()
    {
        float distance = Vector2.Distance(gameObject.transform.position, player.transform.position);

            gameObject.transform.position += transform.forward * moveSpeed * Time.deltaTime;

    }
}
