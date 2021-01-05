using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [Header("DamageEffect")]
    [SerializeField] private Image damageEffect;
    [SerializeField] private float timeForDamageEffectToDisappear = 5;
    private float damageEffectLerpTimer = 0;
    private Color damageEffectBaseColor;
    private Color damageEffectfullAlphaColor;
    private bool canLerpDamagerEffect = false;

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
        GoToGame();
        damageEffectBaseColor = damageEffect.color;
        damageEffectfullAlphaColor = damageEffectBaseColor;
        damageEffectfullAlphaColor.a = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            if (canLerpDamagerEffect)
            {
                damageEffectLerpTimer += Time.deltaTime / timeForDamageEffectToDisappear;
                damageEffect.color = Color.Lerp(damageEffectfullAlphaColor, damageEffectBaseColor, damageEffectLerpTimer);
                if(damageEffectLerpTimer >= 1)
                {
                    canLerpDamagerEffect = false;
                }
            }
        }
    }
    private void GoToGame()
    {
        winPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1;
    }
    private void GoToWin()
    {
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        winPanel.SetActive(true);
    }
    private void GoToLoose()
    {
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        loosePanel.SetActive(true);
    }

    public void ActivateDamageEffect()
    {
        damageEffectLerpTimer = 0;
        damageEffect.color = damageEffectfullAlphaColor;
        canLerpDamagerEffect = true;
    }

}
