﻿using System;
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
using System.IO;
using System.Web.Configuration;
using ShareLib;


public class DanceDb : CommonItemDb
{

    public DanceDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
        _pageKind = "Dance";
        _hasAll = true;
        _defaultKind = "0";
    }

    public string Width
    {
        get
        {
            return "";
        }
    }

    public string Height
    {
        get
        {
            return "";
        }
    }

}