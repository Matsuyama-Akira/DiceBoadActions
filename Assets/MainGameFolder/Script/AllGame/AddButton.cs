using UnityEngine;
using DG.Tweening;
using AllGameManager;

public class AddButton : Button
{
    public enum NowScene
    {
        Title, WeponSellect, DiceBoad, Battle, Result, OperationSetting,
    }
    public enum SENum
    {
        T_GoWS = 0, T_GoOS = 1, WS_WeponChoise = 0, WS_NextScene = 1, DB_WeponChoise = 0, RS_NextScene = 0, OS_KeyInput = 0, OS_SetKey = 1, OS_BackScene = 2,
    }

    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    float onPointerEnterScale = 1;
    [SerializeField]
    float onPointerDownScale = 1;
    [SerializeField]
    float duration = 0.1f;
    [SerializeField]
    Ease ease = Ease.OutQuad;

    [SerializeField]
    private NowScene now;
    [SerializeField]
    SENum SENumber;

    private AllGameSEManager SEManager;

    private void Awake()
    {
        SEManager = GameObject.FindWithTag("GameManager").GetComponent<AllGameSEManager>();
    }

    protected override void AdditionalOnPointerEnterProcess()
    {
        ChangeButtonScale(onPointerEnterScale);
    }

    protected override void AdditionalOnPointerExitProcess()
    {
        ChangeButtonScale(1);
    }

    protected override void AdditionalOnPointerDownProcess()
    {
        ChangeButtonScale(onPointerDownScale);
        if(SEManager.ButtonSESource != null)
        {
            switch (now)
            {
                case NowScene.Title:
                    if (SEManager.TitleButtonSE[(int)SENumber])
                        SEManager.ButtonSESource.PlayOneShot(SEManager.TitleButtonSE[(int)SENumber]);
                    break;
                case NowScene.WeponSellect:
                    if (SEManager.WeponSellectButtonSE[(int)SENumber])
                        SEManager.ButtonSESource.PlayOneShot(SEManager.WeponSellectButtonSE[(int)SENumber]);
                    break;
                case NowScene.DiceBoad:
                    if (SEManager.DB_AnySE[(int)SENumber])
                        SEManager.ButtonSESource.PlayOneShot(SEManager.DB_AnySE[(int)SENumber]);
                    break;
                case NowScene.Battle:
                    break;
                case NowScene.Result:
                    if (SEManager.ResultButtonSE[(int)SENumber])
                        SEManager.ButtonSESource.PlayOneShot(SEManager.ResultButtonSE[(int)SENumber]);
                    break;
                case NowScene.OperationSetting:
                    if (SEManager.OperationSettingButtonSE[(int)SENumber])
                        SEManager.ButtonSESource.PlayOneShot(SEManager.OperationSettingButtonSE[(int)SENumber]);
                    break;
            }
        }
    }

    protected override void AdditionalOnPointerUpProcess()
    {
        ChangeButtonScale(1);
    }

    void ChangeButtonScale(float scale)
    {
        rectTransform.DOScale(Vector3.one * scale, duration).SetLink(gameObject).SetEase(ease).SetUpdate(true);
    }

}