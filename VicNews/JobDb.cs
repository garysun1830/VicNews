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
using System.Web.Configuration;
using ShareLib;


public class JobDb : CommonItemDb
{

    public JobDb(string AClassId, string AIndexName)
        : base(AClassId, AIndexName)
    {
        _pageKind = "Job";
        _hasAll = true;
        _defaultKind = "0";
        _hasOther = true;
        _otherKind = "99999";
    }

    public override string FillKindMenu2()
    {
        SqlParam param = BaseDatabase.GetSqlParam("SelectJobKindMenu2");
        return Utility.FillKindMenu(param, _pageKind, "Text", _pageKind + "KindIndex", Kind);
    }

}