using UnityEngine;
using UnityEngine.UI;

public class SpaceShipMouvement : MonoBehaviour
{

    [SerializeField] private float speed = 2;
    [SerializeField] private float life = 3;
    [SerializeField] private Text lifeText;

    [SerializeField] private SpaceShipSkin shipSkin;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetPlayer(gameObject);
        lifeText.text = "Life : " + life;

        shipSkin.ChangeBananaSprite();
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

            shipSkin.ChangeBananaState();

            if (life <= 0)
            {
                if (FeatureManager.instance.isCameraEffectsOn)
                {
                    Camera.main.GetComponent<CameraShake>().ShakeCamera(0.3f, 0.8f);
                }
                GameManager.instance.canPlay = false;
                Destroy(gameObject);
            }
            else
            {
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
