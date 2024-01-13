using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ShareLib;

/// <summary>
/// Summary description for ClassFactory
/// </summary>
public class ClassFactory
{

    public static object CreateDb(string AClassId, string ALoginId)
    {
        switch (AClassId.ToLower())
        {
            case "":
                return new ManagedPageDb("", "");
            case "news":
                return new NewsDb(AClassId, ALoginId);
            case "focus":
                return new FocusDb(AClassId, ALoginId);
            case "newspicture":
                return new NewsPictureDb(AClassId, ALoginId);
            case "custmessage":
                return new CustMessageDb(AClassId, ALoginId);
            case "onsale":
                return new OnSaleDb(AClassId, ALoginId);
            case "onsaledetail":
                return new OnSaleDetailDb(AClassId, ALoginId);
            case "freeitem":
                return new FreeItemDb(AClassId, ALoginId);
            case "event":
                return new EventDb(AClassId, ALoginId);
            case "book":
                return new BookDb(AClassId, ALoginId);
            case "video":
                return new VideoDb(AClassId, ALoginId);
            case "music":
                return new MusicDb(AClassId, ALoginId);
            case "listen":
                return new ListenDb(AClassId, ALoginId);
            case "photo":
                return new PhotoDb(AClassId, ALoginId);
            case "engaudio":
                return new EngAudioDb(AClassId, ALoginId);
            case "newspic":
                return new NewsPicDb(AClassId, ALoginId);
            case "job":
                return new JobDb(AClassId, ALoginId);
            case "kid":
                return new KidDb(AClassId, ALoginId);
            case "home":
                return new HomeDb(AClassId, ALoginId);
            case "food":
                return new FoodDb(AClassId, ALoginId);
            case "dance":
                return new DanceDb(AClassId, ALoginId);
            case "eduvideo":
                return new EduVideoDb(AClassId, ALoginId);
        }
        return null;
    }

}

