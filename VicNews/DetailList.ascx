<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailList.ascx.cs" Inherits="VicNews.DetailList" %>
<%@ Register Src="EditText.ascx" TagName="EditText" TagPrefix="uc1" %>
<table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td style="text-align: left">
            <asp:Panel ID="pnBrowse" runat="server" Style="width: 100%">
                <table style="width: 1000px">
                    <tr>
                        <td>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            <asp:FileUpload ID="FileUpload2" runat="server" />
                            <asp:FileUpload ID="FileUpload3" runat="server" />
                            <asp:FileUpload ID="FileUpload4" runat="server" />
                            <asp:FileUpload ID="FileUpload5" runat="server" />
                            <asp:FileUpload ID="FileUpload6" runat="server" />
                            <asp:FileUpload ID="FileUpload7" runat="server" />
                            <asp:FileUpload ID="FileUpload8" runat="server" />
                            <asp:FileUpload ID="FileUpload9" runat="server" />
                            <asp:FileUpload ID="FileUpload10" runat="server" />
                            <asp:FileUpload ID="FileUpload11" runat="server" />
                            <asp:FileUpload ID="FileUpload12" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnUpload" runat="server" Text="Upload Files" OnClick="btnUpload_Click" />
                            <asp:TextBox ID="txtImage1" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtImage2" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtImage3" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtImage4" runat="server"></asp:TextBox>
                            <asp:CheckBox ID="chkUrl" runat="server" Checked="True" Text="Url" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DataList ID="DetailData" runat="server" ShowFooter="False" ShowHeader="False"
                Width="100%" OnItemCommand="DetailData_ItemCommand">
                <ItemTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%: GetDetailsImage(DataBinder.Eval(Container.DataItem, "Picture"),
                                                                                                                                                    DataBinder.Eval(Container.DataItem, "PictureUrl")) %>'
                                    Visible='<%: ImageVisible(DataBinder.Eval(Container.DataItem, "Picture"),DataBinder.Eval(Container.DataItem, "PictureUrl")) %>' />
                                <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    Visible='<%: _isMaster %>' Text="删除" OnClientClick="return confirm('确定删除该图片?');"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <asp:Label ID="lblTitle" runat="server" Text='<%: DataBinder.Eval(Container.DataItem, "Title") %>'
                                    Visible='<%: !_isMaster %>'></asp:Label>
                                <asp:TextBox ID="txtTitle" runat="server" TextMode="SingleLine" Width="500px" Text='<%: DataBinder.Eval(Container.DataItem, "Title") %>'
                                    Visible='<%: _isMaster %>'></asp:TextBox>
                                <asp:LinkButton ID="btnChange" runat="server" Text="修改" Visible='<%: _isMaster %>'
                                    CommandArgument="EditTitle" />
                            </td>
                        </tr>
                    </table>
                    <hr style="width: 100%" />
                </ItemTemplate>
            </asp:DataList>
        </td>
    </tr>
</table>
