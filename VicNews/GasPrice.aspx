<%@ Page Title="维多利亚即时油价" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="GasPrice.aspx.cs" Inherits="VicNews.GasPrice" %>

@{
string a=1;
}
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table style="margin: auto">
        <tr>
            <td>
                <div id="threadlist" style="position: relative;">
                    <h3 style="text-align: center">维多利亚即时油价（普通级）
                    </h3>
                    <asp:DataList ID="GasList" runat="server" ataKeyField="Index" RepeatColumns="1" Width="100%">
                        <ItemTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 80px">
                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Price") %>' />
                                    </td>
                                    <td style="width: 150px">
                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Address") %>' />
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TimeWhen") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div class="sep-hr" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" CssClass="txt-label" Text='一个月平均油价走向图'></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <img alt="" src="@_chart" style="border-width: 0" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSource" runat="server" CssClass="txt-normal-gray"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
