using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<EnemyMouvementManager> enemiesMouvementManager = new List<EnemyMouvementManager>();
    public bool canGoDown = true;

    public float increaseSpeed = 0.5f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.numberOfEnemyLines = enemiesMouvementManager.Count;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EveryEnemyHasToChangeDirection(bool shouldGoLeft)
    {
        if (canGoDown)
        {
            canGoDown = false;
            foreach (EnemyMouvementManager enemyMouvMan in enemiesMouvementManager)
            {
                enemyMouvMan.ChangeDirection(shouldGoLeft);
            }

            Debug.Log("Ajout du son de claquement des dents ici");

            IncreaseEnemySpeed();
        }
    }

    private void IncreaseEnemySpeed()
    {
        foreach (EnemyMouvementManager enemyMouvMan in enemiesMouvementManager)
        {
            enemyMouvMan.AddSpeed(increaseSpeed);
        }
    }
}
