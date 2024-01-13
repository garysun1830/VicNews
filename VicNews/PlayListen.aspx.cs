using System;
using System.IO;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class PlayListen : ManagerPage
    {
        protected string _image;
        private FolderFileDb _db;
        protected string _oggFileName;

        protected void Page_Init(object sender, EventArgs e)
        {
            Kind = MyFunc.QueryURL("pagekind", "");
            Id = MyFunc.QueryURL("id", "");
            if (Id != "")
            {
                string dir = MyFunc.GetWebconfigValue("DataFilePath", "") + Kind;
                Folder = Path.Combine(Server.MapPath(dir), Id);
                DbValue = "";
            }
        }

        private string DbValue
        {
            get { return MyFunc.GetObjectValue(ViewState["DbValue"], ""); }
            set { if (value != "") ViewState["DbValue"] = value; }
        }

        private string Kind
        {
            get { return MyFunc.GetObjectValue(ViewState["Kind"], ""); }
            set { if (value != "") ViewState["Kind"] = value; }
        }

        private string Id
        {
            get { return MyFunc.GetObjectValue(ViewState["Id"], ""); }
            set { if (value != "") ViewState["Id"] = value; }
        }

        private string Folder
        {
            get { return MyFunc.GetObjectValue(ViewState["Folder"], ""); }
            set { if (value != "") ViewState["Folder"] = value; }
        }

        private string Playing
        {
            get { return MyFunc.GetObjectValue(ViewState["Playing"], ""); }
            set { if (value != "") ViewState["Playing"] = value; }
        }

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            if (DbValue == "")
            {
                _db = new FolderFileDb(Folder, MyFunc.GetWebconfigValue("MediaFileExt", ""), MyFunc.GetWebconfigValue("DataFilePath", ""));
                _db.BeginRecNo = 0;
                _db.EndRecNo = 1000;
                DbValue = MyFunc.SerializeToString(_db);
            }
            else
                _db = MyFunc.DeserializeFromString(DbValue) as FolderFileDb;
            ADbList = ListenList;
            Db = _db as IManagerDb;
            string page_title = CommonItemDb.PageTitle(Kind);
            lblCaption.Text = page_title + "&raquo;" +
                                            BaseDatabase.ConvertDbValue(CommonItemDb.GetColValue(Kind, Id, "Title"), "");
            Title = MyFunc.GetSessionData("SiteTitle", "");
            if (Id != "")
            {
                Title = Title + " - " + page_title + "【" + lblCaption.Text + "】";
                string src = BaseDatabase.ConvertDbValue(CommonItemDb.GetColValue(Kind, Id, "Image"), "");
                Image.Visible = src != "";
                if (Image.Visible)
                    Image.ImageUrl = MyFunc.GetWebconfigValue("ImageRoot", "") + Kind + "/" + UploadImage.AddTh(src);
            }
        }

        protected override void SetPageTitle()
        {
        }

        protected override void PrepareControl()
        {
            if (_db.FileType.Contains("ogg"))
                MultiView.SetActiveView(viewOgg);
            if (_db.FileType.Contains("mp3") || _db.FileType.Contains("wma"))
                MultiView.SetActiveView(viewMP3);
            if (_db.FileType.Contains("flv") || _db.FileType.Contains("mp4"))
            {
                MultiView.SetActiveView(viewFlash);
                Flash.Width = new Unit(FileConfiguration.GetConfiguration(Folder, "Width", "640"));
                Flash.Height = new Unit(FileConfiguration.GetConfiguration(Folder, "Height", "480"));
            }
            lblPolicy.Text = MyFunc.GetWebconfigValue(Kind + "Policy", "");
            ListenList.RepeatColumns = GetListColumn(); MyFunc.GetWebconfigValue(Kind + "ListColumn", 2);
            string fn, text;
            if (_db.GetFirstFile(out fn, out text))
                PlayFile(fn, text);
        }

        protected override void ItemClick(DataListCommandEventArgs e)
        {
            LinkButton btn = (LinkButton)e.Item.FindControl("btnPlay");
            PlayFile(e.CommandName, btn.Text);
        }

        private void PlayFile(string FileName, string Text)
        {
            EmbedMP3.Text = Text + "：播放中......";
            lblOgg.Text = Text + "：播放中......";
            Flash.Text = Text + "：播放中......";
            Playing = FileName;
            EmbedMP3.FilePath = FileName;
            _oggFileName = FileName;
            Flash.FilePath = FileName;
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            string fn, text;
            if (_db.GetPrevFile(Playing, out fn, out text))
                PlayFile(fn, text);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string fn, text;
            if (_db.GetNextFile(Playing, out fn, out text))
                PlayFile(fn, text);
        }

        private int GetListColumn()
        {
            int n = FileConfiguration.GetConfiguration(Folder, "ListColumn", 0);
            if (n > 0)
                return n;
            return MyFunc.GetWebconfigValue(Kind + "ListColumn", 2);
        }

    }
}