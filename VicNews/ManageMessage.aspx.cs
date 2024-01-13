using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class ManageMessage : ManagerPage
    {
        private CustMessageDb _db;
        protected bool _isMaster;

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            _isMaster = MyCookie.CookieExists("Manager");
            if (!_isMaster)
                throw new Exception("非法地址！");
            AClassId = "CustMessage";
            ALoginId = "CustMessage";
            ADbList = RecList;
            _db = ClassFactory.CreateDb(AClassId, ALoginId) as CustMessageDb;
            Db = _db;
        }

        protected override void SetPageTitle()
        {
            Title = MyFunc.GetSessionData("SiteTitle", "") + " - " + CommonItemDb.PageTitle("Manager");
        }

        protected string DirectPrint(object Text)
        {
            return MyFunc.DirectPrintHtml(BaseDatabase.ConvertDbValue(Text, "")) + "<br/>";
        }

    }
}