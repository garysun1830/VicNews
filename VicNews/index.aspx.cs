using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class index : ManagerPagedPage
    {
        private NewsDb _db;
        protected bool _isMaster;
        protected string _menuWidth;
        private CalendarControl _beginDate;
        private bool _newKind;

        protected void Page_Init(object sender, EventArgs e)
        {
            _isMaster = MyCookie.CookieExists("Manager");
            detNews.IsMaster = _isMaster;
            _menuWidth = "";
            if (MyFunc.GetBrowserName() == BrowserKind.Chrome)
                _menuWidth = "style='width:100px'";
            _beginDate = new CalendarControl(this, pnDate, "News");
            _beginDate.DateChanged += CalendarChanged;
        }

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            AClassId = "News";
            ALoginId = "News";
            _db = ClassFactory.CreateDb(AClassId, ALoginId) as NewsDb;
            Db = _db;
            ((IBasicListDb)_db).Parent = this;
            detNews.SetDb(_db);
            ADbList = detNews.NewsData;
            string kind = MyFunc.QueryURL("itemkind", "");
            if (kind != "")
            {
                _newKind = _db.Kind != kind;
                _db.Kind = kind;
            }
            PageBarData data;
            data.Base = new Panel[1];
            data.Base[0] = pnPageBar;
            data.PageListCount = MyFunc.GetWebconfigValue("News_PageListCount", 10);
            data.Parent = this;
            data.Align = string.Empty;
            data.Db = (IPageDb)_db;
            data.ItemPerPage = _db.TextKind == 0 ? MyFunc.GetWebconfigValue("NewsTextItemPerPage", 0) :
                                                                                                                     MyFunc.GetWebconfigValue("NewsTitleItemPerPage", 0);
            CreatePageBar(data);
        }

        protected override void SetPageTitle()
        {
            Title = MyFunc.GetSessionData("SiteTitle", "") + " - " + CommonItemDb.PageTitle("News");
        }

        protected override void PrepareControl()
        {
            DateData data = new DateData();
            data.BeginYear = 2011;
            data.EndYear = 2020;
            data.ReadOnly = _db.Period == "0";
            data.DefaultDate = DateTime.Today;
            data.Date = _db.BeginDate;
            _beginDate.Data = data;
            btnListTitle.Visible = _isMaster;
            drPublish.Visible = _isMaster;
            drPeriod.SelectedValue = _db.Period;
            txtSearch.Text = MyFunc.UrlDecode(_db.KeyWord);
            switch (_db.Publish)
            {
                case PublishKind.None:
                    drPublish.SelectedIndex = 0;
                    break;
                case PublishKind.Publish:
                    drPublish.SelectedIndex = 1;
                    break;
                case PublishKind.OnHold:
                    drPublish.SelectedIndex = 2;
                    break;
            }
            drPublish.SelectedValue = _db.Publish.ToString();
            btnListTitle.OnClientClick = "copyToClipboard('" + hdTitle.ClientID + "');";
            lblTop.Text = "置顶";
            lblAreaMenu.Text = _db.FillKindMenu();
            btnFull.Checked = _db.TextKind == ItemTextKind.Full;
            btnTitle.Checked = _db.TextKind == ItemTextKind.Title;
            lblCaption.Text = CommonItemDb.PageTitle("News") + "&raquo;" + _db.KindText();
            if (_newKind)
                FirstPage();
        }

        protected override void AddClientDeleteConfirm(DataListItemEventArgs e)
        {
        }

        private void CalendarChanged(object sender, EventArgs e)
        {
            _db.BeginDate = _beginDate.Date;
            ResetPage();
        }

        protected void drPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            _db.Period = drPeriod.SelectedValue;
            DateData data = _beginDate.Data;
            data.ReadOnly = _db.Period == "0";
            _beginDate.Data = data;
            ResetPage();
        }

        public override void DisplayData()
        {
            Views.SetActiveView(_db.TextKind == ItemTextKind.Full ? viewText : viewTitle);
            base.DisplayData();
            if (_db.TextKind == ItemTextKind.Full)
                _db.DisplayTop(TopList);
            else
                _db.Display(TitleList);
            hdTitle.Value = _db.ListTitle();
            detNews.DisplayData();
            pnTop.Visible = TopList.Items.Count > 0;
        }

        protected string NewsUrl(object Title, object NewsId, object News)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<a href=\"");
            int i = BaseDatabase.ConvertDbValue(News, 0);
            if (i == 1)
                sb.Append("NewsDetails");
            else
                sb.Append("SaleDetails");
            sb.Append(".aspx?id=");
            sb.Append(NewsId.ToString());
            sb.Append("\" ");
            sb.Append("BorderWidth=\"0px\" ");
            sb.Append("BorderStyle=\"None\" ");
            sb.Append("target=\"_blank\">");
            sb.Append(BaseDatabase.ConvertDbValue(Title, ""));
            sb.Append("</a>");
            return sb.ToString();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _db.KeyWord = MyFunc.UrlEncode(txtSearch.Text.Trim());
            ResetPage();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtSearch_TextChanged(sender, e);
        }

        protected void TopList_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (!_isMaster)
                return;
            EditText edit = e.Item.FindControl("edtRemoveTop") as EditText;
            if (edit == null)
                return;
            edit.SetButtons(ButtonPosition.Read, false, false);
        }

        protected void TopList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int index = Convert.ToInt32(TopList.DataKeys[e.Item.ItemIndex]);
            switch (e.CommandArgument.ToString())
            {
                case "DeleteItem":
                    _db.RemoveTop(index);
                    break;
                default:
                    return;
            }
            DisplayData();
        }

        protected override void ItemClick(DataListCommandEventArgs e)
        {
            if (!detNews.ItemCommand(e))
                return;
            ((SiteMasterPage)Master).FocusImageBind();
            DisplayData();
        }

        protected void drPublish_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drPublish.SelectedValue == "Published")
                _db.Publish = PublishKind.Publish;
            else if (drPublish.SelectedValue == "Unpublished")
                _db.Publish = PublishKind.OnHold;
            else
                _db.Publish = PublishKind.None;
            ResetPage();
        }

        protected void btnFull_CheckedChanged(object sender, EventArgs e)
        {
            _db.TextKind = ItemTextKind.Full;
            ChangePagebarLine(MyFunc.GetWebconfigValue("NewsTextItemPerPage", 0));
            ResetPage();
        }

        protected void btnTitle_CheckedChanged(object sender, EventArgs e)
        {
            _db.TextKind = ItemTextKind.Title;
            ChangePagebarLine(MyFunc.GetWebconfigValue("NewsTitleItemPerPage", 0));
            ResetPage();
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

        protected override void ItemCreated(object sender, DataListItemEventArgs e)
        {
            detNews.ItemCreated(e);
        }

        protected override void ItemDataBound(object sender, DataListItemEventArgs e)
        {
            detNews.ItemDataBound(e);
        }

    }
}
