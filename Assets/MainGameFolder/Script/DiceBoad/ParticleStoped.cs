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
        manager.SetHealing(false);
        manager.SetLateClear(true);
        Destroy(gameObject, 2f);
    }
}
