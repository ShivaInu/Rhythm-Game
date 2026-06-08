//ミスされたノーツを削除するクラス。
using UnityEngine;

public class GarbageCollection : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("note deleted");
        Destroy(col.gameObject);
    }
}
