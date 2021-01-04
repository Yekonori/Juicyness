using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
