//エディターのUIを生成するクラス。
using UnityEngine;
using UnityEngine.UI;

public class TimelineGrid : MonoBehaviour
{
    public RectTransform gridRoot;
    public RectTransform laneRoot;
    public RectTransform editorWindow;
    public GameObject linePrefab;
    public GameObject laneLinePrefab;

    public int beatsPerMeasure = 4;
    public int subDivisions = 2;
    public int lanes = 4;

    void Start()
    {
        GenerateTimeGrid();
        GenerateLaneGrid();
    }
    //グリッドの横線を描写するメソッド。
    void GenerateTimeGrid()
    {
        float height = editorWindow.rect.height;
        //濃い線の位置を小節によって計算する
        float beatHeight = height / beatsPerMeasure;
        //薄い線を小節の細分化方法によって決める
        float subDivisionHeight = beatHeight / subDivisions;
        for (int i = 0; i<subDivisions*beatsPerMeasure; i++)
        {
            GameObject line = Instantiate(linePrefab, gridRoot);
            RectTransform rt = line.GetComponent<RectTransform>();
            float newY = (i*subDivisionHeight);
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x,newY);
            if(i%subDivisions == 0)
            {
                Image img = line.GetComponent<Image>();
                img.color = Color.white;
            }
        }
    }
    //グリッドの縦線を描くメソッド。
    void GenerateLaneGrid()
    {
        float width = editorWindow.rect.width;
        //レーン数で線の位置を決める。当ゲームは4レーン。
        float lane_width = width / lanes;
        for (int i = 0; i < lanes-1; i++)
        {            
            GameObject line = Instantiate(laneLinePrefab, laneRoot);
            RectTransform rt = line.GetComponent<RectTransform>();
            float newX = (i+1)*lane_width;
            rt.anchoredPosition = new Vector2(newX,rt.anchoredPosition.y);
        }
    }
    }
