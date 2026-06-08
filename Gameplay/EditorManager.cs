//譜面エディターを担当するクラス。
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class EditorManager : MonoBehaviour
{
    public RectTransform editorWindow;
    public RectTransform notesRoot;
    public RectTransform playHead;
    public GameObject notePrefab_white;
    public GameObject notePrefab_blue;

    public AudioSource audioSource;
    public float bpm = 120f;
    float editorHeight;
    float editorWidth;

    public int lanes = 4;
    public int snapDivision = 16;
    EditorNote draggingNote = null;
    bool isDragging = false;

    public List<EditorNote> notes = new List<EditorNote>();

    float beatDuration;
    public string chartName = "test";
    public AudioSource song;
    public int beatsPerMeasure = 4;
    public TMP_InputField saveOutput;

    void Start()
    {
        beatDuration = 60f / bpm;
        editorHeight = editorWindow.rect.height;
        editorWidth = editorWindow.rect.width;
        song.Play();
    }

    void Update()
    {
        UpdatePlayHead();
        UpdateNotes();
        HandleInput();
        HandleMouseInput();
    }
    //マウスの入力を処理するメソッド。
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDragOrPlace();
        }
        if (Input.GetMouseButton(0))
        {
            DragNote();
        }
        if (Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
        if (Input.GetMouseButtonDown(1))
        {
            TryDeleteNote();
        }
    }
    //マウスをクリックするときの処理。
    void StartDragOrPlace()
    {
        Vector2 pos;

        if(!GetMouseLocalPosition(out pos))
        {
            return;
        }
        pos = new Vector2(pos.x,editorHeight+pos.y);
        if (ClickOutOfBounds(pos))
        {
            return;
        }

        int lane = GetLaneFromPosition(pos.x);
        float beat = GetBeatFromPosition(pos.y);

        float threshold = 0.15f;

        //既存のノートをクリックしているかどうかを確認する
        foreach(var note in notes)
        {
            if(note.lane != lane)
            {
                continue;
            }

            if (Mathf.Abs(note.beat - beat) < threshold)
            {
                draggingNote = note;
                isDragging = true;
                return;
            }
        }

        //クリックした場所に設置済みのノートがなかった場合：
        TryPlaceNote();
    }
    //ノートのドラッグを開始するメソッド。
    void DragNote()
    {
        if (!isDragging)
        {
            return;
        }
        Vector2 pos;

        if (!GetMouseLocalPosition(out pos))
            return;
        pos = new Vector2(pos.x,editorHeight+pos.y);
        if (ClickOutOfBounds(pos))
        {
            isDragging = false;
            draggingNote = null;
            return;
        }
        int lane = GetLaneFromPosition(pos.x);
        float beat = GetBeatFromPosition(pos.y);

        draggingNote.lane = lane;
        draggingNote.beat = beat;
    }
    //ノートのドラッグを終了するメソッド。
    void EndDrag()
    {
        if (!isDragging)
        {
            return;
        }

        draggingNote.beat = SnapBeat(draggingNote.beat);

        draggingNote = null;
        isDragging = false;
    }
    //現在の小節の細分を計算するメソッド。
    public float GetCurrentBeat()
    {
        float songTime = audioSource.time;
        return songTime / beatDuration;
    }
    //現在の小節を計算するメソッド。
    public int GetCurrentMeasure()
    {
        float beat = GetCurrentBeat();
        return Mathf.FloorToInt(beat / beatsPerMeasure);
    }
    //シークバーの位置を曲の再生位置と同期させるメソッド。
    void UpdatePlayHead()
    {
        float beat = GetCurrentBeat();

        float beatInMeasure = beat % beatsPerMeasure;

        float y = (beatInMeasure / beatsPerMeasure) * editorHeight;

        playHead.anchoredPosition = new Vector2(0,y);
    }
    //小節を変える際にノートの表示を変更するメソッド。
    void UpdateNotes()
    {
        foreach (Transform child in notesRoot)
        {
            Destroy(child.gameObject);
        }
    
    int currentMeasure = GetCurrentMeasure();

    foreach (var note in notes)
        {
            int measure =
                Mathf.FloorToInt(note.beat / beatsPerMeasure);
            
            if (measure != currentMeasure)
            {
                continue;
            }

            SpawnPreview(note);
        }
    }
    /*マウスの位置を読み込むメソッド。
    localPos - 返り値を保存する変数。
    */
    bool GetMouseLocalPosition(out Vector2 localPos)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(
            editorWindow,
            Input.mousePosition,
            null,
            out localPos
        );
    }
    /*
    マウスのクリックはどのレーンを照準しているのかを計算するメソッド。
    x - マウスのx座標位置
    */
    int GetLaneFromPosition(float x)
    {
        float laneWidth = editorWidth/lanes;
        int lane = Mathf.FloorToInt(x / laneWidth);
        
        return Mathf.Clamp(lane,0,lanes-1);
    }
    /*
    マウスのクリックはどの小節の細分を昇順しているのかを計算するメソッド。
    y - マウスのy座標位置
    */
    float GetBeatFromPosition(float y)
    {
        float beatInMeasure = (y/editorHeight)*beatsPerMeasure;
        int currentMeasure = GetCurrentMeasure();
        float beat = currentMeasure * beatsPerMeasure + beatInMeasure;

        return beat;
    }
    /*
    ユーザーが配置しようとしているノートを細分にスナップするメソッド。
    beat - 照準している細分
    */
    float SnapBeat(float beat)
    {
        float snap = 4f / snapDivision;

        return Mathf.Round(beat / snap) * snap;
    }
    /*
    クリックがグリッドの範囲外かどうかを計算するメソッド。範囲外なら返り値が真。
    pos - マウスの座標位置。
    */
    bool ClickOutOfBounds(Vector2 pos)
    {
        return pos.y < 0 || pos.y > editorHeight || pos.x < 0 || pos.x > editorWidth;
    }
    //ノートをマウスの座標位置に配置するメソッド。
    void TryPlaceNote()
    {
        Vector2 pos;

        if(!GetMouseLocalPosition(out pos))
        {
            Debug.Log("GetMouseLocalPosition returned null");
            return;
        }
        pos = new Vector2(pos.x,editorHeight + pos.y);
        if (ClickOutOfBounds(pos))
        {
            Debug.Log("Mouse position out of editor window.");
            return;
        }

        int lane = GetLaneFromPosition(pos.x);
        float beat = GetBeatFromPosition(pos.y);
        beat = SnapBeat(beat);
        EditorNote note = new EditorNote();
        note.lane = lane;
        note.beat = beat;
        Debug.Log(lane);
        Debug.Log(beat);
        notes.Add(note);
    }
    //ノートを削除するメソッド。
    void TryDeleteNote()
    {
        Vector2 pos;

        if (!GetMouseLocalPosition(out pos))
        {
            return;
        }

        pos = new Vector2(pos.x,editorHeight + pos.y);
        if (ClickOutOfBounds(pos))
        {
            Debug.Log("Mouse position out of editor window.");
            return;
        }

        int lane = GetLaneFromPosition(pos.x);
        float beat = GetBeatFromPosition(pos.y);
        float threshold = 0.2f;
        EditorNote target = null;

        foreach(var note in notes)
        {
            if (note.lane != lane)
            {
                continue;
            }

            if (Mathf.Abs(note.beat - beat) < threshold)
            {
                target = note;
                break;
            }
        }

        if (target != null)
        {
            notes.Remove(target);
        }
    }
    /*
    ノートのエディタ内の表示のスプライトを生成するメソッド。
    note - 生成するノートの情報。
    */
    void SpawnPreview(EditorNote note)
    {
        GameObject obj;
        if(note.lane%2==1){
        obj =
            Instantiate(notePrefab_blue, notesRoot);
        }
        else
        {
            obj = Instantiate(notePrefab_white,notesRoot);
        }
        RectTransform rt = obj.GetComponent<RectTransform>();
        float laneWidth = editorWidth / lanes;
        rt.sizeDelta = new Vector2(laneWidth,rt.sizeDelta.y);

        float beatInMeasure = note.beat % beatsPerMeasure;

        float x = note.lane * laneWidth + laneWidth/2;

        float y = (beatInMeasure / beatsPerMeasure) * editorHeight;
        
        rt.anchoredPosition = new Vector2(x,y);
    }
    //キーボード入力を処理するメソッド。
    void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            AddNote(0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddNote(1);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddNote(2);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddNote(3);
        }
    }
    /*
    ノートを生成するメソッド。
    lane - どのレーンに配置する。([0,3])
    */
    void AddNote(int lane)
    {
        float beat = GetCurrentBeat();

        float snapped = Mathf.Round(beat * 4f) / 4f;

        EditorNote note = new EditorNote();

        note.lane = lane;
        note.beat = snapped;

        notes.Add(note);
    }
    //譜面を保存するメソッド。
    public void SaveChart()
    {
        ChartData data = new ChartData();
        data.bpm = bpm;
        data.notes = ConvertNotes();

        string json =
            JsonUtility.ToJson(data, true);

        File.WriteAllText(
            Application.dataPath + "/Resources/Charts/" + saveOutput.text + ".json", json
        );
        Debug.Log("Saved!");
    }
    //譜面データを読み込むメソッド。
    public void LoadChart()
    {
        TextAsset json = Resources.Load<TextAsset>("Charts/" + chartName);
        ChartData data = JsonUtility.FromJson<ChartData>(json.text);
        bpm = data.bpm;
        notes.Clear();
        foreach (var note in data.notes)
        {
            EditorNote newNote = new EditorNote();
            newNote.beat = note.beat;
            newNote.lane = note.lane;
            notes.Add(newNote);
        }
    }
    //ソート機能をノートデータで使えるように作った比較メソッド。
    private static int CompareEditorNotes(EditorNote x, EditorNote y)
    {
        if(x.beat > y.beat)
        {
            return 1;
        }
        else if (x.beat < y.beat)
        {
            return -1;
        }
        else if (x.beat == y.beat)
        {
            return 0;
        }
        return 0;
    }
    //エディターのノートデータを譜面に使われている形にするメソッド。
    List<NoteData> ConvertNotes()
    {
        List<NoteData> result = new List<NoteData>();
        notes.Sort(CompareEditorNotes);
        foreach (var n in notes)
        {
            result.Add(n.toNoteData());
        }
        return result;
    }
}

[System.Serializable]
public class EditorNote
{
    public int lane;
    public float beat;
    //譜面データに使われている形に変換するメソッド。
    public NoteData toNoteData()
    {
        return new NoteData
        (
            this.lane,
            this.beat
        );
    }
}
