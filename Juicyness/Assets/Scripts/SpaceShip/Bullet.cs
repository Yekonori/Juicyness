using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool isEnemyBullet = false;
    [SerializeField] private float speed = 3;
    [SerializeField] private TrailRenderer trail;

    // Start is called before the first frame update
    void Start()
    {
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

        if (isEnemyBullet)
        {
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
