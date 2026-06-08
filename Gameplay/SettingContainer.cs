
//設定の情報を持つクラス。
using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingContainer : MonoBehaviour
{
public float travelTime;
public string chartName;
public float noteVolume;
public string songPath;
public string songID;
public static SettingContainer instance;

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
