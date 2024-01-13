<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="VicNews.contact" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table style="width: 100%">
        <tr>
            <td style="text-align: left">
                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Width="916px" 
                    Height="172px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Button ID="btnContact" runat="server" Text="留言" onclick="btnContact_Click" 
                    Width="102px" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Label ID="lblSent" runat="server" Text="你的留言已经发送。谢谢。" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="mailto:WebMaster@KanTing365.info">Email: WebMaster@KanTing365.info</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
