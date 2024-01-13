<%@ Page Title="维多利亚即时油价" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="exrate.aspx.cs" Inherits="VicNews.exrate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table>
        <tr>
            <td colspan="3">
                <table style="margin: auto;">
                    <tr>
                        <td>
                            <div id="threadlist" style="position: relative;">
                                <h3 style="text-align: center">
                                    每日行情（Bank of Canada）
                                </h3>
                                <asp:DataList ID="RateList" runat="server" ataKeyField="Index" RepeatColumns="1"
                                    RepeatDirection="Vertical" HorizontalAlign="Center" 
                                    onitemcreated="RateList_ItemCreated">
                                    <ItemTemplate>
                                        <table style="margin: auto">
                                            <tr>
                                                <td style="width: 240px">
                                                    <asp:Label ID="lblData1" runat="server" CssClass="txt-label" Text='<%= DataBinder.Eval(Container.DataItem, "Name") %>' />
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lblData2" runat="server" Text='<%= DataBinder.Eval(Container.DataItem, "Day1") %>' />
                                                </td>
                                                <td style="width: 100px;">
                                                    <asp:Label ID="lblData3" runat="server" Text='<%= DataBinder.Eval(Container.DataItem, "Day2") %>' />
                                                </td>
                                                <td style="width: 100px;">
                                                    <asp:Label ID="lblData4" runat="server" Text='<%= DataBinder.Eval(Container.DataItem, "UpDown") %>' />
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
                </table>
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="Label21" runat="server" CssClass="txt-label" Text="美元/加元（三个月）"></asp:Label>
            </td>
            <td style="width: 20px">
            </td>
            <td style="text-align: right">
                <asp:Label ID="Label22" runat="server" CssClass="txt-label" Text="人民币/加元（三个月）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <img alt="" src="http://chart.finance.yahoo.com/z?s=CADUSD%3dX&t=3m&q=l&l=off&z=m&a=v&p=s&lang=en-CA&region=CA"
                    style="border: 0" />
            </td>
            <td style="width: 20px">
            </td>
            <td>
                <img src="http://chart.finance.yahoo.com/z?s=CADCNY%3dX&t=3m&q=l&l=off&z=m&a=v&p=s&lang=en-CA&region=CA"
                    alt="" style="border: 0" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="Label27" runat="server" CssClass="txt-label" Text="美元/加元（五年）"></asp:Label>
            </td>
            <td style="width: 20px">
            </td>
            <td style="text-align: right">
                <asp:Label ID="Label2" runat="server" CssClass="txt-label" Text="人民币/加元（五年）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <img alt="" src="http://chart.finance.yahoo.com/z?s=CADUSD%3dX&t=5y&q=l&l=off&z=m&a=v&p=s&lang=en-CA&region=CA"
                    style="border: 0" />
            </td>
            <td style="width: 20px">
            </td>
            <td>
                <img src="http://chart.finance.yahoo.com/z?s=CADCNY%3dX&t=5y&q=l&l=off&z=m&a=v&p=s&lang=en-CA&region=CA"
                    alt="" style="border: 0" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right">
                <asp:Label ID="Label24" runat="server" CssClass="txt-label" Text="美元/加元（十年）"></asp:Label>
            </td>
            <td style="width: 20px">
            </td>
            <td style="text-align: right">
                <asp:Label ID="Label25" runat="server" CssClass="txt-label" Text="人民币/加元（十年）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <img alt="" src="http://www.bankofcanada.ca/stats/assets/graphs/currencies_IEXM0102_en.png"
                    style="border: 0" />
            </td>
            <td style="width: 20px">
            </td>
            <td>
                <img src="http://www.bankofcanada.ca/stats/assets/graphs/currencies_IEXM2201_en.png"
                    alt="" style="border: 0" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblSource" runat="server" CssClass="txt-normal-gray"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
