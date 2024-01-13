using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using ShareLib;
using WebNewsLib;

namespace VicNews
{
    public partial class NewsFace : System.Web.UI.UserControl
    {

        private NewsDb _newsDb;
        protected string _rightWidth;
        private string _newsMinW;
        private CalendarControl _date;
        private string _cssClass;
        private bool _readonly;
        private string _htmlText;
        private string _image;
        public string aaa="";

        public void SetDbs(NewsDb Db)
        {
            _newsDb = Db;
        }

        public string Extra
        {
            get
            {
                if (hdExtra.Value == txtExtra.Text)
                    return "";
                return txtExtra.Text.Trim();
            }
        }

        public string PrevExtra
        {
            get { return hdExtra.Value; }
        }

        public string CssClass
        {
            set
            {
                _cssClass = value;
                txtTitle.CssClass = value;
                txtNews.CssClass = value;
            }
        }

        public bool Html
        {
            get { return chkHtml.Checked; }
        }

        public string HtmlText
        {
            set { _htmlText = value; }
            get { return Html ? _htmlText : ""; }
        }

        public string Image
        {
            set { _image = value; }
            get { return _image; }
        }

        public bool ReadOnly
        {
            set
            {
                _readonly = value;
                txtTitle.ReadOnly = value;
                txtNews.ReadOnly = value;
                drArea.Enabled = !value;
                btnSaveWord.Enabled = !value;
                txtCommon.Visible = !value;
            }
        }

        public void InitControls()
        {
            _date = new CalendarControl(this.Page, PnDate, "NewsFace");
            _newsMinW = MyFunc.GetWebconfigValue("NewsFaceWidthMin", "500px");
        }

        public void SetCommonControls()
        {
            _newsDb.FillKind(drArea.Items);
            ((FocusDb)ClassFactory.CreateDb("Focus", "Focus")).FillKind(drFocus.Items);
            drFocus.Items.Insert(0, new ListItem());
            NewsDb.FillSource(drSource);
            txtCommon.Text = NewsDb.ReadNewsWord();
            PresetView();
            chkDict_CheckedChanged(null, null);
            btnMicrosoft_CheckedChanged(null, null);
            chkTool_CheckedChanged(null, null);
            chkFixed.Checked = MyFunc.GetSessionData("FixNewsFaceDate", false);
            DateData data = new DateData();
            data.BeginYear = 2011;
            data.EndYear = 2020;
            data.ReadOnly = _readonly || chkFixed.Checked;
            data.DefaultDate = DateTime.Today;
            _date.Data = data;
            if (MyFunc.GetSessionData("FixNewsFaceDate", false))
                _date.Date = Convert.ToDateTime(MyFunc.GetSessionData("NewsFaceDate", DateTime.Today.ToString()));
            else
                _date.Date = DateTime.Today;
        }

        public bool HasInput()
        {
            return txtNews.Text.Trim() != "" || txtTitle.Text.Trim() != "";
        }

        public void GetControls(ArrayList List)
        {
            List.Add(_date.Date);
            List.Add(txtTitle.Text.Trim());
            List.Add(MyFunc.TrimTextAll(HtmlTree.UserUrlToRealUrl(txtNews.Text)));
            List.Add(drArea.SelectedValue);
            List.Add(chkSimple.Checked);
            List.Add(chkTop.Checked);
            List.Add(drFocus.SelectedValue);
            List.Add(drSource.SelectedValue);
        }

        public void SetEditControls(DataRow Row)
        {
            _date.Date = BaseDatabase.GetRowValue(Row, "NewsDate", DateTime.Now);
            txtTitle.Text = BaseDatabase.GetRowValue(Row, "Title", "");
            txtNews.Text = HtmlTree.RealUrlToUserUrl(BaseDatabase.GetRowValue(Row, "Text", ""));
            drArea.SelectedValue = BaseDatabase.GetRowValue(Row, "Area", "");
            chkSimple.Checked = BaseDatabase.GetRowValue(Row, "Simple", false);
            chkTop.Checked = BaseDatabase.GetRowValue(Row, "OnTop", false);
            drFocus.SelectedValue = BaseDatabase.GetRowValue(Row, "Focus", "");
            txtExtra.Text = BaseDatabase.GetRowValue(Row, "ExtraText", "").Trim();
            hdExtra.Value = txtExtra.Text;
            drSource.SelectedValue = BaseDatabase.GetRowValue(Row, "Source", "");
            HtmlText = BaseDatabase.GetRowValue(Row, "Html", "").Trim();
            chkHtml.Checked = HtmlText != "";
            Image = BaseDatabase.GetRowValue(Row, "ImageUrl", "").Trim();
        }

        protected void btnSaveWord_Click(object sender, EventArgs e)
        {
            NewsDb.SaveNewsWord(txtCommon.Text);
        }

        protected void btnReplace_Click(object sender, EventArgs e)
        {
            ReplaceText();
        }

        private void ReplaceText()
        {
            txtNews.Text = NewsDb.ReplaceWords(txtNews.Text, chkDot.Checked);
            txtTitle.Text = NewsDb.ReplaceWords(txtTitle.Text, chkDot.Checked);
        }

        protected void chkTool_CheckedChanged(object sender, EventArgs e)
        {
            pnTools.Visible = chkTool.Checked;
            if (pnTools.Visible)
            {
                txtNews.Width = new Unit(_newsMinW);
                _rightWidth = "width: auto";
                string w = MyCookie.GetCookie("GooleWidth", "900px");
                pnTools.Width = new Unit(w);
                btnMicrosoft.Checked = true;
                btnMicrosoft_CheckedChanged(null, null);
            }
            else
            {
                txtNews.Width = new Unit("1000px");
                string w = MyCookie.GetCookie("NewsFaceWidth", _newsMinW);
                txtNews.Width = new Unit(w);
                _rightWidth = "width:1px";
            }
        }

        protected void btnWideTool_Click(object sender, EventArgs e)
        {
            double w = pnTools.Width.Value;
            if (w >= 2000)
                return;
            w += 50;
            pnTools.Width = new Unit(w);
            MyCookie.AddCookie("GooleWidth", pnTools.Width.ToString());
        }

        protected void btnNarrowTool_Click(object sender, EventArgs e)
        {
            double w = pnTools.Width.Value;
            if (w <= 400)
                return;
            w -= 50;
            pnTools.Width = new Unit(w);
            MyCookie.AddCookie("GooleWidth", pnTools.Width.ToString());
        }

        protected void btnWideNews_Click(object sender, EventArgs e)
        {
            double w = txtNews.Width.Value;
            if (w >= 2000)
                return;
            w += 50;
            txtNews.Width = new Unit(w);
            MyCookie.AddCookie("NewsFaceWidth", txtNews.Width.ToString());
        }

        protected void btnNarrowNews_Click(object sender, EventArgs e)
        {
            double w = txtNews.Width.Value;
            if (w <= 450)
                return;
            w -= 50;
            txtNews.Width = new Unit(w);
            MyCookie.AddCookie("NewsFaceWidth", txtNews.Width.ToString());
        }

        protected void btnMicrosoft_CheckedChanged(object sender, EventArgs e)
        {
            string view = "";
            if (btnMicrosoft.Checked)
            {
                MultiView.SetActiveView(viewMicrosoft);
                view = "Microsoft";
            }
            if (btn123.Checked)
            {
                MultiView.SetActiveView(view123);
                view = "123";
            }
            if (btnExtra.Checked)
            {
                MultiView.SetActiveView(viewExtra);
                view = "extra";
            }
            MyCookie.AddCookie("NewsFaceViewIndex", view);
        }

        protected void btnSaveRep_Click(object sender, EventArgs e)
        {
            NewsDb.SaveReplace(txtRepFrom.Text.Trim(), txtRepTo.Text.Trim());
            txtRepFrom.Text = "";
            txtRepTo.Text = "";
            ReplaceText();
        }

        private void PresetView()
        {
            btnMicrosoft.Checked = false;
            btn123.Checked = false;
            btnExtra.Checked = false;
            switch (MyCookie.GetCookie("NewsFaceViewIndex", btnMicrosoft.ID))
            {
                case "Microsoft":
                    btnMicrosoft.Checked = true;
                    MultiView.SetActiveView(viewMicrosoft);
                    break;
                case "123":
                    btn123.Checked = true;
                    MultiView.SetActiveView(view123);
                    break;
                case "extra":
                    btnExtra.Checked = true;
                    MultiView.SetActiveView(viewExtra);
                    break;
            }
        }

        protected void chkDict_CheckedChanged(object sender, EventArgs e)
        {
            pnDict.Visible = chkDict.Checked;
        }

        protected void btnPasteUrl_Click(object sender, EventArgs e)
        {
            txtNews.Text = "正文";
            txtTitle.Text = "标题";
            string url = txtNewsUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
                return;
            WebNews news = WebNews.CreateNews(url, ConfigKind.Web);
            PasteToFace(news);
            HtmlText = news.Body;
            Image = news.Image;
        }

        private void PasteToFace(WebNews News)
        {
            if (!News.Success)
                return;
            drSource.SelectedValue = News.Source;
            drArea.SelectedValue = News.Area;
            txtNews.Text = News.Text;
            if (!(News is VicTimesNews))
                PasteCNNewsToFace(News);
        }

        private void PasteCNNewsToFace(WebNews News)
        {
            txtTitle.Text = News.Title;
            ReplaceText();
        }

        protected void chkFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFixed.Checked)
                MyFunc.SaveSessionData("NewFaceDate", _date.Date);
            DateData data = _date.Data;
            data.ReadOnly = chkFixed.Checked;
            _date.Data = data;
            MyFunc.SaveSessionData("FixNewsFaceDate", chkFixed.Checked);
        }

        protected void chkHand_CheckedChanged(object sender, EventArgs e)
        {
            pnSubst.Visible = chkHand.Checked;
            pnWord.Visible = chkHand.Checked;
            if (pnSubst.Visible)
                txtNews.Height = 250;
            else
                txtNews.Height = 500;
            chkTool.Checked = chkHand.Checked;
            chkTool_CheckedChanged(null, null);
        }

    }
}