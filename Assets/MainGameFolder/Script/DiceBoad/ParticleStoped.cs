using UnityEngine;

public class ParticleStoped : MonoBehaviour
{
    private DiceBoadManagement manager;
    private void Awake()
    {
        manager = GameObject.Find("DiceBoadManager").GetComponent<DiceBoadManagement>();
    }
    void OnParticleSystemStopped()
    {
        // マネージャーに処理を渡す
        manager.SetHealing(false);
        manager.SetLateClear(true);

        // 2秒後にオブジェクトを破棄
        Destroy(gameObject, 2f);
    }
}
