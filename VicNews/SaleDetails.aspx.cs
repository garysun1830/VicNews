using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class SaleDetails : ManagerPageUrl
    {
        private OnSaleDetailDb _db;
        protected bool _isMaster;

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            AClassId = "OnSaleDetail";
            ALoginId = "OnSaleDetail";
            ADbList = Details.Data;
            _db = ClassFactory.CreateDb(AClassId, ALoginId) as OnSaleDetailDb;
            Db = _db;
            _db.SaleId = MyFunc.QueryURL("id", "");
            _isMaster = MyCookie.CookieExists("Manager");
            Details.InitVar(_db, _isMaster, false, MyFunc.GetWebconfigValue("SaleImagePath", ""), "PictureIndex");
            Details.DisplayData += RefreshData;
        }

        protected override void PrepareControl()
        {
            string range, store;
            OnSaleDb.FillInfo(_db.SaleId, out store, out range);
            lblRange.Text = range;
            lblStoreName.Text = store;
            lblPolicy.Text = MyFunc.GetWebconfigValue("OnSaleDetailPolicy", "");
            string url = _db.SourceInfo();
            if (url != "")
                lblSource.Text = string.Format(MyFunc.GetWebconfigValue("SaleSource", ""), url);
        }

        protected override void SetPageTitle()
        {
        }


        protected string DirectPrint(object Text)
        {
            if (Text == null)
                return "";
            return MyFunc.DirectPrintHtml(Text.ToString()) + "<br/>";
        }

        /* protected string GetDetailsImage1(object Image)
         {
             if (Image == null)
                 return "";
             string s = Image.ToString();
             if (string.IsNullOrEmpty(s))
                 return "";
             return "Image/News/" + s;
         }

         private void SaveImage1(HttpPostedFile UpFile)
         {
             if (UpFile.ContentLength == 0)
                 return;
             string fn = Server.MapPath("Image/News/") + BaseDatabase.GetNextId().ToString() + Path.GetExtension(UpFile.FileName);
             UpFile.SaveAs(fn);
             _db.AddPictue(Path.GetFileName(fn), false);
         }*/

        protected void RefreshData(object sender, EventArgs e)
        {
            DisplayData();
        }

    }
}