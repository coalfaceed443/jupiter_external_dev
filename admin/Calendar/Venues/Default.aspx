<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CRM.admin.Calendar.Venues.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Calendar :
                <%if (Entity != null)
                  {%>
                Edit
                <%}
                  else
                  {%>
                Add
                <%} %></h3>
        </div>
                <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navVenues"  />

        <div class="innerContentForm">


            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />
               
            <table class="details">
                <tr>
                    <td>
                        <label>
                            Venue: *</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="acVenue" runat="server" Required="true" Name="Venue" />
                        
                    </td>
                    <td>
                        <label>
                            From: *</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtFrom" runat="server" Required="true" ShowTime="true" ShowDate="false" Name="From time" />
                    </td>
                    <td>
                        <label>
                            To: *</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtTo" runat="server" Required="true"  ShowTime="true" ShowDate="false" Name="To time" />
                    </td>
                </tr>
                 <tr>
                    <td colspan="6">
                      <div class="buttons">
                    <label>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Venue" ImagePath="tick.png"
                    Class="positive" /></label>
                    </div>
                    </td>
                </tr> 
                
                
            </table>
            
            <br class="clearFix" />
            <br class="clearFix" />

            <table class="sTable">

            <asp:Repeater ID="rptItems" runat="server">
                <ItemTemplate>
                    
                </ItemTemplate>
            </asp:Repeater>

            <asp:ListView ID="lvItems" runat="server" onitemdatabound="lvItems_ItemDataBound">
                    
                <LayoutTemplate>      
                    
                    <thead>
                        <tr class="header">
                            <th style="text-align:left;">Venue</th>    
                            <th style="text-align:left;">Address</th>   
                            <th style="width: 200px;">Time</th>    
                            <th>&nbsp;</th>    
                        </tr>
                    </thead>
                        
                    <div id="itemPlaceholder" runat="server" />                        
                    
                </LayoutTemplate>
                
                <ItemTemplate>
                        
                    <tr>
                        <td>
                                  <%# Eval("CRM_Venue.Name") %>             
                        </td>  
                        <td>
                                  <%# Eval("CRM_Venue.CRM_Address.FormattedAddress")%>             
                             </td>  
                        <td style="text-align:center;">
                                  <%# Eval("DisplayTimeRange")%>             
                             </td>  
                             <td style="text-align:center;"><asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click" CausesValidation="false">
                                <img src="/_assets/images/admin/icons/remove.png" alt="Remove" title="remove" height="20" />
                             </asp:LinkButton></td>
                    </tr>
                        
                </ItemTemplate>
                    
                <EmptyDataTemplate>
                    
                    <tr><td>No history exists for this record</td></tr>
                    
                </EmptyDataTemplate>
                    
            </asp:ListView>

            </table>
  

            <div class="editor-nolabel">
                <label>Message to users:</label>
                <ucUtil:TextEditor ID="txtMessageToTags" runat="server" ToolbarSet="basic" Height="80" />
            </div>

                      <div class="buttons">
                    <label>
                <ucUtil:Button ID="btnSendChange" runat="server" ButtonText="Send Change to Users" ImagePath="email_go.png"
                    Class="positive" /></label>
                    </div>


            <div class="back">
                <a href='/admin/calendar'>
                    Back to Calendar</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
    
    <ucUtil:LogHistory ID="ucLogHistory" runat="server" />

</asp:Content>
