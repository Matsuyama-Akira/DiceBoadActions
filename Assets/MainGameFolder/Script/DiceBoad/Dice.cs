using UnityEngine;

public class Dice : MonoBehaviour
{
    /// <summary> 親オブジェクトのRigidbody </summary>
    private Rigidbody parent_RB;

    /// <summary> 静止しているか </summary> 
    bool Stoping;
    /// <summary> サイコロの結果 </summary> 
    int Result;

    /// <summary> サイコロを振る時のランダムな回転値 </summary> 
    Vector3 randomRotate;
    /// <summary> 振った時間を0とする </summary> 
    float startTime;
    /// <summary> 経過時間 </summary> 
    float nowTime;
    /// <summary> Raycast用の距離 </summary> 
    float distance = 1;

    private void Awake()
    {
        // 変数の初期化
        startTime = Time.time;
        Stoping = false;

        // 参照と変数のセットアップ
        parent_RB = GetComponentInParent<Rigidbody>();
        randomRotate = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
        transform.parent.Rotate(randomRotate);
    }

    private void Update()
    {
        // 経過時間の計測
        nowTime = Time.time - startTime;

        // 経過時間が1秒以内なら回転をかける
        if (nowTime <= 1) transform.parent.Rotate(randomRotate);

        // このオブジェクトの回転をリセットし、位置を親オブジェクトの中心になるように動かす
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.position = gameObject.transform.parent.position + Vector3.up;

        // Raycastの計測
        DiceRoal();

        // 親オブジェクトのRigidbodyが静止したか
        if (parent_RB.IsSleeping()) Stoping = true;
    }

    void DiceRoal()
    {
        // Rayを生成
        Ray ray = new Ray(transform.position, Vector3.down);

        // 生成したRayを元に計測
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            // 衝突した対象の名前を取得し、名前毎に結果を挿入する
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

    /// <returns> 静止したか </returns>
    public bool GetIsStoping() { return Stoping; }
    /// <returns> サイコロの結果 </returns>
    public int GetResult() {return Result; }
}
