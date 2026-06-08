//スコアの計算を担当するメソッド。
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int score {get; private set;}
    public int combo {get; private set;}
    public int maxCombo {get; private set;}
    public int maxNotes;
    public float hitNotes;
    public int missCount;
    public int goodCount;
    public int perfectCount;

    [Header("スコア設定")]
    public int perfectScore = 1000;
    public int goodScore = 500;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /*
    Perfectの判定が決められた時の処理を行うメソッド。
    */
    public void JudgePerfect()
    {
        score+=perfectScore;
        perfectCount++;
        maxNotes++;
        hitNotes+=1.0f;
        AddCombo();
    }
    /*
    Goodの判定が決められた時の処理を行うメソッド。
    */
    public void JudgeGood()
    {
        score+=goodScore;
        goodCount++;
        maxNotes++;
        hitNotes+=0.5f;
        AddCombo();
    }
    /*
    Missの時の処理を行うメソッド。
    */
    public void JudgeMiss()
    {
        maxNotes++;
        missCount++;
        combo = 0;
    }
    /*
    コンボの数字を増やすメソッド。
    */
    void AddCombo()
    {
        combo++;
        if (combo > maxCombo)
        {
            maxCombo = combo;
        }
    }
    //プレイヤーの精度を計算するメソッド。
    public float Accuracy()
    {
        if(maxNotes == 0)
        {
            return 0.00f;
        }
        return hitNotes / maxNotes;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    if(scene.name != "Game" && scene.name != "Result")
        {
            Destroy(this.gameObject);
        }
    }
}
