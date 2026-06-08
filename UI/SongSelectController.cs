using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
/*
選曲画面の様々な機能を担当するクラス。
*/
public class SongSelectController : MonoBehaviour
{
    public Object[] songs;
    public SongData currentSong;
    public int songIndex;

    public TextMeshProUGUI songTitle, artist;
    public AudioSource songPreview;
    public TMP_Dropdown difficultyMenu;
    public Slider displayTime, noteVolume;
    public SceneChanger sceneChanger;
    public RankingManager rankingManager;
    public ScreenWiper screenWiper;
    public PlayerProfileManager profileManager;
    void Start()
    {
        //一時停止メニューから戻ってきた場合はこのAudioListenerが停止しているから再開する
        AudioListener.pause = false;
        //曲フォルダから曲の情報を読み込む
        songs = Resources.LoadAll("SongInformation");
        songIndex = 0;
        ReadInfo(songIndex);
    }

    public void NextSong()
    {
        songIndex = (songIndex+1)%songs.Length;
        ReadInfo(songIndex);
    }
    /*
    曲のデータをUIに表示するメソッド。
    引数：
    index -　曲リストにアクセスするための添字 
    */
    private void ReadInfo(int index)
    {
        //曲情報を表示して
        TextAsset json = Resources.Load<TextAsset>("SongInformation/" + songs[index].name);
        SongData song = JsonUtility.FromJson<SongData>(json.text);
        songTitle.text = song.title;
        artist.text = song.artist;

        //曲プレビューを再生して
        songPreview.clip = Resources.Load<AudioClip>(song.songPath);
        songPreview.Play();

        //難易度データをメニューに追加
        List<string> difficultyOptions = new List<string>();
        foreach(DifficultyData d in song.difficulties)
        {
            difficultyOptions.Add(d.difficultyName);
        }
        difficultyMenu.ClearOptions();
        difficultyMenu.AddOptions(difficultyOptions);

        //今選択されている曲を更新
        currentSong = song;

        //オンラインランキングデータを表示
        DisplayRanking();
    }

    /*
    選択された曲を選択された難易度で開始するメソッド。プレイヤーの設定も保存し、ゲームのシーンで読み込まれるように
    SettingContainerに残す。
    */
    public void StartSong()
    {
        SettingContainer.instance.travelTime = displayTime.value;
        SettingContainer.instance.noteVolume = noteVolume.value;
        SettingContainer.instance.songPath = currentSong.songPath;
        SettingContainer.instance.songID = currentSong.title + "_" + difficultyMenu.captionText.text;
        profileManager.SetPreferences(displayTime.value, noteVolume.value);
        foreach(DifficultyData d in currentSong.difficulties)
        {
            if(d.difficultyName == difficultyMenu.captionText.text)
            {
                SettingContainer.instance.chartName = d.chartFilename;
                break;
            }
        }
        screenWiper.WipeOut(1.0f,"Game");
    }
    //選択されている曲のオンラインランキングを表示する。
    public void DisplayRanking()
    {
        StartCoroutine(rankingManager.GetRanking(currentSong.title + "_" + difficultyMenu.captionText.text));
    }
}
