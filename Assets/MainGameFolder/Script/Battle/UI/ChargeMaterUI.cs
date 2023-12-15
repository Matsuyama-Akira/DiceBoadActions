using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChargeMaterUI : MonoBehaviour
{

    //イメージをインスペクターから紐付け。
    [SerializeField] Image meterImage1;
    [SerializeField] Slider ReroadUI;
    [SerializeField] BowAttack bow;
    [SerializeField] Color maxColor;
    [SerializeField] Color baseColor;

    //コルーチンの管理用。
    Coroutine chargeMeter;

    private void Start()
    {
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
        meterImage1.enabled = true;

        while (true)
        {
            meterImage1.fillAmount = bow.MathCharge();
            if (meterImage1.fillAmount == 1)
            {
                meterImage1.color = maxColor;
            }

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