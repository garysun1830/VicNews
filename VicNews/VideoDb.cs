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


public class VideoDb : CommonItemDb
{
    private const string ERR = "地址不正确";

    public VideoDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
        _pageKind = "video";
        _hasAll = true;
        _defaultKind = "0";
    }

    private int _total
    {
        set { MyFunc.SaveSessionData(_pageKind + "VideoDbTotal", value); }
        get { return MyFunc.GetSessionData(_pageKind + "VideoDbTotal", 0); }
    }

    private int _upTo
    {
        set { MyFunc.SaveSessionData(_pageKind + "VideoDbUpTo", value); }
        get { return MyFunc.GetSessionData(_pageKind + "VideoDbUpTo", 0); }
    }

    private string _url
    {
        set { MyFunc.SaveSessionData(_pageKind + "VideoDbUrl", value); }
        get { return MyFunc.GetSessionData(_pageKind + "VideoDbUrl", ""); }
    }

    private string _img
    {
        set { MyFunc.SaveSessionData(_pageKind + "VideoDbImg", value); }
        get { return MyFunc.GetSessionData(_pageKind + "VideoDbImg", ""); }
    }

    public static DataTable CreateVidoeSource(int Total)
    {
        string[] cols = { "Number" };
        string[] dtype = { "System.String" };
        DataTable table = BaseDatabase.CreateTable("", "Index", true, cols, dtype);
        for (int i = 1; i <= Total; i++)
        {
            DataRow row = table.NewRow();
            row["Number"] = i;
            table.Rows.Add(row);
        }
        return table;
    }

    public override void ClickExtra(string Extra, TextBox Title, TextBox Body, string Option, out string Data)
    {
        Data = "";
        int index = 0;
        Match m = Regex.Match(Extra, "/\\d+/");
        if (!m.Success)
            m = Regex.Match(Extra, "/\\d+\\.html");
        if (!m.Success)
            throw new Exception(ERR);
        string s = m.Value.Replace("/", "").Replace(".html", "");
        index = Convert.ToInt32(s);
        _url = string.Format("http://www.letv.com/ptv/pplay/{0}/{1}.html", index, "{0}");
        string web = string.Format("http://so.letv.com/tv/{0}.html", index);
        web = MyFunc.ReadWeb(web, Encoding.UTF8);
        s = HtmlTree.TextOfTag(web, "h1", "", (object)null, " class=\"fl\"");
        if (s != string.Empty)
            ParseHtmlH1(web, s, Title);
        s = HtmlTree.TextOfTag(web, "h3", "", (object)null, "</i>");
        if (s != string.Empty)
            ParseHtmlH3(web, s, Title);
        List<HtmlTag> list = HtmlTree.FindTags(web, "a", (object)null, "img", "src=", "imga");
        if (list.Count == 0)
            return;
        _img = HtmlTree.ExtractImgSrc(list[0].ToString());
    }

    private void ParseHtmlH3(string Web, string Text, TextBox Title)
    {
        string s = Regex.Replace(Text, "<i>((?!</i>).)*</i>", string.Empty);
        s = Regex.Replace(s, "</?((?!>).)*>", "");
        Title.Text = s;
        Match m = Regex.Match(Text, "<b>\\d+</b>");
        if (!m.Success)
            throw new Exception(ERR);
        s = Regex.Replace(m.Value, "</?((?!>).)*>", "");
        _total = Convert.ToInt32(s);
        _upTo = _total;
        s = HtmlTree.TextOfTag(Web, "span", "", (object)null, "class=\"s-t\"");
        m = Regex.Match(s, "更新至[第]?\\d+集");
        if (m.Success)
        {
            m = Regex.Match(m.Value, "\\d+");
            _upTo = Convert.ToInt32(m.Value);
        }
    }

    private void ParseHtmlH1(string Web, string Text, TextBox Title)
    {
        string s = Text;
        Match m = Regex.Match(s, "《((?!》).)*》");
        if (!m.Success)
            throw new Exception(ERR);
        Title.Text = m.Value.Replace("《", "").Replace("》", "");
        m = Regex.Match(s, "共\\d+集");
        if (!m.Success)
            throw new Exception(ERR);
        m = Regex.Match(m.Value, "\\d+");
        _total = Convert.ToInt32(m.Value);
        _upTo = _total;
        m = Regex.Match(s, "更新至[第]+\\d+集");
        if (m.Success)
        {
            m = Regex.Match(m.Value, "\\d+");
            _upTo = Convert.ToInt32(m.Value);
        }
    }

    protected override void AddItemExtra(string Extra, string Data)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateVideoData");
        param.ParamList[0].Value = _url;
        param.ParamList[1].Value = _total;
        param.ParamList[2].Value = _upTo;
        param.ParamList[3].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        UploadData data;
        data.Db = (IImageDb)this;
        data.ImageWebPath = ImagePath;
        data.ImageServerPath = "";
        data.Url = _img;
        data.PenFile = "";
        data.PrevImageUrl = "";
        data.ThumbWidth = 150;
        UploadImage upload = new UploadImage(data);
        try { upload.Upload(); }
        catch { }
    }

}