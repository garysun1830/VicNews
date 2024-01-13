<%@ Page Title="免费物品" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="FreeItem.aspx.cs" Inherits="VicNews.FreeItem" %>

<%@ Register Src="EditText.ascx" TagName="EditText" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <asp:Panel ID="pnTrans" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="翻译"></asp:Label>
                    <asp:TextBox ID="txtEng" runat="server" Width="150px"></asp:TextBox>&nbsp;
                    <asp:TextBox ID="txtCh" runat="server" Width="150px"></asp:TextBox>
                    <asp:Button ID="btnSaveCh" runat="server" Text="保存" OnClick="btnSave_Click" Width="75px" />
                </td>
                <td>
                    <asp:Label ID="Label32" runat="server" Text="分类"></asp:Label>
                    <asp:TextBox ID="txtCategory" runat="server" Width="100px"></asp:TextBox>
                    <asp:DropDownList ID="drCategory" runat="server">
                    </asp:DropDownList>
                    <asp:Button ID="btnSaveCategory" runat="server" OnClick="btnSaveCategory_Click" Text="保存"
                        Width="75px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <table>
        <tr>
            <td style="text-align: left">
                <div class="container-article">
                    <div class="tabs-container">
                        <asp:Label ID="lblKind" runat="server" Text="菜单"></asp:Label>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: left" >
                <asp:CheckBox ID="chkSimple" runat="server" Text="简要" AutoPostBack="True" Checked="True"
                    OnCheckedChanged="chkSimple_CheckedChanged" />
                <asp:CheckBox ID="chkCh" runat="server" Text="中文" AutoPostBack="True" Checked="True"
                    OnCheckedChanged="chkCh_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:DataList ID="RecList" runat="server" DataKeyField="FreeIndex" ShowFooter="False"
                    ShowHeader="False" RepeatColumns="3" RepeatDirection="Horizontal" Width="100%"
                    OnItemCommand="RecList_ItemCommand" OnItemCreated="RecList_ItemCreated" OnItemDataBound="RecList_ItemDataBound">
                    <ItemTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="height: 150px; width: 290px">
                                    <a href='@Url(DataBinder.Eval(Container.DataItem, "Url"))' target="_blank">
                                        <asp:Image ID="Image2" runat="server" BorderWidth="0" ImageUrl='<%# ImageSrc(DataBinder.Eval(Container.DataItem, "Image")) %>' /></a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text='<%# StayDays(DataBinder.Eval(Container.DataItem, "BeginDate")) %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc1:EditText ID="edtTitle" runat="server" CssClass="txt-label" TextMode="SingleLine"
                                        NavigateUrl='@Url(DataBinder.Eval(Container.DataItem, "Url"))' NavigateTarget='_blank'
                                        NeedDecode="true" TextBoxWidth="260" Text='@Translate(DataBinder.Eval(Container.DataItem, "Title"))' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="drCategory" runat="server" AutoPostBack="true" Visible='<%# _isMaster %>'
                                        OnSelectedIndexChanged="drCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="地点：" Visible='<%# !chkSimple.Checked %>'></asp:Label>
                                    <asp:Label ID="Label7" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Location") %>'
                                        Visible='<%# !chkSimple.Checked %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="Label1" runat="server" Text='<%# DirectPrint(DataBinder.Eval(Container.DataItem, "Text"))%>'
                                        Visible='<%# !chkSimple.Checked %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" Text="查看详情..." NavigateUrl='<%# Url(DataBinder.Eval(Container.DataItem, "Url")) %>'
                                        Visible='<%# !chkSimple.Checked %>' />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle VerticalAlign="Top" />
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td style="text-align: left" >
                <asp:Panel ID="pnPageBar" runat="server">
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="text-align: left" >
                <asp:Label ID="lbl222" runat="server" Text="来源："></asp:Label>
                <asp:HyperLink ID="linkSrc" runat="server" Target="_blank">www.UsedVictoria.com</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td style="text-align: left" >
                <br />
                <asp:Label ID="lblPolicy" runat="server" Text="Label" CssClass="txt-normal-gray" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
