//ノートの判定パーティクル効果を生成するクラス。
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject hitEffectPrefab;  
    //パーティクル効果を生成するメソッド。
    //position - 生成する位置
    //color - パーティクル効果の色
    public void Spawn(Vector3 position, Color color)
    {
        GameObject obj =
            Instantiate(hitEffectPrefab, position, Quaternion.identity);
        var particleSystem = obj.GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.startColor = color;
        particleSystem.Play();
        Destroy(obj,1f);
    }

}
