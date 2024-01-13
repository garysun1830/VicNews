using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ShareLib;

public partial class HostUrl : ManagerPage
{
    protected string _src;
    protected string _frameHeight;

    protected void Page_Init(object sender, EventArgs e)
    {
        Kind = MyFunc.QueryURL("pagekind", "");
        Id = MyFunc.QueryURL("id", "");
        Src = MyFunc.QueryURL("src", "");
        _src = Src;
        Title = MyFunc.GetSessionData("SiteTitle", "");
        Title = Title + " - " + CommonItemDb.PageTitle(Kind);
        if (Id != "")
            Title = Title + "【" +
                BaseDatabase.ConvertDbValue(CommonItemDb.GetColValue(Kind, Id, "Title"), "") + "】";
        _frameHeight = MyFunc.GetWebconfigValue(Kind + "FrameHeight", "1000px");
    }

    protected override void SetPageTitle()
    {
    }

    private string Kind
    {
        get { return MyFunc.GetObjectValue(ViewState["Kind"],""); }
        set { if (value != "")ViewState["Kind"]= value; }
    }

    private string Id
    {
        get { return MyFunc.GetObjectValue(ViewState["Id"],""); }
        set { if (value != "")ViewState["Id"]= value; }
    }

    private string Src
    {
        get { return MyFunc.GetObjectValue(ViewState["Src"],""); }
        set { if (value != "")ViewState["Src"]= value; }
    }

}
