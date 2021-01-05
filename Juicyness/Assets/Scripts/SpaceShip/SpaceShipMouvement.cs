using UnityEngine;
using UnityEngine.UI;

public class SpaceShipMouvement : MonoBehaviour
{

    [SerializeField] private float speed = 2;
    [SerializeField] private float life = 3;
    [SerializeField] private Text lifeText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetPlayer(gameObject);
        lifeText.text = "Life : " + life;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.position -= Vector3.right * speed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            life--;
            lifeText.text = "Life : " + life;
            if (life <= 0)
            {
                Camera.main.GetComponent<CameraShake>().ShakeCamera(0.3f, 0.8f);
                GameManager.instance.canPlay = false;
                Destroy(gameObject);
            }
            else
            {
                Camera.main.GetComponent<CameraShake>().ShakeCamera(0.2f, 0.1f);
            }
            //TODO ELSE FEEDBACK, INVINCIBILITY ?????
        }
    }
}
