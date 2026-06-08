//ランキングデータベースに繋げてランキング情報をダウンロード、アップロードするクラス。
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RankingData
{
    public int id;
    public string created_at;

    public string name;
    public int score;
    public float accuracy;
    public string player_id;
}
//SupabaseのGET返り値のリスト化
[System.Serializable]
public class Response
{
        public List<RankingData> response;
}
public class RankingManager : MonoBehaviour
{
    public string supabaseURL;
    public string uuid;
    public string apiKey;
    public Transform contentRoot;
    public RankingListManager rankingItemPrefab;
    public PlayerProfileManager profileManager;
    public TextMeshProUGUI resultText,rankingGetErrorText;
    public GameObject reconnectButton;
    private Response currentRankingList;
    public float serverHighScore;
    private SettingContainer settings;
    private ScoreManager scoreManager;

    //サーバーのハイスコアを変更するメソッド。
    public void UpdateHighScore()
    {
        settings = GameObject.FindWithTag("Settings").GetComponent<SettingContainer>();
        scoreManager = GameObject.FindWithTag("ScoreManager").GetComponent<ScoreManager>();
        StartCoroutine(GetHighscore(settings.songID,profileManager.PlayerID));
        //rankingManager.SubmitScore(profileManager.PlayerID, nameInput.text, settings.songID,scoreManager.score,scoreManager.Accuracy());
    }
    /*
    ハイスコアをデータベースにアップロードする。
    playerID - プレイヤーのuuid。
    playerName - プレイヤー名
    songID - 譜面のデータベース側の名前。
    score - プレイヤーのスコア。
    accuracy - プレイヤーの精度。
    */
    public void SubmitScore(string playerID,string playerName, string songID, int score, float accuracy)
    {
        StartCoroutine(PostScore(playerID, playerName, songID, score, accuracy));
    }

    /*
    データベースのハイスコアを変更するルーチン。
    playerID - プレイヤーのuuid。
    songID - 譜面のデータベース側の名前。
    */
    IEnumerator UpdateScore(string playerID, string songID)
    {
        string json = 
        $"{{\"score\":{scoreManager.score},\"accuracy\":{scoreManager.Accuracy()}}}";

        UnityWebRequest request =
            new UnityWebRequest(supabaseURL + $"?song_id=eq.{songID}&player_id=eq.{playerID}", "PATCH");
        
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type","application/json");
        request.SetRequestHeader("apikey", apiKey);
        request.SetRequestHeader("x-app-api-key", uuid);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
            reconnectButton.SetActive(true);
            resultText.enabled = true;
            resultText.text = 
            "スコアサーバに接続できませんでした。\nインターネットの環境を確認した上で\n以下のボタンを押して再接続してください。";
        }
        else
        {
            reconnectButton.SetActive(false);
            resultText.enabled = true;
            resultText.text = "サーバのハイスコア更新！";
            Debug.Log("スコア更新に成功しました。");
        }
    }
    /*
    データベースにプレイヤーの既存スコアはない場合のスコア変更するメソッド。
    playerID - プレイヤーのuuid。
    playerName - プレイヤー名
    songID - 譜面のデータベース側の名前。
    score - プレイヤーのスコア。
    accuracy - プレイヤーの精度。
    */
    IEnumerator PostScore(string playerID,string playerName, string songID, int score, float accuracy)
    {
        string json =
            $"{{\"name\":\"{playerName}\",\"song_id\":\"{songID}\",\"score\":{score},\"accuracy\":{accuracy},\"player_id\":\"{playerID}\"}}";
        
        UnityWebRequest request =
            new UnityWebRequest(supabaseURL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type","application/json");
        request.SetRequestHeader("apikey", apiKey);
        request.SetRequestHeader("x-app-api-key", uuid);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            reconnectButton.SetActive(true);
            resultText.enabled = true;
            resultText.text = 
            "スコアサーバに接続できませんでした。\nインターネットの環境を確認した上で\n以下のボタンを押して再接続してください。";
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        }
        else
        {
            reconnectButton.SetActive(false);
            resultText.enabled = true;
            resultText.text = "サーバのハイスコア更新！";
            Debug.Log("スコアアップロード成功しました。");
        }
    }
    /*
    譜面のランキング情報をダウンロードするメソッド。
    songID - 譜面のデータベース側の名前。
    */
    public IEnumerator GetRanking(string songID)
        {
    string url =
        supabaseURL
        + $"?song_id=eq.{songID}&order=score.desc&limit=10";

    UnityWebRequest request =
        UnityWebRequest.Get(url);

    request.SetRequestHeader("apikey", apiKey);
    request.SetRequestHeader("Authorization", "Bearer " + apiKey);


    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
        {
            rankingGetErrorText.enabled = false;
            reconnectButton.SetActive(false);
            string json = request.downloadHandler.text;
            json = "{\"response\": " + json + "}";
            Debug.Log(json);
            currentRankingList = JsonUtility.FromJson<Response>(json);
            DisplayRanking(currentRankingList.response);
        }
        else
        {
            resultText.enabled = true;
            reconnectButton.SetActive(true);
        }
    }
/*
プレイヤーのハイスコアをデータベースから読み込むメソッド。
    songID - 譜面のデータベース側の名前。
    playerID - プレイヤーのuuid。
*/
public IEnumerator GetHighscore(string songID, string playerID)
    {
        string url =
        supabaseURL + $"?song_id=eq.{songID}&player_id=eq.{playerID}&limit=1";

        UnityWebRequest request =
        UnityWebRequest.Get(url);

    request.SetRequestHeader("apikey", apiKey);
    request.SetRequestHeader("Authorization", "Bearer " + apiKey);


    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        string json = request.downloadHandler.text;
        json = "{\"response\": " + json + "}";
        Debug.Log(json);
        currentRankingList = JsonUtility.FromJson<Response>(json);
        CheckHighScore(currentRankingList.response);
    }
        else
        {
            reconnectButton.SetActive(true);
            resultText.enabled = true;
            resultText.text = 
            "スコアサーバに接続できませんでした。\nインターネットの環境を確認した上で\n以下のボタンを押して再接続してください。";
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        }
    }

/*
データベースのハイスコアを変更する必要があるかどうかを決めるメソッド。
data - データベースからダウンロードしたプレイヤーのスコアデータ。
*/
void CheckHighScore(List<RankingData> data)
    {
        if(data.Count == 0)
        {
            serverHighScore = 0.0f;
        }
        else
        {
            serverHighScore = data[0].score;
        }
        if(scoreManager.score > serverHighScore)
        {
            Debug.Log("High score updated.");
            if(data.Count == 0)
            {
                StartCoroutine(PostScore(profileManager.PlayerID, profileManager.PlayerName, settings.songID, scoreManager.score,
                scoreManager.Accuracy()));
            }
            else
            {
                StartCoroutine(UpdateScore(profileManager.PlayerID, settings.songID));
            }

        }
        else
        {
            Debug.Log("High score not updated.");
            resultText.enabled = false;
            reconnectButton.SetActive(false);
        }
    }
/*
リストの形でランキング情報を表示するメソッド。
data - 譜面の上位10位スコアデータ。
*/
void DisplayRanking(List<RankingData> data)
    {

        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        for (int i = 0; i < data.Count; i++)
        {
            bool isPlayer = data[i].player_id == profileManager.PlayerID;
            RankingListManager item = Instantiate(rankingItemPrefab,contentRoot);
            item.Setup(data[i].name,data[i].score,data[i].accuracy, isPlayer);
            Vector2 newPos = item.gameObject.GetComponent<RectTransform>().anchoredPosition;
            newPos = new Vector2(newPos.x,newPos.y-(70*i));
            item.gameObject.GetComponent<RectTransform>().anchoredPosition = newPos;
        }
    }
}
