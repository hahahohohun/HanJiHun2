using System;

public class CDataInAppOne : CDataBase
{
    #region [code] IDsposable Func 
    private bool disposed = false;
    protected override void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //관리되는 자원 해제.
                _cDataAssetInApp = null;
                _cDataNetInAppInfoIn = null;
            }
            //관리되지 않는 자원해제.

            disposed = true;
        }
    }
    #endregion

    //AssetData
    //엑셀로 작성된 업적 데이터.
    CDataAssetInAppData.CData _cDataAssetInApp = null;
    CDataNetInAppInfoIn _cDataNetInAppInfoIn = null;

    public void Setdata(CDataAssetInAppData.CData cDataAssetInApp)
    {
        _cDataAssetInApp = cDataAssetInApp;
    }

    public void SetNetData(CDataNetInAppInfoIn cDataNetInAppInfoIn)
    {
        _cDataNetInAppInfoIn = null;
        _cDataNetInAppInfoIn = cDataNetInAppInfoIn;
    }

    #region [code] Get Memory Data
    public int GetSaleID()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.SaleID);
    }
    public int GetStdID()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.StdID);
    }
    public int GetPayedGemValue()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.PayedGemValue);
    }
    public int GetFreeGemValue()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.FreeGemValue);
    }
    public bool IsFreePurchase()
    {
        return _cDataAssetInApp.GetDataByEncIntArray(CDataAssetInAppData.EmData.SalePrice)[CDataManager.In.m_cDataNetPlayerLogin.m_cDataRes.Country].V <= 0;
    }
    public int GetDiscountPrice()
    {
        return _cDataAssetInApp.GetDataByEncIntArray(CDataAssetInAppData.EmData.DiscountPrice)[CDataManager.In.m_cDataNetPlayerLogin.m_cDataRes.Country].V;
    }
    public bool IsDiscountPercent()
    {
        return _cDataAssetInApp.GetDataByEncIntArray(CDataAssetInAppData.EmData.DiscountPercent)[CDataManager.In.m_cDataNetPlayerLogin.m_cDataRes.Country].V != 0;
    }
    public int GetDiscountPercent()
    {
        return _cDataAssetInApp.GetDataByEncIntArray(CDataAssetInAppData.EmData.DiscountPercent)[CDataManager.In.m_cDataNetPlayerLogin.m_cDataRes.Country].V;
    }


    //할인을 적용하지 않은 원래의 가격.
    public string GetStrOriginSalePrice()
    {
        return string.Format("{0} {1}", CDataGameInfo.GetLanguageCurrency(), CLanguageManager.In.GetCommaString(GetSalePrice(), true));
    }

    //추가 설명을 사용할지 안할지 체크.
    public bool IsUseSubDescription()
    {
        //추가 설명사용 && 첫구매는 효율표시 안함. 
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.SubDescription) != -1;
    }
    public int GeSubDescription()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.SubDescription);
    }
    //

    public int GetPackageItemKeyID()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.PackageItemKeyID);
    }
    public bool IsPackageItem()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.PackageItemKeyID) > -1;
    }
    //.
    private int GetSalePrice()
    {
        return _cDataAssetInApp.GetDataByEncIntArray(CDataAssetInAppData.EmData.SalePrice)[CDataManager.In.m_cDataNetPlayerLogin.m_cDataRes.Country].V;
    }
    public string GetBaseIconID()
    {
        return _cDataAssetInApp.GetDataByString(CDataAssetInAppData.EmData.BaseIconID);
    }

    public string GetStoreItemID()
    {
        return _cDataAssetInApp.GetDataByString(CDataAssetInAppData.EmData.StoreItemID);
    }
    public EmInAppLink GetInAppLink()
    {
        return (EmInAppLink)_cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.InAppLink);
    }
    public EmInAppViewType GetInAppViewType()
    {
        return (EmInAppViewType)_cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.ViewType);
    }
    public int GetLimitCount()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.LimitCount);
    }
    //0무제한, 1월, 2년.
    public int GetLimitType()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.LimitType);
    }
    //구매 제한으로 분류된 상품인지.
    public bool IsLimitType()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.LimitType) > 0;
    }
    //구매 횟수 제한이 있는 상품인지.
    public bool IsLimitCount()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.LimitCount) > 0;
    }
    public DateTime GetStartDate()
    {
        return DateTime.ParseExact(_cDataAssetInApp.GetDataByString(CDataAssetInAppData.EmData.StartDate), CDataGameInfo.M_strInAppDateStandard, System.Globalization.CultureInfo.InvariantCulture);
    }
    public DateTime GetEndDate()
    {
        return DateTime.ParseExact(_cDataAssetInApp.GetDataByString(CDataAssetInAppData.EmData.EndDate), CDataGameInfo.M_strInAppDateStandard, System.Globalization.CultureInfo.InvariantCulture);
    }

    //위쪽부터 나열될 순서.
    public int GetUIPosition()
    {
        return _cDataAssetInApp.GetDataByInt(CDataAssetInAppData.EmData.UIPosition);
    }
    #endregion

    #region [code] Get Net Data
    public bool IsOpen()
    {
        if (_cDataNetInAppInfoIn == null)
            return false;
        return true;
    }
    /// <summary>
    /// 구매 가능한횟수가 0이면.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentLimitCount()
    {
        return _cDataNetInAppInfoIn.LimitCnt;
    }
    public bool IsCurrentLimitCount()
    {
        return _cDataNetInAppInfoIn.LimitCnt > 0;
    }
    public bool IsDoubleCount()
    {
        return _cDataNetInAppInfoIn.DoubleCnt > 0;
    }

    public int GetReOpenDate()
    {
        return _cDataNetInAppInfoIn.ReOpenDate;
    }
    //재 오픈으로 보여줄 표시기간이 있으면.
    public bool IsReOpenDate()
    {
        return _cDataNetInAppInfoIn.ReOpenDate > 0;
    }
    public bool IsBuyState()
    {
        return IsCurrentLimitCount() || !IsLimitType();
    }
    public void SetOneSecound(out bool bRemove)
    {
        _cDataNetInAppInfoIn.ReOpenDate--;
        if (_cDataNetInAppInfoIn.ReOpenDate < 0)
            _cDataNetInAppInfoIn.ReOpenDate = 0;

        bRemove = _cDataNetInAppInfoIn.ReOpenDate == 0;
    }
    #endregion

    #region asset & net 조합데이터.

    //상품금액.
    public string GetStrPurchasePrice()
    {
        string currencyText = string.Empty;

        if (IsFreePurchase())
        {
            int nNum = IsCurrentLimitCount() || !IsLimitType() ? 1 : 2;
            currencyText = CLanguageManager.In.GetText(CLanguageManager.EmKind.Std, CDataGameInfo.M_strKeyInAppPurchaseItemDouble + CDataGameInfo.GetNumStr(nNum));

        }
        else
        {
            long lValue = IsDiscountPercent() ? GetDiscountPrice() : GetSalePrice();
            currencyText = string.Format("{0} {1}", CDataGameInfo.GetLanguageCurrency(), CLanguageManager.In.GetCommaString(lValue, true));
        }

        return currencyText;
    }
    public string GetstrDiscountPrice()
    {
        return string.Format("{0} {1}", CDataGameInfo.GetLanguageCurrency(), CLanguageManager.In.GetCommaString(GetDiscountPrice(), true));
    }
    public int GetPurchasePrice()
    {
        if (IsFreePurchase())
        {
            return 0;
        }
        else
        {
            //할인하는 상품.
            if (IsDiscountPercent())
            {
                return GetDiscountPrice();
            }
            else
            {
                return GetSalePrice();
            }
        }
    } 

    //무료상품인데 구매가능한 상태이다.
    public bool IsFreePurchaseBuyState()
    {
        return IsFreePurchase() && IsBuyState(); 
    }
    #endregion
}
