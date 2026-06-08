//スライダーの値を表示するクラス。
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SliderDisplayController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI field;
    [SerializeField] float defaultValue;
    [SerializeField] PlayerProfileManager profile;
    [SerializeField] string sliderType;
    [SerializeField] Slider slider;
    void Start()
    {
        //表示をデフォルト値に設定する
        field = GetComponent<TextMeshProUGUI>();
        slider = GetComponentInParent<Slider>();
        defaultValue = sliderType == "DisplayTime" ? profile.prefDisplayTime : profile.prefNoteVolume;
        field.text = defaultValue.ToString();
        slider.value = sliderType == "DisplayTime" ? defaultValue : Mathf.Round(defaultValue);
    }

    /*
    スライダーの値が変わった時の表示更新を行うメソッド。
    value - 新しい値
    */
    public void HandleSliderValueChanged(float value)
    {
        field.text = sliderType == "DisplayTime" ? value.ToString("0.00") : value.ToString("0");
    }
}
