using UnityEngine;

public class SingletonCheck : SingletonMonoBehaviour<SingletonCheck>
{
}

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // この変数に入れられたオブジェクトはシングルトンとして利用する
    private static T instance;
    // オブジェクトロード時にシングルトンの形成を行う
    public static T Instance
    {
        get
        {
            // このシーンにシングルトンのオブジェクトが存在しないなら
            if (instance == null)
            {
                // このオブジェクトをシングルトンのオブジェクトにする
                instance = (T)FindObjectOfType(typeof(T));

                // エラー回避
                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
            }
            return instance;
        }
    }

    protected void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        // このオブジェクトがシングルトンであれば何もせずreturnする
        if (this == Instance) { return true; }

        // シングルトンでなければ破棄する
        Destroy(this.gameObject);
        return false;
    }
}
