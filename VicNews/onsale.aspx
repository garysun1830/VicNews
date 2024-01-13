<%@ Page Title="打折降价" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="onsale.aspx.cs" Inherits="VicNews.onsale" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table style="width: 100%">
        <tr>
            <td>
                <asp:DataList ID="RecList" runat="server" DataKeyField="SaleIndex" ShowFooter="False"
                    ShowHeader="False" Width="100%" RepeatColumns="3" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" CssClass="txt-label" Text='<%# DataBinder.Eval(Container.DataItem, "StoreName") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRange" runat="server" Text='<%: DisplayRange(DataBinder.Eval(Container.DataItem, "BeginDate"),
                                                                                                                                    DataBinder.Eval(Container.DataItem, "EndDate")) %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href='SaleDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "SaleIndex") %>'
                                        target="_blank">
                                        <asp:Image ID="Image2" runat="server" BorderWidth="0" ImageUrl='<%: Logo(DataBinder.Eval(Container.DataItem, "Image")) %>' /></a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href='SaleDetails.aspx?id=<%# DataBinder.Eval(Container.DataItem, "SaleIndex") %>'
                                        target="_blank">查看全部...</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Panel ID="pnEdit" runat="server" Visible='<%: _isMaster %>'>
                                        <asp:LinkButton ID="btnAddItem" runat="server" Text="增加" CommandName="Edit" CommandArgument="Add"></asp:LinkButton>
                                        <asp:LinkButton ID="btnCopy" runat="server" Text="复制" CommandName="Edit" CommandArgument="Copy"></asp:LinkButton>
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="修改" CommandArgument="Edit"></asp:LinkButton>
                                        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            Text="删除"></asp:LinkButton>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle VerticalAlign="Top" />
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:Label ID="lblPolicy" runat="server" Text="Label" CssClass="txt-normal-gray"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:LinkButton ID="btnAdd" runat="server" Text="增加" OnClick="btnAdd_Click"></asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>
