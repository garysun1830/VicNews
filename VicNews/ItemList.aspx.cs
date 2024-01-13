using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public enum CommonViewKind { Full, Short, Title };

    public partial class ItemList : ManagerPagedPage
    {
        private IBasicListDb _db;
        private IManagerDb _dbPage;
        public  bool _isMaster;
        protected string IsMaster { get { return _isMaster ? "true" : "false"; } }
        protected string _dataKeyCol;
        protected string _dataTextCol;
        protected string _dataTitleCol;
        protected string _dataImageCol;
        protected string _dataExtraTitleCol;
        protected string _imagePath;
        private bool _newKind;
        protected bool _hasDate;
        protected bool _noFooter;
        protected bool _noImage;
        protected bool _canAdd;
        protected bool _hasExtra;
        protected bool _hasExtraTitle;
        protected bool _showLink;
        protected bool _hasLink;
        protected bool _showText;
        protected bool _showMore;
        protected bool _showImage;
        protected bool _showMenu2;
        protected bool _titleReadOnly;
        protected bool _showTextKind;
        protected bool _showSubmit;
        protected bool _showShortList;
        protected CommonViewKind _viewKind;

        protected CommonViewKind ViewKind
        {
            get
            {
                if (_db.TextKind == ItemTextKind.Title)
                    return CommonViewKind.Title;
                if (_isMaster)
                    return CommonViewKind.Full;
                if (_showShortList)
                    return CommonViewKind.Short;
                return CommonViewKind.Full;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            string pagekind = MyFunc.QueryURL("PageKind", "");
            if (pagekind != "")
                PageKind = pagekind;
            _isMaster = MyCookie.CookieExists("Manager");
            _dataKeyCol = MyFunc.GetWebconfigValue(PageKind + "KeyCol", PageKind + "Index");
            _dataTitleCol = MyFunc.GetWebconfigValue(PageKind + "TitleCol", "Title");
            _dataTextCol = MyFunc.GetWebconfigValue(PageKind + "TextCol", "Text");
            _dataImageCol = MyFunc.GetWebconfigValue(PageKind + "ImageCol", "Image");
            _dataExtraTitleCol = MyFunc.GetWebconfigValue(PageKind + "ExtraTitleCol", "");
            _imagePath = GetImagePath();
            _hasDate = MyFunc.GetWebconfigValue(PageKind + "HasDate", false);
            _noFooter = MyFunc.GetWebconfigValue(PageKind + "NoFooter", true);
            _noImage = MyFunc.GetWebconfigValue(PageKind + "NoImage", false);
            _canAdd = MyFunc.GetWebconfigValue(PageKind + "CanAdd", true);
            _hasExtra = MyFunc.GetWebconfigValue(PageKind + "HasExtra", false);
            _showLink = MyFunc.GetWebconfigValue(PageKind + "ShowLink", false);
            _hasLink = MyFunc.GetWebconfigValue(PageKind + "HasLink", false);
            _showMore = MyFunc.GetWebconfigValue(PageKind + "ShowMore", true);
            _showText = MyFunc.GetWebconfigValue(PageKind + "ShowText", true);
            _showImage = MyFunc.GetWebconfigValue(PageKind + "ShowImage", true);
            _showMenu2 = MyFunc.GetWebconfigValue(PageKind + "ShowMenu2", false);
            _showTextKind = MyFunc.GetWebconfigValue(PageKind + "ShowTextKind", true);
            _titleReadOnly = MyFunc.GetWebconfigValue(PageKind + "TitleReadOnly", false);
            _hasExtraTitle = MyFunc.GetWebconfigValue(PageKind + "HasExtraTitle", false);
            _showSubmit = MyFunc.GetWebconfigValue(PageKind + "ShowSubmit", false);
            _showShortList = MyFunc.GetWebconfigValue(PageKind + "ShowShortList", false);
        }

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            AClassId = MyFunc.GetWebconfigValue(PageKind + "ClassId", PageKind);
            ALoginId = AClassId;
            object db = ClassFactory.CreateDb(AClassId, ALoginId);
            _db = db as IBasicListDb;
            _db.Parent = this;
            Db = db as IManagerDb;
            _dbPage = Db;
            ChangeDataList();
            string itemkind = MyFunc.QueryURL("ItemKind", "");
            if (itemkind != "")
            {
                _newKind = _db.Kind != itemkind;
                _db.Kind = itemkind;
            }
            PageBarData data;
            data.Base = new Panel[2];
            data.Base[0] = barTop;
            data.Base[1] = barBot;
            data.PageListCount = MyFunc.GetWebconfigValue("News_PageListCount", 10);
            data.Align = "right";
            data.Parent = this;
            data.Db = (IPageDb)_db;
            data.ItemPerPage = _db.TextKind == ItemTextKind.Full ? MyFunc.GetWebconfigValue(PageKind + "ItemPerPage", 0) : CalItemPerPage();
            CreatePageBar(data);
        }

        private void ChangeDataList()
        {
            switch (ViewKind)
            {
                case CommonViewKind.Title:
                    DbList = TitleList;
                    mvData.SetActiveView(viewTitle);
                    break;
                case CommonViewKind.Short:
                    DbList = IconList;
                    mvData.SetActiveView(viewIcon);
                    break;
                case CommonViewKind.Full:
                    DbList = listItems;
                    mvData.SetActiveView(viewFull);
                    break;
            }
        }

        private int CalItemPerPage()
        {
            int line = MyFunc.GetWebconfigValue("TitlePageLine", 0);
            int col = MyFunc.GetWebconfigValue(PageKind + "RepeatColumns", 6);
            return line * col;
        }

        private string PageKind
        {
            set { ViewState["PageKind"] = value; }
            get { return MyFunc.GetObjectValue(ViewState["PageKind"], ""); }
        }

        private string NewItemData
        {
            set { ViewState["Data"] = value; }
            get { return MyFunc.GetObjectValue(ViewState["Data"], ""); }
        }

        protected override void SetPageTitle()
        {
            Title = MyFunc.GetSessionData("SiteTitle", "") + " - " + CommonItemDb.PageTitle(PageKind);
        }

        protected override void PrepareControl()
        {
            drPublish.Visible = _isMaster;
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
            lblCaption1.Text = CommonItemDb.PageTitle(PageKind) + "&raquo;" + _db.KindText();
            lblCaption2.Text = lblCaption1.Text;
            lblPolicy.Text = MyFunc.GetWebconfigValue(PageKind + "Policy", "");
            lblKind.Text = _db.FillKindMenu();
            lblKind2.Visible = _showMenu2;
            if (_showMenu2)
                lblKind2.Text = _db.FillKindMenu2();
            txtSearch.Text = _db.KeyWord;
            btnTxt1.Visible = MyFunc.GetWebconfigValue(PageKind + "ShowTextOption", false);
            btnTxt2.Visible = MyFunc.GetWebconfigValue(PageKind + "ShowTextOption", false);
            btnTxt1.Text = MyFunc.GetWebconfigValue(PageKind + "TextOption1", "");
            btnTxt2.Text = MyFunc.GetWebconfigValue(PageKind + "TextOption2", "");
            btnFull.GroupName = PageKind;
            btnTitle.GroupName = PageKind;
            btnFull.Checked = _db.TextKind == ItemTextKind.Full;
            btnTitle.Checked = _db.TextKind == ItemTextKind.Title;
            pnAdd.Visible = _isMaster && _canAdd & btnFull.Checked;
            lblExtra.Visible = _hasExtra;
            txtExtra.Visible = _hasExtra;
            btnExtra.Visible = _hasExtra;
            if (_hasExtra)
            {
                lblExtra.Text = MyFunc.GetWebconfigValue(PageKind + "ExtraLabel", "");
                btnExtra.Text = MyFunc.GetWebconfigValue(PageKind + "ExtraButton", "");
            }
            TitleList.RepeatColumns = MyFunc.GetWebconfigValue(PageKind + "RepeatColumns", 6);
            if (_showTextKind)
                mvCapBar.SetActiveView(viewTextKind);
            else
                mvCapBar.SetActiveView(viewCaption);
            if (_newKind)
                FirstPage();
            if (pnAdd.Visible)
            {
                _db.FillKind(drKind.Items);
                drKind.SelectedValue = _db.Kind;
            }
            if (_newKind)
                FirstPage();
        }

        public override void DisplayData()
        {
            base.DisplayData();
            lblNoRecord.Visible = DbList.Items.Count == 0;
            listItems.Visible = !lblNoRecord.Visible;
            IconList.Visible = !lblNoRecord.Visible;
        }

        protected override void AddClientDeleteConfirm(DataListItemEventArgs e)
        {
        }

        private string GetImagePath()
        {
            string kind = PageKind;
            string subst = MyFunc.GetWebconfigValue(kind + "ImageEqualTo", "");
            if (subst != "")
                kind = subst;
            string result = MyFunc.GetWebconfigValue("ImageRoot", "") + kind + "/";
            return Utility.MakeRealUrl(result, true);
        }

        protected bool GetImageVisible(object Image)
        {
            if (!_showImage)
                return false;
            if (_isMaster)
                return true;
            return BaseDatabase.ConvertDbValue(Image, "").Trim() != "";
        }

        protected int NewsWidth(object Image)
        {
            if (GetImageVisible(Image))
                return 800;
            else
                return 900;
        }

        protected string NewsTDStyle(object Image)
        {
            return "width:" + NewsWidth(Image).ToString() + "px";
        }

        protected string NewsEditWidth(object Image)
        {
            return NewsWidth(Image).ToString();
        }

        protected string GetImageHRef(object Id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_db.GetTargetPage(Id.ToString()));
            sb.Append("?id=");
            sb.Append(Id.ToString());
            sb.Append("&pagekind=");
            sb.Append(PageKind);
            if (_noFooter)
                sb.Append("&nofooter=1");
            if (_noImage)
                sb.Append("&noimage=1");
            if (_showLink)
                sb.Append("&link=1");
            return Utility.MakeRealUrl(sb.ToString(), false);
        }

        protected string GetImageThumb(object Image)
        {
            string result = BaseDatabase.ConvertDbValue(Image, "");
            if (result == "")
                return "";
            return _imagePath + result.Replace(".", "th.");
        }

        protected override string RenderHtml(string Html)
        {
            Html = HtmlTree.RemoveBlankHtml(Html, "td,tr,table,td,tr,table");
            Html = HtmlTree.RemoveBlankHtml(Html, "span");
            string s = MyFunc.GetWebconfigValue("SepHtml", "");
            int i = Html.LastIndexOf(s);
            if (i != -1)
                Html = Html.Remove(i, s.Length);
            return Html;
        }

        protected string PrintSep()
        {
            return MyFunc.GetWebconfigValue("SepHtml", "");
        }

        protected string DirectPrint(object Text, object Simple)
        {
            string text = BaseDatabase.ConvertDbValue(Text, string.Empty);
            if (text == string.Empty)
                return string.Empty;
            if (Simple == null)
                return string.Empty;
            text = MyFunc.DirectPrintHtml(text);
            if (BaseDatabase.ConvertDbValue(Simple, false))
            {
                int len = MyFunc.GetWebconfigValue("SimpleTextLength", 100);
                if (text.Length > len)
                    text = text.Substring(0, len) + "...";
            }
            return text + "<br/>";
        }

        protected string PrintMore(object Id)
        {
            string href = GetImageHRef(Id);
            return string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", href, MyFunc.GetWebconfigValue(PageKind + "ReadMore", "【阅读全文...】"));
        }

        protected string PrintLine()
        {
            return "<br>";
        }

        private void DeleteImage(DataListCommandEventArgs e)
        {
            EditImage editImage = (EditImage)e.Item.FindControl("EditImage");
            string fn = UploadImage.RemoveTh(editImage.PrevImageUrl);
            if (string.IsNullOrEmpty(fn))
                return;
            string imgPath = Server.MapPath(Utility.RemoveDomain(_imagePath));
            fn = Path.Combine(imgPath, Path.GetFileName(fn));
            try
            {
                File.Delete(fn);
            }
            catch { }
            try
            {
                File.Delete(UploadImage.AddTh(fn));
            }
            catch { }
            _db.DeleteImage();
        }

        private void DoUploadImage(DataListCommandEventArgs e)
        {
            EditImage editImage = (EditImage)e.Item.FindControl("EditImage");
            editImage.Db = _db;
            editImage.ImagePath = Utility.RemoveDomain(_imagePath);
            if (editImage.PendingFile == "" && editImage.PasteUrl.Trim() == string.Empty)
            {
                string img = _db.GetAutoImage();
                if (!string.IsNullOrEmpty(img))
                    editImage.Upload(img);
            }
            else
                editImage.Upload();
        }

        protected override void ItemClick(DataListCommandEventArgs e)
        {
            switch (e.CommandArgument.ToString())
            {
                case "DeleteImage":
                    DeleteImage(e);
                    break;
                case "EditItem":
                    {
                        if (e.CommandName == "Right")
                        {
                            EditText edit = (EditText)e.Item.FindControl("edtTitle");
                            _db.EditTitle(edit.EditingText);
                        }
                        if (e.CommandName == "Top")
                        {
                            EditText edit = (EditText)e.Item.FindControl("edItemText");
                            _db.EditText(edit.EditingText);
                        }
                        break;
                    }
                case "DeleteItem":
                    _db.DeleteItem();
                    break;
                case "UploadImage":
                    DoUploadImage(e);
                    break;
                case "LiveImage":
                    _db.LiveImage();
                    break;
                case "Submit":
                    _db.Submit();
                    break;
                default:
                    return;
            }
            ((SiteMasterPage)Master).FocusImageBind();
            DisplayData();
        }

        protected override void ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (!_isMaster)
                return;
            EditText edit = (EditText)e.Item.FindControl("edtTitle");
            if (edit != null)
            {
                if (_titleReadOnly)
                    edit.SetButtons(ButtonPosition.Read, true, false);
                else
                    edit.SetButtons(ButtonPosition.Right, true, false);
            }
            edit = (EditText)e.Item.FindControl("edItemText");
            if (edit != null)
                edit.SetButtons(ButtonPosition.Top, true, true);
        }

        protected void btnNewItem_Click(object sender, EventArgs e)
        {
            _db.AddItem(DateTime.Today, txtTitle.Text.Trim(), txtBody.Text.Trim(), txtExtra.Text, drKind.SelectedValue, NewItemData);
            ((SiteMasterPage)Master).FocusImageBind();
            DisplayData();
            txtTitle.Text = "标题";
            txtBody.Text = "正文";
            txtExtra.Text = "";
        }

        protected override void ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (_hasDate)
            {
                Label lbl = (Label)e.Item.FindControl("lblDate");
                lbl.Text = DisplayDate(BaseDatabase.GetDataListItemDbValue(lbl, "DayDate", DateTime.Today));
            }
            if (_hasLink)
            {
                EditText edit = (EditText)e.Item.FindControl("edtTitle");
                if (edit != null)
                {
                    edit.NavigateUrl = BaseDatabase.GetDataListItemDbValue(edit, "Url", "").Trim();
                    edit.NavigateTarget = "_blank";
                }
            }
            if (_hasExtraTitle)
            {
                Label lbl = (Label)e.Item.FindControl("lblExtraTitle");
                if (lbl != null)
                    lbl.Text = DisplayDate(BaseDatabase.GetDataListItemDbValue(lbl, _dataExtraTitleCol, ""));
            }
            if (_isMaster)
            {
                DropDownList kind = (DropDownList)e.Item.FindControl("drItemKind");
                if (kind != null)
                {
                    _db.FillKind(kind.Items);
                    kind.SelectedValue = BaseDatabase.GetDataListItemDbValue(kind, "Kind", "");
                    kind.Attributes["1"] = listItems.DataKeys[e.Item.ItemIndex].ToString();
                }
            }
        }

        protected void btnExtra_Click(object sender, EventArgs e)
        {
            string data;
            string opt = btnTxt1.Checked ? btnTxt1.Text : btnTxt2.Text;
            _db.ClickExtra(txtExtra.Text.Trim(), txtTitle, txtBody, opt, out data);
            NewItemData = data;
        }

        protected void drItemKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dr = sender as DropDownList;
            _db.ChangeKind(PageKind, dr.SelectedValue, Convert.ToInt32(dr.Attributes["1"]));
            DisplayData();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch_TextChanged(null, null);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtSearch_TextChanged(null, null);
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _db.KeyWord = txtSearch.Text.Trim();
            ResetPage();
        }

        protected void btnFull_CheckedChanged(object sender, EventArgs e)
        {
            _db.TextKind = ItemTextKind.Full;
            pnAdd.Visible = _isMaster && _canAdd;
            ChangePagebarLine(MyFunc.GetWebconfigValue(PageKind + "ItemPerPage", 0));
            ChangeDataList();
            ResetPage();
        }

        protected void btnTitle_CheckedChanged(object sender, EventArgs e)
        {
            _db.TextKind = ItemTextKind.Title;
            pnAdd.Visible = false;
            ChangePagebarLine(CalItemPerPage());
            ChangeDataList();
            ResetPage();
        }

        protected string TitleUrl(object Title, object Id)
        {
            string href = GetImageHRef(Id);
            return string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", href, Title);
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

        protected bool ShowSubmit()
        {
            return _isMaster && _showSubmit && _db.Publish == PublishKind.OnHold;
        }

    }
}