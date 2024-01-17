using UnityEngine;

[ExecuteInEditMode]
public class MoveChecker : MonoBehaviour
{
    /// <summary> マスのステータス </summary>
    public enum Status
    {
        Normal = 0, Items = 1, Heal = 2, Enemy1 = 3, Enemy2 = 4, Boss = 5,
    }

    /// <summary> 宝箱のレアリティ </summary>
    public enum ItemLevel
    {
        Common = 0, Rare = 1, Unique = 2, Legend = 3,
    }

    /// <summary> すごろくのマネージャー </summary>
    private DiceBoadManagement manager;

    [Header("Mat slot")]
    [SerializeField, Tooltip("進めるマスの色")] Material OKMat;
    [SerializeField, Tooltip("進めないマスの色")] Material NGMat;

    [Space, Header("Status of model")]
    [SerializeField, Tooltip("救急キットのオブジェクト")] GameObject HealthKit;
    [SerializeField, Tooltip("宝箱のレアリティ毎のオブジェクト"), NamedArray(new string[] { "Common", "Rare", "Unique", "Legend" })]
    GameObject[] ItemBox;
    [SerializeField, Tooltip("敵のレベル毎のオブジェクト"), NamedArray(new string[] { "Enemy1", "Enemy2", "Boss" })]
    GameObject[] Enemys;

    //Instance
    [SerializeField, Tooltip("救急キットのインスタンス")] GameObject _healthKit;
    [SerializeField, Tooltip("宝箱のインスタンス")] GameObject _itemBox;
    [SerializeField, Tooltip("敵オブジェクトのインスタンス")] GameObject _enemys;
    [SerializeField, Tooltip("宝箱と敵オブジェクトのインスタンスのアニメーター")] Animator animator;
    /// <summary> アニメーターで再生しているアニメーションの名前 </summary>
    AnimatorClipInfo[] clipInfos;
    /// <summary> アニメーションがnull </summary>
    bool isNullAnim;

    [Space, Header("Mass status")]
    [SerializeField, Tooltip("このマスに進めるか")] bool moveCheck;
    [SerializeField, Tooltip("マスのX軸のナンバー")] int massNum1;
    [SerializeField, Tooltip("マスのY軸のナンバー")] int massNum2;
    [SerializeField, Tooltip("このマスの現在のステータス")] Status status;
    [SerializeField, Tooltip("このマスの一つ前のステータス")] Status lateStatus;
    [SerializeField, Tooltip("ステータスが変化したか")] private bool statusSwitch;
    [SerializeField, Tooltip("宝箱のレアリティ")] ItemLevel item;
    [SerializeField, Tooltip("レアリティが変化したか")] private bool itemLevelSwitch;
    /// <summary> 一つ前のレアリティ </summary>
    private ItemLevel lateItem;
    [SerializeField, Tooltip("クリアしたか")] bool clear;
    [SerializeField, Tooltip("敵マスとしてクリアした時に処理を遅らせるための変数")] bool lateClear;
    [SerializeField, Tooltip("過去にクリアしたか")] bool cleared;

    [Space, Header("Instance rotation")]
    [SerializeField, Tooltip("生成するインスタンスに掛ける回転")] Quaternion instanceRotation;
    [Header("Items transform")]
    /// <summary> 救急キットのインスタンス生成位置調整 </summary>
    private Vector3 healPosition = new Vector3(0, 1, 0);
    [SerializeField, Tooltip("救急キットのインスタンス生成に掛ける回転")] Quaternion healRotation;

    private void Awake()
    {
        // マネージャーの取得
        manager = GameObject.Find("DiceBoadManager").GetComponent<DiceBoadManagement>();

        // マスのクリア情報を取得
        clear = false;
        cleared = manager.GetClearMass(massNum1, massNum2);
        Clear();
    }

    private void Update()
    {
        // 
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
                        if(instanceRotation == new Quaternion(0, 0, 0, 0)) _healthKit = Instantiate(HealthKit, transform.position + healPosition, transform.rotation * healRotation);
                        else _healthKit = Instantiate(HealthKit, transform.position + healPosition, transform.rotation * instanceRotation * healRotation);
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
        if (cleared)
        {
            ResetInstance();
            clear = true;
            status = Status.Normal;
        }
        /* 
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
        */
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