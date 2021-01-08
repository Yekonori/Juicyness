using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    INGAME,
    WIN,
    LOOSE
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public State state;
    public System.Action onStateChange;
    public System.Action onPlayerAssigned;
    public System.Action onNewGame;

    public bool canPlay;

    public GameObject player;
    [HideInInspector] public float numberOfEnemyLines = 0;

    private bool canPlayAgain = false;

    [SerializeField] private GameObject confettiParticle;

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
        onStateChange += () =>
        {
            if(state == State.INGAME)
            {
                canPlay = true;
            }
            else
            {
                canPlay = false;
                canPlayAgain = true;
            }
        };
        ChangeState(State.INGAME);
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlayAgain)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                canPlayAgain = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public void ChangeState(State newState)
    {
        state = newState;
        if (onStateChange != null) onStateChange.Invoke();
    }

    public void RemoveALine()
    {
        numberOfEnemyLines--;
        if(numberOfEnemyLines == 0)
        {
            InterfaceManager.instance.DeactivateEffects();
            AudioManager.instance.Stop("Music");
            AudioManager.instance.Play("Win");
            if (FeatureManager.instance.isParticleEffectsOn)
            {
                confettiParticle.SetActive(true);
            }
            if (!FeatureManager.instance.isCameraEffectsOn)
            {
                ChangeState(State.WIN);
            }
            else
            {
                canPlay = false;
                Camera.main.GetComponent<CameraMouvement>().SetUpVictoryPosition();
            }
        }
    }

    public void LooseProcess()
    {
        InterfaceManager.instance.DeactivateEffects();
        AudioManager.instance.Play("Lose");
        AudioManager.instance.Stop("Music");
        canPlay = false;
        if (FeatureManager.instance.isCameraEffectsOn)
        {
            Camera.main.GetComponent<CameraShake>().ShakeCamera(0.3f, 0.8f);
        }
        else if (!FeatureManager.instance.isAnimationOn && !FeatureManager.instance.isParticleEffectsOn)
        {
            ChangeState(State.LOOSE);
        }
    }

}
