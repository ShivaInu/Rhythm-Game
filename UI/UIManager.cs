//ゲームのコンボやスコア表示を担当するクラス。
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public ScoreManager scoreManager;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI accuracyText;

    void Update()
    {
        scoreText.text = $"Score : {scoreManager.score}";
        comboText.text = $"Combo : {scoreManager.combo}";
        accuracyText.text = $"Accuracy : {scoreManager.Accuracy():0.00%}";
    }

}
