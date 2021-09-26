using UnityEngine;
using UnityEngine.UI;

public class CUIInAppPurchaseSingleItemCell : CUIInAppPurchaseItemCell
{
    public GameObject ins_objRedDot;
    public Text ins_txtPurchaseName;
    public Text ins_txtPaidCurrencyInfo;
    public Text ins_txtLimitCount;

    public override void SetData(CUIInAppShopModel cAppShopModel)
    {
        base.SetData(cAppShopModel);

        CDataInAppOne cDataOne = cAppShopModel.m_CDataInAppOne;
        ins_imgIcon.sprite = CAssetBundleManager.In.LoadAssetFromDic<Sprite>(CDataGameInfo.M_strABNameUI, cDataOne.GetBaseIconID());
        ins_txtPurchaseName.text = CLanguageManager.In.GetText(CLanguageManager.EmKind.Std, string.Format(CDataGameInfo.M_strKeyInAppPurchaseName, cDataOne.GetStdID()));
        ins_txtLimitCount.text = GetLimitCount(cAppShopModel.m_CDataInAppOne);

        //무료상품 레드닷.
        ins_objRedDot.SetActive(cDataOne.IsFreePurchaseBuyState());

        //구매횟수.
        if (!cDataOne.IsLimitType())
            ins_txtPaidCurrencyInfo.text = string.Empty;
        else
            CLanguageManager.In.SetText(CLanguageManager.EmKind.Std, ins_txtPaidCurrencyInfo, CDataGameInfo.M_strKeyInAppPurchaseLimitType + cDataOne.GetLimitType(), cDataOne.GetCurrentLimitCount(), cDataOne.GetLimitCount());

        SetDiscountInfo(cDataOne);

        AISoldoutState(cAppShopModel);
    }

    protected override void SetDiscountInfo(CDataInAppOne cDataOne)
    {
        base.SetDiscountInfo(cDataOne);
    }

    protected override string GetLimitCount(CDataInAppOne cDataOne)
    {
        if (cDataOne.IsLimitCount())
            return CLanguageManager.In.GetText(CLanguageManager.EmKind.Std, CDataGameInfo.M_strKeyInAppPurchaseLimitType + cDataOne.GetLimitType(), cDataOne.GetCurrentLimitCount(), cDataOne.GetLimitCount());
        else
            return string.Empty;
    }

    protected override void AISoldoutState(CUIInAppShopModel cAppShopModel)
    {
        if (!cAppShopModel.m_CDataInAppOne.IsCurrentLimitCount() && cAppShopModel.m_CDataInAppOne.IsLimitCount())
        {
            CLanguageManager.In.SetText(CLanguageManager.EmKind.Std, ins_txtCurrency, CDataGameInfo.M_strKeyInAppPurchaseItemDouble + CDataGameInfo.GetNumStr(2));
            ins_objDiscountPriceGroup.SetActive(false);
            ins_txtCurrency.gameObject.SetActive(true);
            ins_objBuyedMask.SetActive(true);
        }
        else
        {
            ins_objBuyedMask.SetActive(false);
        }
    }
}
