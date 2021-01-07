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

    private float enemyWidth;
    private Vector3 screenBounds;

    private Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        mouvementManager = transform.parent.GetComponent<EnemyMouvementManager>();
        mouvementManager.enemies.Add(gameObject);
        enemyValue = mouvementManager.LineEnemyValue;
        minShootTime = mouvementManager.minShootTime;
        maxShootTime = mouvementManager.maxShootTime;
        enemyWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            CheckBoundary();
            if (canCheckIfCanShoot)
            {
                RaycastHit2D hit = Physics2D.Raycast(bulletSpawner.position, -Vector2.up);

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

                Animator.SetBool("IsShooting", false);
            }
            if (canShoot)
            {
                canCheckIfCanShoot = false;
                canShoot = false;
                StartCoroutine(WaitAndShoot());
            }
        }
    }
    private void CheckBoundary()
    {
        if (transform.position.x < screenBounds.x + enemyWidth)
        {
            EnemyManager.instance.EveryEnemyHasToChangeDirection(true);
        }
        else if (transform.position.x > -screenBounds.x - enemyWidth)
        {
            EnemyManager.instance.EveryEnemyHasToChangeDirection(false);
        }
    }

    public void Die()
    {
        AudioManager.instance.Play("EnemyDamaged");
        mouvementManager.enemies.Remove(gameObject);
        mouvementManager.CheckIfNoMoreEnemies();
        Destroy(this.gameObject);
    }

    IEnumerator WaitAndShoot()
    {
        float waitTime = Random.Range(minShootTime, maxShootTime);
        yield return new WaitForSeconds(waitTime);
        if (GameManager.instance.canPlay)
        {
            Animator.SetBool("IsShooting", true);

            AudioManager.instance.Play("EnemyShoot");
            Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
            canCheckIfCanShoot = true;
        }
    }
}
