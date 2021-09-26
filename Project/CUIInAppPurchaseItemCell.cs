using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CUIInAppPurchaseItemCell : MonoBehaviour
{
    public Image ins_imgIcon;
    public Text ins_txtCurrency;
    public Button ins_btn;
    public GameObject ins_objBuyedMask;
    protected Coroutine _CorTimer = null;

    //추가정보.

    //첫구매.
    public GameObject ins_objFirstPurchase = null;
    public Text ins_txtFirstPurchase = null;
    public Text ins_txtFirstValue = null; //추가로 얼마나 줄지.

    public GameObject ins_objEventStamp; //효율정보 or Sold아웃문구.
    public Text ins_txtEventStampDes;

    //할인.
    public GameObject ins_objDiscount = null;
    public Text ins_txtDiscount = null;

    //할인 금액정보.
    [Header("[할인하는 경우]")]
    public GameObject ins_objDiscountPriceGroup = null;
    public Text ins_txtOriginPrice = null;
    public Text ins_txtDiscountPrice = null;

    public virtual void SetData(CUIInAppShopModel cAppShopModel)
    {
        CDataInAppOne cDataOne = cAppShopModel.m_CDataInAppOne;

        //추가 설명사용 && 첫구매는 효율표시 안함. 
        bool bSubDescription = cDataOne.IsUseSubDescription() && !cDataOne.IsDoubleCount();
        if (bSubDescription)
        { 
            ins_txtEventStampDes.text = cAppShopModel.m_CDataInAppOne.GetStrEfficiency();
        }
        ins_objEventStamp.SetActive(bSubDescription);

        ins_txtCurrency.text = cDataOne.GetStrPurchasePrice();

        //첫구매.
        if (cAppShopModel.m_CDataInAppOne.IsDoubleCount())
        {
            CLanguageManager.In.SetText(CLanguageManager.EmKind.Std, ins_txtFirstPurchase, CDataGameInfo.M_strKeyInAppPurchaseItemDouble);
            ins_txtFirstValue.text = cAppShopModel.m_CDataInAppOne.GetStrFirstGemPayed();
        }
        ins_objFirstPurchase.SetActive(cAppShopModel.m_CDataInAppOne.IsDoubleCount());

        ins_btn.onClick.SetListener(() =>
        {
            CSoundManager.In.PlayOneShotsEffect(CSoundManager.EmEffect.BtnClick);
            OpenPurchaseInfo(cDataOne);
        });


        //즉시 오픈 상품.
        if (CUIManager.In.ins_scrPopupInAppShop.IsImmediatelyOpen(cDataOne))
        {
            OpenPurchaseInfo(cDataOne, () => CUIManager.In.ins_scrPopupInAppShop.ResetEnterInAppLink());
        }
    }

    private void OpenPurchaseInfo(CDataInAppOne cDataOne, CFunc.OnVoidFunc onVoidFunc = null)
    {
        CUIManager.In.ins_scrPopupInAppShop.OpenPurchaseInfo(cDataOne, onVoidFunc);
    }

    protected abstract string GetLimitCount(CDataInAppOne cDataOne);
    protected virtual void AISoldoutState(CUIInAppShopModel cAppShopModel) { }
    protected virtual void SetDiscountInfo(CDataInAppOne cDataOne)
    {
        bool bDisCount = cDataOne.IsDiscountPercent();
        if (bDisCount && cDataOne.IsBuyState())
        {
            ins_txtOriginPrice.text = cDataOne.GetStrOriginSalePrice();
            ins_txtDiscountPrice.text = cDataOne.GetstrDiscountPrice();
            ins_txtDiscount.text = string.Format(CDataGameInfo.M_strQstPercentage, cDataOne.GetDiscountPercent());
        }
        ins_txtCurrency.gameObject.SetActive(!bDisCount);
        ins_objDiscount.SetActive(bDisCount);
        ins_objDiscountPriceGroup.SetActive(bDisCount); //AISoldoutState함수에서 솔드아웃됐을때 꺼짐.
    }

    protected IEnumerator CorTimer(Text txtRemainTime, int nRemain)
    {
        while (nRemain > 0)
        {
            CUtil.SetTimeText(txtRemainTime, nRemain);
            yield return CYieldInstructionCache.WaitForSecondsRealtime(1f);
            nRemain--;
        }
    }

    public void StopTimer()
    {
        if (_CorTimer != null)
        {
            StopCoroutine(_CorTimer);
        }
    }
}