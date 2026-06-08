//タイトル画面の処理を担当するクラス。
using UnityEngine;
using System.Collections;
public class TitleScreenManager : MonoBehaviour
{
    public ScreenWiper screenWiper;
    private bool wipeFinished;
    private bool inTransition;
    void Start()
    {
        wipeFinished = false;
        inTransition = false;
        StartCoroutine(Wait(screenWiper.wipeInDuration));
    }
    //何かしらの入力があれば選曲画面に遷移するメソッド。
    void Update()
    {
        if(wipeFinished && !inTransition){
        if (Input.anyKey){
            screenWiper.WipeOut(1.0f,"SongSelect");
            inTransition = true;
        }
        }
    }
    //画面演出が終わるまで入力を受け付けないようにするルーチン。
    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        wipeFinished=true;
    }
}
