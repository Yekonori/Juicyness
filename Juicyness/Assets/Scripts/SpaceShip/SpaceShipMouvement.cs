using UnityEngine;
using UnityEngine.UI;

public class SpaceShipMouvement : MonoBehaviour
{

    [SerializeField] private float speed = 2;
    [SerializeField] private int life = 3;
    [SerializeField] private Text lifeText;
    [SerializeField] private GameObject lifeIcons;
    private Vector3 screenBounds;
    private float playerWidth;

    [SerializeField] private SpaceShipSkin shipSkin;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetPlayer(gameObject);

        lifeText.text = "Life : " + life;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        playerWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        shipSkin.SetBananaState(4);
        shipSkin.ChangeBananaSprite();

        animator = GetComponent<Animator>();

        FeatureManager.instance.onUIEffectsToggle += () =>
        {
            if (FeatureManager.instance.isUIEffecstOn)
            {
                lifeText.gameObject.SetActive(false);
                lifeIcons.SetActive(true);
            }
            else
            {
                lifeText.gameObject.SetActive(true);
                lifeIcons.SetActive(false);
            }
        };

        FeatureManager.instance.onSpritesToggle += () =>
        {
            if (FeatureManager.instance.isSpriteOn)
            {
                shipSkin.SetBananaState(3 - life);
                shipSkin.ChangeBananaSprite();
            }
            else
            {
                shipSkin.SetBananaState(4);
                shipSkin.ChangeBananaSprite();
                animator.enabled = false;
            }
        };
        FeatureManager.instance.onAnimationsToggle += () =>
        {
            if (FeatureManager.instance.isAnimationOn)
            {
                animator.enabled = true;
            }
            else
            {
                animator.enabled = false;
                shipSkin.SetBananaState(4);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            if (Input.GetAxis("Horizontal") > 0f)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else if (Input.GetAxis("Horizontal") < 0f)
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
            UpdateLifeIcons();
            if (FeatureManager.instance.isSpriteOn)
            {
                shipSkin.ChangeBananaState();
            }

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

    private void UpdateLifeIcons()
    {
        lifeIcons.transform.GetChild(life).GetComponent<Animation>().Play();
        //lifeIcons.transform.GetChild(life).gameObject.SetActive(false);
    }
}
