using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{

    public partial class FolderImageList : ManagerPagedPage
    {
        private List<string> _subFolders;
        private DataTable _subTable;
        private string _id;
        private string _kind;
        protected bool _showFooter;
        private int _pageLine;
        protected bool _showImage;
        protected bool _isMaster;
        private FolderFileDb _db;

        protected void Page_Init(object sender, EventArgs e)
        {
            _isMaster = MyCookie.CookieExists("Manager");
            _id = MyFunc.QueryURL("id", "");
            _kind = MyFunc.QueryURL("pagekind", "");
            _showImage = MyFunc.QueryURL("noimage", "") == "";
            _pageLine = MyFunc.GetWebconfigValue(_kind + "ItemPerPage", 5);
            if (_id != "")
            {
                string dir = MyFunc.GetWebconfigValue("DataFilePath", "") + _kind;
                Folder = Path.Combine(Server.MapPath(dir), _id);
                DbValue = "";
            }
            _showFooter = MyFunc.QueryURL("nofooter", "") == "" || TitleExists();
            _subFolders = new List<string>();
            DirectoryInfo di = new DirectoryInfo(Folder);
            DirectoryInfo[] sub = di.GetDirectories();
            for (int i = 0; i < sub.Length; i++)
                _subFolders.Add(GetSubDirName(sub, i));
            CreateSubTable();
        }

        private bool TitleExists()
        {
            return File.Exists(Path.Combine(Folder, MyFunc.GetWebconfigValue("FolderImageTitleFile", "")));
        }

        private string DbValue
        {
            get { return MyFunc.GetObjectValue(ViewState["DbValue"], ""); }
            set { if (value != "") ViewState["DbValue"] = value; }
        }

        private string Folder
        {
            get { return MyFunc.GetObjectValue(ViewState["FileFolder"], ""); }
            set { if (value != "") ViewState["FileFolder"] = value; }
        }

        private string GetSubDirName(DirectoryInfo[] Sub, int Index)
        {
            foreach (DirectoryInfo info in Sub)
                if (info.Name.StartsWith(Index.ToString() + ","))
                    return info.Name;
            return "";
        }

        private void CreateSubTable()
        {
            string[] img = { "SubName" };
            string[] dtype = { "System.String" };
            _subTable = BaseDatabase.CreateTable("", "Index", true, img, dtype);
            for (int i = 0; i < _subFolders.Count; i++)
            {
                DataRow row = _subTable.NewRow();
                string s = Regex.Replace(_subFolders[i], "\\d+,", "");
                if (i < _subFolders.Count - 1)
                    s += "|";
                row["SubName"] = s;
                _subTable.Rows.Add(row);
            }
        }

        private int SubIndex
        {
            get { return MyFunc.GetObjectValue(ViewState["SubIndex"], 0); }
            set { ViewState["SubIndex"] = value; }
        }

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref DataList ADbList, ref IManagerDb Db)
        {
            if (DbValue == "")
            {
                string sub = GetSub(SubIndex);
                _db = new FolderFileDb(sub, MyFunc.GetWebconfigValue("PhotoFileExt", ""), MyFunc.GetWebconfigValue("DataFilePath", ""));
                DbValue = MyFunc.SerializeToString(_db);
            }
            else
                _db = MyFunc.DeserializeFromString(DbValue) as FolderFileDb;
            ADbList = ImageList;
            Db = _db as IManagerDb;
            string page_title = CommonItemDb.PageTitle(_kind);
            lblCaption.Text = page_title + "&raquo;" +
                                            BaseDatabase.ConvertDbValue(CommonItemDb.GetColValue(_kind, _id, "Title"), "");
            Title = MyFunc.GetSessionData("SiteTitle", "");
            if (_id != "" && _kind != "")
                Title = Title + " - " + page_title + "【" + lblCaption.Text + "】";
            PageBarData data;
            data.Base = new Panel[2];
            data.Base[0] = barTop;
            data.Base[1] = barBot;
            data.Align = "right";
            data.Parent = this;
            data.Db = (IPageDb)_db;
            data.ItemPerPage = _pageLine;
            data.PageListCount = MyFunc.GetWebconfigValue(_kind + "PageListCount", 10);
            CreatePageBar(data);
        }

        private string GetSub(int Index)
        {
            if (_subFolders.Count == 0)
                return Folder;
            if (Index >= _subFolders.Count)
                Index = _subFolders.Count;
            return Path.Combine(Folder, _subFolders[Index]);
        }

        protected override void SetPageTitle()
        {
        }

        protected override void PrepareControl()
        {
            btnUp.Visible = _isMaster;
            btnDown.Visible = _isMaster;
            SubList.DataSource = _subTable;
            SubList.DataBind();
            SubList.Visible = SubList.Items.Count > 0;
            ResetPage();
            drlstSize.SelectedValue = "0";
        }

        private int GetSubByText(string Text)
        {
            Text = Text.Replace("|", "");
            for (int i = 0; i < _subFolders.Count; i++)
                if (_subFolders[i].Contains(Text))
                    return i;
            return 0;
        }

        protected void SubList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            LinkButton btn = e.Item.FindControl("btnSub") as LinkButton;
            if (btn == null)
                return;
            SubIndex = GetSubByText(btn.Text);
            DbValue = "";
            ReloadPage();
        }

        protected string PrintLine()
        {
            return _showImage ? "<br/>" : "";
        }

        protected override void ItemCreated(object sender, DataListItemEventArgs e)
        {
            Image img = (Image)e.Item.FindControl("Image");
            int i = Convert.ToInt32(drlstSize.SelectedValue);
            if (i > 0)
                img.Width = i;
            if (!_isMaster)
                return;
            EditText edit = e.Item.FindControl("edtTitle") as EditText;
            if (edit == null)
                return;
            edit.SetButtons(ButtonPosition.Right, true, false);
        }

        protected override void ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (!_isMaster)
                return;
            EditText edit = e.Item.FindControl("edtTitle") as EditText;
            if (edit == null)
                return;
            edit.Attributes["1"] = ImageList.DataKeys[e.Item.ItemIndex].ToString();
        }

        protected override void ItemClick(DataListCommandEventArgs e)
        {
            switch (e.CommandArgument.ToString())
            {
                case "EditItem":
                    {
                        if (e.CommandName == "Right")
                        {
                            EditText edit = (EditText)e.Item.FindControl("edtTitle");
                            _db.EditTitle(edit.EditingText, edit.Attributes["1"]);
                            DbValue = MyFunc.SerializeToString(_db);
                            DisplayData();
                        }
                        break;
                    }
            }
        }

        protected override string RenderHtml(string Html)
        {
            return HtmlTree.RemoveBlankHtml(Html, "div,td,tr,table,span");
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            _db.TitleUp();
            DbValue = "";
            ReloadPage();
        }

        protected void btnDown_Click(object sender, EventArgs e)
        {
            _db.TitleDown();
            DbValue = "";
            ReloadPage();
        }

        protected void drlstSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayData();
        }

        protected string GetImageUrl(object ImageUrl)
        {
            string url = ImageUrl.ToString();
            bool is365 = MyCookie.CookieExists("Kanting365");
            if (is365)
                url = MyFunc.GetWebconfigValue("DataKanTing365", "") + url.Replace(MyFunc.GetWebconfigValue("DataFilePath", ""), "");
            return url;
        }

    }
}
