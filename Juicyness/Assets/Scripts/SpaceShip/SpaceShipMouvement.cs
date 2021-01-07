using UnityEngine;
using UnityEngine.UI;

public class SpaceShipMouvement : MonoBehaviour
{

    [SerializeField] private float speed = 2;
    public int life = 3;
    [SerializeField] private Text lifeText;
    [SerializeField] private GameObject lifeIcons;

    private Vector3 screenBounds;
    private float playerWidth;

    [SerializeField] private SpaceShipSkin shipSkin;
    [SerializeField]
    private UnityEditor.Animations.AnimatorController[] bananaAnimators;
    private Animator animator;

    private GameObject enemy;

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
                shipSkin.SetBananaState(3 - life);
                shipSkin.ChangeBananaSprite();
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
            if (enemy != collision.transform.parent.gameObject)
            {
                enemy = collision.transform.parent.gameObject;
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
                    if (FeatureManager.instance.isAnimationOn)
                    {
                        animator.SetBool("isDead", true);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    animator.runtimeAnimatorController = bananaAnimators[3 - life];
                    AudioManager.instance.Play("BananaDamaged", 1 + Random.Range(-0.5f, 0.5f));
                    if (FeatureManager.instance.isCameraEffectsOn)
                    {
                        Camera.main.GetComponent<CameraShake>().ShakeCamera(0.2f, 0.1f);
                        InterfaceManager.instance.ActivateDamageEffect();
                    }
                }
            }
        }
    }

    private void UpdateLifeIcons()
    {
        lifeIcons.transform.GetChild(life).GetComponent<Animation>().Play();
    }

    public void LooseAfterDeath()
    {
        GameManager.instance.ChangeState(State.LOOSE);
    }
}
