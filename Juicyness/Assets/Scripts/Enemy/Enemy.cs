using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawner;

    private EnemyMouvementManager mouvementManager;
    [HideInInspector] public float enemyValue;

    private float minShootTime = 0;
    private float maxShootTime = 0;
    private bool canShoot = false;
    private bool canCheckIfCanShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        mouvementManager = transform.parent.GetComponent<EnemyMouvementManager>();
        mouvementManager.enemies.Add(gameObject);
        enemyValue = mouvementManager.LineEnemyValue;
        minShootTime = mouvementManager.minShootTime;
        maxShootTime = mouvementManager.maxShootTime;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBoundary();
        if (canCheckIfCanShoot)
        {
            RaycastHit2D hit = Physics2D.Raycast(bulletSpawner.position - new Vector3(0, 0.2f, 0), -Vector2.up);

            if (hit.collider != null)
            {
                if (!hit.collider.gameObject.CompareTag("Enemy"))
                {
                    canShoot = true;
                }
            }
            else
            {
                canShoot = true;
            }
        }
        if (canShoot)
        {
            canCheckIfCanShoot = false;
            canShoot = false;
            StartCoroutine(WaitAndShoot());
        }
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

    IEnumerator WaitAndShoot()
    {
        float waitTime = Random.Range(minShootTime, maxShootTime);
        yield return new WaitForSeconds(waitTime);
        Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
        canCheckIfCanShoot = true;
    }
}
