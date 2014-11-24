<%@ Page Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" Inherits="CRM.Admin.AdminUser.List" Codebehind="List.aspx.cs" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="mainContent" runat="Server">

    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Admin Users</h3></div>

        <div class="innerContentForm">
        <p class="top"><a href="details.aspx">Add a new Admin User</a></p>

            <table class="sTable">

            <asp:ListView ID="lvItems" runat="server" OnPagePropertiesChanging="lvItems_PagePropertiesChanging">
                    
                <LayoutTemplate>      
                    
                    <thead>
                        <tr class="header">
                            <th style="text-align:left;"><strong>Name</strong></th>
                            <th style="text-align:center;  width:140px" colspan="3"><strong>Actions</strong></th>
                        </tr>
                    </thead>
                        
                    <div id="itemPlaceholder" runat="server" />                        
                    
                </LayoutTemplate>
                
                <ItemTemplate>
                        
                    <tr>
                        <td style="text-align:left;"><%#Eval("FirstName") + " " + Eval("Surname") %></td>
                       <td style="text-align:center;width:220px;">
                            <a href="BespokePermissions/list.aspx?id=<%#Eval("ID") %>">Bespoke permissions</a>
                        </td>
                       <td style="text-align:center;width:220px;">
                            <a href="Permissions/list.aspx?id=<%#Eval("ID") %>">High level permissions</a>
                        </td>
                        <td style="text-align:center;width:100px;">
                            <a href="details.aspx?id=<%#Eval("ID") %>">Details</a>
                        </td>

                    </tr>
                        
                </ItemTemplate>
                    
                <EmptyDataTemplate>
                    
                    <tr><td>No admins have been added</td></tr>
                    
                </EmptyDataTemplate>
                    
                    
            </asp:ListView>
            
                <tr class="pagerTr">
                    <td colspan="4">
                        <div class="pager">
                            <asp:DataPager ID="dpMain" runat="server" PagedControlID="lvItems" PageSize="20">
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

        </div>

    </div>

</asp:Content>