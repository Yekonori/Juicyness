﻿using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawner;
    [SerializeField] private GameObject particleExplosion;

    private EnemyMouvementManager mouvementManager;
    [HideInInspector] public float enemyValue;
    public Transform bananaEater;

    private float minShootTime = 0;
    private float maxShootTime = 0;
    private bool canShoot = false;
    private bool canCheckIfCanShoot = true;

    private float enemyWidth;
    private Vector3 screenBounds;


    private Animator animator;
    [SerializeField] private Sprite originalSprite;
    [SerializeField] private Sprite coolSprite;
    private SpriteRenderer spriteRenderer;

    private BoxCollider2D collider;

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
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.enabled = false;
        spriteRenderer.sprite = originalSprite;
        collider = GetComponent<BoxCollider2D>();

        FeatureManager.instance.onSpritesToggle += () =>
        {
            if (this)
            {
                if (!FeatureManager.instance.isSpriteOn)
                {
                    spriteRenderer.sprite = originalSprite;
                }
                else
                {
                    spriteRenderer.sprite = coolSprite;
                }
            }
        };
        FeatureManager.instance.onAnimationsToggle += () =>
        {
            if (this)
            {
                if (FeatureManager.instance.isAnimationOn)
                {
                    animator.enabled = true;
                }
                else
                {
                    animator.enabled = false;
                    spriteRenderer.sprite = coolSprite;
                }
            }
        };
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

                animator.SetBool("IsShooting", false);
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
        if (FeatureManager.instance.isAnimationOn)
        {
            collider.enabled = false;
            StartCoroutine(DieAnimations());
        }
        else
        {
            mouvementManager.enemies.Remove(gameObject);
            mouvementManager.CheckIfNoMoreEnemies();
            spriteRenderer.enabled = false;
            if (FeatureManager.instance.isParticleEffectsOn)
            {
                particleExplosion.SetActive(true);
            }
            spriteRenderer.enabled = false;
            StopAllCoroutines();
            StartCoroutine(WaitAndDestroy());
        }
    }

    public IEnumerator DieAnimations()
    {
        animator.Play("DieAnimation");

        yield return new WaitForSeconds(1.4f);

        mouvementManager.enemies.Remove(gameObject);
        mouvementManager.CheckIfNoMoreEnemies();
        if (FeatureManager.instance.isParticleEffectsOn)
        {
            particleExplosion.SetActive(true);
        }
        spriteRenderer.enabled = false;
        StopAllCoroutines();
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(this.gameObject);
    }

    IEnumerator WaitAndShoot()
    {
        float waitTime = Random.Range(minShootTime, maxShootTime);
        yield return new WaitForSeconds(waitTime);
        if (GameManager.instance.canPlay)
        {
            animator.SetBool("IsShooting", true);

            AudioManager.instance.Play("EnemyShoot");
            Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
            canCheckIfCanShoot = true;
        }
    }
}
