<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPage.Master" AutoEventWireup="true" CodeBehind="VideoDetails.aspx.cs" Inherits="VicNews.VideoDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table style="width: 100%">
        <tr>
            <td>
                <div id="threadlist" style="position: relative;">
                    <h3>
                        <asp:Label ID="lblTitle" runat="server" />
                    </h3>
                    <asp:DataList ID="VideoList" runat="server" Width="100%" DataKeyField="Index" RepeatColumns="10"
                        RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%: VideoUrl(DataBinder.Eval(Container.DataItem, "Number")) %>'
                                                Text='<%: VideoNumber(DataBinder.Eval(Container.DataItem, "Number")) %>' CssClass='<%: LinkClass(DataBinder.Eval(Container.DataItem, "Number")) %>' />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
