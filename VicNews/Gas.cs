using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using ShareLib;

public class Gas
{

    private static string ParseGasUrl(string Text)
    {
        Regex rgx = new Regex("http://[^\"]*\"");
        MatchCollection m = rgx.Matches(Text);
        if (m.Count > 0)
            return m[0].Value.Replace("\"", "");
        return "";
    }

    public static string GasChart()
    {
        string text = MyFunc.ReadWeb(MyFunc.GetWebconfigValue("GasChart", "")).ToLower();
        Regex rgx = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*[^<>]*?/?[\s\t\r\n]*>");
        MatchCollection m = rgx.Matches(text);
        foreach (Match mm in m)
            if (mm.Value.Contains("gaschart"))
                return ParseGasUrl(mm.Value);
        return "";
    }

    private static string GetLowGas()
    {
        List<HtmlTag> list = GetGasList();
        if (list == null)
            return "";
        if (list.Count == 0)
            return "";
        return list[0].InnerText();
    }

    private static List<HtmlTag> GetGasList()
    {
        string text = MyFunc.ReadWeb(MyFunc.GetWebconfigValue("GasPriceUrl", ""));
        text = HtmlTree.TextOfTag(text, "tbody", "", (object)null, "rrlow_0");
        if (text == "")
            return null;
        return HtmlTree.FindTags(text, "tr", (object)null, "update");
    }

    private static string AdjTime(string Text)
    {
        string num = "";
        Regex rgx = new Regex("\\d+");
        MatchCollection m = rgx.Matches(Text);
        if (m.Count > 0)
            num = m[0].Value;
        string tmu = "";
        if (Text.ToLower().Contains("min"))
            tmu = "m";
        if (Text.ToLower().Contains("hour"))
            tmu = "h";
        return num + tmu + " ago";
    }

    private static string AdjLocation(string Text)
    {
        string s = MyFunc.GetWebconfigValue("GasLocReplace", "");
        string[] loc = s.Split(new char[] { ',' });
        Dictionary<string, string> dic = new Dictionary<string, string>();
        foreach (string item in loc)
        {
            string ss = item.Replace("\r", "").Replace("\n", "").Trim();
            string[] strs = ss.Split(new char[] { '=' });
            if (strs.Length == 2)
                dic.Add(strs[0], strs[1]);
        }
        foreach (KeyValuePair<string, string> kv in dic)
            Text = Text.Replace(kv.Key, kv.Value);
        return Text;
    }

    private static string ParsePrice(string Text)
    {
        Regex rgx = new Regex("<div class=\"sp_p((?!href=).)*href=", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        MatchCollection m = rgx.Matches(Text);
        if (m.Count == 0)
            return "";
        rgx = new Regex("<div class=\"p[\\dd]{1}");
        m = rgx.Matches(m[0].Value);
        if (m.Count == 0)
            return "";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < m.Count; i++)
            sb.Append(m[i].Value.Replace("<div class=\"p", "").Replace("d", "."));
        decimal price = 0m;
        if (!decimal.TryParse(sb.ToString(), out price))
            return "";
        return price.ToString();
    }

    private static string ParseAddress(string Text)
    {
        string text = HtmlTree.TextOfTag(Text, "dl", "", (object)null, "address");
        if (text == "")
            return "";
        text = HtmlTree.TextOfTag(text, "a", "");
        if (text == "")
            return "";
        text = HtmlTree.RemoveTag(text, "");
        return HttpUtility.HtmlDecode(text);
    }

    private static string ParseTime(string Text)
    {
        return HtmlTree.RemoveTag(HtmlTree.TextOfTag(Text, "div", "", (object)null, "class=\"tm\""), "div");
    }

    private static void ParseLocTime(string Text, out string Location, out string Time)
    {
        Location = ParseAddress(Text);
        string[] strs = Location.Split(new char[] { ' ' });
        if (strs.Length > 0)
            Location = AdjLocation(strs[0]);
        else
            Location = "";
        Time = AdjTime(ParseTime(Text));
    }

    private static void GetLowestPrice(out string Price, out string Location, out string Time)
    {
        Price = "";
        Location = "";
        Time = "";
        string text = GetLowGas();
        if (text == "")
            return;
        Price = ParsePrice(text);
        ParseLocTime(text, out Location, out Time);
    }

    public static void LowestPrice(bool Refresh, out string Price, out string Location, out string Time)
    {
        Price = "N/A";
        Location = "";
        Time = "";
        string gas = MyCookie.GetCookie("GasLowest", "");
        if (gas == "" || Refresh)
        {
            string g, l, t;
            GetLowestPrice(out g, out l, out t);
            if (g == "")
                return;
            gas = g + "," + l + "," + t;
            MyCookie.AddCookie("GasLowest", gas, 1800);
        }
        string[] strs = gas.Split(new char[] { ',' });
        if (strs.Length == 3)
        {
            Price = strs[0];
            Location = strs[1];
            Time = strs[2];
        }
    }

    public static DataTable CreateGasTable()
    {
        string[] img = {  "Address", "Price", "TimeWhen" };
        string[] dtype = { "System.String", "System.String", "System.String" };
        DataTable table = BaseDatabase.CreateTable("", "Index", true, img, dtype);
        List<HtmlTag> list = GetGasList();
        foreach (HtmlTag tag in list)
        {
            string s = tag.InnerText();
            DataRow row = table.NewRow();
            row["Address"] = ParseAddress(s).Trim();
            row["Price"] = ParsePrice(s).Trim();
            row["TimeWhen"] = ParseTime(s).Trim();
            table.Rows.Add(row);
        }
        return table;
    }

}
