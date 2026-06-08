//名前変更を担当するクラス。
using UnityEngine;
using System.Collections;
using TMPro;

public class NameSave : MonoBehaviour
{
    public PlayerProfileManager profileManager;
    public TMP_InputField nameInput;
    public GameObject saveText;
    private bool saveDisplay;
    private Coroutine saveConfirmation;

    void Start()
    {
        saveDisplay = false;
        nameInput.text = profileManager.PlayerName;
    }
    //名前の変更ボタンが押された時に名前を変更するメソッド。
    public void OnSaveName()
    {
        profileManager.SetPlayerName(nameInput.text);
        if (saveDisplay)
        {
            StopCoroutine(saveConfirmation);
        }
        saveConfirmation = StartCoroutine(SaveConfirmation());

    }
    //名前を変更した確認メッセージを表示するルーチン。
    IEnumerator SaveConfirmation()
    {
        saveText.SetActive(true);
        saveDisplay = true;
        yield return new WaitForSeconds(3.0f);
        saveText.SetActive(false);
        saveDisplay = false;
    }
}
