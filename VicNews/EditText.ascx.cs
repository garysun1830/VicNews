using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShareLib;

namespace VicNews
{
    public partial class EditText : System.Web.UI.UserControl
    {
        private bool _needDecode = true;

        private void SetReadOnly(bool Value)
        {
            lnkText.Visible = Value;
            lblText.Visible = Value;
            txtText.Visible = !Value;
        }

        public bool NeedDecode
        {
            set { _needDecode = value; }
        }

        public string NavigateUrl
        {
            set
            {
                lnkText.NavigateUrl = value;
                lnkText.Visible = true;
                lblText.Visible = false;
            }
        }

        public string NavigateTarget
        {
            set { lnkText.Target = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!lnkText.Visible)
                return;
            lblText.Visible = string.IsNullOrEmpty(lnkText.NavigateUrl);
            lnkText.Visible = !lblText.Visible;
        }

        public string CssClass
        {
            set
            {
                lblText.CssClass = value;
                txtText.CssClass = value;
                lnkText.CssClass = value;
            }
        }

        public void SetButtons(ButtonPosition Position, bool CanEdit, bool CanDelete)
        {
            switch (Position)
            {
                case ButtonPosition.Read:
                    pnRead.Visible = true;
                    SetReadOnly(true);
                    break;
                case ButtonPosition.Top:
                    pnTop.Visible = true;
                    btnTopDelete.Visible = CanDelete;
                    btnTopEdit.Visible = CanEdit;
                    SetReadOnly(false);
                    break;
                case ButtonPosition.Right:
                    pnRight.Visible = true;
                    btnRightDelete.Visible = CanDelete;
                    btnRightEdit.Visible = CanEdit;
                    SetReadOnly(false);
                    break;
            }
        }

        public string Text
        {
            set
            {
                if (_needDecode)
                    value = MyFunc.CurrentServer().HtmlDecode(value);
                lblText.Text = value;
                txtText.Text = HtmlTree.RealUrlToUserUrl(value);
                lnkText.Text = value;
            }
        }

        public string EditingText
        {
            set { txtText.Text = HtmlTree.RealUrlToUserUrl(value); }
            get { return HtmlTree.UserUrlToRealUrl(txtText.Text); }
        }

        public TextBoxMode TextMode
        {
            set { txtText.TextMode = value; }
        }

        public string TextBoxWidth
        {
            set { txtText.Width = Convert.ToInt32(value.ToLower().Replace("px", "")); }
        }

        public string TextBoxHeight
        {
            set { txtText.Height = Convert.ToInt32(value.ToLower().Replace("px", "")); }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter myWriter = new HtmlTextWriter(new StringWriter(sb));
            base.Render(myWriter);
            string html = HtmlTree.RemoveBlankHtml(sb.ToString(), "td,tr,table,span");
            html = HtmlTree.RemoveTag(html, "div");
            writer.Write(html);
        }

    }
}
