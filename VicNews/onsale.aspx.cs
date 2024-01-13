using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class onsale : ManagerPageUrl
    {
        private OnSaleDb _db;
        protected bool _isMaster;

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            AClassId = "OnSale";
            ALoginId = "OnSale";
            ADbList = RecList;
            _db = ClassFactory.CreateDb(AClassId, ALoginId) as OnSaleDb;
            Db = _db;
            _isMaster = MyCookie.CookieExists("Manager");
        }

        protected override void PrepareControl()
        {
            btnAdd.Visible = _isMaster;
            lblPolicy.Text = MyFunc.GetWebconfigValue("OnSalePolicy", "");
        }

        protected string Logo(object Image)
        {
            string src = BaseDatabase.ConvertDbValue(Image, "");
            if (src == "")
                return "";
            return MyFunc.GetWebconfigValue("StoreLogoPath", "") + src;
        }

        protected string ListImages(object Index)
        {
            return "";
        }

        protected override void EditCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandArgument.ToString() != "Copy")
                return;
            _db.CopySale();
            Edit();
        }

        protected string DisplayRange(object Begin, object End)
        {
            string begin = DisplayDate(Begin).Trim();
            string end = DisplayDate(End).Trim();
            if ((begin + end) != "")
                return begin + " 至 " + end;
            return "";
        }

    }
}