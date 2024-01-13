using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using ShareLib;

public enum ButtonPosition { Top, Right, Read };
public enum ItemTextKind { Full, Title };
public enum PublishKind { Publish, OnHold, None };

public interface IBasicListDb
{
    string Kind { set; get; }
    PublishKind Publish { set; get; }
    string KeyWord { set; get; }
    ItemTextKind TextKind { set; get; }
    Control Parent { set; }
    void DeleteImage();
    void EditTitle(string Text);
    void EditText(string Text);
    void DeleteItem();
    void SetIndexValue(int index);
    void Submit();
    string FillKindMenu();
    string FillKindMenu2();
    void FillKind(ListItemCollection Items);
    void AddItem(DateTime DateValue, string Title, string Text, string Extra, string Kind, string Data);
    void ClickExtra(string Extra, TextBox Title, TextBox Body, string Option, out string Data);
    void ChangeKind(string PageKind, string ItemKind, int Index);
    string GetAutoImage();
    string GetTargetPage(string Index);
    string KindText();
    void LiveImage();
}

public interface IDetailList
{
    void AddPictue(string Picture, bool IsUrl);
    void EditTitle(string Text);
    void SetIndexValue(int Index);
}

public class Utility
{

    public static string FillKindMenu(SqlParam Param, string PageKind, string ColText, string ColIndex, string Kind, string AllText, int AllIndex)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<table cellpadding='0' cellspacing='0'>\n<tr>\n");
        DataTable table = BaseDatabase.GetDataTable(Param);
        if (AllText != "")
        {
            DataRow r = table.NewRow();
            r[ColText] = AllText;
            r[ColIndex] = AllIndex;
            table.Rows.InsertAt(r, 0);
        }
        foreach (DataRow row in table.Rows)
        {
            string text = BaseDatabase.GetRowValue(row, ColText, "");
            string index = BaseDatabase.GetRowValue(row, ColIndex, 0).ToString();
            if (index == Kind)
            {
                sb.Append("<td class='tab-selected'>\n\r");
                sb.Append(text);
            }
            else
            {
                sb.Append("<td class='tab-unselected'>\n\r");
                sb.Append("<a href='");
                sb.Append(MyFunc.GetCurrentPageFileName());
                sb.Append("?pagekind=");
                sb.Append(PageKind);
                sb.Append("&itemkind=");
                sb.Append(index);
                sb.Append("'>");
                sb.Append(text);
                sb.Append("</a>\n");
            }
            sb.Append("</td>\n");
        }
        sb.Append("</tr>\n</table>\n");
        return sb.ToString();
    }

    public static string FillKindMenu(string KindSql, string PageKind, string ColText, string ColIndex, string Kind, string AllText, int AllIndex)
    {
        SqlParam param = BaseDatabase.GetSqlParam(KindSql);
        return FillKindMenu(param, PageKind, ColText, ColIndex, Kind, AllText, AllIndex);
    }

    public static string FillKindMenu(string KindSql, string PageKind, string ColText, string ColIndex, string Kind)
    {
        return FillKindMenu(KindSql, PageKind, ColText, ColIndex, Kind, "", 0);
    }

    public static string FillKindMenu(SqlParam Param, string PageKind, string ColText, string ColIndex, string Kind)
    {
        return FillKindMenu(Param, PageKind, ColText, ColIndex, Kind, "", 0);
    }

    public static void FillKind(ListItemCollection List, SqlParam param, string ColText, string ColIndex)
    {
        List.Clear();
        DataRowCollection rows = BaseDatabase.GetDataRows(param);
        foreach (DataRow row in rows)
        {
            ListItem item = new ListItem(BaseDatabase.GetRowValue(row, ColText, ""), BaseDatabase.GetRowValue(row, ColIndex, ""));
            List.Add(item);
        }
    }

    public static void FillKind(ListItemCollection List, string KindSql, string ColText, string ColIndex)
    {
        FillKind(List, BaseDatabase.GetSqlParam(KindSql), ColText, ColIndex);
    }

    public static void FillKind(ListItemCollection List, SqlParam param, string ColText, string ColIndex, bool TopBlank)
    {
        FillKind(List, param, ColText, ColIndex);
        if (TopBlank)
            List.Insert(0, new ListItem());
    }

    public static string MakeRealUrl(string Url, bool DisplayPic)
    {
        if (!MyCookie.CookieExists("Kanting365"))
            return Url;
        string domain = "";
        if (DisplayPic)
            domain = MyFunc.GetWebconfigValue("DomainKanTing365Pic", "");
        else
            domain = MyFunc.GetWebconfigValue("DomainKanTing365Link", "");
        return string.Format("http://{0}/{1}", domain, Url);
    }

    public static string RemoveDomain(string Url)
    {
        if (!MyCookie.CookieExists("Kanting365"))
            return Url;
        string domain = MyFunc.GetWebconfigValue("DomainKanTing365", "");
        Url = Regex.Replace(Url, "^[\\w\\. /:\\d]+" + domain.Replace(".", "\\."), "");
        return Url.Remove(0, 1); ;
    }

}

public static class ConstData
{
}

