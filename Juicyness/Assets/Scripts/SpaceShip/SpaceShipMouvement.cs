using UnityEngine;
using UnityEngine.UI;

public class SpaceShipMouvement : MonoBehaviour
{

    [SerializeField] private float speed = 2;
    [SerializeField] private float life = 3;
    [SerializeField] private Text lifeText;
    private Vector3 screenBounds;
    private float playerWidth;

    [SerializeField] private SpaceShipSkin shipSkin;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetPlayer(gameObject);

        lifeText.text = "Life : " + life;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        playerWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        //shipSkin.ChangeBananaSprite();
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
            float x = Mathf.Clamp(transform.position.x, screenBounds.x + playerWidth, -screenBounds.x - playerWidth);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            life--;
            lifeText.text = "Life : " + life;

            shipSkin.ChangeBananaState();

            if (life <= 0)
            {
                AudioManager.instance.Play("BananaDie");
                GameManager.instance.LooseProcess();
                Destroy(gameObject);
            }
            else
            {
                AudioManager.instance.Play("BananaDamaged");
                if (FeatureManager.instance.isCameraEffectsOn)
                {
                    Camera.main.GetComponent<CameraShake>().ShakeCamera(0.2f, 0.1f);
                    InterfaceManager.instance.ActivateDamageEffect();
                }
            }
            //TODO ELSE FEEDBACK, INVINCIBILITY ?????
        }
    }
}
