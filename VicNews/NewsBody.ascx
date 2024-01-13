<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsBody.ascx.cs" Inherits="VicNews.NewsBody" %>
<%@ Register Src="EditText.ascx" TagName="EditText" TagPrefix="uc2" %>
<%@ Register Src="EditImage.ascx" TagName="EditImage" TagPrefix="uc3" %>
<table style="border-collapse: collapse; width: 100%">
    <tr>
        <td>
            <div class="noborder">
                <asp:DataList ID="ItemList" runat="server" DataKeyField="NewsIndex" ShowFooter="False"
                    ShowHeader="False" Width="100%" CellPadding="0" CellSpacing="0">
                    <ItemTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnEdit" runat="server" Visible='<%# _isMaster %>'>
                                        <asp:LinkButton ID="btnAddItem" runat="server" Text="增加" CommandName="Edit" CommandArgument="Add" />
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="修改" CommandArgument="Edit" />
                                        <asp:LinkButton ID="btnSubmit" runat="server" Visible="<%# ShowSubmit  %>"
                                            CommandName="Submit" Text="提交" CommandArgument="Submit" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 160px;">
                                                <asp:Label ID="Label6" runat="server" Text='<%# DisplayDate(DataBinder.Eval(Container.DataItem, "NewsDate")) %>'></asp:Label>
                                            </td>
                                            <td style="text-align: center;">
                                                <uc2:EditText ID="edtTitle" runat="server" CssClass="txt-label" TextMode="SingleLine"
                                                    TextBoxWidth="300" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>' />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label121" runat="server" Text='<%# GetSource(DataBinder.Eval(Container.DataItem, "SourceName")) %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td valign="top">
                                                <div>
                                                    <uc3:EditImage ID="EditImage" runat="server" ImageUrl='<%# GetImageThumb(DataBinder.Eval(Container.DataItem, "Image")) %>'
                                                        LinkUrl='<%# GetImageHRef(DataBinder.Eval(Container.DataItem, "NewsIndex")) %>'
                                                        LinkTarget='_blank' Visible='<%# GetImageVisible(DataBinder.Eval(Container.DataItem, "Image")) %>'
                                                        Editing='<%# _isMaster %>' UploadArgument="UploadImage" DeleteArgument="DeleteImage"
                                                        LiveArgument="LiveImage" />
                                                </div>
                                                <asp:Panel ID="Panel1" runat="server" Visible="<%# _isMaster %>">
                                                    <div>
                                                        <asp:CheckBox ID="chkSimple" runat="server" Text="简明" Checked='<%#  DataBinder.Eval(Container.DataItem, "Simple")%>' />
                                                        <asp:CheckBox ID="chkTop" runat="server" Text="置顶" Checked='<%#  DataBinder.Eval(Container.DataItem, "OnTop")%>' />
                                                    </div>
                                                    <div>
                                                        <asp:DropDownList ID="drFocus" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div>
                                                        <asp:DropDownList ID="drArea" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div>
                                                        <asp:DropDownList ID="drSource" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div>
                                                        <asp:Button ID="btnChange" runat="server" Text="修改" CommandArgument="ChangeNews"
                                                            Width="75px" />
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                            <td valign="top" style='<%# NewsTDStyle(DataBinder.Eval(Container.DataItem, "Image")) %>'>
                                                <uc2:EditText ID="edtNewsText" runat="server" TextBoxHeight="200" TextBoxWidth='<%# NewsEditWidth(DataBinder.Eval(Container.DataItem, "Image")) %>'
                                                    TextMode="MultiLine" Text='<%# DirectPrint(DataBinder.Eval(Container.DataItem, "Text"),
                                                                    DataBinder.Eval(Container.DataItem, "Simple")) %>' EditingText='<%# DataBinder.Eval(Container.DataItem, "Text") %>' />
                                                <asp:Panel ID="pnMore" runat="server" Visible='<%# ReadMoreVisible(DataBinder.Eval(Container.DataItem, "PictureCount"),
                                                                DataBinder.Eval(Container.DataItem, "Simple"),  DataBinder.Eval(Container.DataItem, "ExtraCount")) %>'>
                                                    <asp:Label ID="lblMorePicture" runat="server" Text='<%# PrintMoreOrSimpleUrl(DataBinder.Eval(Container.DataItem, "NewsIndex")) %>'
                                                        CssClass="txt-link" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="sep-hr" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle BorderStyle="None" />
                </asp:DataList>
            </div>
            <asp:Label ID="lblNoRecord" runat="server" Text="没有数据！"></asp:Label>
            <div style="text-align: left">
                <asp:LinkButton ID="btnAdd" runat="server" Text="增加" OnClick="btnAdd_Click" Visible="False"></asp:LinkButton></div>
        </td>
    </tr>
</table>
