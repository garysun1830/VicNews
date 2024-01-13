using System;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class contact : ManagerPage
    {
        private CustMessageDb _db;

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            _db = new CustMessageDb("CustMessage", "CustMessage");
        }

        protected override void SetPageTitle()
        {
            Title = MyFunc.GetSessionData("SiteTitle", "") + " - " + CommonItemDb.PageTitle("Contact");
        }

        protected void btnContact_Click(object sender, EventArgs e)
        {
            _db.AddMessage(txtMessage.Text.Trim());
            lblSent.Visible = true;
        }

    }
}