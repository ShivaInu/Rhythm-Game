//入力を担当するクラス。
using UnityEngine;


public class InputSystem : MonoBehaviour
{
    public JudgeManager judgeManager;
    public GameManager gameManager;
    public LaneEffects laneEffects;
    public KeyCode[] lanekeys =
    {
        KeyCode.D,
        KeyCode.F,
        KeyCode.J,
        KeyCode.K
    };

    //レーンのキーが押されたら判定できるノーツがあるかどうかを確認する。
    void Update()
    {
        if(!gameManager.isPaused){
            for(int i=0;i<lanekeys.Length;i++)
            {
                if (Input.GetKeyDown(lanekeys[i]))
                {
                    judgeManager.CheckLane(i);
                }
            laneEffects.setLaneColor(i,Input.GetKey(lanekeys[i]));
            }
        }
        //一時停止
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManager.isPaused)
            {
                gameManager.Unpause();
            }
            else
            {
                gameManager.Pause();
            }
        }
    }
}
