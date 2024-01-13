<%@ Page Language="C#" MasterPageFile="~/SiteMasterPage.master" AutoEventWireup="true"
    CodeFile="living.aspx.cs" Inherits="living" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderGlobal" runat="Server">
    <center>
        <table style="width: 1000px" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="text-align: left">
                                <asp:Menu ID="menuMain" Width="300px" runat="server" Orientation="Horizontal" CssClass="txt-normal" Visible="false">
                                    <DynamicMenuItemStyle CssClass="txt-normal" />
                                    <Items>
                                        <asp:MenuItem Text="购物" Value="0">
                                            <asp:MenuItem Text="购物中心" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="知名连锁商店" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="1元店(Dollar Store)" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="服装店" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="蔬菜副食商店" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="百货店" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="服装店" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="电器店" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="建筑材料店" Value="New Item"></asp:MenuItem>
                                        </asp:MenuItem>
                                        <asp:MenuItem Text="网络买卖" Value="0">
                                            <asp:MenuItem Text="站点" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="免费物品" Value="New Item"></asp:MenuItem>
                                        </asp:MenuItem>
                                        <asp:MenuItem Text=" 银行" Value="1">
                                            <asp:MenuItem Text="传统银行" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="网络银行" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="保险公司" Value="New Item"></asp:MenuItem>
                                             <asp:MenuItem Text="房屋贷款" Value="New Item"></asp:MenuItem>
                                           <asp:MenuItem Text="投资理财" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text=" 房屋买卖" Value="1">
                                            <asp:MenuItem Text="市场动态" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="经纪人" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="房屋检验" Value="New Item"></asp:MenuItem>
                                             <asp:MenuItem Text="房屋贷款" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text=" 家居生活" Value="1">
                                            <asp:MenuItem Text="房屋装修" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="电工" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="水暖工" Value="New Item"></asp:MenuItem>
                                             <asp:MenuItem Text="随叫服务工" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text=" 交通" Value="1">
                                            <asp:MenuItem Text="公交车" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="出租车" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="飞机场" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="轮渡" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text="景点" Value="2">
                                            <asp:MenuItem Text="旅游区" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="社区公园" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="露营地" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="城镇" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text="餐饮" Value="3">
                                            <asp:MenuItem Text="中餐" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="西餐" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="快餐" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text="娱乐" Value="4">
                                            <asp:MenuItem Text="电影院" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="剧场" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text="体育" Value="4">
                                            <asp:MenuItem Text="活动中心" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="球场" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text="网络论坛" Value="4">
                                            <asp:MenuItem Text="中文论坛" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                        <asp:MenuItem Text="报纸杂志" Value="4">
                                            <asp:MenuItem Text="中文" Value="New Item"></asp:MenuItem>
                                            <asp:MenuItem Text="英文" Value="New Item"></asp:MenuItem></asp:MenuItem>
                                    </Items>
                                </asp:Menu>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
