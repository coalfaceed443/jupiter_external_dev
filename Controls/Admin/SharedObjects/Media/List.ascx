<%@ Control Language="C#" AutoEventWireup="true" Inherits="Website.Controls.Admin.SharedObjects.Media.List" Codebehind="List.ascx.cs" %>


         

            <table class="sTable">

                <asp:ListView ID="lvItems" runat="server" onpagepropertieschanging="lvItems_PagePropertiesChanging">
                    
                    <LayoutTemplate>
                    
                        <thead>
                            <tr class="header">
                                <th style="text-align:left;"><strong>Name</strong></th>
                                <th style="width: 20px;">&nbsp;</th>
                                <th style="width: 20px;">&nbsp;</th>
                                <th style="width: 60px; text-align:center;">Is Active</th>
                                <th style="width: 60px; text-align:center;">Actions</th>
                            </tr>
                        </thead>
                        
                        <div id="itemPlaceholder" runat="server" />                        
                    
                    </LayoutTemplate>
                    
                    <ItemTemplate>
                        
                        <tr>
                            <td style="text-align:left;"><%# Eval("Name") %></td>
                            <td style="text-align:center;"><asp:ImageButton ID="btnMoveUp" runat="server" OnClick="btnMoveUp_Click" CommandArgument='<%#Eval("ID") %>' ImageUrl="/_assets/images/admin/butt_up.gif" AlternateText="up"/></td>
                            <td style="text-align:center;"><asp:ImageButton ID="btnMoveDown" runat="server" OnClick="btnMoveDown_Click" CommandArgument='<%#Eval("ID") %>' ImageUrl="/_assets/images/admin/butt_down.gif" AlternateText="down"/></td>
                            <td style="text-align:center;"><%# (bool)Eval("IsActive") ? "Yes" : "No" %></td>
                            <td style="text-align:center;"><a href='details.aspx?mid=<%# Eval("ID") %>&<%= Request.QueryString %>'>Edit</a></td>
                        </tr>
                        
                    </ItemTemplate>
                    
                    <EmptyDataTemplate>
                    
                        <tr><td>No media has been added</td></tr>
                    
                    </EmptyDataTemplate>
                    
                </asp:ListView>

                <tr class="pagerTr"><td colspan="5">
            
                    <div class="pager">
                
                        <asp:DataPager ID="dpMain" runat="server" PagedControlID="lvItems" PageSize="10">                       
                            <Fields>
                    
                                <asp:TemplatePagerField><PagerTemplate>Page:</PagerTemplate></asp:TemplatePagerField>
                                <asp:numericpagerfield ButtonCount="20" NumericButtonCssClass="pager-link" CurrentPageLabelCssClass="pager-current" />
                    
                            </Fields>
                        </asp:DataPager>

                    </div>
            
                </td></tr>

            </table>

            <br class="clearFix" /><br class="clearFix" />