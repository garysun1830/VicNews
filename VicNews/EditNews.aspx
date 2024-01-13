<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditNews.aspx.cs" Inherits="VicNews.EditNews" %>
<%--<%@ Register Src="NewsFace.ascx" TagName="NewsFace" TagPrefix="uc1" %>--%>
<%@ Register Src="NewsFace.ascx" TagName="NewsFace" TagPrefix="uc12" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>增加新闻</title>
    <link href="vicnews.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
   
	   function GoParent()
	   { 
	        window.location.replace("index.aspx");
        }
        
    </script>

</head>
<body>
    <form id="form2" runat="server">
    <table style="width: 100%">
        <tr>
            <td valign="top" style="width: 560px; text-align: left;">
                <asp:Button ID="btnSubmit" runat="server" Text="提交" Width="80px"
                    CssClass="txt-normal" onclick="btnSubmit_Click" />
                  <asp:Button ID="btnSave" runat="server" Text="保存" Width="80px"
                    CssClass="txt-normal" onclick="btnOk_Click" />
              <input id="Button1" type="button" value="取消" onclick="GoParent();" class="txt-normal" />
                <%--<uc1:NewsFace ID="faceNews" runat="server" CssClass="txt-normal" ReadOnly="False" />--%>
                <uc12:NewsFace ID="faceNews" runat="server" CssClass="txt-normal" ReadOnly="False" />
            </td>
            <td valign="top">
                <asp:Panel ID="pnTool" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnDict" runat="server" Visible="false">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnTrans" runat="server" Visible="false">
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdParent" runat="server" />
    </form>
</body>
</html>


