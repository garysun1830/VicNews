<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditImage.ascx.cs" Inherits="VicNews.EditImage" %>
<asp:HyperLink ID="Link" runat="server" BorderStyle="None" BorderWidth="0px"></asp:HyperLink>
<asp:Panel ID="pnEdit" runat="server">
    <asp:FileUpload ID="SelFile" runat="server" /><br />
    <asp:Button ID="btnUpload" runat="server" Text="上载" Width="75px" />
    <asp:Button ID="btnDelete" runat="server" Text="删除" Width="75px"
        OnClientClick="return confirm('确定删除该图片?');" /><br />
    <asp:TextBox ID="txtUrl" runat="server"></asp:TextBox><br />
    <asp:Button ID="btnLive" runat="server" Text="活动" Width="75px" />
</asp:Panel>
<asp:HiddenField ID="hdValue" runat="server" />
