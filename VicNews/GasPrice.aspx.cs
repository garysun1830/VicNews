using ShareLib;

namespace VicNews
{
    public partial class GasPrice : ManagerPage
    {
        protected string _chart;

        protected override void PrepareControl()
        {
            _chart = Gas.GasChart();
            GasList.DataSource = Gas.CreateGasTable();
            GasList.DataBind();
            lblSource.Text = HtmlTree.UserUrlToRealUrl(MyFunc.GetWebconfigValue("GasSource", ""));
        }

        protected override void SetPageTitle()
        {
            Title = MyFunc.GetSessionData("SiteTitle", "") + " - " + CommonItemDb.PageTitle("Gas");
        }

        protected override string RenderHtml(string Html)
        {
            Html = HtmlTree.RemoveBlankHtml(Html, "td,tr,table,span");
            string s = MyFunc.GetWebconfigValue("SepHtml", "");
            int i = Html.LastIndexOf(s);
            if (i != -1)
                Html = Html.Remove(i, s.Length);
            return Html;
        }

    }
}