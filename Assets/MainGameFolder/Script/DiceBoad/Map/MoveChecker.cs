using UnityEngine;

[ExecuteInEditMode]
public class MoveChecker : MonoBehaviour
{
    public enum Status
    {
        Normal = 0, Items = 1, Heal = 2, Enemy1 = 3, Enemy2 = 4, Boss = 5,
    }

    public enum ItemLevel
    {
        Common = 0, Rare = 1, Unique = 2, Legend = 3,
    }

    private DiceBoadManagement manager;

    [Header("Mat slot")]
    [SerializeField] Material OKMat;
    [SerializeField] Material NGMat;

    [Space, Header("Status of model")]
    [SerializeField] GameObject HealthKit;
    [SerializeField, NamedArray(new string[] { "Common", "Rare", "Unique", "Legend"})]
    GameObject[] ItemBox;
    [SerializeField, NamedArray(new string[] { "Enemy1", "Enemy2", "Boss" })]
    GameObject[] Enemys;

    //Instance
    [SerializeField] GameObject _healthKit;
    [SerializeField] GameObject _itemBox;
    [SerializeField] GameObject _enemys;
    [SerializeField] Animator animator;
    AnimatorClipInfo[] clipInfos;
    bool isNullAnim;

    [Space, Header("Mass status")]
    [SerializeField] bool moveCheck;
    [SerializeField] int massNum1;
    [SerializeField] int massNum2;
    [SerializeField] Status status;
    [SerializeField] Status lateStatus;
    [SerializeField] private bool statusSwitch;
    [SerializeField] ItemLevel item;
    [SerializeField] private bool itemLevelSwitch;
    private ItemLevel lateItem;
    [SerializeField] bool clear;
    [SerializeField] bool lateClear;
    [SerializeField] bool cleared;
    [Header("Instance rotation")]
    [SerializeField] Quaternion instanceRotation;

    private void Awake()
    {
        manager = GameObject.Find("DiceBoadManager").GetComponent<DiceBoadManagement>();
        clear = false;
        cleared = manager.GetClearMass(massNum1, massNum2);
    }

    private void Update()
    {
        Clear();
        if (moveCheck) gameObject.GetComponent<Renderer>().material = OKMat;
        else gameObject.GetComponent<Renderer>().material = NGMat;
        if(_enemys != null & _healthKit != null & _itemBox != null) SetInstance();

        if (!Application.isPlaying)
        {
            statusSwitch = status != lateStatus;
            itemLevelSwitch = item != lateItem;
            lateStatus = status;
            lateItem = item;

            if (statusSwitch | itemLevelSwitch)
            {
                ResetInstance();
                switch (status)
                {
                    case Status.Normal:
                        break;
                    case Status.Items:
                        _itemBox = Instantiate(ItemBox[(int)item], transform.position, transform.rotation * instanceRotation);
                        _itemBox.transform.parent = this.transform;
                        animator = _itemBox.GetComponent<Animator>();
                        break;
                    case Status.Heal:
                        _healthKit = Instantiate(HealthKit, transform.position, transform.rotation * instanceRotation);
                        _healthKit.transform.parent = this.transform;
                        break;
                    case Status.Enemy1:
                        _enemys = Instantiate(Enemys[0], transform.position, transform.rotation * instanceRotation);
                        _enemys.transform.parent = this.transform;
                        animator = _enemys.GetComponent<Animator>();
                        break;
                    case Status.Enemy2:
                        _enemys = Instantiate(Enemys[1], transform.position, transform.rotation * instanceRotation);
                        _enemys.transform.parent = this.transform;
                        animator = _enemys.GetComponent<Animator>();
                        break;
                    case Status.Boss:
                        _enemys = Instantiate(Enemys[2], transform.position, transform.rotation * instanceRotation);
                        _enemys.transform.parent = this.transform;
                        animator = _enemys.GetComponent<Animator>();
                        break;
                }
            }
        }
        Math();
    }

    void Math()
    {
        if (Application.isPlaying)
        {
            if ((status == Status.Enemy1 | status == Status.Enemy2 | status == Status.Boss) & _enemys != null)
            {
                if (lateClear)
                {
                    animator.SetBool("IsDie", lateClear);
                    clipInfos = animator.GetCurrentAnimatorClipInfo(1);
                    isNullAnim = clipInfos.Length == 0;
                    if (isNullAnim)
                    {
                        clear = true;
                        manager.SetEnemyDown(false);
                        manager.AddEventMass();
                        ResetInstance();
                    }
                }
            }
            if(status == Status.Items & _itemBox != null)
            {
                animator.SetBool("Open", clear);
                clipInfos = animator.GetCurrentAnimatorClipInfo(0);
                isNullAnim = clipInfos.Length == 0;
                if (isNullAnim)
                {
                    int randomWepon = Random.Range(0, 5), randomLevel = Random.Range(0, 3);
                    if (randomWepon == 1) randomWepon = 0;
                    if (randomWepon > 2) randomWepon = 2;
                    manager.SetNewWepons(randomWepon, randomLevel);
                    manager.AddEventMass();
                    ResetInstance();
                }
            }
            if(status == Status.Heal & _healthKit != null)
            {
                if(clear & lateClear)
                {
                    manager.AddEventMass();
                    ResetInstance();
                }
            }
        }

        if (!gameObject.HasChild())
        {
            status = Status.Normal;
        }
    }

    void SetStatus()
    {
        foreach(Transform c in transform)
        {
            string tag = c.gameObject.tag.ToString();
            switch (tag)
            {
                case "DB_Treasure": status = Status.Items; break;
                case "DB_Health": status = Status.Heal; break;
                case "DB_Enemy1": status = Status.Enemy1; break;
                case "DB_Enemy2": status = Status.Enemy2; break;
                case "DB_Enemy_Boss": status = Status.Boss; break;
                default: status = Status.Normal; break;
            }
        }
    }

    void SetInstance()
    {
        foreach (Transform c in transform)
        {
            switch (status)
            {
                case Status.Normal:
                    break;
                case Status.Items:
                    _itemBox = c.gameObject;
                    animator = _itemBox.GetComponent<Animator>();
                    break;
                case Status.Heal:
                    _healthKit = c.gameObject;
                    break;
                case Status.Enemy1:
                    _enemys = c.gameObject;
                    animator = _enemys.GetComponent<Animator>();
                    break;
                case Status.Enemy2:
                    _enemys = c.gameObject;
                    animator = _enemys.GetComponent<Animator>();
                    break;
                case Status.Boss:
                    _enemys = c.gameObject;
                    animator = _enemys.GetComponent<Animator>();
                    break;
            }
        }
    }

    void Clear()
    {
        switch (status)
        {
            case Status.Items:
                if (cleared) ResetInstance();
                break;
            case Status.Heal:
                if (cleared) ResetInstance();
                break;
            case Status.Enemy1:
                if (cleared) ResetInstance();
                break;
            case Status.Enemy2:
                if (cleared) ResetInstance();
                break;
        }
    }

    void ResetInstance()
    {
        foreach (Transform c in transform)
        {
            DestroyObject(c.gameObject);
        }
    }

    void DestroyObject(GameObject myObject)
    {
        if (myObject != null)
        {
            if (Application.isPlaying)
            {
                Destroy(myObject);
            }
            else
            {
                DestroyImmediate(myObject);
            }
        }
    }

    public void AddClear(bool addClear) { clear = addClear; }
    public void AddEnemysClear(bool addLateClear) { lateClear = addLateClear; }

    public bool GetMoveCheck() { return moveCheck; }
    public int GetMassStatus() { return (int)status; }
    public int GetItemLevel() { return (int)item; }
    public bool GetClear() { return clear; }
    public int GetMassNum1() { return massNum1; }
    public int GetMassNum2() { return massNum2; }
}

public static partial class GameObjectExtensions
{
    public static bool HasChild(this GameObject gameObject)
    {
        return 0 < gameObject.transform.childCount;
    }
}