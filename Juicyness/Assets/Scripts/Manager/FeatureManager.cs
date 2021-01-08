using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManager : MonoBehaviour
{
    public static FeatureManager instance;

    private GameObject player;
    private bool isPlayerBlack = false;
    private bool areEnemiesBlack = false;
    [HideInInspector] public bool isCameraEffectsOn = false;
    [HideInInspector] public bool isCameraTilted = false;
    [HideInInspector] public bool isParticleEffectsOn = false;
    [HideInInspector] public bool isSoundEffectOn = false;
    [HideInInspector] public bool isUIEffecstOn = false;
    [HideInInspector] public bool isMusicOn = false;
    [HideInInspector] public bool isSpriteOn = false;
    [HideInInspector] public bool isAnimationOn = false;

    public System.Action onCameraTiltedToggle;
    public System.Action onUIEffectsToggle;
    public System.Action onSpritesToggle;
    public System.Action onAnimationsToggle;
    public System.Action onCameraEffectToggle;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 cameraBasePosition;
    [SerializeField] private Vector3 cameraBaseRotation;
    [SerializeField] private Vector3 cameraTiltedPosition;
    [SerializeField] private Vector3 cameraTiltedRotation;

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
        GameManager.instance.onPlayerAssigned += () =>
        {
            player = GameManager.instance.player;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.canPlay)
        {
            if (Input.anyKeyDown)
            {
                string input = Input.inputString;
                switch (input)
                {
                    case "1":
                    case "&":
                        print("Second feature : Sprites");
                        ToggleSprites();
                        break;
                    case "2":
                    case "é":
                        print("Second feature : Sound Effect");
                        ToggleSoundEffect();
                        break;
                    case "3":
                    case @"""":
                        print("Third feature : Animations");
                        ToggleAnimations();
                        break;
                    case "4":
                    case "'":
                        print("Fourth feature : Particles Effect");
                        ToggleParticleEffects();
                        break;
                    case "5":
                    case "(":
                        print("Fifth feature : Music");
                        ToggleMusic();
                        break;
                    case "6":
                    case "-":
                        print("Sixth feature : Camera effects");
                        ToggleCameraEffects();
                        break;
                    case "7":
                    case "è":
                        print("Seventh feature : UI Effects");
                        ToggleUIEffects();
                        break;
                    case "8":
                    case "_":
                        print("Eigth feature : Tilted Camera");
                        ToggleCameraTiltingEffect();
                        break;
                    case "9":
                    case "ç":
                        ToggleSprites();
                        ToggleSoundEffect();
                        ToggleAnimations();
                        ToggleParticleEffects();
                        ToggleMusic();
                        ToggleCameraEffects();
                        ToggleUIEffects();
                        ToggleCameraTiltingEffect();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ToggleSprites()
    {
        isSpriteOn = !isSpriteOn;
        if (!isSpriteOn)
        {
            isAnimationOn = false;
            if (onAnimationsToggle != null) onAnimationsToggle.Invoke();
        }
        if (onSpritesToggle != null) onSpritesToggle.Invoke();
    }

    public void ToggleAnimations()
    {
        isAnimationOn = !isAnimationOn;
        if (isAnimationOn)
        {
            if (!isSpriteOn)
            {
                isSpriteOn = true;
                if (onSpritesToggle != null) onSpritesToggle.Invoke();
            }
        }
        if (onAnimationsToggle != null) onAnimationsToggle.Invoke();
    }

    public void ToggleCameraEffects()
    {
        isCameraEffectsOn = !isCameraEffectsOn;
        if (onCameraEffectToggle != null) onCameraEffectToggle.Invoke();
    }

    public void ToggleCameraTiltingEffect()
    {
        isCameraTilted = !isCameraTilted;
        if (onCameraTiltedToggle != null) onCameraTiltedToggle.Invoke();
    }

    public void ToggleParticleEffects()
    {
        isParticleEffectsOn = !isParticleEffectsOn;
    }

    public void ToggleSoundEffect()
    {
        isSoundEffectOn = !isSoundEffectOn;
    }

    public void ToggleUIEffects()
    {
        isUIEffecstOn = !isUIEffecstOn;
        if (onUIEffectsToggle != null) onUIEffectsToggle.Invoke();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        if (isMusicOn)
        {
            AudioManager.instance.PlayMusic();
        }
        else
        {
            AudioManager.instance.StopMusic();
        }
    }

}
