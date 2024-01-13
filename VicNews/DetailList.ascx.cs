using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class DetailList : UserControl
    {
    	    private IDetailList _db;
    protected bool _editing;
    private string _imgPath;
    public EventHandler DisplayData;
    protected bool _isMaster;
    private List<TextBox> _box;

        public DataList Data
        {
            get { return DetailData; }
        }

        public bool PasteUrl
        {
            set { chkUrl.Enabled = value; chkUrl.Checked = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            _box = new List<TextBox>();
            base.OnLoad(e);
            AddTextBox(this);
        }

        private void AddTextBox(Control Ctrl)
        {
            if (!string.IsNullOrEmpty(Ctrl.ID))
                if (Ctrl.ID.Contains("txtImage"))
                    _box.Add(Ctrl as TextBox);
            foreach (Control ctrl in Ctrl.Controls)
                AddTextBox(ctrl);
        }

        public void InitVar(IDetailList Db, bool IsMaster, bool Editing, string ImagePath, string DataKeyField)
        {
            _db = Db;
            _isMaster = IsMaster;
            _editing = Editing || _isMaster;
            pnBrowse.Visible = _editing;
            _imgPath = ImagePath;
            Data.DataKeyField = DataKeyField;
        }

        protected string DirectPrint(object Text)
        {
            return MyFunc.DirectPrintHtml(BaseDatabase.ConvertDbValue(Text, "")) + "<br/>";
        }

        protected string GetDetailsImage(object Image, object ImageUrl)
        {
            string img = BaseDatabase.ConvertDbValue(ImageUrl, "");
            if (img != "")
                return img;
            img = BaseDatabase.ConvertDbValue(Image, "");
            if (img == "")
                return "";
            return _imgPath + img;
        }

        protected bool ImageVisible(object Image, object ImageUrl)
        {
            return BaseDatabase.ConvertDbValue(ImageUrl, "") != "" || BaseDatabase.ConvertDbValue(Image, "") != "";
        }

        private void SaveImage(HttpPostedFile UpFile)
        {
            if (UpFile.ContentLength == 0)
                return;
            string fn = Server.MapPath(_imgPath) + BaseDatabase.GetNextId().ToString() + Path.GetExtension(UpFile.FileName);
            UpFile.SaveAs(fn);
            _db.AddPictue(Path.GetFileName(fn), false);
        }

        private bool TextExist()
        {
            foreach (TextBox box in _box)
                if (box.Text.Trim() != "")
                    return true;
            return false;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (TextExist())
            {
                LoadImgFromText();
                foreach (TextBox box in _box)
                    box.Text = "";
            }
            else
                LoadImgFromBrowse();
            if (DisplayData != null)
                DisplayData(null, null);
        }

        private void LoadImgFromBrowse()
        {
            HttpFileCollection files = Request.Files;
            for (int i = 0; i < files.Count; i++)
                SaveImage(files[i]);
        }

        private void LoadImgFromText()
        {
            foreach (TextBox box in _box)
                SaveUrlImage(box);
        }

        private void SaveUrlImage(TextBox Box)
        {
            string url = Box.Text.Trim();
            int i = url.IndexOf("?");
            if (i != -1)
                url = url.Remove(i);
            if (url == "")
                return;
            if (chkUrl.Checked)
                _db.AddPictue(url, true);
            else
            {
                string fn = Server.MapPath(_imgPath) + BaseDatabase.GetNextId().ToString() + Path.GetExtension(url);
                UploadImage.MoveImageFromUrlToLocal(url, fn, "");
                _db.AddPictue(Path.GetFileName(fn), false);
            }
        }

        protected void DetailData_ItemCommand(object source, DataListCommandEventArgs e)
        {
            _db.SetIndexValue(Convert.ToInt32(DetailData.DataKeys[e.Item.ItemIndex]));
            if (e.CommandArgument.ToString() == "EditTitle")
            {
                TextBox txt = e.Item.FindControl("txtTitle") as TextBox;
                if (txt == null)
                    return;
                _db.EditTitle(txt.Text);
            }
        }

    }
}
