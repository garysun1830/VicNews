<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditText.ascx.cs" Inherits="VicNews.EditText" %>
<asp:Label ID="lblText" runat="server" />
<asp:HyperLink ID="lnkText" runat="server" Visible="False" >[lnkText]</asp:HyperLink>
<asp:Panel ID="pnRead" runat="server" Visible="False">
    <asp:LinkButton ID="btnReadDelete" runat="server" CommandArgument="DeleteItem" CommandName="Read"
        OnClientClick='return confirm("确定移除该记录?");'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;移除</asp:LinkButton>
</asp:Panel>
<table>
    <tr>
        <td style="text-align: right">
            <asp:Panel ID="pnTop" runat="server" Visible="false">
                <asp:LinkButton ID="btnTopEdit" runat="server" Visible="false" CommandArgument="EditItem"
                    CommandName="Top">修改&nbsp;</asp:LinkButton>
                <asp:LinkButton ID="btnTopDelete" runat="server" Visible="false" CommandArgument="DeleteItem"
                    CommandName="Top" OnClientClick='return confirm("确定删除该记录?");'>删除</asp:LinkButton>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtText" runat="server" Visible="False"></asp:TextBox>
        </td>
        <td valign="bottom">
            <asp:Panel ID="pnRight" runat="server" Visible="false">
                <asp:LinkButton ID="btnRightEdit" runat="server" Visible="false" CommandArgument="EditItem"
                    CommandName="Right">修改&nbsp;</asp:LinkButton>
                <asp:LinkButton ID="btnRightDelete" runat="server" Visible="false" CommandArgument="DeleteItem"
                    CommandName="Right" OnClientClick='return confirm("确定删除该记录?");'>删除</asp:LinkButton>
            </asp:Panel>
        </td>
    </tr>
</table>
