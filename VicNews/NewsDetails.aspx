<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="NewsDetails.aspx.cs" Inherits="VicNews.NewsDetails" %>

<%@ Register Src="DetailList.ascx" TagName="DetailList" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table style="width: 1000px; margin: auto" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="text-align: left; width: 140px">
                            <asp:Label ID="lblDate" runat="server" Text='Date'></asp:Label>
                        </td>
                        <td style="text-align: center">
                            <asp:Label ID="lblMainTitle" runat="server" Text='Title' CssClass="txt-label" /><br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Image ID="imgMain" runat="server" />
                <asp:CheckBox ID="chkHide" runat="server" AutoPostBack="True" Text="隐藏" OnCheckedChanged="chkHide_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <uc1:DetailList ID="Details" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%; margin: auto;">
                    <tr>
                        <td style="text-align: left">
                            <asp:Label ID="lblDetails" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
