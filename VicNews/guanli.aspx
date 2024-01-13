<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guanli.aspx.cs" Inherits="VicNews.guanli" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>管理</title>
</head>
<body>
    <form id="form2" runat="server">
    <div>
        <table>
            <tr>
                <td style="text-align: left">
                    <asp:CheckBox ID="chkManager" runat="server" Text="Manager" AutoPostBack="True" OnCheckedChanged="chkManager_CheckedChanged"
                        Enabled="False" />
                    <asp:TextBox ID="txtCode" runat="server" Width="83px"></asp:TextBox>
                    <asp:Button ID="btnCode" runat="server" Text="密码" OnClick="btnCode_Click" Width="75px" />
                </td>
            </tr>
            <tr>
                <td style="width: 180px">
                    <asp:Panel ID="PnDate" runat="server">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:TextBox ID="txtUrl" runat="server" Width="344px"></asp:TextBox><br />
                    <asp:Button ID="btnPaste" runat="server" Text="粘贴" OnClick="btnPaste_Click" Width="75px" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>