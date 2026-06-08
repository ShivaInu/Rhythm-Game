//ノートを正しいタイミングで生成するクラス。
using UnityEngine;
using System.Collections.Generic;
public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab_blue;
    public GameObject notePrefab_white;
    [SerializeField] Transform notesRoot;
    [SerializeField] JudgeManager judgeManager;
    [SerializeField] AudioManager audioManager;

    public Transform[] laneSpawnPositions;
    public Transform noteDeleterPos;
    public float spawnY = 6f;
    public float judgeY = -3.5f;
    public float travelTime = 2f;
    private int noteIndex;
    private float bpm;
    private List<NoteData> notes;

    public void Initialize(ChartData chart)
    {
        bpm = chart.bpm;
        notes = chart.notes;
        noteIndex = 0;
        GameObject settingsObject = GameObject.FindGameObjectWithTag("Settings");
        SettingContainer settings = settingsObject.GetComponent<SettingContainer>();
        travelTime = settings.travelTime;
        float distancePerSecond = (spawnY - judgeY) / travelTime;
        noteDeleterPos.position = new Vector3(noteDeleterPos.position.x,judgeManager.judgeLineY-((distancePerSecond*2f)*judgeManager.goodWindow),noteDeleterPos.position.z);
    }

    void Update()
    {
        if (notes == null || noteIndex >= notes.Count)
            return;
        
        double currentTime = audioManager.CurrentTime();

        //タイミングが来たノートを生成し次のノートの生成の準備をする
        while (noteIndex < notes.Count && (notes[noteIndex].beat*(60f/bpm)) <= currentTime + travelTime)
        {
            Spawn(notes[noteIndex]);
            noteIndex++;
        }
    }

    /*
    ノートを生成するメソッド。
    data - ノートの情報。
    */
    void Spawn(NoteData data)
    {
        int lane = data.lane;
        Vector3 pos = laneSpawnPositions[lane].position;
        pos.y = spawnY;
    GameObject noteObj;
    if(lane%2==0){
        noteObj =
            Instantiate(notePrefab_white, pos,
                Quaternion.identity, notesRoot);
    }
    else{
        noteObj = 
            Instantiate(notePrefab_blue, pos,
                Quaternion.identity, notesRoot);
    }
        Note note = noteObj.GetComponent<Note>();

        note.Initialize(
            (data.beat*(60f/bpm)),
            travelTime,
            spawnY,
            judgeY,
            lane,
            audioManager
        );

        judgeManager.RegisterNote(note);
    }
}
