
using UnityEngine;
using System.Collections;
/*
ゲームの本番でのオーディオ処理を行うクラス。
*/
public class AudioManager : MonoBehaviour
{

    public AudioSource song;
    public AudioSource hitSoundPerfect;
    public AudioSource hitSoundGood;
    public SceneChanger sceneManager;
    public ScreenWiper screenWiper;
    public bool songStarted;
    private double startTime;

    void Start()
    {
        //プレイヤーの選んだ設定を読み込む
        GameObject settingsObject = GameObject.FindGameObjectWithTag("Settings");
        SettingContainer settings = settingsObject.GetComponent<SettingContainer>();

        //曲を読み込む
        song.clip = Resources.Load<AudioClip>(settings.songPath);

        //ノートを押すときの効果音の音量を設定から読み込んで設定する
        hitSoundPerfect.volume = settings.noteVolume;
        hitSoundGood.volume = settings.noteVolume;

        songStarted = false;
    }
    //曲の再生を始めるメソッド
    public void Play()
    {
        startTime = AudioSettings.dspTime;
        song.Play();
        songStarted = true;

        //曲が終わったら結果発表に遷移するタイマー
        StartCoroutine(SceneSwitch(song.clip.length));
    }
    //曲を中止して
    public void Pause()
    {
        song.Pause();
    }
    //中止になっている曲を再開して
    public void Resume()
    {
        song.UnPause();
    }
    //Perfect判定の効果音を流す
    public void PlayPerfect()
    {
        hitSoundPerfect.PlayOneShot(hitSoundPerfect.clip);
    }
    //Good判定の効果音を流す
    public void PlayGood()
    {
        hitSoundGood.PlayOneShot(hitSoundGood.clip);
    }
    /*返り値　ー曲のはじめから経過時間（秒単位）*/
    public double CurrentTime()
    {
        if(songStarted)
            return AudioSettings.dspTime - startTime;
        else
        {
            return 0.0f;
        }
    }
    /*time秒待ってから結果発表に遷移する
    引数:
    time - 待つ時間(秒単位)*/
    IEnumerator SceneSwitch(float time)
    {
        yield return new WaitForSeconds(time);
        screenWiper.WipeOut(1.0f,"Result");
    }
}
