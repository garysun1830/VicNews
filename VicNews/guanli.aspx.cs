using System;
using ShareLib;

namespace VicNews
{
    public partial class guanli : System.Web.UI.Page
    {

        private CalendarControl _date;

        protected void Page_Init(object sender, EventArgs e)
        {
            _date = new CalendarControl(this, PnDate, "Guanli");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            DateData data = new DateData();
            data.BeginYear = 2011;
            data.EndYear = 2020;
            data.DefaultDate = DateTime.Today;
            data.Date = DateTime.Today;
            _date.Data = data;
            chkManager.Checked = MyCookie.CookieExists("Manager");
        }

        protected void btnCode_Click(object sender, EventArgs e)
        {
            chkManager.Enabled = txtCode.Text == "senderobj";
        }

        protected void btnPaste_Click(object sender, EventArgs e)
        {
            NewsDb.PasteAnything(txtUrl.Text, _date.Date);
            txtUrl.Text = "";

        }

        protected void chkManager_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManager.Checked)
            {
                MyCookie.AddCookie("Manager", "True");
                WebMessageBox.Show("管理员开启。");
            }
            else
            {
                MyCookie.DeleteCookie("Manager");
                WebMessageBox.Show("管理员关闭。");
            }
        }
    }
}