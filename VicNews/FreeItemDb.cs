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


public class FreeItemDb : ManagedPageDb, IManagerDb, IEditorDb, IPageDb
{
    private Dictionary<string, string> _dic;
    private int _beginRecord;
    private int _endRecord;

    public FreeItemDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
        _dic = new Dictionary<string, string>();
        LoadDictionary();
    }

    public bool Simple
    {
        set { MyFunc.SaveSessionData("FreeItem_Simple", value); }
        get { return MyFunc.GetSessionData("FreeItem_Simple", true); }
    }

    public bool Chinese
    {
        set { MyFunc.SaveSessionData("FreeItem_Chinese", value); }
        get { return MyFunc.GetSessionData("FreeItem_Chinese", true); }
    }

    public string Kind
    {
        set { MyFunc.SaveSessionData("FreeItem_Kind", value); }
        get { return MyFunc.GetSessionData("FreeItem_Kind", "0"); }
    }

    public int BeginRecNo { set { _beginRecord = value; } }

    public int EndRecNo { set { _endRecord = value; } }


    private string SqlCriteria()
    {
        if (Kind == "0")
            return "";
        if (Kind == "100")
            return " AND (Category=0 OR Category IS NULL)";
        return " AND Category=" + Kind;
    }

    protected override SqlParam GetFilterParam(string sql)
    {
        sql = string.Format(sql, SqlCriteria(), MyFunc.GetWebconfigValue( "FreeItemSortCol",""), 
                                                                "ORDER BY "+ MyFunc.GetWebconfigValue( "FreeItemOrderBy",""));
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        param.ParamList[0].Value = _beginRecord;
        param.ParamList[1].Value = _endRecord;
        return param;
    }

    public int GetRecordCount()
    {
        string sql = BaseDatabase.GetSqlText("GetFreeItemCount");
        sql = string.Format(sql, SqlCriteria());
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        return BaseDatabase.GetSingleValue(param, 0);
    }

    public void EditTitle(string Text)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateFreeItemTitle");
        param.ParamList[0].Value = Text;
        param.ParamList[1].Value = IndexValue;
        BaseDatabase.ExecSql(param);
    }

    public void SaveTrans(string Eng, string Ch)
    {
        SqlParam param = BaseDatabase.GetSqlParam("TransExists");
        param.ParamList[0].Value = Eng;
        if (BaseDatabase.RecordExists(param))
        {
            param = BaseDatabase.GetSqlParam("UpdateTrans");
            param.ParamList[0].Value = Ch;
            param.ParamList[1].Value = Eng;
            BaseDatabase.ExecSql(param);
        }
        else
        {
            param = BaseDatabase.GetSqlParam("AddTrans");
            param.ParamList[1].Value = Eng;
            param.ParamList[2].Value = Ch;
            param.ParamList[3].Value = CountEng(Eng);
            BaseDatabase.AddRecord(param);
        }
        LoadDictionary();
    }

    private int CountEng(string Text)
    {
        Text = Text.Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ");
        string[] strs = Text.Split(new char[] { ' ' });
        return strs.Length;
    }

    private void LoadDictionary()
    {
        _dic.Clear();
        SqlParam param = BaseDatabase.GetSqlParam("SelectDictionary");
        DataRowCollection rows = BaseDatabase.GetDataRows(param);
        foreach (DataRow row in rows)
        {
            param = BaseDatabase.GetSqlParamDirect("UPDATE trans set count=[@count,integer] where transindex=[@transindex,Integer]");
            param.ParamList[0].Value = CountEng(BaseDatabase.GetRowValue(row, "English", ""));
            param.ParamList[1].Value = BaseDatabase.GetRowValue(row, "transindex", 0);
            BaseDatabase.ExecSql(param);
            _dic.Add(BaseDatabase.GetRowValue(row, "English", ""), BaseDatabase.GetRowValue(row, "Chinese", ""));
        }
    }

    public string Translate(string Text)
    {
        if (!Chinese)
            return Text;
        Text = Text.Replace('-', ' ').Replace('/', ' ').Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Trim();
        if (string.IsNullOrEmpty(Text))
            return "";
        foreach (KeyValuePair<string, string> item in _dic)
        {
            Regex r = new Regex("\\b" + item.Key + "\\b", RegexOptions.IgnoreCase);
            Text = r.Replace(Text, item.Value);
        }
        return Text;
    }

    public static void FillCategory(DropDownList List, bool TopBlank)
    {
        SqlParam param = BaseDatabase.GetSqlParam("SelectStuffCategory");
        BaseDatabase.FillDropDownList(List, param, "Text", "StuffCategoryIndex");
        if (TopBlank)
            List.Items.Insert(0, new ListItem());
    }

    public static void ChangeCategory(string Category, int Index)
    {
        SqlParam param = BaseDatabase.GetSqlParam("UpdateStuffCategory");
        param.ParamList[0].Value = Category;
        param.ParamList[1].Value = Index;
        BaseDatabase.ExecSql(param);
    }

    public static string FillKindMenu(string Kind)
    {
        string sql = BaseDatabase.GetSqlText("SelectStuffCategory");
        SqlParam param = BaseDatabase.GetSqlParamDirect(sql);
        return Utility.FillKindMenu(param, "freeitem", "Text", "StuffCategoryIndex", Kind, "全部", 0);
    }

    private static int WordCount(string Text)
    {
        string[] strs = Text.Trim().Split(new char[] { ' ' });
        int result = 0;
        foreach (string s in strs)
            if (s.Trim() != "")
                result++;
        return result;
    }

    public static void AddCategoryWord(string Text, string CatIndex)
    {
        SqlParam param = BaseDatabase.GetSqlParam("AddCategoryWord");
        param.ParamList[1].Value = Text;
        param.ParamList[2].Value = CatIndex;
        param.ParamList[3].Value = WordCount(Text);
        BaseDatabase.AddRecord(param);
        param = BaseDatabase.GetSqlParam("UpdateFreeItemCategory");
        param.ParamList[0].Value = CatIndex;
        param.ParamList[1].Value = "%" + Text + "%";
        BaseDatabase.ExecSql(param);
    }

    public static bool CategoryWordExists(string Text)
    {
        SqlParam param = BaseDatabase.GetSqlParam("CategoryWordExists");
        param.ParamList[0].Value = Text;
        return BaseDatabase.RecordExists(param);
    }

}


