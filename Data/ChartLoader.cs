//譜面のデータの読み込みを担当するクラス。
using UnityEngine;

public class ChartLoader : MonoBehaviour
{
    public string chartFilename;

    void Awake()
    {
        GameObject settingsObject = GameObject.FindGameObjectWithTag("Settings");
        SettingContainer settings = settingsObject.GetComponent<SettingContainer>();
        chartFilename = settings.chartName;
    }
    //譜面のデータを読み込んで、譜面の情報を持っているChartDataのオブジェクトを生成するメソッド。
    public ChartData LoadChart()
    {
        Debug.Log(chartFilename);
        TextAsset json = Resources.Load<TextAsset>("Charts/" + chartFilename);
        return JsonUtility.FromJson<ChartData>(json.text);
    }
}
