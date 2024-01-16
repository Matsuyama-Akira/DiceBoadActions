using UnityEngine;

public class ThisPositionColliderChecker : MonoBehaviour
{
    [SerializeField] DiceBoadManagement _Manager;

    private void Awake()
    {
        _Manager = GameObject.Find("DiceBoadManager").GetComponent<DiceBoadManagement>();
    }

    private void OnTriggerStay(Collider other)
    {
        // 現在いるマスからステータスを取得し、処理を行う
        if(other.gameObject.tag == "Map" & other.gameObject.GetComponent<MoveChecker>() != null)
        {
            MoveChecker status = other.gameObject.GetComponent<MoveChecker>();
            if(status.GetMassStatus() < 3) status.AddClear(_Manager.GetIsMoved());
            status.AddEnemysClear(_Manager.GetLateClaer());
            int massStatus = status.GetMassStatus();
            if(status.GetClear()) _Manager.SetClearMass(status.GetMassNum1(), status.GetMassNum2(), status.GetClear());
            _Manager.CurrentMass(massStatus);
        }
    }
}
