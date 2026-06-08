//結果発表画面を担当するクラス。
using UnityEngine;
using TMPro;


public class ResultManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject resultsPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI perfectCountText;
    public TextMeshProUGUI goodCountText;
    public TextMeshProUGUI missCountText;
    //これを後JSONから読み取るように実装して
    public string songID = "Test";
    public TextMeshProUGUI highScoreText;
    public RankingManager rankingManager;
    void Start()
    {
        scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
        GameObject settingsObject = GameObject.FindGameObjectWithTag("Settings");
        SettingContainer settings = settingsObject.GetComponent<SettingContainer>();
        songID = settings.songID;
        this.displayResult();
        rankingManager.UpdateHighScore();
    }
    //プレイの結果データをUIに描写するメソッド。
    void displayResult()
    {
        resultsPanel.SetActive(true);
        int prevHighScore = PlayerPrefs.GetInt($"{songID}_HighScore",0);

        highScoreText.text = $"Previous High Score :\n {prevHighScore}";
        scoreText.text = $"Score:\n {scoreManager.score}";
        comboText.text = $"Max Combo:\n {scoreManager.maxCombo}";
        accuracyText.text = $"Accuracy:\n {scoreManager.Accuracy()*100.0f:F2}%";
        perfectCountText.text = $"Perfect:\n {scoreManager.perfectCount}";
        goodCountText.text = $"Good:\n {scoreManager.goodCount}";
        missCountText.text = $"Miss:\n {scoreManager.missCount}";
        rankText.text = $"Rank: {GetRank(scoreManager.Accuracy())}";
        //ハイスコア変更
        if (scoreManager.score > prevHighScore)
        {
            highScoreText.text+="\nNew High Score!";
            saveHighScore();

        }
    }
    //プレイヤーの精度を使ってランクを決めるメソッド。
    string GetRank(float accuracy)
    {
        if(accuracy >= .95f) return "S";
        if(accuracy >= .85f) return "A";
        if(accuracy >= .70f) return "B";
        if(accuracy >= .60f) return "C";
        return "D";
    }
    //ハイスコアを保存するメソッド。
    void saveHighScore()
    {
        string scoreKey = $"{songID}_HighScore";
        string accKey = $"{songID}_HighAcc";

        PlayerPrefs.SetInt(scoreKey,scoreManager.score);
        PlayerPrefs.SetFloat(accKey, scoreManager.Accuracy());

        PlayerPrefs.Save();
    }
}
