using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool isEnemyBullet = false;
    [SerializeField] private float speed = 3;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Sprite coolSprite;
    private SpriteRenderer spriteRenderer;
    private bool shouldBeEaten = false;
    private CapsuleCollider2D collider;
    private bool isInSkull = false;
    private Transform bananaEater;
    private Animator animator;

    [SerializeField] private GameObject smokeParticles;
    private bool bulletCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail.enabled = false;
        if (FeatureManager.instance.isParticleEffectsOn)
        {
            trail.enabled = true;
        }
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay && !bulletCollision)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            if (isInSkull && transform.position.y > bananaEater.position.y + 0.35f)
            {
                Destroy(this.gameObject);
            }
        }

        if (FeatureManager.instance.isSpriteOn)
        {
            spriteRenderer.sprite = coolSprite;
            if (FeatureManager.instance.isAnimationOn && animator != null)
            {
                animator.enabled = true;
            }
        }
        else
        {
            if (animator != null) animator.enabled = false;
            spriteRenderer.sprite = baseSprite;
        }

        if (!isEnemyBullet)
        {
            if (FeatureManager.instance.isAnimationOn)
            {
                shouldBeEaten = true;
            }
            else
            {
                shouldBeEaten = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isEnemyBullet)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                return;
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemyCollided = collision.GetComponent<Enemy>();

                if (enemyCollided != null)
                {
                    ScoreManager.instance.ChangeScore(collision.GetComponent<Enemy>().enemyValue);
                    collision.GetComponent<Enemy>().Die();
                }
                else
                {
                    bulletCollision = true;
                    collider.enabled = false;
                    if (FeatureManager.instance.isParticleEffectsOn)
                    {
                        smokeParticles.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                return;
            }
        }
        if (isEnemyBullet)
        {
            Destroy(this.transform.parent.gameObject);
        }
        if (shouldBeEaten && collision.GetComponent<Enemy>())
        {
            trail.enabled = false;
            collider.enabled = false;
            bananaEater = collision.GetComponent<Enemy>().bananaEater;
            transform.position = bananaEater.position;
            speed = 2f;
            isInSkull = true;
        }
        else
        {
            if (bulletCollision)
            {
                if (FeatureManager.instance.isAnimationOn)
                {
                    animator.SetTrigger("isCooked");
                    AudioManager.instance.Play("BananaCooked");
                    StartCoroutine(WaitAndDestroy(0.6f));
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    StartCoroutine(WaitAndDestroy(0.3f));
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator WaitAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }
}
