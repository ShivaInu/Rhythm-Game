//ゲーム内のノートを定義するクラス。
using UnityEngine;

public class Note : MonoBehaviour
{
    private double hitTime;
    private float travelTime;
    private float spawnY;
    private float judgeY;

    public int Lane { get; private set; }

    private AudioManager audioManager;
    public void Initialize(double targetTime, float travelDuration, 
    float startY, float targetY, int laneIndex, AudioManager manager)
    {
        hitTime = targetTime;
        travelTime = travelDuration;
        spawnY = startY;
        judgeY = targetY;
        Lane = laneIndex;
        audioManager = manager;
    }

    //曲の再生とともに落ちる。
    void FixedUpdate()
    {
        double currentTime = audioManager.CurrentTime();
        double timeUntilHit = hitTime - currentTime;
        float distancePerSecond = (spawnY - judgeY) / travelTime; //落下スピードを計算する

        float y = judgeY + (float)(timeUntilHit * distancePerSecond);

        transform.position = new Vector3(
            transform.position.x,
            y,
            0
        );
    }

    //ノートの正しい判定タイミングを返り値で返すメソッド。
    public double HitTime()
    {
        return hitTime;
    }

}
