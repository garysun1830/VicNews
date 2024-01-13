using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class EditNews : EditorPage
    {
        private NewsDb _db;
        private bool _submit;

        protected override void DoLoadPage(ref string AClassId, ref string ALoginId, ref string AParentPage, ref string APageName, ref IEditorDb Db)
        {
            if (!MyCookie.CookieExists("Manager"))
                throw new Exception("非法地址!");
            _db = ClassFactory.CreateDb("News", "News") as NewsDb;
            Db = _db;
            AParentPage = "index.aspx";
            faceNews.InitControls();
            faceNews.SetDbs(_db);
        }

        protected override void SetPageTitle()
        {
        }

        protected override void SetCommonControls()
        {
            faceNews.SetCommonControls();
            faceNews.ReadOnly = Kind == EditKind.Info;
            faceNews.CssClass = "txt-normal";
            pnTool.Visible = Kind != EditKind.Info;
            hdParent.Value = MyFunc.QueryURL("ParentData", "");
        }

        protected override void SetEditControls(DataRow Row)
        {
            faceNews.SetEditControls(Row);
            string img = BaseDatabase.GetRowValue(Row, "Image", "");
        }

        protected override void GetNewControls(List<NewValues> ValueList)
        {
            if (faceNews.HasInput())
            {
                NewValues values = new NewValues();
                faceNews.GetControls(values.Values);
                ValueList.Add(values);
            }
        }

        protected override void GetEditControls(ArrayList Values)
        {
            faceNews.GetControls(Values);
        }

        protected override void RedirectPage()
        {
            Response.Redirect(ParentPage + "?" + hdParent.Value);
        }

        protected string ParentUrl()
        {
            return ParentPage + "?" + hdParent.Value;
        }

        private void SaveHtml()
        {
            _db.SaveHtml(faceNews.HtmlText);
            if (faceNews.Html)
                _db.SetSimpleTrue();
            if (!string.IsNullOrEmpty(faceNews.Image))
            {
                UploadData data;
                data.Db = (IImageDb)_db;
                data.ImageWebPath = faceNews.Image;
                data.ImageServerPath = "";
                data.Url = faceNews.Image;
                data.PenFile = "";
                data.PrevImageUrl = "";
                data.ThumbWidth = 150;
                UploadImage upload = new UploadImage(data);
                upload.Upload();
            }
        }

        protected override void AfterAdd(List<NewValues> ValueList)
        {
            string extra = faceNews.Extra;
            if (extra != "")
                _db.AddExtra(extra);
            if (_submit)
                _db.Submit();
            SaveHtml();
        }

        protected override void AfterEdit(ArrayList Values)
        {
            if (_submit)
                _db.Submit();
            string extra = faceNews.Extra;
            if (extra != "")
            {
                if (faceNews.PrevExtra == "")
                    _db.AddExtra(extra);
                else
                    _db.EditExtra(extra);
            }
            SaveHtml();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            _submit = true;
            btnOk_Click(null, null);
        }

    }
}