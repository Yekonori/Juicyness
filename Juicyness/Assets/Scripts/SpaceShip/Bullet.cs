using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
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
        Destroy(this.gameObject);
    }
}
