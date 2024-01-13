using System;
using System.IO;
using ShareLib;

namespace VicNews
{
    public partial class EditImage : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private IBasicListDb _db;
        private string _imagePath;

        public IBasicListDb Db
        {
            set { _db = value; }
        }

        public string ImagePath
        {
            set { _imagePath = value; }
        }

        public bool Editing
        {
            set
            {
                pnEdit.Visible = value;
            }
        }

        public string ImageUrl
        {
            set
            {
                Link.ImageUrl = value;
                PrevImageUrl = value;
            }
        }

        public string PendingFile
        {
            get { return SelFile.FileName; }
        }

        public string PrevImageUrl
        {
            set { hdValue.Value = value; }
            get { return hdValue.Value; }
        }

        public string LinkUrl
        {
            set
            {
                Link.NavigateUrl = value;
            }
        }

        public bool CanPasteUrl
        {
            set
            {
                txtUrl.Visible = value;
            }
        }

        private string TrimExtra(string Text)
        {
            int i = Text.IndexOf("?");
            if (i == -1)
                return Text;
            return Text.Remove(i);
        }

        public string PasteUrl
        {
            get { return TrimExtra(txtUrl.Text.Trim()); }
        }

        public string LinkTarget
        {
            set
            {
                Link.Target = value;
            }
        }

        public string LiveArgument
        {
            set
            {
                btnLive.CommandArgument = value;
            }
        }

        public string UploadArgument
        {
            set
            {
                btnUpload.CommandArgument = value;
            }
        }

        public string DeleteArgument
        {
            set
            {
                btnDelete.CommandArgument = value;
            }
        }

        public void SaveFile(string Src, string Dest)
        {
            if (Src == "")
                SelFile.PostedFile.SaveAs(Dest);
            else
                File.Copy(Src, Dest);
        }

        public void Clear()
        {
            txtUrl.Text = "";
        }

        public void Upload(string FileName)
        {
            UploadData data;
            data.Db = (IImageDb)_db;
            data.ImageWebPath = _imagePath;
            data.ImageServerPath = "";
            data.Url = PasteUrl;
            data.PenFile = PendingFile;
            data.PrevImageUrl = PrevImageUrl;
            data.ThumbWidth = 150;
            UploadImage upload = new UploadImage(data);
            upload.OnSaveImageFile += new SaveFileHandler(SaveFile);
            if (FileName == "")
                upload.Upload();
            else
            {
                upload.UploadLocal(FileName);
            }
            ImageUrl = upload.ImageUrl;
            Clear();
        }

        public void Upload()
        {
            Upload("");
        }

    }
}

