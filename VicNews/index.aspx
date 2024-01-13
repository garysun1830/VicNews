<%@ Page Title="每日新闻" Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="VicNews.index" %>

<%@ Register Src="EditText.ascx" TagName="EditText" TagPrefix="uc2" %>
<%@ Register Src="NewsBody.ascx" TagName="NewsBody" TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">

    <script type="text/javascript">

        function copyToClipboard(Ctrl) {
            var s = document.getElementById(Ctrl).value;
            if (window.clipboardData) {      // the IE-manier   
                window.clipboardData.setData("Text", s);      // waarschijnlijk niet de beste manier om Moz/NS te detecteren;   // het is mij echter onbekend vanaf welke versie dit precies werkt:  
            }
            else if (window.netscape) {
                netscape.security.PrivilegeManager.enablePrivilege('UniversalXPConnect');
                var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
                if (!clip) return true;
                var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
                if (!trans) return true;
                trans.addDataFlavor('text/unicode');      // om de data uit de transferable te halen hebben we 2 nieuwe objecten nodig   om het in op te slaan  
                var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
                var copytext = s;
                str.data = copytext;
                trans.setTransferData("text/unicode", str, copytext.length * 2);
                var clipid = Components.interfaces.nsIClipboard;
                if (!clip) return true;
                clip.setData(trans, null, clipid.kGlobalClipboard);
            }
            return true;
        }

    </script>

    <table style="width: 100%">
        <tr>
            <td style="text-align: left">
                <div class="container-article">
                    <div class="tabs-container">
                        <asp:Label ID="lblAreaMenu" runat="server" Text="菜单"></asp:Label>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnTop" runat="server">
                    <div id="toplist" style="position: relative;">
                        <h3>
                            <asp:Label ID="lblTop" runat="server" />
                        </h3>
                        <asp:DataList ID="TopList" runat="server" DataKeyField="NewsIndex" ShowFooter="False"
                            ShowHeader="False" Width="100%" OnItemCreated="TopList_ItemCreated" OnItemCommand="TopList_ItemCommand">
                            <ItemTemplate>
                                <div class="topnews">
                                    <table width="100%" style="border-width: 0; border-spacing: 0; padding: 0">
                                        <tr>
                                            <td style="text-align: left; width: 140px">
                                                <asp:Label ID="Label6" runat="server" Text='<%= DisplayDate(DataBinder.Eval(Container.DataItem, "NewsDate")) %>'></asp:Label>
                                            </td>
                                            <td style="text-align: left">
                                                <uc2:EditText ID="edtRemoveTop" runat="server" Text='<%# NewsUrl(DataBinder.Eval(Container.DataItem, "Title"),
                                                    DataBinder.Eval(Container.DataItem, "NewsIndex"),DataBinder.Eval(Container.DataItem, "News")) %>' />
                                                <uc2:EditText ID="edtRemoveTop2" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <div id="threadlist" style="position: relative;">
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <h3>
                                    <asp:Label ID="lblCaption" runat="server" />
                                </h3>
                            </td>
                            <td style="text-align: right" class="tabletop">
                                <asp:RadioButton ID="btnFull" runat="server" Text="全文" Checked="true" AutoPostBack="True"
                                    GroupName="News" OnCheckedChanged="btnFull_CheckedChanged" />
                                <asp:RadioButton ID="btnTitle" runat="server" Text="标题" AutoPostBack="True" GroupName="News"
                                    OnCheckedChanged="btnTitle_CheckedChanged" />
                            </td>
                        </tr>
                    </table>
                    <asp:MultiView ID="Views" runat="server">
                        <asp:View ID="viewText" runat="server">
                            <uc3:NewsBody ID="detNews" runat="server" />
                        </asp:View>
                        <asp:View ID="viewTitle" runat="server">
                            <asp:DataList ID="TitleList" runat="server" DataKeyField="NewsIndex" ShowFooter="False"
                                ShowHeader="False" Width="100%">
                                <ItemTemplate>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="text-align: left; width: 140px">
                                                            <asp:Label ID="Label6" runat="server" Text='<%# DisplayDate(DataBinder.Eval(Container.DataItem, "NewsDate")) %>'></asp:Label>
                                                        </td>
                                                        <td style="text-align: left">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# NewsUrl(DataBinder.Eval(Container.DataItem, "Title"),
                                                                    DataBinder.Eval(Container.DataItem, "NewsIndex"),1) %>' />
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
                        <asp:View ID="viewAbstract" runat="server">
                        </asp:View>
                    </asp:MultiView>
                </div>
            </td>
        </tr>
        <tr>
            <td style="text-align: left;">
                <table>
                    <tr>
                        <td style="width: auto">
                            <asp:Panel ID="pnPageBar" runat="server" Width="100%">
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="pnDate" runat="server">
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:DropDownList ID="drPeriod" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drPeriod_SelectedIndexChanged"
                                CssClass="txt-normal">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">一天</asp:ListItem>
                                <asp:ListItem Value="4">三天</asp:ListItem>
                                <asp:ListItem Value="2">一周</asp:ListItem>
                                <asp:ListItem Value="3">一个月</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="搜索"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" Width="84px" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnRefresh" runat="server" Text="执行" Style="height: 26px; width: 55px" />
                        </td>
                        <td>
                            <asp:Button ID="btnClear" runat="server" Text="清除" OnClick="btnClear_Click" Style="height: 26px; width: 55px" />
                        </td>
                        <td>
                            <asp:DropDownList ID="drPublish" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drPublish_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="Published" Selected="True">公布</asp:ListItem>
                                <asp:ListItem Value="Unpublished">保留</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="text-align: left">
                <asp:LinkButton ID="btnListTitle" runat="server" Text="列标题" CssClass="txt-normal"
                    Visible="False"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdTitle" runat="server" />
</asp:Content>
