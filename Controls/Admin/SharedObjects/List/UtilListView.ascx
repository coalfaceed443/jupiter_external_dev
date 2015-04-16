<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="UtilListView.ascx.cs"
    Inherits="CRM.Controls.Admin.SharedObjects.List.UtilListView" %>
<%@ Import Namespace="CRM.Code.Helpers" %>
<% if (!_CanOrder)
   {%>
<script type="text/javascript">

    $(document).ready(function () {

        $(".fancybox").fancybox({
            type : "iframe"
        });
        $(".admin-ordering").hide();
    })
    
</script>
<%} %>
<asp:Panel ID="pnlQueryManager" runat="server">
<div id="query-manage">
    <h4>
        <a href="#" onclick="ToggleQueryManager($(this));return false;" class="query-toggle">Show Query Manager</a></h4>

        <asp:Panel ID="pnlCustomQuery" runat="server">

    <div class="query-section" style="margin:20px 0;">
        <asp:DropDownList ID="ddlExistingQueries" runat="server" DataTextField="Text" DataValueField="Value" 
            AutoPostBack="true" 
            onselectedindexchanged="ddlExistingQueries_SelectedIndexChanged" AppendDataBoundItems="true">
            <asp:ListItem Value="" Text="No Filter / Create new Query" />
        </asp:DropDownList>

       
        <asp:MultiView ID="mvManageQuery" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewAdd" runat="server">

                <asp:ValidationSummary ID="valQuery" runat="server" ValidationGroup="CheckNewQuery"  CssClass="validation" EnableClientScript="false" />
                <ucUtil:TextBox ID="txtName" runat="server" CssClass="textbox" Width="272" Height="20" Required="true" DefaultValue="Name this query" ValidationGroup="CheckNewQuery" />
                <asp:CheckBox ID="chkIsPublic" runat="server" Text="Make query public to other admins" CssClass="makepublic" style="float:none;" />
                <asp:LinkButton ID="lnkAdd" runat="server" onclick="lnkAdd_Click">
                    <img src="/_assets/images/admin/icons/new.png" alt="" />
                </asp:LinkButton>            
            </asp:View>
            <asp:View ID="viewDelete" runat="server">
                <asp:LinkButton ID="lnkDeleteQuery" runat="server" ValidationGroup="CheckNewQuery" title="Delete this query" OnClick="lnkDeleteQuery_Click">
                    <img src="/_assets/images/admin/icons/remove.png" alt=""  /><span style='margin-right:10px;'>Delete query</span>
                </asp:LinkButton>
            </asp:View>           
        </asp:MultiView>
    </div>

    <asp:Panel ID="pnlManageQuery" runat="server" Visible="false">
    <div class="query-section">
        <asp:Repeater ID="rptQuery" runat="server" OnItemDataBound="rptQuery_ItemDataBound">
            <ItemTemplate>
                <ucUtil:DataQuery ID="ucDataQuery" runat="server" />
            </ItemTemplate>
        </asp:Repeater>

        <ucUtil:DataQuery ID="ucDataQuery" runat="server" ShowAdd="true" />
    
    </div>
    </asp:Panel>

      <div class="query-section">
    
    <p>This query will be used for:&nbsp;
    <asp:RadioButton ID="rbNone" runat="server" CssClass="mailouts" Text="Data only" GroupName="mailout" Checked="true" />&nbsp;
    <asp:RadioButton ID="rbMailOut" runat="server" CssClass="mailouts" Text="Mailouts" GroupName="mailout" />&nbsp;
    <asp:RadioButton ID="rbEmail" runat="server" CssClass="mailouts" Text="Emailing"  GroupName="mailout" />     
    </p>
    <asp:LinkButton ID="lnkRunQuery" runat="server" onclick="lnkRunQuery_Click" CssClass="run-query">Save and Run Query</asp:LinkButton>
    </div>

        <asp:CheckBox ID="chkGroupByRelationship" runat="server" Text="Group by relationship" style="float:left;" />
            
        <asp:CheckBox ID="chkGroupByAddress" runat="server" Text="Group by address" style="float:left;" />

        </asp:Panel>

    <div class="query-section change-view">        
        <asp:DropDownList ID="ddlChangeView" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
            OnSelectedIndexChanged="ddlChangeView_SelectedIndexChanged">
            <asp:ListItem Value="1" Text="Default View" />
            <asp:ListItem Value="-1" Text="My View" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlFields" runat="server" DataTextField="Text" AppendDataBoundItems="true"
            AutoPostBack="true" DataValueField="Value" OnSelectedIndexChanged="ddlFields_SelectedIndexChanged">
            <asp:ListItem Text="Add a new Column" />
        </asp:DropDownList>
    </div>

</div>
</asp:Panel>

<div id="filters">
</div>
<table class="sTable" style="width: <%=Width%>px">
    <thead <%= IsDataEmpty ? "style=\"display:none;\"" : "" %>>
        <tr class="header">
            <th class="admin-ordering">
                &nbsp;
            </th>
            <asp:Repeater ID="rptHeader" runat="server" OnItemDataBound="rptHeader_ItemDataBound">
                <ItemTemplate>
                    <th style="text-align: left;">
                        <strong>
                            <%# Eval("_DataFieldFriendly")%></strong>
                        <div class="list-controls">
                            <div class="header-order">
                                <asp:ImageButton ID="btnMoveLeft" runat="server" OnClick="btnMoveLeft_Click" CommandArgument='<%#Eval("ID") %>'
                                    ImageUrl="/_assets/images/admin/butt_left.gif" AlternateText="left" />
                                <asp:ImageButton ID="btnMoveRight" runat="server" OnClick="btnMoveRight_Click" CommandArgument='<%#Eval("ID") %>'
                                    ImageUrl="/_assets/images/admin/butt_right.gif" AlternateText="right" />
                            </div>
                            <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click">x</asp:LinkButton>
                        </div>
                    </th>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
    </thead>
    <asp:ListView ID="lvItems" runat="server" OnItemDataBound="lvItems_ItemBound" OnPagePropertiesChanging="lvItems_PagePropertiesChanging">
        <LayoutTemplate>
            <div id="itemPlaceholder" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td style="text-align: center; width: 100px;" class="admin-ordering">
                    <asp:ImageButton ID="btnMoveTop" runat="server" OnClick="btnMoveTop_Click" CommandArgument='<%#Eval("ID") %>'
                        ImageUrl="/_assets/images/admin/butt_dup.gif" AlternateText="up" Style="margin-bottom: 3px" />
                    <asp:ImageButton ID="btnMoveUp" runat="server" OnClick="btnMoveUp_Click" CommandArgument='<%#Eval("ID") %>'
                        ImageUrl="/_assets/images/admin/butt_up.gif" AlternateText="up" />
                    <asp:ImageButton ID="btnMoveDown" runat="server" OnClick="btnMoveDown_Click" CommandArgument='<%#Eval("ID") %>'
                        ImageUrl="/_assets/images/admin/butt_down.gif" AlternateText="down" />
                    <asp:ImageButton ID="btnMoveBottom" runat="server" OnClick="btnMoveBottom_Click"
                        CommandArgument='<%#Eval("ID") %>' ImageUrl="/_assets/images/admin/butt_ddown.gif"
                        AlternateText="down" />
                </td>
                <asp:Repeater ID="rptFields" runat="server">
                    <ItemTemplate>
                        <td style="text-align: left;">
                            <%# CRM.Code.Utils.List.Custom.Eval((((ListDataItem)Container.DataItem).DataItem), ((ListDataItem)Container.DataItem).DataName)%>
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <tr>
                <td colspan="20">
                    There are no results to display
                </td>
            </tr>
        </EmptyDataTemplate>
    </asp:ListView>
    <tr class="pagerTr">
        <td colspan="5" id="tdPage" runat="server">
            <div class="pager">
                <asp:DataPager ID="dpMain" runat="server" PagedControlID="lvItems" PageSize="40">
                    <Fields>
                        <asp:TemplatePagerField>
                            <PagerTemplate>
                                Page:</PagerTemplate>
                        </asp:TemplatePagerField>
                        <asp:NumericPagerField ButtonCount="20" NumericButtonCssClass="pager-link" CurrentPageLabelCssClass="pager-current" />
                    </Fields>
                </asp:DataPager>
            </div>
        </td>
    </tr>
</table>
<div class="buttons">
    <ucUtil:Button ID="btnExport" runat="server" ButtonText="Export Data" ImagePath="table_go.png"
        Class="positive" />
<a class="neutral fancybox" href='/Controls/Admin/SharedObjects/List/SubmitMailout.aspx?target=<%= Type.FullName %>'><img src="/_assets/images/admin/icons/table.png" alt="">Log Communication</a>
</div>
