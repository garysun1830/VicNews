<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="ItemList.aspx.cs" Inherits="VicNews.ItemList" %>
<%@ Register Src="EditText.ascx" TagName="EditText" TagPrefix="uc2" %>
<%@ Register Src="EditImage.ascx" TagName="EditImage" TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table style="width: 100%;">
        <tr>
            <td style="text-align: left">
                <div class="container-article">
                    <div class="tabs-container">
                        <asp:Label ID="lblKind" runat="server" Text="菜单"></asp:Label>
                        <asp:Label ID="lblKind2" runat="server" Text="菜单"></asp:Label>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="barTop" runat="server">
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td >
                <div id="threadlist" style="position: relative;">
                    <asp:MultiView ID="mvCapBar" runat="server">
                        <asp:View ID="viewCaption" runat="server">
                            <h3>
                                <asp:Label ID="lblCaption1" runat="server" />
                            </h3>
                        </asp:View>
                        <asp:View ID="viewTextKind" runat="server">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <h3>
                                            <asp:Label ID="lblCaption2" runat="server" />
                                        </h3>
                                    </td>
                                    <td style="text-align: right" class="tabletop">
                                        <asp:RadioButton ID="btnFull" runat="server" Text="全文" Checked="true" AutoPostBack="True"
                                            OnCheckedChanged="btnFull_CheckedChanged" />
                                        <asp:RadioButton ID="btnTitle" runat="server" Text="标题" AutoPostBack="True" OnCheckedChanged="btnTitle_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                    <asp:MultiView ID="mvData" runat="server">
                        <asp:View ID="viewFull" runat="server">
                            <asp:DataList ID="listItems" runat="server" DataKeyField="<%= _dataKeyCol %>" ShowFooter="False"
                                ShowHeader="False" Width="100%">
                                <ItemTemplate>
                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <%= IsMaster %>
                                                            <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, _dataKeyCol) %>'
                                                                Visible='<%# _isMaster %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="btnSubmit" runat="server" CommandName="Submit" Text="提交" CommandArgument="Submit"
                                                                Visible='<%# ShowSubmit()  %>'></asp:LinkButton>
                                                        </td>
                                                        <td style="width: 120px">
                                                            <asp:Label ID="lblDate" runat="server" ></asp:Label>
                                                        </td>
                                                        <td style="width: 650px;">
                                                            <uc2:EditText ID="edtTitle" runat="server" CssClass="txt-label" TextMode="SingleLine"
                                                                TextBoxWidth="300" Text='<%# DataBinder.Eval(Container.DataItem, _dataTitleCol) %>' />
                                                        </td>
                                                        <td style="width: 200px;">
                                                            <asp:Label ID="lblExtraTitle" runat="server" Visible='<%# _hasExtraTitle %>' />
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td valign="top">
                                                            <uc3:EditImage ID="EditImage" runat="server" ImageUrl='<%# GetImageThumb(DataBinder.Eval(Container.DataItem, _dataImageCol)) %>'
                                                                LinkUrl='<%# GetImageHRef(DataBinder.Eval(Container.DataItem, _dataKeyCol)) %>'
                                                                LinkTarget='_blank' Visible='<%# GetImageVisible(DataBinder.Eval(Container.DataItem, "Image")) %>'
                                                                Editing='<%# _isMaster %>' UploadArgument="UploadImage" DeleteArgument="DeleteImage"
                                                                LiveArgument="LiveImage" />
                                                            <asp:Label ID="Label3" runat="server" Text='<%# PrintLine() %>' Visible='<%# _isMaster %>' />
                                                            <asp:DropDownList ID="drItemKind" runat="server" Visible='<%# _isMaster %>' AutoPostBack="true"
                                                                OnSelectedIndexChanged="drItemKind_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="top" style='<%# NewsTDStyle(DataBinder.Eval(Container.DataItem, _dataImageCol)) %>'>
                                                            <uc2:EditText ID="edItemText" runat="server" TextBoxHeight="200" TextBoxWidth='<%# NewsEditWidth(DataBinder.Eval(Container.DataItem, _dataImageCol)) %>'
                                                                TextMode="MultiLine" Text='<%# DirectPrint(DataBinder.Eval(Container.DataItem, _dataTextCol),DataBinder.Eval(Container.DataItem, "Simple")) %>'
                                                                EditingText='<%# DataBinder.Eval(Container.DataItem, _dataTextCol) %>' Visible='<%# _showText %>' />
                                                            <asp:Label ID="lblMorePicture" runat="server" Text='<%= PrintMore(DataBinder.Eval(Container.DataItem, _dataKeyCol)) %>'
                                                                Visible='<%# _showMore %>' CssClass="txt-link" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="sep-hr" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </asp:View>
                        <asp:View ID="viewIcon" runat="server">
                            <div id="wrap">
                                <div class="partB">
                                    <asp:DataList ID="IconList" runat="server" DataKeyField="<%# _dataKeyCol %>" ShowFooter="False"
                                        ShowHeader="False" HorizontalAlign="Center" RepeatColumns="4" RepeatDirection="Horizontal" >
                                        <ItemTemplate>
                                            <div class="picBox" onmouseover="this.className += ' cur';" onmouseout="this.className=this.className.replace(' cur','');">
                                                <table style="height: 180px;">
                                                    <tr>
                                                        <td>
                                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%= GetImageHRef(DataBinder.Eval(Container.DataItem, _dataKeyCol)) %>'
                                                                Target="_blank" ImageUrl='<%# GetImageThumb(DataBinder.Eval(Container.DataItem, _dataImageCol)) %>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <h2 style="line-height: 22px; text-align: left">
                                                    <asp:HyperLink ID="HyperLink14" runat="server" NavigateUrl='<%= GetImageHRef(DataBinder.Eval(Container.DataItem, _dataKeyCol)) %>'
                                                        Target="_blank" Text='<%# DataBinder.Eval(Container.DataItem, _dataTitleCol) %>' />
                                                </h2>
                                                <asp:Label ID="lblDate" runat="server"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="viewTitle" runat="server">
                            <asp:DataList ID="TitleList" runat="server" DataKeyField="<%# _dataKeyCol %>" ShowFooter="False"
                                ShowHeader="False" Width="100%" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="text-align: left; width: 120px">
                                                            <asp:Label ID="lblDate" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# TitleUrl(DataBinder.Eval(Container.DataItem, _dataTitleCol),
                                                                                                        DataBinder.Eval(Container.DataItem, _dataKeyCol))  %>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </asp:View>
                    </asp:MultiView>
                    <asp:Label ID="lblNoRecord" runat="server" Text="没有数据！"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="搜索"></asp:Label>
                            <asp:TextBox ID="txtSearch" runat="server" Width="84px" OnTextChanged="txtSearch_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <asp:Button ID="btnRefresh" runat="server" Text="执行" Style="height: 26px; width: 55px"
                                OnClick="btnRefresh_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="清除" Style="height: 26px; width: 55px"
                                OnClick="btnClear_Click" />
                            <asp:DropDownList ID="drPublish" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drPublish_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="Published" Selected="True">公布</asp:ListItem>
                                <asp:ListItem Value="Unpublished">保留</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Panel ID="barBot" runat="server">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnAdd" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID="PnDate" runat="server">
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:DropDownList ID="drKind" runat="server">
                                </asp:DropDownList>
                                <asp:Label ID="Label1" runat="server" Text="标题" />
                                <asp:TextBox ID="txtTitle" runat="server" Width="220px" Text="标题"></asp:TextBox>
                                <asp:Label ID="lblExtra" runat="server" Text="" />
                                <asp:TextBox ID="txtExtra" runat="server" Width="294px" Text=""></asp:TextBox>
                                <asp:Button ID="btnExtra" runat="server" Text="" Width="75px" OnClick="btnExtra_Click" />
                                <asp:RadioButton ID="btnTxt1" runat="server" Checked="True" GroupName="1" Visible="False" /><asp:RadioButton
                                    ID="btnTxt2" runat="server" GroupName="1" Visible="False" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td valign="top">
                                <asp:TextBox ID="txtBody" runat="server" Width="700px" Height="200px" TextMode="MultiLine"
                                    Text="正文"></asp:TextBox>
                            </td>
                            <td valign="top">
                                <asp:Button ID="btnNewItem" runat="server" Text="增加" Width="75px" OnClick="btnNewItem_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Label ID="lblPolicy" runat="server" CssClass="txt-normal-gray" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
