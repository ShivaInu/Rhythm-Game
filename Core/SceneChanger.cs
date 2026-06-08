//シーン遷移を管理するクラス。
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    
    public static SceneChanger instance;

    void Awake()
    {
        //このクラスはシングルトン。
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //シーンを変えるメソッド。
    //sceneName - 次のシーンの名前
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    //ゲームのシーンに遷移するメソッド。
    public void startSong()
    {
        SceneManager.LoadScene("Game");
    }
    //スコアの結果発表画面に遷移するメソッド。
    public void showResult(){
    SceneManager.LoadScene("Result");
    }
    //選曲画面に遷移するメソッド。
    public void toSongSelect()
    {
         SceneManager.LoadScene("SongSelect");
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}