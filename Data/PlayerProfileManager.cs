/*プレイヤーのプロフィールデータの保存と読み込みを担当するクラス。
プレイヤーのプロフィールにはそのプレイヤーのIDや名前が含まれた他、
プレイヤーの好みの設定も保存されている。*/
using UnityEngine;
using System;
public class PlayerProfileManager : MonoBehaviour
{
    public string PlayerID { get; private set; }
    public string PlayerName { get; private set; }
    public float prefDisplayTime { get; private set; }
    public float prefNoteVolume { get; private set; }

    const string ID_KEY = "PlayerID";
    const string NAME_KEY = "PlayerName";
    const string SPEED_KEY = "DisplayTime";
    const string NOTE_VOL_KEY = "NoteVolume";

    void Awake()
    {
        LoadOrCreateProfile();
    }

    //既存のプレイヤープロフィールあれば読み込んでなければ作るメソッド。
    void LoadOrCreateProfile()
    {
        if (PlayerPrefs.HasKey(ID_KEY))
        {
            PlayerID = PlayerPrefs.GetString(ID_KEY);
            PlayerName = PlayerPrefs.GetString(NAME_KEY, "Player");
            prefDisplayTime = PlayerPrefs.GetFloat(SPEED_KEY, 1.0f);
            prefNoteVolume = PlayerPrefs.GetFloat(NOTE_VOL_KEY, 100.0f);
        }
        else
        {
            PlayerID = Guid.NewGuid().ToString();
            PlayerName = "Player";

            PlayerPrefs.SetString(ID_KEY, PlayerID);
            PlayerPrefs.SetString(NAME_KEY,PlayerName);
            prefDisplayTime = 1.0f;
            prefNoteVolume = 100.0f;
            PlayerPrefs.SetFloat(SPEED_KEY, prefDisplayTime);
            PlayerPrefs.SetFloat(NOTE_VOL_KEY,prefNoteVolume);
            PlayerPrefs.Save();
        }
    }
    /*プレイヤーの名前を変更する。
    name - 新しい名前。
    */
    public void SetPlayerName(String name)
    {
        PlayerName = name;
        PlayerPrefs.SetString(NAME_KEY,name);
        PlayerPrefs.Save();
    }
    /*プレイヤーの設定の好みを変更する。
    displayTime - ノーツの表示時間。低ければ低いほどノーツの落下スピードが上がる。[0.2,1.5]
    noteVolume - ノーツの判定に成功した時の音の音量。 ([0,1.0])
    */
    public void SetPreferences(float displayTime, float noteVolume)
    {
        prefDisplayTime = displayTime;
        prefNoteVolume = noteVolume;
        PlayerPrefs.SetFloat(SPEED_KEY, displayTime);
        PlayerPrefs.SetFloat(NOTE_VOL_KEY, noteVolume);
        PlayerPrefs.Save();
    }
}
