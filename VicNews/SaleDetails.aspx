<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleDetails.aspx.cs" Inherits="VicNews.SaleDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>详细内容</title>
    <link href="vicnews.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="txt-normal" enctype="multipart/form-data" style="width:100%">
    <center>
        <table style="width: 1000px" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                             <td style="text-align: center">
                                <asp:Label ID="lblStoreName" runat="server" Text='Title' CssClass="txt-label" /><br />
                            </td>
                           <td style="text-align: left; width: 300px">
                                <asp:Label ID="lblRange" runat="server" Text='Date'></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr><td>
            </td></tr>
            <tr>
                <td>
                    <uc1:DetailList ID="Details" runat="server" PasteUrl="false" />
                </td>
            </tr>
          <tr>
            <td>
                <asp:Label ID="lblSource" runat="server" CssClass="txt-normal-gray"></asp:Label>
            </td>
        </tr>
       <tr>
            <td style="text-align: left">
            <br />
                <asp:Label ID="lblPolicy" runat="server" CssClass="txt-normal-gray"></asp:Label>
            </td>
        </tr>
       </table>
    </center>
    </form>
</body>
</html>
