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


public class NewsPictureDb : ManagedPageDb, IManagerDb, IEditorDb, IDetailList
{
    private NewsDb _db;

    public NewsPictureDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
        _db = ClassFactory.CreateDb("News", "News") as NewsDb;
    }

    public int NewsId
    {
        set { _db.SetIndexValue(value); }
    }

    public void FillInfo(out string Date, out string Title, out string Details, out string Image, out bool HideMainImage)
    {
        _db.FillInfo(out Date, out Title, out Details, out Image, out HideMainImage);
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
        param.ParamList[0].Value = _db.IndexValue;
        return param;
    }

    public void AddPictue(string Text, bool IsUrl)
    {
        string sql = IsUrl ? "AddNewsDetailPictureUrl" : "AddNewsDetailPictureFile";
        SqlParam param = BaseDatabase.GetSqlParam(sql);
        param.ParamList[1].Value = Text;
        param.ParamList[2].Value = _db.IndexValue; ;
        BaseDatabase.AddRecord(param);
    }

    public void EditTitle(string Title)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateNewsDetailPictureTitle");
        param.ParamList[0].Value = Title;
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void HideMainImage(bool Hide)
    {
        _db.HideMainImage(Hide);
    }

}


