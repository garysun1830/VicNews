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
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using ShareLib;

public class CommonItemDb : ManagedPageDb, IManagerDb, IBasicListDb, IPageDb, IImageDb
{
    private int _beginRecord;
    private int _endRecord;
    protected string _pageKind;
    protected bool _hasAll;
    protected string _defaultKind;
    protected bool _hasOther;
    protected string _otherKind;
    private ViewStateHolder _holder;

    public CommonItemDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
    }

    public Control Parent
    {
        set
        {
            _holder = new ViewStateHolder(value);
        }
    }

    public string DataFilePath
    {
        get
        {
            int id = ((ManagedPageDb)this).IndexValue;
            string web = string.Format("{0}/{1}", MyFunc.GetWebconfigValue("DataFilePath", "") + _pageKind, id);
            return MyFunc.CurrentServer().MapPath(web);
        }
    }

    public PublishKind Publish
    {
        set { MyFunc.SaveSessionData(_pageKind + "Publish", value); }
        get
        {
            object o = MyFunc.GetSessionData(_pageKind + "Publish");
            if (o == null)
                return PublishKind.Publish;
            return (PublishKind)o;
        }
    }

    public string Kind
    {
        set { MyFunc.SaveSessionData(_pageKind + "Kind", value); }
        get { return MyFunc.GetSessionData(_pageKind + "Kind", _defaultKind); }
    }

    public ItemTextKind TextKind
    {
        set { MyFunc.SaveSessionData(_pageKind + "TextKind", value); }
        get
        {
            object o = MyFunc.GetSessionData(_pageKind + "TextKind");
            if (o == null)
                return ItemTextKind.Full;
            return (ItemTextKind)o;
        }
    }

    public string ImagePath
    {
        get { return MyFunc.GetWebconfigValue("ImageRoot", "") + _pageKind; }
    }

    public int BeginRecNo { set { _beginRecord = value; } }

    public int EndRecNo { set { _endRecord = value; } }

    public string KeyWord
    {
        set { MyFunc.SaveSessionData(_pageKind + "CommonItemDbKeyword", value); }
        get { return MyFunc.GetSessionData(_pageKind + "CommonItemDbKeyword", ""); }
    }

    protected string DataTitleCol                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
    {
        get { return MyFunc.GetWebconfigValue(_pageKind + "TitleCol", "Title"); }
    }

    public string FillKindMenu()
    {
        string sql = BaseDatabase.GetSqlText("SelectCommonItemKind");
        string menu2 = MyFunc.GetWebconfigValue(_pageKind + "Menu2Sql", "");
        sql = string.Format(sql, _pageKind, menu2);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        if (_hasAll)
            return Utility.FillKindMenu(param, _pageKind, "Text", _pageKind + "KindIndex", Kind, "　全部　", 0);
        else
            return Utility.FillKindMenu(param, _pageKind, "Text", _pageKind + "KindIndex", Kind);
    }

    public virtual string FillKindMenu2()
    {
        return "";
    }

    public void FillKind(ListItemCollection List)
    {
        string sql = BaseDatabase.GetSqlText("SelectCommonItemKind");
        sql = string.Format(sql, _pageKind, "");
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        Utility.FillKind(List, param, "Text", _pageKind + "KindIndex", true);
    }

    public void EditTitle(string Text)
    {
        string sql = BaseDatabase.GetSqlText("UpdateCommonItemTitle");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = MyFunc.TrimTextAll(Text);
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void EditText(string Text)
    {
        string sql = BaseDatabase.GetSqlText("UpdateCommonItemText");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = MyFunc.TrimTextAll(Text);
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void SaveImage(string FileName, string Url)
    {
        string sql = BaseDatabase.GetSqlText("UpdateCommonItemImage");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = FileName;
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        LiveImage();
    }

    public void DeleteImage()
    {
        string sql = BaseDatabase.GetSqlText("DeleteCommonItemImage");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void LiveImage()
    {
        string sql = BaseDatabase.GetSqlText("GetCommonItemInfo");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = IndexValue;
        DataRow row = BaseDatabase.GetSingleRow(param);
        AddLiveImage(IndexValue, BaseDatabase.GetRowValue(row, "Image", ""), BaseDatabase.GetRowValue(row, "Title", ""), _pageKind);
    }

    public static void AddLiveImage(int Index, string Image, string Title, string Kind)
    {
        SqlParam param = BaseDatabase.GetSPParam("sp_LivePhoto");
        param.ParamList[0].Value = Index;
        param.ParamList[1].Value = Image;
        param.ParamList[2].Value = Title;
        param.ParamList[3].Value = Kind;
        BaseDatabase.ExecSql(param);
    }

    public static void DeleteLiveImage(int Index)
    {
        SqlParam param = BaseDatabase.GetSqlParam("DeleteLivePhoto");
        param.ParamList[0].Value = Index;
        BaseDatabase.ExecSql(param);
    }

    public virtual  void DeleteItem()
    {
        string sql = BaseDatabase.GetSqlText("DeleteCommonItem");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        DeleteLiveImage(IndexValue);
    }

    public void Submit()
    {
        string sql = BaseDatabase.GetSqlText("SubmitCommonItem");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        LiveImage();
    }

    protected void AddItemDate(DateTime DateValue)
    {
        if (!MyFunc.GetWebconfigValue(_pageKind + "HasDate", false))
            return;
        string sql = BaseDatabase.GetSqlText("UpdateCommonItemDate");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = DateValue;
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    protected virtual void AddItemExtra(string Extra, string Data)
    {
    }

    protected virtual void CheckDuplicates(string Title, string Kind)
    {
        string sql = BaseDatabase.GetSqlText("CheckDupCommonItem");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = Title;
        param.ParamList[1].Value = Kind;
        if (BaseDatabase.RecordExists(param))
            throw new Exception("项目已经存在");
    }

    public void AddItem(DateTime DateValue, string Title, string Text, string Extra, string Kind, string Data)
    {
        CheckDuplicates(Title, Kind);
        string sql = BaseDatabase.GetSqlText("AddCommonItem");
        sql = string.Format(sql, _pageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[1].Value = Text;
        param.ParamList[2].Value = Title;
        param.ParamList[3].Value = Kind;
        IndexValue = BaseDatabase.AddRecord(param);
        AddItemDate(DateValue);
        AddItemExtra(Extra, Data);
        AddAutoImage();
    }

    protected override string GetSelectSqlName()
    {
        return "SelectCommonItems";
    }

    private string KindCriteria()
    {
        if (_hasOther && Kind == _otherKind)
            return "AND (Kind IS NULL OR Kind='')";
        if (!_hasAll || Kind != "0")
            return "AND Kind=" + Kind;
        return "";
    }

    protected virtual string PublishCriteria()
    {
        return "";
    }

    private string SqlCriteria()
    {
        string cri = "";
        if (KeyWord != "")
            cri += " AND (Text LIKE '%" + KeyWord + "%') OR (Title LIKE '%" + KeyWord + "%') ";
        cri += KindCriteria();
        cri += PublishCriteria();
        return cri;
    }

    protected override SqlParam GetFilterParam(string sql)
    {
        string sortcol = MyFunc.GetWebconfigValue(_pageKind + "SortCol", "");
        string orderby = MyFunc.GetWebconfigValue(_pageKind + "OrderBy", "");
        sql = string.Format(sql, _pageKind, sortcol, SqlCriteria(), orderby);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = _beginRecord;
        param.ParamList[1].Value = _endRecord;
        return param;
    }

    public int GetRecordCount()
    {
        string sql = BaseDatabase.GetSqlText("GetCommonItemCount");
        sql = string.Format(sql, _pageKind, SqlCriteria());
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        return BaseDatabase.GetSingleValue(param, 0);
    }

    public static object GetColValue(string PageKind, string Id, string ColName)
    {
        string sql = BaseDatabase.GetSqlText("GetCommonItemColumnValue");
        sql = string.Format(sql, PageKind, ColName);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = Id;
        return BaseDatabase.GetSingleValue(param);
    }

    public virtual void ClickExtra(string Extra, TextBox Title, TextBox Body, string Option, out string Data)
    {
        Data = "";
    }

    public void ChangeKind(string PageKind, string ItemKind, int Index)
    {
        string sql = BaseDatabase.GetSqlText("UpdateCommonItemKind");
        sql = string.Format(sql, PageKind);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = ItemKind;
        param.ParamList[1].Value = Index;
        BaseDatabase.ExecSql(param);
    }

    public string GetAutoImage()
    {
        string img = MyFunc.GetWebconfigValue(_pageKind + "AutoImage", string.Empty);
        if (string.IsNullOrEmpty(img))
            return string.Empty;
        string[] files = img.Split(new char[] { ',' });
        foreach (string s in files)
        {
            img = Path.Combine(DataFilePath, s);
            if (File.Exists(img))
                return img;
        }
        return string.Empty;
    }

    protected virtual void AddAutoImage()
    {
    }

    public string GetTargetPage(string Index)
    {
        string dir = MyFunc.GetWebconfigValue("DataFilePath", "") + _pageKind; 
        dir = Path.Combine( MyFunc.CurrentServer().MapPath(dir), Index);
        if (FileConfiguration.GetConfiguration(dir, "media", "") == "video")
            return "PlayListen.aspx";
        return MyFunc.GetWebconfigValue(_pageKind + "TargetPage", "");
    }

    public string KindText()
    {
        if (Kind == "0")
            return "全部";
        string sql = BaseDatabase.GetSqlText("SelectCommonItemKind");
        sql = string.Format(sql, _pageKind, "");
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        DataSet ds = BaseDatabase.FillDataset(param);
        DataRow[] rows = ds.Tables[0].Select(_pageKind + "KindIndex=" + Kind);
        if (rows.Length == 0)
            return "";
        return BaseDatabase.GetRowValue(rows[0], "Text", "");
    }

    public static string PageTitle(string Kind)
    {
        SqlParam param    = BaseDatabase.GetSqlParam("SelectPageTitle");
        param.ParamList[0].Value=Kind;
        DataRow row = BaseDatabase.GetSingleRow(param);
        return BaseDatabase.GetRowValue(row, "Text", "");
   }

}
