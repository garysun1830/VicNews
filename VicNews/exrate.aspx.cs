using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class exrate : ManagerPage
    {
        private bool _classSet;

        protected override void PrepareControl()
        {
            RateList.DataSource = RateDb.CreateRateTable();
            RateList.DataBind();
            lblSource.Text = HtmlTree.UserUrlToRealUrl(MyFunc.GetWebconfigValue("RateSource", ""));
        }

        protected override void SetPageTitle()
        {
            Title = MyFunc.GetSessionData("SiteTitle", "") + " - " + CommonItemDb.PageTitle("Rate");
        }

        protected override string RenderHtml(string Html)
        {
            string s = MyFunc.GetWebconfigValue("SepHtml", "");
            int i = Html.LastIndexOf(s);
            if (i != -1)
                Html = Html.Remove(i, s.Length);
            Html = HtmlTree.RemoveBlankHtml(Html, "td,tr,table,span");
            return Html;
        }

        protected void RateList_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (_classSet)
                return;
            foreach (Control c in e.Item.Controls)
            {
                Label lbl = c as Label;
                if (lbl == null)
                    continue;
                lbl.CssClass = "txt-label";
            }
            _classSet = true;
        }

    }
}