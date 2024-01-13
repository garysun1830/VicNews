<%@ Page Title="管理消息" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="ManageMessage.aspx.cs" Inherits="VicNews.ManageMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <asp:DataList ID="RecList" runat="server" DataKeyField="CustMessageIndex" ShowFooter="False"
        ShowHeader="False" Width="100%">
        <ItemTemplate>
            <table style="width: 100%; text-align: left">
                <tr>
                    <td style="width: 200px">
                        <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MessageTime") %>'></asp:Label>
                    </td>
                    <td style="width: 140px">
                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "IP") %>'></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="Label1" runat="server" Text='<%= DirectPrint(DataBinder.Eval(Container.DataItem, "Message")) %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:DataList>
    <iframe marginheight="0" marginwidth="0" src="http://counter.vicnewscn.com" align="top" width="100%" height="600px"
        frameborder="0" scrolling="no"></iframe>
</asp:Content>
