using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class NewsBody : System.Web.UI.UserControl
    {
        protected bool _isMaster;
        private ManagerPage _page;
        private NewsDb _db;
        private DropDownList _areaList;
        private DropDownList _focusList;
        private DropDownList _SourceList;

        public DataList NewsData
        {
            get { return ItemList; }
        }

        public bool IsMaster
        {
            set { _isMaster = value; }
        }

        protected bool ShowSubmit
        {
            get
            {
                return _db.Publish == PublishKind.OnHold && _isMaster;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            _page = Page as ManagerPage;
        }

        public void SetDb(NewsDb Db)
        {
            _db = Db;
            _areaList = new DropDownList();
            _db.FillKind(_areaList.Items);
            _focusList = new DropDownList();
            ((FocusDb)ClassFactory.CreateDb("Focus", "Focus")).FillKind(_focusList.Items);
            _focusList.Items.Insert(0, new ListItem());
            _SourceList = new DropDownList();
            NewsDb.FillSource(_SourceList);
        }

        protected string DisplayDate(object Value)
        {
            return _page.DisplayDate(Value);
        }

        protected string GetImageThumb(object Image)
        {
            string result = BaseDatabase.ConvertDbValue(Image, "");
            if (result == "")
                return "";
            return MyFunc.GetWebconfigValue("NewsImagePath", "") + result.Replace(".", "th.");
        }

        protected string GetImageSrc(object Image)
        {
            string result = BaseDatabase.ConvertDbValue(Image, "");
            if (result == "")
                return "";
            result = MyFunc.GetWebconfigValue("NewsImagePath", "") + result;
            return result;
        }

        protected string GetImageHRef(object NewsId)
        {
            return GetDetailHRef(NewsId);
        }

        private string GetDetailHRef(object NewsId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("NewsDetails.aspx?pagekind=news&id=");
            sb.Append(NewsId.ToString());
            return sb.ToString();
        }

        protected bool GetImageVisible(object Image)
        {
            if (_isMaster)
                return true;
            return BaseDatabase.ConvertDbValue(Image, "").Trim() != "";
        }

        protected string DirectPrint(object Text, object Simple)
        {
            if (Text == null)
                return "";
            if (Simple == null)
                return "";
            string text = MyFunc.DirectPrintHtml(Text.ToString());
            if (BaseDatabase.ConvertDbValue(Simple, false))
            {
                int len = MyFunc.GetWebconfigValue("SimpleTextLength", 100);
                if (text.Length > len)
                    text = text.Substring(0, len) + "...";
            }
            return text;
        }

        protected bool ReadMoreVisible(object Picture, object Simple, object ExtraCount)
        {
            return _isMaster || BaseDatabase.ConvertDbValue(Picture, 0) > 0 || BaseDatabase.ConvertDbValue(Simple, false) || BaseDatabase.ConvertDbValue(ExtraCount, 0) > 0;
        }

        protected string PrintMoreOrSimpleUrl(object NewsId)
        {
            return string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", GetDetailHRef(NewsId), MyFunc.GetWebconfigValue("ReadMore", ""));
        }

        protected bool DisplayBoolean(object Value)
        {
            return _page.DisplayBoolean(Value);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            _page.btnAdd_Click(sender, e);
        }

        protected int NewsWidth(object Image)
        {
            if (_isMaster)
                return 750;
            if (GetImageVisible(Image))
                return 850;
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

        protected string GetSource(object Value)
        {
            string src = BaseDatabase.ConvertDbValue(Value, "");
            if (src == "")
                return "";
            return "消息来源：" + src;
        }

        private void DoUploadImage(DataListCommandEventArgs e)
        {
            EditImage editImage = (EditImage)e.Item.FindControl("EditImage");
            editImage.Db = _db;
            editImage.ImagePath = MyFunc.GetWebconfigValue("NewsImagePath", "");
            editImage.Upload();
        }

        private void ChangeNews(DataListCommandEventArgs e)
        {
            CheckBox box = (CheckBox)e.Item.FindControl("chkSimple");
            bool simple = box.Checked;
            box = (CheckBox)e.Item.FindControl("chkTop");
            bool top = box.Checked;
            DropDownList dr = (DropDownList)e.Item.FindControl("drArea");
            string area = dr.SelectedValue;
            dr = (DropDownList)e.Item.FindControl("drFocus");
            string focus = dr.SelectedValue;
            dr = (DropDownList)e.Item.FindControl("drSource");
            string src = dr.SelectedValue;
            _db.ChangeNews(simple, top, area, focus, src);
        }

        public bool ItemCommand(DataListCommandEventArgs e)
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
                            EditText edit = (EditText)e.Item.FindControl("edtNewsText");
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
                case "ChangeNews":
                    ChangeNews(e);
                    break;
                default:
                    return false;
            }
            return true;
        }

        private void DeleteImage(DataListCommandEventArgs e)
        {
            EditImage editImage = (EditImage)e.Item.FindControl("EditImage");
            string fn = UploadImage.RemoveTh(editImage.PrevImageUrl);
            if (string.IsNullOrEmpty(fn))
                return;
            string imgPath = Server.MapPath(MyFunc.GetWebconfigValue("NewsImagePath", ""));
            fn = Path.Combine(imgPath, Path.GetFileName(fn));
            try { File.Delete(fn); }
            catch { }
            try { File.Delete(UploadImage.AddTh(fn)); }
            catch { }
            _db.DeleteImage();
        }

        public void DisplayData()
        {
            btnAdd.Visible = _isMaster && ItemList.Items.Count == 0;
            lblNoRecord.Visible = ItemList.Items.Count == 0;
            ItemList.Visible = !lblNoRecord.Visible;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter myWriter = new HtmlTextWriter(new StringWriter(sb));
            base.Render(myWriter);
            string html = HtmlTree.RemoveBlankHtml(sb.ToString(), "td,tr,table,span");
            html = Regex.Replace(html, "[ ]+", " ", RegexOptions.Singleline);
            html = html.Replace('\r', '\n');
            html = Regex.Replace(html, "\\n+", "\n", RegexOptions.Singleline);
            writer.Write(html);
        }

        private void CopyItems(DropDownList Src, DropDownList Dest)
        {
            Dest.Items.Clear();
            foreach (ListItem item in Src.Items)
                Dest.Items.Add(new ListItem(item.Text, item.Value));
        }

        public void ItemCreated(DataListItemEventArgs e)
        {
            if (!_isMaster)
                return;
            EditText edit = (EditText)e.Item.FindControl("edtTitle");
            edit.SetButtons(ButtonPosition.Right, true, false);
            edit = (EditText)e.Item.FindControl("edtNewsText");
            edit.SetButtons(ButtonPosition.Top, true, true);
        }

        public void ItemDataBound(DataListItemEventArgs e)
        {
            if (!_isMaster)
                return;
            DropDownList dr = (DropDownList)e.Item.FindControl("drArea");
            CopyItems(_areaList, dr);
            dr.SelectedValue = BaseDatabase.GetDataListItemDbValue(dr, "Area", "");
            dr = (DropDownList)e.Item.FindControl("drFocus");
            CopyItems(_focusList, dr);
            dr.SelectedValue = BaseDatabase.GetDataListItemDbValue(dr, "Focus", "");
            dr = (DropDownList)e.Item.FindControl("drSource");
            CopyItems(_SourceList, dr);
            dr.SelectedValue = BaseDatabase.GetDataListItemDbValue(dr, "Source", "");
        }

    }
}