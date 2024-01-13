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
using System.Web.Configuration;
using ShareLib;


/// <summary>
/// Summary description for PaymentDb
/// </summary>
/// 

public class EventDb : CommonItemDb
{
    public EventDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
        _pageKind = "event";
        _defaultKind = "1";
    }

}