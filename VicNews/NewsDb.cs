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
using System.Text.RegularExpressions;
using System.Web.Configuration;
using ShareLib;

/// <summary>
/// Summary description for PaymentDb
/// </summary>
/// 

public class NewsDb : ManagedPageDb, IManagerDb, IEditorDb, IBasicListDb, IPageDb,IImageDb
{
    private int _beginRecord;
    private int _endRecord;
    private ViewStateHolder _holder;

    public NewsDb(string AClassId, string AIndexName)
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

    public string Kind
    {
        set { MyFunc.SaveSessionData("NewsDbKind", value); }
        get { return MyFunc.GetSessionData("NewsDbKind", "0"); }
    }

    public DateTime BeginDate
    {
        set { MyFunc.SaveSessionData("NewsDbBeginDate", value); }
        get { return MyFunc.GetSessionData("NewsDbBeginDate", DateTime.Today); }
    }

    public string Period
    {
        set { MyFunc.SaveSessionData("NewsDbPeriod", value); }
        get { return MyFunc.GetSessionData("NewsDbPeriod", "0"); }
    }

    public string KeyWord
    {
        set { MyFunc.SaveSessionData("NewsDbKeyword", value); }
        get { return MyFunc.GetSessionData("NewsDbKeyword", ""); }
    }

    public int BeginRecNo { set { _beginRecord = value; } }

    public int EndRecNo { set { _endRecord = value; } }

    public PublishKind Publish
    {
        set { MyFunc.SaveSessionData("NewsDbPublish", value); }
        get
        {
            object o = MyFunc.GetSessionData("NewsDbPublish");
            if (o == null)
                return PublishKind.Publish;
            return (PublishKind)o;
        }
    }

    public ItemTextKind TextKind
    {
        set { MyFunc.SaveSessionData("NewsDbTextKind", value); }
        get
        {
            object o = MyFunc.GetSessionData("NewsDbTextKind");
            if (o == null)
                return ItemTextKind.Full;
            return (ItemTextKind)o;
        }
    }

    public DateTime EndDate
    {
        get
        {
            int diff = -3;
            switch (Period)
            {
                case "1":
                    diff = -1;
                    break;
                case "2":
                    diff = -7;
                    break;
                case "3":
                    diff = -31;
                    break;
                case "4":
                    diff = -3;
                    break;
            }
            return BeginDate.AddDays(diff);
        }
    }

    public override void CheckDuplicates(ArrayList Values)
    {
    }

    protected override bool CheckCanDelete()
    {
        return true;
    }

    protected override void SaveSessionData()
    {
        MyFunc.SaveSessionData("NewsDbIndex", IndexValue);
    }

    protected override void LoadSessionData()
    {
        IndexValue = MyFunc.GetSessionData("NewsDbIndex", 0);
    }

    private string SqlCriteria(int Number)
    {
        string cri = "";
        if (Kind != "0")
            cri += " AND Area=" + Kind;
        if (Period != "0")
            cri += string.Format(" AND NewsDate<=[@BeginDate{0}, DBDate] AND NewsDate>[@EndDate{0}, DBDate]", Number);
        if (KeyWord != "")
            cri += string.Format(" AND (Title LIKE \'%{0}%\' OR Text LIKE \'%{0}%\' )", MyFunc.UrlDecode(KeyWord));
        if (Publish == PublishKind.Publish)
            cri += " AND Active='true'";
        if (Publish == PublishKind.OnHold)
            cri += " AND (Active='false' OR Active IS NULL)";
        return cri;
    }

    public int GetRecordCount()
    {
        string sql = BaseDatabase.GetSqlText("GetNewsCount");
        sql = string.Format(sql, SqlCriteria(0));
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        if (param.ParamList != null)
            if (param.ParamList.Length == 2)
            {
                param.ParamList[0].Value = BeginDate;
                param.ParamList[1].Value = EndDate;
            }
        return BaseDatabase.GetSingleValue(param, 0);
    }

    protected override SqlParam GetFilterParam(string sql)
    {
        sql = string.Format(sql, SqlCriteria(0), SqlCriteria(1));
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        if (param.ParamList.Length == 2)
        {
            param.ParamList[0].Value = _beginRecord;
            param.ParamList[1].Value = _endRecord;
        }
        if (param.ParamList.Length == 6)
        {
            param.ParamList[0].Value = _beginRecord;
            param.ParamList[1].Value = BeginDate;
            param.ParamList[2].Value = EndDate;
            param.ParamList[3].Value = _endRecord;
            param.ParamList[4].Value = BeginDate;
            param.ParamList[5].Value = EndDate;
        }
        return param;
    }

    public string ListTitle()
    {
        string err = "非本日全部新闻";
        if (Kind != "0")
            return err;
        if (Period != "1")
            return err;
        StringBuilder sb = new StringBuilder();
        DataRowCollection rows = null;// BaseDatabase.GetDataRows();
        if (rows == null)
            return "";
        if (rows.Count == 0)
            return "";
        sb.Append(BaseDatabase.GetRowValue(rows[0], "NewsDate", "").Replace("0:00:00", ""));
        sb.Append(" 新闻标题");
        sb.Append("Line-Break");
        foreach (DataRow row in rows)
        {
            sb.Append(BaseDatabase.GetRowValue(row, "Title", ""));
            sb.Append("Line-Break");
        }
        return sb.ToString().Replace("Line-Break", "\r\n");
    }

    public void SaveImage(string FileName, string Url)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateNewsImage");
        param.ParamList[0].Value = FileName;
        param.ParamList[1].Value = Url;
        param.ParamList[2].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        param = BaseDatabase.GetSqlParam("SelectNewsHtml");
        param.ParamList[0].Value = IndexValue;
        string html = BaseDatabase.GetSingleValue(param, "");
        if (html != "")
            HideMainImage(true);
    }

    public void DeleteImage()
    {
        SqlParam param = BaseDatabase.GetSqlParam("DeleteNewsImage");
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        CommonItemDb.DeleteLiveImage(IndexValue);
    }

    public void LiveImage()
    {
        SqlParam param = BaseDatabase.GetSqlParam("GetNewsInfo");
        param.ParamList[0].Value = IndexValue;
        DataRow row = BaseDatabase.GetSingleRow(param);
        string img = BaseDatabase.GetRowValue(row, "Image", "").Trim();
        if (img == "")
            return;
        string title = BaseDatabase.GetRowValue(row, "Title", "");
        CommonItemDb.AddLiveImage(IndexValue, img, title, "News");
    }

    public void FillInfo(out string Date, out string Title, out string Details, out string Image, out bool HideMainImage)
    {
        Image = "";
        SqlParam param = BaseDatabase.GetSqlParam("GetNewsInfo");
        param.ParamList[0].Value = IndexValue;
        DataRow row = BaseDatabase.GetSingleRow(param);
        DateTime date = BaseDatabase.GetRowValue(row, "NewsDate", new DateTime(1, 1, 1));
        Date = string.Format("{0:D}", date);
        Title = BaseDatabase.GetRowValue(row, "Title", "");
        HideMainImage = BaseDatabase.GetRowValue(row, "HideMainImage", false);
        if (!HideMainImage)
        {
            Image = BaseDatabase.GetRowValue(row, "ImageUrl", "");
            if (Image == "")
            {
                Image = BaseDatabase.GetRowValue(row, "Image", "");
                if (Image != "")
                    Image = MyFunc.GetWebconfigValue("NewsImagePath", "") + BaseDatabase.GetRowValue(row, "Image", "");
            }
        }
        Details = MyFunc.DirectPrintHtml(BaseDatabase.GetRowValue(row, "Html", ""));
        if (string.IsNullOrEmpty(Details.Trim()))
            Details = MyFunc.DirectPrintHtml(BaseDatabase.GetRowValue(row, "Text", ""));
        Details += "<br/>";
        string extra = BaseDatabase.GetRowValue(row, "ExtraText", "");
        if (extra != "")
            Details += MyFunc.DirectPrintHtml(extra) + "<br/>";
    }

    public void EditTitle(string Text)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateNewsTitle");
        param.ParamList[0].Value = Text.Trim();
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void EditText(string Text)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateNewsText");
        param.ParamList[0].Value = MyFunc.TrimTextAll(Text);
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void DeleteItem()
    {
        SqlParam param = BaseDatabase.GetSPParam("sp_DeleteNewsItem");
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        CommonItemDb.DeleteLiveImage(IndexValue);
    }

    public void CopyNews()
    {
        int newid = BaseDatabase.GetNextId();
        string sql = BaseDatabase.GetSqlText("CopyNews");
        sql = string.Format(sql, newid);
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        IndexValue = newid;
    }

    public void DisplayTop(DataList DtList)
    {
        SqlParam param = BaseDatabase.GetSqlParam("SelectNewsTop");
        BaseDatabase.FillDataList(DtList, param);
    }

    public static string ReadNewsWord()
    {
        SqlParam param = BaseDatabase.GetSqlParam("SelectNewsWord");
        return BaseDatabase.GetSingleValue(param, "");
    }

    public static void SaveNewsWord(string Text)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateNewsWord");
        param.ParamList[0].Value = Text;
        BaseDatabase.ExecSql(param);
    }

    public void RemoveTop(int Index)
    {
        SqlParam param = BaseDatabase.GetSqlParam("RemoveTopNews");
        param.ParamList[0].Value = Index;
        BaseDatabase.ExecSql(param);
    }

    public void AddExtra(string Text)
    {
        SqlParam param = BaseDatabase.GetSqlParam("AddNewsExtra");
        param.ParamList[1].Value = Text;
        param.ParamList[2].Value = IndexValue;
        BaseDatabase.AddRecord(param);
    }

    public void EditExtra(string Text)
    {
        SqlParam param = BaseDatabase.GetSqlParam("EditNewsExtra");
        param.ParamList[0].Value = Text;
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public static void FillSource(DropDownList List)
    {
        SqlParam param = BaseDatabase.GetSqlParam("SelectNewsSource");
        BaseDatabase.FillDropDownList(List, param, "SourceName", "SourceIndex");
        List.Items.Insert(0, new ListItem());
    }

    public static string ReplaceWords(string Text, bool Dot)
    {
        SqlParam param = BaseDatabase.GetSqlParam("SelectReplaceWord");
        DataRowCollection rows = BaseDatabase.GetDataRows(param);
        foreach (DataRow row in rows)
        {
            string org = BaseDatabase.GetRowValue(row, "Origin", "");
            if (org == "")
                continue;
            string rep = BaseDatabase.GetRowValue(row, "Replace", "");
            if (!Dot)
                if (org == ".")
                    continue;
            Text = Text.Replace(org, rep);
        }
        return AdjDot(Text);
    }

    private static string AdjDot(string Text)
    {
        Regex r = new Regex("[\\d]+。[\\d]+");
        MatchCollection mm = r.Matches(Text);
        foreach (Match m in mm)
            Text = Text.Replace(m.Value, m.Value.Replace("。", "."));
        return r.Replace(Text, ",");
    }

    public static void SaveReplace(string From, string To)
    {
        if (string.IsNullOrEmpty(From))
            return;
        SqlParam param = BaseDatabase.GetSqlParam("AddReplacePair");
        param.ParamList[1].Value = From;
        param.ParamList[2].Value = To;
        BaseDatabase.AddRecord(param);
    }

    public void Submit()
    {
        SqlParam param = BaseDatabase.GetSqlParam("SubmitNews");
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        LiveImage();
    }

    public void SaveHtml(string Html)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateNewsHtml");
        param.ParamList[0].Value = Html;
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void SetSimpleTrue()
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateSimpleTrue");
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void HideMainImage(bool Hide)
    {
        SqlParam param = BaseDatabase.GetSqlParam("NewsHideMainImage");
        param.ParamList[0].Value = Hide;
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }


    public void AddItem(DateTime DateValue, string Title, string Text, string Extra, string Kind, string Data)
    {
    }

    public void FillKind(ListItemCollection List)
    {
        Utility.FillKind(List, "SelectNewsArea", "AreaText", "AreaIndex");
    }

    public string FillKindMenu()
    {
        return Utility.FillKindMenu("SelectNewsArea", "news", "AreaText", "AreaIndex", Kind, "全部", 0);
    }

    public object GetColValue(string ColName)
    {
        return null;
    }

    public void ClickExtra(string Extra, TextBox Title, TextBox Body, string Option, out string Data)
    {
        Data = "";
    }

    public void ChangeKind(string PageKind, string ItemKind, int Index)
    {
    }

    public string GetAutoImage()
    {
        return "";
    }

    public string DisplayTitle(object DataItem)
    {
        return "";
    }

    public string FillKindMenu2()
    {
        return "";
    }

    public string GetTargetPage(string Index)
    {
        return "";
    }

    public string KindText()
    {
        if (Kind == "0")
            return "全部";
        SqlParam param = BaseDatabase.GetSqlParam("SelectNewsArea");
        DataSet ds = BaseDatabase.FillDataset(param);
        DataRow[] rows = ds.Tables[0].Select("AreaIndex=" + Kind);
        if (rows.Length == 0)
            return "";
        return BaseDatabase.GetRowValue(rows[0], "AreaText", "");
    }

    public static DataRowCollection GetFocusImages()
    {
        string url = "{0}?pagekind={1}&amp;id={2}";
        string urlpath = MyFunc.GetWebconfigValue("FoucsImageNewsTarget", "");
        string imgpath = MyFunc.GetWebconfigValue("NewsImagePath", "");
        SqlParam param = BaseDatabase.GetSqlParam("SelectFocusImage");
        DataRowCollection result = BaseDatabase.GetDataRows(param);
        foreach (DataRow row in result)
        {
            row["Image"] = Utility.MakeRealUrl(MyFunc.GetWebconfigValue("ImageRoot", "") + row["Kind"] + "/" + row["Image"], true);
            row["Url"] = Utility.MakeRealUrl(string.Format(url, MyFunc.GetWebconfigValue(row["Kind"] + "TargetPage", ""), row["Kind"], row["ItemIndex"]), false);
        }
        return result;
    }

    public static void PasteAnything(string Url, DateTime DayDate)
    {
        Url = Url.Trim();
        if (string.IsNullOrEmpty(Url))
            return;
        SqlParam param = BaseDatabase.GetSqlParam("PasteUrl");
        param.ParamList[1].Value = Url;
        param.ParamList[2].Value = DayDate;
        BaseDatabase.AddRecord(param);
    }

    public void ChangeNews(bool Simple, bool Top, string Area, string Focus, string Src)
    {
        SqlParam param = BaseDatabase.GetSqlParam("EditNewsShort");
        param.ParamList[0].Value = Simple;
        param.ParamList[1].Value = Top;
        param.ParamList[2].Value = Area;
        param.ParamList[3].Value = Focus;
        param.ParamList[4].Value = Src;
        param.ParamList[5].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }


}

