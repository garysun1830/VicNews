<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="PlayListen.aspx.cs" Inherits="VicNews.PlayListen" %>
<%@ Register Assembly="EmbedFlash" Namespace="ShareLib" TagPrefix="cc1" %>
<%@ Register Assembly="EmbedMP3" Namespace="ShareLib" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <table style="width: 100%">
        <tr>
            <td>
                <div id="threadlist" style="position: relative;">
                    <h3>
                        <asp:Label ID="lblCaption" runat="server" />
                    </h3>
                    <table>
                        <tr>
                            <td valign="top">
                                <asp:Image ID="Image" runat="server" />
                            </td>
                            <td valign="top" style="width: 800px">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:DataList ID="ListenList" runat="server" Width="100%" DataKeyField="Index" RepeatDirection="Horizontal"
                                                RepeatColumns="6">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="text-align: left">
                                                                <asp:LinkButton ID="btnPlay" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>'
                                                                    CommandName='<%# DataBinder.Eval(Container.DataItem, "WebPath") %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <p />
                                            <table>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:MultiView ID="MultiView" runat="server">
                                                            <asp:View ID="viewOgg" runat="server">
                                                                <asp:Label ID="lblOgg" runat="server" CssClass="txt-label" /><br />
                                                                <audio controls="controls" autoplay="autoplay">
                                                        浏览器不能播放音乐，请使用“Firefox”或者“Chrome”。
                                                        <source src="<%: _oggFileName %>" type="audio/ogg" />
                                                </audio>
                                                            </asp:View>
                                                            <asp:View ID="viewMP3" runat="server">
                                                                <cc2:EmbedMP3 ID="EmbedMP3" runat="server" CSSClass="txt-label" 
                                                                    AutoStart="true" />
                                                            </asp:View>
                                                            <asp:View ID="viewFlash" runat="server">
                                                                <cc1:EmbedFlash ID="Flash" runat="server" Height="600px" Width="800px" CSSClass="txt-label" />
                                                            </asp:View>
                                                        </asp:MultiView>
                                                    </td>
                                                    <td valign="top">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnPrev" runat="server" Text="上一个" Width="100px" OnClick="btnPrev_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnNext" runat="server" Text="下一个" Width="100px" OnClick="btnNext_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblPolicy" runat="server" CssClass="txt-normal-gray" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: left" colspan="2">
            </td>
        </tr>
    </table>
</asp:Content>
