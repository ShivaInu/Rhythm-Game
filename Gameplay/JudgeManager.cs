//判定システムを担当するクラス。
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class JudgeManager : MonoBehaviour
{


    public float judgeLineY = -3.5f;
    public float perfectWindow = 0.3f;
    public float goodWindow = 0.6f;

    public List<Note> activeNotes = new List<Note>();
    public AudioManager audioManager;
    public ScoreManager scoreManager;
    public JudgeEffectController effectController;
    public ParticleSpawner particleSpawner;
    public bool gamePaused;
    public Transform[] laneTransforms;
    private Vector3[] hitEffectSpawnLocations;
    
    void Start()
    {
        hitEffectSpawnLocations = new Vector3[laneTransforms.Length];
        for (int i=0; i < laneTransforms.Length; i++)
        {
            hitEffectSpawnLocations[i] = new Vector3(laneTransforms[i].position.x,judgeLineY,0);
        }
    }
    //ノートをアクティブノートのリストに追加するメソッド。
    public void RegisterNote(Note note)
    {
        activeNotes.Add(note);
    }
    //ノートをアクティブノートのリストから外すメソッド。
    public void RemoveNote(Note note)
    {
        activeNotes.Remove(note);
    }


    void Update()
    {
        if (gamePaused)
        {
            return;
        }
        if (activeNotes.Count != 0)
        {
            Note note = activeNotes[0];
            if (audioManager.CurrentTime() > note.HitTime() + goodWindow)
            {
                effectController.Show("Miss...", Color.red);
                scoreManager.JudgeMiss();
                RemoveNote(note);
            }
        }
    }
    /*
    ノートの判定をするメソッド。
    lane - どのレーンの判定を行う ([0,3])
    */
    public void CheckLane(int lane)
    {
        var laneNotes = activeNotes
            .Where(n => n.Lane == lane)
            .OrderBy(n => Mathf.Abs((float)(audioManager.CurrentTime()-n.HitTime())))
            .ToList();

        if (laneNotes.Count() == 0)
        {
            return;
        }

        Note target = laneNotes[0];

        double diff = System.Math.Abs(audioManager.CurrentTime() - target.HitTime());

        //Perfectのタイミング猶予内ならPerfect
        if (diff <= perfectWindow)
        {
            effectController.Show("Perfect!",Color.green);
            audioManager.PlayPerfect();
            scoreManager.JudgePerfect();
            particleSpawner.Spawn(hitEffectSpawnLocations[lane],Color.green); //判定演出
            DestroyNote(target);
        }
        else if (diff <= goodWindow) {
            effectController.Show("Good",Color.yellow);
            audioManager.PlayGood();
            particleSpawner.Spawn(hitEffectSpawnLocations[lane],Color.yellow);
            scoreManager.JudgeGood();
            DestroyNote(target);
        }
        else
        {
            Debug.Log("Too Early / Late");
        }
    }

    /*
    判定済みのノートを削除するメソッド。
    note - 削除するノート。
    */
    public void DestroyNote(Note note)
    {
        RemoveNote(note);
        Destroy(note.gameObject);
    }
}
