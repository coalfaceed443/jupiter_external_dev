<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True"
    CodeBehind="List.aspx.cs" Inherits="CRM.admin.Calendar.UserTags.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                User Tags</h3>
        </div>
        <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navUserTags" />
        <div class="innerContentForm">
            <p class="top">
                Tag another user to this calender item</p>
            <table class="details">
                <tr>
                    <td>
                        <label>
                            <strong>1) Write a message to the user, they will receive this when you change their
                                attendance status</strong></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <ucUtil:TextEditor ID="txtMessage" runat="server" Height="100" ToolbarSet="basic" />
                    </td>
                    <tr>
                        <td>
                            <label>
                                <strong>2a) If this user is not already in the attendance list below, add the user to
                                    the event:</strong></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ucUtil:AutoComplete ID="acAdminUser" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                <strong>2b) Alternatively toggle their status to update them and send them their message:</strong></label>
                        </td>
                    </tr>
            </table>
            <div class="editor-nolabel">
                <table class="sTable">
                    <tr class="header">
                        <th>Admin</th>
                        <th>Date Added</th>
                        <th>Invitation Status</th>                    
                        <th>Remove</th>
                    </tr>
                    <asp:ListView ID="lvItems" runat="server" OnItemDataBound="lvItems_ItemBound">
                        <LayoutTemplate>
                            <div id="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Admin.DisplayName") %>
                                </td>
                                
                                <td>
                                    <%# ((DateTime)Eval("Timestamp")).ToString(CRM.Code.Constants.DefaultDateStringFormat)%>
                                </td>
                                
                                <td>
                                    <%# Eval("StatusOutput")%>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove from list" OnClick="lnkRemove_Click" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <tr>
                                <td colspan="20">
                                    There are no tags to display
                                </td>
                            </tr>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
