using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web.Configuration;
using ShareLib;


public class OnSaleDetailDb : ManagedPageDb, IManagerDb, IEditorDb, IDetailList
{
    private string _saleId;

    public OnSaleDetailDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
    }

    public string SaleId
    {
        set { _saleId = value; }
        get { return _saleId; }
    }

    public override void CheckDuplicates(ArrayList Values)
    {
    }

    protected override bool CheckCanDelete()
    {
        return true;
    }

    protected override SqlParam GetFilterParam(string sql)
    {
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = _saleId;
        return param;
    }

    public void AddPictue(string FileName, bool Url)
    {
        SqlParam param = BaseDatabase.GetSqlParam("AddSaleDetailPicture");
        param.ParamList[1].Value = FileName;
        param.ParamList[2].Value = _saleId;
        BaseDatabase.AddRecord(param);
    }

    public void EditTitle(string Title)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateSaleDetailTitle");
        param.ParamList[0].Value = Title;
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public string SourceInfo()
    {
        SqlParam param = BaseDatabase.GetSqlParam("GetStoreInfo");
        param.ParamList[0].Value = _saleId;
        DataRow row = BaseDatabase.GetSingleRow(param);
        string url = BaseDatabase.GetRowValue(row, "Url", "");
        if (url == "")
            return "";
        if (url.ToLower().IndexOf("http") != 0)
            url = "http://" + url;
        url = string.Format("<a href='{0}' target='_blank'>{0}</a>", url);
        return url;
    }

}


