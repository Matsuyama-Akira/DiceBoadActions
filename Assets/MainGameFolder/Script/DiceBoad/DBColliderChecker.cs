using UnityEngine;

public class DBColliderChecker : MonoBehaviour
{
    [SerializeField] private DiceBoadManagement _Manager;

    private void Awake()
    {
        _Manager = GameObject.Find("DiceBoadManager").GetComponent<DiceBoadManagement>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Map" & other.gameObject.GetComponent<MoveChecker>() != null)
        {
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
}
