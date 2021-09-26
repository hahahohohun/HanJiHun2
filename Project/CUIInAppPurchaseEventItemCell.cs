using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUIInAppPurchaseEventItemCell : CUIInAppPurchaseItemCell
{
    public override void SetData(CUIInAppShopModel cAppShopModel)
    {
        base.SetData(cAppShopModel);

        CDataInAppOne cDataOne = cAppShopModel.m_CDataInAppOne;
        //다국어 이미지.
        CSpriteImgInfo cSpriteImgInfo = CUIManager.In.ins_scrPopupInAppShop.m_scrSpriteLanguageImgInfo;
        ins_imgIcon.sprite = cSpriteImgInfo.GetSprite(cDataOne.GetLanguageIconID());

        SetDiscountInfo(cDataOne);
        //데이터 셋팅 후 솔드아웃은 마지막에.
        AISoldoutState(cAppShopModel);
    }

    protected override void SetDiscountInfo(CDataInAppOne cDataOne)
    {
        base.SetDiscountInfo(cDataOne);
    }

    protected override string GetLimitCount(CDataInAppOne cDataOne)
    {
        if (!cDataOne.IsLimitType())
            return string.Empty;
        return CLanguageManager.In.GetText(CLanguageManager.EmKind.Std, CDataGameInfo.M_strKeyInAppPurchaseLimitType + cDataOne.GetLimitType(), cDataOne.GetCurrentLimitCount(), cDataOne.GetLimitCount());
    }

    protected override void AISoldoutState(CUIInAppShopModel cAppShopModel)
    {
        if (!cAppShopModel.m_CDataInAppOne.IsCurrentLimitCount())
        {
            ins_txtEventStampDes.text = "SOLD OUT";
            ins_objBuyedMask.SetActive(true);
            ins_objEventStamp.SetActive(true);
            ins_btn.enabled = false;

            //구매완료.
            CLanguageManager.In.SetText(CLanguageManager.EmKind.Std, ins_txtCurrency, CDataGameInfo.M_strKeyInAppPurchaseItemDouble + CDataGameInfo.GetNumStr(2));
        }
        else
        {
            ins_objBuyedMask.SetActive(false);
            ins_btn.enabled = true;
        }
    }
}
