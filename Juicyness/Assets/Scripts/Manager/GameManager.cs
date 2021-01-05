using UnityEngine;

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

    public bool canPlay;

    [HideInInspector] public GameObject player;
    [HideInInspector] public float numberOfEnemyLines = 0;

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
            }
        };
        ChangeState(State.INGAME);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            ChangeState(State.WIN);
        }
    }

    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        if (onPlayerAssigned != null) onPlayerAssigned.Invoke();
    }

}
