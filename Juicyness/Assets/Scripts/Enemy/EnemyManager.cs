using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<EnemyMouvementManager> enemiesMouvementManager;

    private void Awake()
    {
        enemiesMouvementManager = new List<EnemyMouvementManager>();
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EveryEnemyHasToChangeDirection(bool shouldGoLeft)
    {
        foreach (EnemyMouvementManager enemyMouvMan in enemiesMouvementManager)
        {
            enemyMouvMan.ChangeDirection(shouldGoLeft);
        }
    }
}
