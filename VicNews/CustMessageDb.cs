using System;
using System.Web;
using ShareLib;

public class CustMessageDb : ManagedPageDb, IManagerDb, IEditorDb
{

    public CustMessageDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
    }

    public void AddMessage(string Text)
    {
        SqlParam param = BaseDatabase.GetSqlParam("AddCustMessage");
        param.ParamList[1].Value = Text;
        param.ParamList[2].Value = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        param.ParamList[3].Value = DateTime.Now;
        BaseDatabase.AddRecord(param);
    }

}


