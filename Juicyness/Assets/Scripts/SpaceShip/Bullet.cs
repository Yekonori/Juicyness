using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool isEnemyBullet = false;
    [SerializeField] private float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        
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
                ScoreManager.instance.ChangeScore(collision.GetComponent<Enemy>().enemyValue);
                collision.GetComponent<Enemy>().Die();
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                return;
            }
        }
        Destroy(this.gameObject);
    }
}
