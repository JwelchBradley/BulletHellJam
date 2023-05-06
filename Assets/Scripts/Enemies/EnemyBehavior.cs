using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    public enum EnemyType
    {
        GenericEnemy,
        MutantFish,
        Harpoon,
        FishHook,
        Bobber
    }
    public EnemyType enemyType;
    public GameObject projectile;
    public GameObject player;
    public float moveSpeed;
    public float bulletSpeed;

    public virtual void InitializeFields()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void MoveEnemy()
    {

    }

    public virtual void ShootProjectile()
    {

    }


}
