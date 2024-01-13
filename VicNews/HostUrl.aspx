<%@ Page Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true"
    CodeFile="HostUrl.aspx.cs" Inherits="HostUrl" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <iframe style="width: 100%; height: <%= _frameHeight %>" marginheight="0" marginwidth="0" src="<%=_src %>" frameborder="0" scrolling="yes">
    </iframe>
</asp:Content>
