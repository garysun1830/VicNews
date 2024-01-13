using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using ShareLib;

namespace VicNews
{
    public partial class SiteMasterPage : SiteMasterBase
    {
        private bool _isMaster;
        protected string CounterUrl;

        private void Page_Init(object sender, EventArgs e)
        {
            if (!ResetKanting365Cookie())
                SetKanting365Cookie();
            FocusImageBind();
            bool is365 = MyCookie.CookieExists("Kanting365");
            if (is365)
                MyFunc.SaveSessionData("SiteTitle", MyFunc.GetWebconfigValue("SiteTitleKanTing365", ""));
            else
                MyFunc.SaveSessionData("SiteTitle", MyFunc.GetWebconfigValue("SiteTitleVicNews", ""));
        }

        protected override void DoPage_Load(object sender, EventArgs e)
        {
            string kind = MyFunc.QueryURL("PageKind", "").ToLower();
            if (kind != "")
                PageKind = kind;
            bool is365 = MyCookie.CookieExists("Kanting365");
            if (is365)
                CounterUrl = MyFunc.GetWebconfigValue("CounterUrlKanting365", "");
            else
                CounterUrl = MyFunc.GetWebconfigValue("CounterUrlVicNews", "");
        }

        private void SetKanting365Cookie()
        {
            bool is365 = MyCookie.CookieExists("Kanting365");
            if (is365)
                return;
            object o = MyFunc.CurrentRequest().UrlReferrer;
            if (o != null)
                is365 = o.ToString().ToLower().Contains("kanting365.info");
            if (!is365)
                is365 = MyFunc.QueryURL("Kanting365", 0) == 1;
            if (is365)
                MyCookie.AddCookie("Kanting365", true);
        }

        private bool ResetKanting365Cookie()
        {
            object o = MyFunc.CurrentRequest().UrlReferrer;
            if (o == null)
                return false;
            bool isvicnews = o.ToString().ToLower().Contains("vicnewscn.com");
            if (isvicnews)
                MyCookie.DeleteCookie("Kanting365");
            return isvicnews;
        }

        public void FocusImageBind()
        {
            focusImage.DataSource = NewsDb.GetFocusImages();
            focusImage.DataBind();
        }

        private string PageKind
        {
            get { return MyFunc.GetObjectValue(ViewState["PageKind"], ""); }
            set { if (value != "") ViewState["PageKind"] = value; }
        }

        protected override void PrepareControls()
        {
            _isMaster = MyCookie.CookieExists("Manager");
            bool is365 = MyCookie.CookieExists("Kanting365");
            imgLogo.Visible = !is365;
            imgLogo365.Visible = is365;
            pnlWeather.Visible = !is365;
            pnlGas.Visible = !is365;
            FillMasterMenu();
            FillMasterMenu2(!is365);
            if (is365)
                return;
            lblPolicy.Text = MyFunc.GetWebconfigValue("MasterPolicy", "");
            DisplayGas(false);
            DisplayRate();
            PageUpdate();
        }

        private void PageUpdate()
        {
            switch (Request.AppRelativeCurrentExecutionFilePath)
            {
                case "~/gasprice.aspx":
                    DisplayGas(true);
                    break;
            }
        }

        private void DisplayGas(bool Refresh)
        {
            string price, location, time;
            Gas.LowestPrice(Refresh, out price, out location, out time);
            lblGas.Text = price;
            lblGasLoc.Text = location + " " + time;
        }

        private void DisplayRate()
        {
            RateDb.DisplayData(dtRate);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            DisplayGas(true);
        }

        protected string PrintRate(object Rate, object Unit)
        {
            string r = BaseDatabase.ConvertDbValue(Rate, "N/A");
            if (r == "N/A")
                return r;
            string u = BaseDatabase.ConvertDbValue(Unit, "");
            if (u == "")
                return "N/A";
            decimal f = 0;
            if (!decimal.TryParse(r, out f))
                return "N/A";
            f = Math.Round(f, 2);
            return f.ToString() + u;
        }

        private string GetMenuText(string Sql)
        {
            if (!_isMaster)
                Sql = string.Format(Sql, "AND (Kind!='Master' OR Kind IS NULL)");
            else
                Sql = Sql.Replace("{0}", "");
            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellpadding='0' cellspacing='0'>\n<tr>\n");
            SqlParam param = BaseDatabase.GetSqlParamDirect(Sql);
            DataRowCollection rows = BaseDatabase.GetDataRows(param);
            foreach (DataRow row in rows)
            {
                string url = BaseDatabase.GetRowValue(row, "PageUrl", "");
                string text = BaseDatabase.GetRowValue(row, "BlockText", "");
                string aspx = Regex.Replace(Request.ServerVariables["URL"].ToLower(), "/[\\w\\d]+/", "");
                string urlkind = "";
                Match m = Regex.Match(url, "pagekind=\\w+", RegexOptions.IgnoreCase);
                if (m.Success)
                    urlkind = Regex.Replace(m.Value, "pagekind=", "", RegexOptions.IgnoreCase);
                bool sel = url.ToLower().Contains(aspx);
                if (sel && PageKind != string.Empty)
                    sel = string.Compare(PageKind, urlkind, true) == 0;
                if (!sel && PageKind == urlkind)
                    sel = true;
                if (sel)
                    sb.Append("<td class='tab-selected' style='font-size:1.2em'>\n");
                else
                    sb.Append("<td class='tab-unselected' style='font-size:1.2em'>\n");
                sb.Append("<a href='");
                url = url.Replace("~/", "");
                url = Utility.MakeRealUrl(url, false);
                sb.Append(url);
                sb.Append("'>");
                sb.Append(text);
                sb.Append("</a>\n");
                sb.Append("</td>\n");
            }
            sb.Append("</tr>\n</table>\n");
            return sb.ToString();
        }

        public void FillMasterMenu()
        {
            string sql = BaseDatabase.GetSqlText("SelectBlock");
            lblMenu.Text = GetMenuText(sql);
        }

        public void FillMasterMenu2(bool ShowMenu)
        {
            lblMenu2.Visible = ShowMenu;
            if (!ShowMenu)
                return;
            string sql = BaseDatabase.GetSqlText("SelectBlock2");
            lblMenu2.Text = GetMenuText(sql);
        }

    }
}