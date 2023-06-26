using UnityEngine;

public class Dice : MonoBehaviour
{
    private DiceBoadManagement manager;
    private Rigidbody parent_RB;
    bool Stoping;
    int Result;

    Vector3 randomRotate;
    float startTime;
    float time;
    float distance = 1;
    float duration = 1;

    private void Awake()
    {
        startTime = Time.time;
        manager = GameObject.Find("DiceBoadManager").GetComponent<DiceBoadManagement>();
        parent_RB = GetComponentInParent<Rigidbody>();
        randomRotate = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
        transform.parent.Rotate(randomRotate);
        Stoping = false;
    }

    private void Update()
    {
        time = Time.time - startTime;
        if (time <= 1) transform.parent.Rotate(randomRotate);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.position = gameObject.transform.parent.position + Vector3.up;
        DiceRoal();
        if (parent_RB.IsSleeping()) Stoping = true;
    }

    void DiceRoal()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        //Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            string name = hit.collider.gameObject.name;
            switch (name)
            {
                case "Col_1":
                    Result = 1; break;
                case "Col_2":
                    Result = 2; break;
                case "Col_3":
                    Result = 3; break;
                case "Col_4":
                    Result = 4; break;
                case "Col_5":
                    Result = 5; break;
                case "Col_6":
                    Result = 6; break;
            }
        }
    }

    public bool GetIsStoping() { return Stoping; }
    public int GetResult() {return Result; }
}
