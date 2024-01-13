<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsFace.ascx.cs" Inherits="VicNews.NewsFace" %>

<table>
    <tr>
        <td valign="top">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:TextBox ID="txtNewsUrl" runat="server" Width="500px"></asp:TextBox><br />
                        <asp:Button ID="btnPasteUrl" runat="server" Text="粘贴" OnClick="btnPasteUrl_Click"
                            Width="75px" />
                        <asp:CheckBox ID="chkHtml" runat="server" Text="Html" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 180px">
                                                <asp:Panel ID="PnDate" runat="server">
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkFixed" runat="server" AutoPostBack="True" OnCheckedChanged="chkFixed_CheckedChanged"
                                                    Text="固定" />
                                                <asp:Button ID="btnWideNews" runat="server" Text="加宽" Width="45px" OnClick="btnWideNews_Click" />
                                                <asp:Button ID="btnNarrowNews" runat="server" Text="缩短" Width="45px" OnClick="btnNarrowNews_Click" />
                                                <asp:CheckBox ID="chkHand" runat="server" Text="撰写" AutoPostBack="True" OnCheckedChanged="chkHand_CheckedChanged" />
                                                <asp:CheckBox ID="chkTool" runat="server" Text="工具" Checked="false" AutoPostBack="True"
                                                    OnCheckedChanged="chkTool_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkSimple" runat="server" Text="简明" />
                                    <asp:CheckBox ID="chkTop" runat="server" Text="置顶" />
                                    <asp:DropDownList ID="drFocus" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:DropDownList ID="drArea" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:DropDownList ID="drSource" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtTitle" runat="server" required="required" Width="100%">标题</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNews" runat="server" Height="500px" Width="500px" TextMode="MultiLine"
                            required="required">正文</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnSubst" runat="server" Visible="false">
                            <asp:Button ID="btnReplace" runat="server" Text="替换中文" OnClick="btnReplace_Click" />
                            <asp:TextBox ID="txtRepFrom" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtRepTo" runat="server"></asp:TextBox>
                            <asp:Button ID="btnSaveRep" runat="server" Text="保存" Width="75px" OnClick="btnSaveRep_Click" />
                            <asp:CheckBox ID="chkDot" runat="server" Text="标点" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnWord" runat="server" Visible="false">
                            <asp:TextBox ID="txtCommon" runat="server" Height="115px" Width="100%" TextMode="MultiLine"></asp:TextBox>
                            <asp:Button ID="btnSaveWord" runat="server" Text="保存" OnClick="btnSaveWord_Click"
                                Width="75px" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top" style="<%: _rightWidth %>" abd="<%: aaa %>">
            <asp:Panel ID="pnTools" runat="server" Visible="False" Width="900px">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:RadioButton ID="btnMicrosoft" runat="server" Text="Microsoft" AutoPostBack="True"
                                Checked="True" GroupName="Tab" OnCheckedChanged="btnMicrosoft_CheckedChanged" />
                            <asp:RadioButton ID="btn123" runat="server" Text="繁体" AutoPostBack="True" GroupName="Tab"
                                OnCheckedChanged="btnMicrosoft_CheckedChanged" />
                            <asp:RadioButton ID="btnExtra" runat="server" Text="更多内容" AutoPostBack="True" GroupName="Tab"
                                OnCheckedChanged="btnMicrosoft_CheckedChanged" />
                            <asp:Button ID="btnWideTool" runat="server" Text="加宽" Width="75px" OnClick="btnWideTool_Click" />
                            <asp:Button ID="btnNarrowTool" runat="server" Text="缩短" Width="75px" OnClick="btnNarrowTool_Click" />
                            <asp:CheckBox ID="chkDict" runat="server" AutoPostBack="True" OnCheckedChanged="chkDict_CheckedChanged"
                                Text="字典" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:MultiView ID="MultiView" runat="server" ActiveViewIndex="0">
                                <asp:View ID="viewMicrosoft" runat="server">
                                    <iframe style="width: 100%; height: 600px" marginheight="0" marginwidth="0" id="iframe3"
                                        src="http://www.microsofttranslator.com/" frameborder="0" scrolling="yes"></iframe>
                                    <asp:Panel ID="pnDict" runat="server">
                                        <iframe style="width: 100%; height: 600px" marginheight="0" marginwidth="0" id="iframe2"
                                            src="http://dict.cn" frameborder="0" scrolling="yes"></iframe>
                                    </asp:Panel>
                                </asp:View>
                                <asp:View ID="view123" runat="server">
                                    <iframe style="width: 100%; height: 600px" marginheight="0" marginwidth="0" id="iframe1"
                                        src="http://www.hao123.com/haoserver/jianfanzh.htm" frameborder="0" scrolling="yes"></iframe>
                                </asp:View>
                                <asp:View ID="viewExtra" runat="server">
                                    <asp:Label ID="Label1" runat="server" Text="更多："></asp:Label>
                                    <asp:TextBox ID="txtExtra" runat="server" Height="493px" Width="794px" TextMode="MultiLine"></asp:TextBox>
                                </asp:View>
                            </asp:MultiView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdExtra" runat="server" />
