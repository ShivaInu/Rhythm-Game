//譜面のデータを持つクラス。
using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
//個別のノーツ。
public class NoteData
{
    public float beat;
    public int lane;

    public NoteData(int lane, float beat)
    {
        this.beat = beat;
        this.lane = lane;
    }
}
[System.Serializable]
//譜面の情報とノーツの配置データ。
public class ChartData
{
    public string title;
    public float bpm;
    public List<NoteData> notes;
}
