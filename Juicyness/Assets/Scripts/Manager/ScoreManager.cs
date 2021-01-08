using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    [SerializeField] private Text scoreText;
    private float score = 0;
    private Animation scoreAnimation;
    [SerializeField] private Color scoreColor;

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
        scoreAnimation = scoreText.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FeatureManager.instance.isUIEffecstOn)
        {
            scoreText.color = scoreColor;
        }
        else
        {
            scoreText.color = Color.white;
        }
    }

    public void ChangeScore(float scoreToAdd)
    {
        if (FeatureManager.instance.isUIEffecstOn)
        {
            scoreAnimation.Play();
        }
        score += scoreToAdd;
        scoreText.text = "Score : " + score.ToString();
    }
}
