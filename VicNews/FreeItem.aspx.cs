using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class FreeItem : ManagerPagedPage
    {
        private FreeItemDb _db;
        protected bool _isMaster;
        private bool _newKind;

        protected void Page_Init(object sender, EventArgs e)
        {
            _isMaster = MyCookie.CookieExists("Manager");
        }

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            AClassId = "FreeItem";
            ALoginId = "FreeItem";
            ADbList = RecList;
            _db = ClassFactory.CreateDb(AClassId, ALoginId) as FreeItemDb;
            Db = _db;
            string itemkind = MyFunc.QueryURL("ItemKind", "");
            if (itemkind != "")
            {
                _newKind = _db.Kind != itemkind;
                _db.Kind = itemkind;
            }
            PageBarData data;
            data.Base = new Panel[1];
            data.Base[0] = pnPageBar;
            data.ItemPerPage = MyFunc.GetWebconfigValue("FreeItem_ItemPerPage", 12);
            data.PageListCount = MyFunc.GetWebconfigValue("FreeItem_PageListCount", 10);
            data.Align = string.Empty;
            data.Parent = this;
            data.Db = (IPageDb)_db;
            CreatePageBar(data);
            MaintainScrollPositionOnPostBack = false;
        }

        protected override void PrepareControl()
        {
            lblKind.Text = FreeItemDb.FillKindMenu(_db.Kind);
            FreeItemDb.FillCategory(drCategory, false);
            drCategory.SelectedIndex = 0;
            pnTrans.Visible = _isMaster;
            linkSrc.NavigateUrl = MyFunc.GetWebconfigValue("UsedVictoriaFreeItem", "");
            lblPolicy.Text = MyFunc.GetWebconfigValue("FreeItemPolicy", "");
            if (_newKind)
                FirstPage();
        }

        protected override void SetPageTitle()
        {
            Title = MyFunc.GetSessionData("SiteTitle", "") + " - " + CommonItemDb.PageTitle("FreeItem");
        }

        protected string DirectPrint(object Text)
        {
            string text = BaseDatabase.ConvertDbValue(Text, "");
            if (text == "")
                return "";
            return MyFunc.DirectPrintHtml(text) + "<br/>";
        }

        protected override void ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (!_isMaster)
                return;
            EditText edit = e.Item.FindControl("edtTitle") as EditText;
            if (edit == null)
                return;
            edit.SetButtons(ButtonPosition.Right, true, false);
        }

        protected override void ItemClick(DataListCommandEventArgs e)
        {
            if (e.CommandArgument.ToString() == "EditItem")
            {
                EditText edit = (EditText)e.Item.FindControl("edtTitle");
                _db.EditTitle(edit.EditingText);
                DisplayData();
            }
        }

        protected string ImageSrc(object Image)
        {
            return BaseDatabase.ConvertDbValue(Image, MyFunc.GetWebconfigValue("NoImage", ""));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            _db.SaveTrans(txtEng.Text.Trim(), txtCh.Text.Trim());
            txtEng.Text = "";
            txtCh.Text = "";
            DisplayData();
        }

        protected string Translate(object Text)
        {
            return _db.Translate(BaseDatabase.ConvertDbValue(Text, ""));
        }

        protected void chkSimple_CheckedChanged(object sender, EventArgs e)
        {
            _db.Simple = chkSimple.Checked;
            DisplayData();
        }

        protected void chkCh_CheckedChanged(object sender, EventArgs e)
        {
            _db.Chinese = chkCh.Checked;
            DisplayData();
        }

        protected override void ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DropDownList dr = e.Item.FindControl("drCategory") as DropDownList;
            FreeItemDb.FillCategory(dr, true);
            DbDataRecord rd = (DbDataRecord)e.Item.DataItem;
            dr.SelectedValue = BaseDatabase.ConvertDbValue(rd["Category"], "0");
            dr.Attributes["1"] = RecList.DataKeys[e.Item.ItemIndex].ToString();
        }

        protected void drCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dr = sender as DropDownList;
            FreeItemDb.ChangeCategory(dr.SelectedValue, Convert.ToInt32(dr.Attributes["1"]));
            DisplayData();
        }

        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            string cat = txtCategory.Text.Trim();
            if (cat == "")
                return;
            if (FreeItemDb.CategoryWordExists(cat))
            {
                WebMessageBox.Show("关键词已经存在。");
                return;
            }
            FreeItemDb.AddCategoryWord(cat, drCategory.SelectedValue);
            txtCategory.Text = "";
            DisplayData();
        }

        protected string StayDays(object BeginDate)
        {
            DateTime bgn = BaseDatabase.ConvertDbValue(BeginDate, DateTime.Today);
            TimeSpan span = DateTime.Today.Subtract(bgn);
            return string.Format("广告已持续：{0}天", span.Days);
        }

        protected override string RenderHtml(string Html)
        {
            return HtmlTree.RemoveBlankHtml(Html, "td,tr,table,span");
        }

        protected string Url(object ItemUrl)
        {
            return string.Format("HostUrl.aspx?pagekind=freeitem&src={0}", ItemUrl);
        }

    }
}