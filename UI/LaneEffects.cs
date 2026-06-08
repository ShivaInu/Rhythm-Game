//ボタンを押した時のレーンの色変更演出を担当するクラス。
using UnityEngine;

public class LaneEffects : MonoBehaviour
{
    public SpriteRenderer[] lanes;
    public Transform[] laneTransforms;
    public Color notPressed = Color.black;
    public Color pressed = Color.cyan;
    public float pressedScale = 1.1f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = laneTransforms[0].localScale;
    }
    /*レーンの色をアクティブ（担当のボタンが押されている）か非アクティブの色塗りをするメソッド。
    laneIndex - どのレーンの色を変える ([0,3])
    lanePressed - レーンの担当のボタンが押されているかどうか*/
    public void setLaneColor(int laneIndex, bool lanePressed)
    {
        if (lanePressed)
        {
            lanes[laneIndex].color = pressed;
            //さらに少しサイズを大きくすればより綺麗。
            Vector3 scaledVect = originalScale;
            scaledVect.x *= pressedScale;
            laneTransforms[laneIndex].localScale = scaledVect;
        }
        else
        {
            lanes[laneIndex].color = notPressed;
            laneTransforms[laneIndex].localScale = originalScale;
        }
    }
}
