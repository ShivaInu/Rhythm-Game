//選曲画面のランキング表示を担当するクラス。
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingListManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI accText;

    /*ランキングの項目を一個作るメソッド。
    name - プレイヤーの名前。
    score - プレイヤーのスコア。
    accuracy - プレイヤーの精度。
    isPlayer - 現在のユーザーであるかどうか。
    */
    public void Setup(string name, int score,
    float accuracy, bool isPlayer)
    {
        nameText.text = name;
        scoreText.text = score.ToString();
        accText.text = (accuracy*100.0f).ToString("F2") + "%";

        if (isPlayer){
            nameText.color = Color.blue;
            scoreText.color = Color.blue;
            accText.color = Color.blue;       
        }
    }
}

