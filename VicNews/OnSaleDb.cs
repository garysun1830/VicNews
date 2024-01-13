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


public class OnSaleDb : ManagedPageDb, IManagerDb, IEditorDb
{

    public OnSaleDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
    }

    public void CopySale()
    {
        int newid = BaseDatabase.GetNextId();
        string sql = BaseDatabase.GetSqlText("CopySale");
        sql = string.Format(sql, newid);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        IndexValue = newid;
    }

    public static void FillStore(DropDownList List)
    {
        SqlParam param = BaseDatabase.GetSqlParam("SelectStore");
        BaseDatabase.FillDropDownList(List, param, "StoreName", "StoreIndex");
    }

    public override void CheckDuplicates(ArrayList Values)
    {
    }

    protected override bool CheckCanDelete()
    {
        return true;
    }

    public static void FillInfo(string SaleId, out string Store, out string Range)
    {
        SqlParam param = BaseDatabase.GetSqlParam("GetOnSaleInfo");
        param.ParamList[0].Value = SaleId;
        DataRow row = BaseDatabase.GetSingleRow(param);
        Store = BaseDatabase.GetRowValue(row, "StoreName", "");
        DateTime begin = BaseDatabase.GetRowValue(row, "BeginDate", new DateTime(1, 1, 1));
        if (begin.Year == 1)
            Range = "";
        else
            Range = string.Format("{0:D}", begin);
        Range += " 至 ";
        DateTime end = BaseDatabase.GetRowValue(row, "EndDate", new DateTime(1, 1, 1));
        if (end.Year != 1)
            Range += string.Format("{0:D}", end);
    }

    protected override SqlParam GetFilterParam(string sql)
    {
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = DateTime.Today;
        return param;
    }

}


