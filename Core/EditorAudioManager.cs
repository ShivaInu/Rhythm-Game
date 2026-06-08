using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
//譜面エディターのシークバーや曲の再生を管理するクラス
public class EditorAudioManager : MonoBehaviour
{
    public AudioSource song;
    public Slider seek;
    public EditorManager editorManager;
    bool paused=false;
    public int bpm;
    public int beatsPerMeasure;
    public TextMeshProUGUI measureText;

    void Start()
    {   
        //曲を読み込む
        AudioClip clip = song.clip;
        float songDuration = clip.length;

        //シークバーの位置を曲と同期させる
        seek.maxValue = songDuration;
        seek.onValueChanged.AddListener(delegate {ChangeTime();});
    }
    void Update()
    {
        //シークバーを曲の経過時間と同期
        seek.value = song.time;

        //現在の小節を計算し表示する
        measureText.text = $"Measure: {editorManager.GetCurrentMeasure()}";
    }

    //曲の停止ボタンを押す処理を行う
    public void PauseButton()
    {
        if (paused)
        {
            song.Play();
            paused = false;
        }
        else
        {
            song.Pause();
            paused = true;
        }
    }
    //シークバーをドラッグする時に曲をシークバーの位置に同期する処理
    public void ChangeTime()
    {
        if(paused)
        {
            song.time = seek.value;
        }
    }
}
