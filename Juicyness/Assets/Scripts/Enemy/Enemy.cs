﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyMouvementManager mouvementManager;

    // Start is called before the first frame update
    void Start()
    {
        mouvementManager = transform.parent.GetComponent<EnemyMouvementManager>();
        mouvementManager.enemies.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckBoundary();
    }
    private void CheckBoundary()
    {
        if (transform.position.x < mouvementManager.boundaryLeft)
        {
            EnemyManager.instance.canGoDown = true;
            EnemyManager.instance.EveryEnemyHasToChangeDirection(true);
        }
        else if (transform.position.x > mouvementManager.boundaryRight)
        {
            EnemyManager.instance.canGoDown = true;
            EnemyManager.instance.EveryEnemyHasToChangeDirection(false);
        }
    }

    public void Die()
    {
        mouvementManager.enemies.Remove(gameObject);
        mouvementManager.CheckIfNoMoreEnemies();
        Destroy(this.gameObject);
    }
}
