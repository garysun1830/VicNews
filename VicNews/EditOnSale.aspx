<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditOnSale.aspx.cs" Inherits="VicNews.EditOnSale" %>

<%@ Register Src="NewsFace.ascx" TagName="NewsFace" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>增加修改打折</title>
    <link href="vicnews.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">

        function GoParent() {
            window.location.replace("onsale.aspx");
        }

    </script>

    <style type="text/css">
        #form1 {
            text-align: left;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <table style="text-align: left">
            <tr>
                <td>
                    <asp:DropDownList ID="drStore" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Panel ID="pnBeginDate" runat="server">
                    </asp:Panel>
                </td>
                <td>至
                </td>
                <td>
                    <asp:Panel ID="pnEndDate" runat="server">
                    </asp:Panel>
                </td>
                <td></td>
            </tr>
        </table>
        <asp:Button ID="btnOk" runat="server" Text="Ok" OnClick="btnOk_Click" Width="80px"
            CssClass="txt-normal" />
        <input id="Button1" type="button" value="Cancel" onclick="GoParent();" class="txt-normal" />
        <asp:HiddenField ID="hdParent" runat="server" />
    </form>
</body>
</html>
