//シーン遷移演出を担当するクラス。
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ScreenWiper : MonoBehaviour
{
    public Image wipeImage;
    public float wipeInDuration = 2.0f;

    void Awake()
    {
        wipeImage = GetComponent<Image>();
        WipeIn(wipeInDuration);
    } 

    /*次のシーンに遷移するメソッド。
    duration - 演出のアニメーションの長さ（秒単位）
    nextScene - 次のシーン名*/
    public void WipeOut(float duration, string nextScene)
    {
        wipeImage.DOFillAmount(1.0f,duration).OnComplete(()=>{
        SceneChanger.instance.ChangeScene(nextScene);
        }
        );
    }
    /*シーン開始の演出を行うメソッド。
    duration - 演出のアニメーションの長さ(秒単位)*/
    public void WipeIn(float duration)
    {
        wipeImage.DOFillAmount(0.0f,duration);
    }
    //ゲームのシーンから選曲に戻るメソッド。UIボタンから呼び出し用。
    public void ReturnToSongSelect()
    {
        WipeOut(1.0f,"SongSelect");
    }
}
