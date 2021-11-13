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


  /////////////////////////
 
    #endregion
}
