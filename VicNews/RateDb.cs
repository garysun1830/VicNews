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
using System.Collections.Generic;
using ShareLib;

public struct DailyRate
{
    public string Day1;
    public string Day2;
    public string[][] Value;
}

public class RateDb
{
    public static void DisplayData(DataList List)
    {
        SqlParam param = BaseDatabase.GetSqlParam("SelectRate");
        BaseDatabase.FillDataList(List, param);
    }

    private static string GetRateValue(string Text)
    {
        List<HtmlTag> list = HtmlTree.FindTags(Text, "td");
        if (list.Count < 4)
            return "";
        string s = "";
        for (int i = 1; i < 4; i++)
            s += list[i].InnerText() + ",";
        return MyFunc.RemoveCtrlChars(s);
    }

    public static bool ParseRate(string Text, out DailyRate Data)
    {
        Data.Value = null;
        Data.Day1 = "";
        Data.Day2 = "";
        string table = HtmlTree.TextOfTag(Text, "table", "", (object)null, "class='static'", "Exchange Rates");
        if (table == "")
            return false;
        HtmlTree tree = new HtmlTree(table, "th,tr");
        List<HtmlTag> list = tree.FindTags("th");
        if (list.Count < 3)
            return false;
        Data.Day1 = list[1].InnerText().ToLower().Replace("<th>", "").Replace("</th>", "").Trim();
        Data.Day2 = list[2].InnerText().ToLower().Replace("<th>", "").Replace("</th>", "").Trim(); ;
        list = tree.FindTags("tr");
        if (list.Count < 6)
            return false;
        Data.Value = new string[5][];
        for (int i = 0; i < 5; i++)
            Data.Value[i] = new string[4];
        Data.Value[0][0] = "加元/美元闭市汇率";
        Data.Value[1][0] = "加元/美元正午汇率";
        Data.Value[2][0] = "美元/加元正午汇率";
        Data.Value[3][0] = "加元有效汇率指数（CERI）";
        Data.Value[4][0] = "刨除美元后加元有效汇率指数";
        for (int i = 0; i < 5; i++)
        {
            string s = GetRateValue(list[i + 1].InnerText());
            if (s == "")
                return false;
            string[] strs = s.Split(new char[] { ',' });
            if (strs.Length < 3)
                return false;
            for (int j = 1; j < 4; j++)
                Data.Value[i][j] = strs[j - 1];
        }
        return true;
    }

    public static DataTable CreateRateTable()
    {
        string url = MyFunc.GetWebconfigValue("RateDailyDigest", "");
        if (url == "")
            return null;
        string html = MyFunc.ReadWeb("http://www.bankofcanada.ca/rates/daily-digest/");
        DailyRate rate;
        if (!ParseRate(html, out rate))
            return null;
        string[] img = { "Name", "Day1", "Day2", "UpDown" };
        string[] dtype = { "System.String", "System.String", "System.String", "System.String" };
        DataTable table = BaseDatabase.CreateTable("", "Index", true, img, dtype);
        DataRow row = table.NewRow();
        row["Name"] = " 汇率<a href='" + MyFunc.GetWebconfigValue("RateDailyDigest", "") + "' target='_blank'>[查看详情...]</a>";
        row["Day1"] = rate.Day1;
        row["Day2"] = rate.Day2;
        row["UpDown"] = "+/-";
        table.Rows.Add(row);
        foreach (string[] strs in rate.Value)
        {
            row = table.NewRow();
            row["Name"] = strs[0];
            row["Day1"] = strs[1];
            row["Day2"] = strs[2];
            row["UpDown"] = strs[3];
            table.Rows.Add(row);
        }
        return table;
    }

}

