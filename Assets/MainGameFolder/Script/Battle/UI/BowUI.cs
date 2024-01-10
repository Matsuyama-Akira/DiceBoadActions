using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BowUI : MonoBehaviour
{
    // 画像とスライダー
    [SerializeField] Image meterImage1;
    [SerializeField] Slider ReroadUI;

    // 対応スクリプト
    [SerializeField] BowAttack bow;

    // チャージ中のカラー
    [SerializeField] Color maxColor;
    [SerializeField] Color baseColor;

    //コルーチンの管理用。
    Coroutine chargeMeter;

    private void Start()
    {
        // 初期化とセットアップ
        meterImage1.enabled = false;
        meterImage1.color = baseColor;
        if(GameObject.Find("Player_Bow(Clone)")) bow = GameObject.Find("Player_Bow(Clone)").GetComponent<BowAttack>();
    }

    public void ReroadingUI(float reroadTime)
    {
        ReroadUI.value = reroadTime;
    }

    //円形のパワーメーターを開始したい時に呼ぶ。
    public void StartMeterRadial()
    {
        chargeMeter = StartCoroutine("MeterRadial");
    }


    //円形
    IEnumerator MeterRadial()
    {
        // メーターを表示
        meterImage1.enabled = true;

        // メーターの数値を計算し出力
        while (true)
        {
            // チャージ量を取得(0~1)
            meterImage1.fillAmount = bow.MathCharge();
            // チャージ量が最大になったら色を変更
            if (meterImage1.fillAmount == 1)
            {
                meterImage1.color = maxColor;
            }

            // チャージを解いたら初期化してメーターを非表示に
            if (!bow.isCharging)
            {
                meterImage1.color = baseColor;
                meterImage1.fillAmount = 0;
                meterImage1.enabled = false;
                yield break;
            }

            yield return null;
        }
    }

}