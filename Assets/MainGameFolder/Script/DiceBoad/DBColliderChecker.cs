using UnityEngine;

public class DBColliderChecker : MonoBehaviour
{
    [SerializeField] private DiceBoadManagement _Manager;
    /// <summary> コライダー衝突判定 </summary>
    private bool colliderHit;

    private void Awake()
    {
        _Manager = GameObject.Find("DiceBoadManager").GetComponent<DiceBoadManagement>();
    }

    private void FixedUpdate()
    {
        // コライダーが衝突していない場合、マネージャーにfalseを渡す
        if (colliderHit == false)
        {
            switch (gameObject.name)
            {
                case "Col_F": _Manager.SetMoveCheck(0, false); break;
                case "Col_B": _Manager.SetMoveCheck(1, false); break;
                case "Col_R": _Manager.SetMoveCheck(2, false); break;
                case "Col_L": _Manager.SetMoveCheck(3, false); break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // コライダーがマップのマスに衝突しているならば
        if (other.gameObject.tag == "Map" & other.gameObject.GetComponent<MoveChecker>() != null)
        {
            // コライダー衝突判定をtrueにする
            colliderHit = true;

            // マスのステータスを取得して、boolとtransformを渡す
            MoveChecker status = other.gameObject.GetComponent<MoveChecker>();
            bool moveCheck = status.GetMoveCheck();
            switch (gameObject.name)
            {
                case "Col_F": _Manager.SetMoveCheck(0, moveCheck); _Manager.SetNextMass(0, other.transform); break;
                case "Col_B": _Manager.SetMoveCheck(1, moveCheck); _Manager.SetNextMass(1, other.transform); break;
                case "Col_R": _Manager.SetMoveCheck(2, moveCheck); _Manager.SetNextMass(2, other.transform); break;
                case "Col_L": _Manager.SetMoveCheck(3, moveCheck); _Manager.SetNextMass(3, other.transform); break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // コライダーが衝突していない場合は衝突判定をfalseにする
        if (other.gameObject.tag == "Map" & other.gameObject.GetComponent<MoveChecker>() != null)
        {
            colliderHit = false;
        }
    }
}
