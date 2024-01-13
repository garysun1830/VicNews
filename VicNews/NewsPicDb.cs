using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Configuration;
using ShareLib;
using WebNewsLib;


public class NewsPicDb : CommonItemDb
{

    public NewsPicDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
        _pageKind = "newspic";
        _hasAll = true;
        _defaultKind = "0";
    }

    public override void ClickExtra(string Extra, TextBox Title, TextBox Body, string Option, out string Data)
    {
        Data = "";
        bool txtup = string.Compare(Option, MyFunc.GetWebconfigValue("NewsPicTextOption1", "")) == 0;
        WebPic loader = WebPic.CreateDownloader(Extra,txtup,ConfigKind.Web);
        loader.Load();
        Title.Text = loader.Title;
        Body.Text = "";
        Data = MyFunc.SerializeToString(loader);
    }

    protected override void AddItemExtra(string Extra, string Data)
    {
    }

    protected override void AddAutoImage()
    {
        UploadData data;
        data.Db = this;
        data.ImageWebPath = ImagePath;
        data.ImageServerPath = "";
        data.Url = "";
        data.PenFile = "";
        data.PrevImageUrl = "";
        data.ThumbWidth = 150;
        UploadImage upload = new UploadImage(data);
        upload.UploadLocal(GetAutoImage());
    }

    public override void DeleteItem()
    {
        SqlParam param = BaseDatabase.GetSPParam("sp_DeleteNewsPic");
        param.ParamList[0].Value = IndexValue;
        BaseDatabase.ExecSql(param);
        DeleteLiveImage(IndexValue);
        DeleteImageFolder();
    }

    private void DeleteImageFolder()
    {
        if (!Directory.Exists(DataFilePath))
            return;
        DirectoryInfo di = new DirectoryInfo(DataFilePath);
        FileInfo[] files = di.GetFiles("*.*");
        foreach (FileInfo fi in files)
            try
            {
                File.Delete(fi.FullName);
            }
            catch { }
        try
        {
            Directory.Delete(DataFilePath);
        }
        catch { }
    }

    protected override string PublishCriteria()
    {
        string cri = "";
        if (Publish == PublishKind.Publish)
            cri += " AND  Active=1";
        if (Publish == PublishKind.OnHold)
            cri += " AND  (Active IS NULL OR Active=0)";
        return cri;
    }

}

