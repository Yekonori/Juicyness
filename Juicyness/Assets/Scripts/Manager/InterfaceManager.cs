using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

    [Header("Panels")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private GameObject controlPanel;
    [Header("DamageEffect")]
    [SerializeField] private Image damageEffect;
    [SerializeField] private float timeForDamageEffectToDisappear = 0.25f;
    private float damageEffectLerpTimer = 0;
    private Color damageEffectBaseColor;
    private Color damageEffectfullAlphaColor;
    private bool canLerpDamageEffect = false;
    [Header("KillEffect")]
    [SerializeField] private Image killEffect;
    [SerializeField] private float timeForKillEffectToDisappear = 0.25f; 
    private float killEffectLerpTimer = 0;
    private Color killEffectBaseColor;
    private Color killEffectfullAlphaColor;
    private bool canLerpKillEffect = false;
    [Header("DangerEffect")]
    [SerializeField] private Image dangerEffect;
    [SerializeField] private GameObject background;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.onStateChange += () => {
            if (GameManager.instance.state == State.INGAME)
            {
                GoToGame();
            }
            else if (GameManager.instance.state == State.WIN)
            {
                GoToWin();
            }
            else if (GameManager.instance.state == State.LOOSE)
            {
                GoToLoose();
            }
        };

        FeatureManager.instance.onCameraEffectToggle += () =>
        {
            if (!FeatureManager.instance.isCameraEffectsOn)
            {
                ActivateDangerEffect(0);
            }
        };

        FeatureManager.instance.onUIEffectsToggle += () =>
        {
            if (FeatureManager.instance.isUIEffecstOn)
            {
                background.SetActive(true);
            }
            else
            {
                background.SetActive(false);
            }
        };

        GoToGame();

        damageEffectBaseColor = damageEffect.color;
        damageEffectfullAlphaColor = damageEffectBaseColor;
        damageEffectfullAlphaColor.a = 0.35f;

        killEffectBaseColor = killEffect.color;
        killEffectfullAlphaColor = killEffectBaseColor;
        killEffectfullAlphaColor.a = 0.35f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            if (canLerpDamageEffect)
            {
                damageEffectLerpTimer += Time.deltaTime / timeForDamageEffectToDisappear;
                damageEffect.color = Color.Lerp(damageEffectfullAlphaColor, damageEffectBaseColor, damageEffectLerpTimer);
                if(damageEffectLerpTimer >= 1)
                {
                    canLerpDamageEffect = false;
                }
            }

            if (canLerpKillEffect)
            {
                killEffectLerpTimer += Time.deltaTime / timeForKillEffectToDisappear;
                killEffect.color = Color.Lerp(killEffectfullAlphaColor, killEffectBaseColor, killEffectLerpTimer);
                if (killEffectLerpTimer >= 1)
                {
                    canLerpKillEffect = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (controlPanel.activeSelf)
                {
                    GoToGame();
                }
                else
                {
                    GoToControl();
                }
            }
        }
    }
    private void GoToGame()
    {
        controlPanel.SetActive(false);
        winPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1;
    }
    private void GoToWin()
    {
        Time.timeScale = 0;
        controlPanel.SetActive(false);
        gamePanel.SetActive(false);
        winPanel.SetActive(true);
    }
    private void GoToLoose()
    {
        Time.timeScale = 0;
        controlPanel.SetActive(false);
        gamePanel.SetActive(false);
        loosePanel.SetActive(true);
    }

    private void GoToControl()
    {
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void ActivateDamageEffect()
    {
        damageEffectLerpTimer = 0;
        damageEffect.color = damageEffectfullAlphaColor;
        canLerpDamageEffect = true;
    }

    public void ActivateKillEffect()
    {
        Camera.main.GetComponent<CameraShake>().ShakeCamera(0.2f, 0.1f);
        killEffectLerpTimer = 0;
        killEffect.color = killEffectfullAlphaColor;
        canLerpKillEffect = true;
    }

    public void DeactivateEffects()
    {
        killEffect.gameObject.SetActive(false);
        damageEffect.gameObject.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ActivateDangerEffect(float t)
    {
        Color baseColor = new Color(dangerEffect.color.r, dangerEffect.color.g, dangerEffect.color.b, 0);
        Color goalColor = new Color(dangerEffect.color.r, dangerEffect.color.g, dangerEffect.color.b, 1);
        dangerEffect.color = Color.Lerp(baseColor, goalColor, t);
    }

}
