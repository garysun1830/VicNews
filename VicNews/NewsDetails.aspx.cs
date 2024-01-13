using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class NewsDetails : ManagerPage
    {
        private NewsPictureDb _db;
        protected bool _isMaster;

        protected void Page_Init(object sender, EventArgs e)
        {
            _isMaster = MyCookie.CookieExists("Manager");
        }

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            AClassId = "NewsPicture";
            ALoginId = "NewsPicture";
            ADbList = Details.Data;
            _db = ClassFactory.CreateDb(AClassId, ALoginId) as NewsPictureDb;
            Db = _db;
            _db.NewsId = MyFunc.QueryURL("id", 0);
            Details.InitVar(_db, _isMaster, false, MyFunc.GetWebconfigValue("NewsImagePath", ""), "PictureIndex");
            Details.DisplayData += RefreshData;
        }

        protected override void PrepareControl()
        {
            string date, title, detail, img;
            bool hideimg;
            _db.FillInfo(out date, out title, out detail, out img, out hideimg);
            lblDate.Text = date;
            lblMainTitle.Text = title;
            lblDetails.Text = detail;
            imgMain.ImageUrl = img;
            imgMain.Visible = !hideimg || _isMaster;
            chkHide.Visible = _isMaster;
            chkHide.Checked = hideimg;
        }

        protected override void SetPageTitle()
        {
        }

        protected void RefreshData(object sender, EventArgs e)
        {
            DisplayData();
        }

        protected override string RenderHtml(string Html)
        {
            return HtmlTree.RemoveBlankImage(Html);
        }

        protected void chkHide_CheckedChanged(object sender, EventArgs e)
        {
            _db.HideMainImage(chkHide.Checked);
        }

    }
}