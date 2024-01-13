using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class VideoDetails : ManagerPage
    {
        private string _id;
        private string _url;
        private int _upTo;

        protected void Page_Init(object sender, EventArgs e)
        {
            _id = MyFunc.QueryURL("id", "");
        }

        protected override void PrepareControl()
        {
            Title = MyFunc.GetSessionData("SiteTitle", "");
            if (_id != "")
                Title = Title + " - " + CommonItemDb.PageTitle("Video") + "【" + lblTitle.Text + "】";

            lblTitle.Text = MyFunc.GetWebconfigValue("VideoPageTitle", "") + "&raquo;" +
                        BaseDatabase.ConvertDbValue(CommonItemDb.GetColValue("video", _id, "Title"), "");
            int total = BaseDatabase.ConvertDbValue(CommonItemDb.GetColValue("video", _id, "Total"), 0);
            _url = BaseDatabase.ConvertDbValue(CommonItemDb.GetColValue("video", _id, "Url"), "");
            _upTo = BaseDatabase.ConvertDbValue(CommonItemDb.GetColValue("video", _id, "UpdateTo"), total);
            VideoList.DataSource = VideoDb.CreateVidoeSource(total);
            VideoList.DataBind();
        }

        protected string VideoNumber(object Number)
        {
            return "第" + BaseDatabase.ConvertDbValue(Number, "") + "集";
        }

        protected string VideoUrl(object Number)
        {
            return string.Format("HostUrl.aspx?pagekind=video&id={0}&src={1}", _id, string.Format(_url, BaseDatabase.ConvertDbValue(Number, 0)));
        }

        protected string LinkClass(object Number)
        {
            int n = BaseDatabase.ConvertDbValue(Number, 0);
            return n > _upTo ? "txt-normal-gray" : "";
        }

        protected override void SetPageTitle()
        {
        }

    }
}