using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMouvementManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public float boundaryLeft = -5;
    public float boundaryRight = 5;
    [SerializeField] private bool moveLeft = true;
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float goDownStep = 0.5f;
    private Vector3 moveDir;
    private EnemyManager enemyManager;

    private void Awake()
    {
        enemies = new List<GameObject>();
        enemyManager = EnemyManager.instance;
        enemyManager.enemiesMouvementManager.Add(this);
    }

    void Update()
    {
        Move();
    }

    // Moves the line of enemies left / right
    private void Move()
    {
        if (moveLeft)
        {
            moveDir = new Vector3(1.0f, 0.0f, 0.0f);

            foreach (GameObject enemyLine in enemies)
            {
                enemyLine.transform.Translate(moveDir * Time.deltaTime * speed, Space.World);
            }
        }
        else
        {
            moveDir = new Vector3(-1.0f, 0.0f, 0.0f);

            foreach (GameObject enemyLine in enemies)
            {
                enemyLine.transform.Translate(moveDir * Time.deltaTime * speed, Space.World);
            }
        }
    }

    // Method called from an enemy ship once it reaches a boundary
    public void ChangeDirection(bool shouldMoveLeft)
    {
        moveLeft = shouldMoveLeft;
        foreach (GameObject enemyLine in enemies)
        {
            enemyLine.transform.position = new Vector3(enemyLine.transform.position.x, enemyLine.transform.position.y - goDownStep, enemyLine.transform.position.z);
        }
    }

    public void CheckIfNoMoreEnemies()
    {
        if (enemies.Count == 0)
        {
            GameManager.instance.RemoveALine();
        }
    }
}
