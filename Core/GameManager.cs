//ゲーム部分を管理するクラス。
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] NoteSpawner noteSpawner;
    [SerializeField] AudioManager audioManager;
    [SerializeField] ChartLoader chartLoader;
    [SerializeField] JudgeManager judgeManager;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] GameObject controlDisplay;
    [SerializeField] AudioSource countdownPlayer;
    [SerializeField] AudioClip countdownTick, countdownStart;
    [SerializeField] GameObject pauseOverlay;
    [SerializeField] ScreenWiper screenWiper;
    public bool isPaused,inIntro;
    void Start()
    {
        AudioListener.pause = false;
        ChartData chart = chartLoader.LoadChart();
        noteSpawner.Initialize(chart);
        StartCoroutine(StartCountDown());
    }
    //最初のカウントダウンを行うルーチン。
    IEnumerator StartCountDown()
    {
        inIntro = true;
        judgeManager.gamePaused = true;
        yield return new WaitForSeconds(1.0f);
        count.gameObject.SetActive(true);
        countdownPlayer.clip = countdownTick;
        countdownPlayer.Play();
        count.DOFade(0.0f,1.0f).OnComplete(() =>
        {
            count.text = "2";
            RestoreAlpha(count);
            countdownPlayer.Play();
            count.DOFade(0.0f, 1.0f).OnComplete(() =>
            {
                count.text = "1";
                RestoreAlpha(count);
                countdownPlayer.Play();
                count.DOFade(0.0f,1.0f).OnComplete(() =>
                {
                    count.text = "Start!";
                    RestoreAlpha(count);
                    countdownPlayer.clip = countdownStart;
                    countdownPlayer.Play();
                    count.DOFade(0.0f,1.0f).OnComplete(() =>
                    {
                        judgeManager.gamePaused = false;
                        audioManager.Play();
                    });
                });
            });
        });
        yield return new WaitForSeconds(4.1f);
        controlDisplay.SetActive(false);
        inIntro = false;
    }
    /*
    透明なTextMeshProUGUIを不透明にするヘルパー関数
    textObject - 処理の対象となるテキストオブジェクト
    */

    private void RestoreAlpha(TextMeshProUGUI textObject)
    {
            Color tmp = textObject.color;
            tmp.a = 1.0f;
            textObject.color = tmp;
    }
    //Escを押す時に呼ばれる一時停止。
    public void Pause()
    {
        if(!inIntro){
            isPaused = true;
            pauseOverlay.SetActive(true);
            judgeManager.gamePaused = true;
            AudioListener.pause = true;
            audioManager.Pause();
            Time.timeScale = 0.0f;
        }
   }
    //停止メニューで再開を選ぶ時の処理を行うメソッド。
    public void Unpause()
    {
        isPaused = false;
        pauseOverlay.SetActive(false);
        judgeManager.gamePaused = false;
        AudioListener.pause = false;
        audioManager.Resume();
        Time.timeScale = 1.0f;
    }
    //メニューから「やり直し」か「戻る」を選ぶ時の処理をするメソッド。
    //nextScene - 次のシーンの名前。
    public void RestartOrQuit(string nextScene)
    {
        Time.timeScale = 1.0f;
        screenWiper.WipeOut(1.0f,nextScene);
    }
}
