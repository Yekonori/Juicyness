using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    [SerializeField] private Text scoreText;
    private float score = 0;

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
        scoreText.text = "Score : " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScore(float scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score : " + score.ToString();
    }
}
