using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<EnemyMouvementManager> enemiesMouvementManager = new List<EnemyMouvementManager>();
    public bool canGoDown = true;
    public float goDownStep = 0.2f;
    [SerializeField] private float numberOfLinesBeforeDeath = 10;
    [SerializeField] private float firstAlpha = 0.2f;
    [SerializeField] private float firstLineOffset = 44;
    [SerializeField] private float spaceBetweenEnemyLines = 7;

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
        for (int i = 0; i < transform.childCount; i++)
        {
            enemiesMouvementManager.Add(transform.GetChild(i).GetComponent<EnemyMouvementManager>()); 
            enemiesMouvementManager[i].transform.position = new Vector3(enemiesMouvementManager[i].transform.position.x, GameManager.instance.player.transform.position.y + (firstLineOffset * goDownStep), enemiesMouvementManager[i].transform.position.z);
            firstLineOffset -= spaceBetweenEnemyLines;
        }
        FeatureManager.instance.onCameraEffectToggle += () =>
        {
            if (FeatureManager.instance.isCameraEffectsOn)
            {
                CheckDistanceWithPlayer();
            }
        };
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

            AudioManager.instance.Play("Teeth");
            AudioManager.instance.ChangeMusicPitch(0.01f);
            IncreaseEnemySpeed();
            CheckDistanceWithPlayer();
            StartCoroutine(WaitBeforeGoingDownAgain());
        }
    }

    IEnumerator WaitBeforeGoingDownAgain()
    {
        yield return new WaitForSeconds(0.15f);
        canGoDown = true;
    }

    private void IncreaseEnemySpeed()
    {
        foreach (EnemyMouvementManager enemyMouvMan in enemiesMouvementManager)
        {
            enemyMouvMan.AddSpeed(increaseSpeed);
        }
    }

    public void RemoveEnemyMouvementManager(EnemyMouvementManager lineToRemove)
    {
        enemiesMouvementManager.Remove(lineToRemove);
        CheckDistanceWithPlayer();
    }

    private void CheckDistanceWithPlayer()
    {
        float lowest = Mathf.Infinity;
        foreach (EnemyMouvementManager enemyMouvMan in enemiesMouvementManager)
        {
            if(lowest > enemyMouvMan.enemies[0].transform.position.y)
            {
                lowest = enemyMouvMan.enemies[0].transform.position.y;
            }
        }
        if(lowest - GameManager.instance.player.transform.position.y <= GameManager.instance.player.GetComponent<SpriteRenderer>().bounds.size.y - goDownStep)
        {
            GameManager.instance.LooseProcess();
            GameManager.instance.canPlay = false;
        }
        if (FeatureManager.instance.isCameraEffectsOn)
        {
            if (lowest - GameManager.instance.player.transform.position.y <= numberOfLinesBeforeDeath * goDownStep)
            {
                InterfaceManager.instance.ActivateDangerEffect(firstAlpha + (1 - ((lowest - GameManager.instance.player.transform.position.y) / (numberOfLinesBeforeDeath * 0.2f))));
            }
        }
    }
}
