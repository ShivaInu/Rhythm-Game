//判定結果を表示するクラス。
using UnityEngine;
using TMPro;
using System.Collections;

public class JudgeEffectController : MonoBehaviour
{
    public TextMeshProUGUI judgeText;

    public float duration = 0.5f;
    public float scaleAmount = 1.3f;

    Coroutine currentRoutine;

    /*判定結果を表示する。
    message - 表示するテキスト
    color - テキストの色
    */
    public void Show(string message, Color color)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(PlayEffect(message,color));
    }
    
    //判定結果を表示し、演出を行うルーチン。
    IEnumerator PlayEffect(string message, Color color)
        {
            judgeText.text = message;
            judgeText.color = color;

            judgeText.alpha = 1f;
            judgeText.transform.localScale = Vector3.one;

            float time = 0f;
            //テキストがどんどん大きくなる演出。
            while (time < duration)
            {
                time += Time.deltaTime;

                float t = time / duration;

                judgeText.alpha = 1f - t;

                float scale = Mathf.Lerp(1f, scaleAmount, t);
                judgeText.transform.localScale = 
                    Vector3.one * scale;

                yield return null;
            }

            judgeText.text = "";
        }
    }
