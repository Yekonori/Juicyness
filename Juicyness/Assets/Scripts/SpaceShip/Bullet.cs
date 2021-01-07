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
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
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
        }
        else
        {
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
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                return;
            }
        }
        if (shouldBeEaten && collision.gameObject.CompareTag("Enemy"))
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
            Destroy(this.gameObject);
        }
    }
}
