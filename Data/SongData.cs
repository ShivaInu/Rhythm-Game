//曲に関するデータを持つクラス。
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DifficultyData
{
    public string difficultyName;
    public string chartFilename;
}
[System.Serializable]
public class SongData
{
    public string title;
    public string artist;
    public int bpm;
    public string songPath;
    public List<DifficultyData> difficulties;

}
