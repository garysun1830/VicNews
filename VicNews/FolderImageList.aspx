<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="FolderImageList.aspx.cs" Inherits="VicNews.FolderImageList" %>

<%@ Register Src="EditText.ascx" TagName="EditText" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table style="width: 100%">
        <tr>
            <td>
                <asp:Panel ID="barTop" runat="server">
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <div id="threadlist" style="position: relative;">
                    <h3>
                        <asp:Label ID="lblCaption" runat="server" />
                    </h3>
                    <table style="width: 100%">
                        <tr>
                            <td align="right">
                                <asp:DataList ID="SubList" runat="server" RepeatDirection="Horizontal" DataKeyField="Index"
                                    OnItemCommand="SubList_ItemCommand">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnSub" runat="server" CommandName="Sub" Text='<%# DataBinder.Eval(Container.DataItem, "SubName") %>' />
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnUp" runat="server" Text="标题上移" OnClick="btnUp_Click" />
                                <asp:Button ID="btnDown" runat="server" Text="标题下移" OnClick="btnDown_Click" />
                                <asp:DropDownList ID="drlstSize" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="drlstSize_SelectedIndexChanged">
                                    <asp:ListItem Value="0">原始大小</asp:ListItem>
                                    <asp:ListItem Value="600">宽度600</asp:ListItem>
                                    <asp:ListItem Value="800">宽度800</asp:ListItem>
                                    <asp:ListItem Value="1000">宽度1000</asp:ListItem>
                                    <asp:ListItem Value="1200">宽度1200</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DataList ID="ImageList" runat="server" DataKeyField="FileIndex"
                                    HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <table style="margin: auto">
                                            <tr>
                                                <td style="text-align: center;">
                                                    <asp:Image ID="Image" runat="server" AlternateText="" src='<%= GetImageUrl(DataBinder.Eval(Container.DataItem, "WebPath")) %>'
                                                        Style="border-width: 0;" Visible='<%= _showImage %>' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left; width: 500px">
                                                    <uc2:EditText ID="edtTitle" runat="server" TextMode="SingleLine" TextBoxWidth="500"
                                                        Text='<%= DataBinder.Eval(Container.DataItem, "Title") %>' Visible='<%= _showFooter %>'
                                                        NeedDecode="true" />
                                                    <asp:Label ID="Label12" runat="server" Text='<%= PrintLine() %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="barBot" runat="server">
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
