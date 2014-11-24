<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" Inherits="CRM.Admin.Calendar.Details" Codebehind="Details.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
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
                <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navDetails"  />

        <div class="innerContentForm">


            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                 runat="server" />

            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Calendar Type: *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCalendarType" runat="server" DataTextField="Name" DataValueField="ID"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Display Name: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtDisplayName" runat="server" Required="true" Name="Display Name" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Primary Contact :</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="acPersons" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Status: *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" DataTextField="Value" DataValueField="Key" />
                    </td>
                </tr>         
                <tr>
                    <td>
                        <label>
                            Is Cancelled: *</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsCancelled" runat="server" />
                    </td>
                </tr>                       
            </table>
            
            <table class="details searchTableRight">

                <tr>
                    <td>
                        <label>
                            Created By:</label>
                    </td>
                    <td>
                        <asp:Label ID="lblCreatedBy" runat="server" />
                    </td>
                </tr>  
                <tr>
                    <td>
                        <label>
                           Privacy Status:</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPrivacy" runat="server" DataTextField="Value" DataValueField="Key" />
                    </td>
                </tr>  
                <tr>
                    <td>
                        <label>
                            Start Date/Time:</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtStartDate" runat="server" Required="true" ShowTime="true" />
                    </td>
                </tr>  
                <tr>
                    <td>
                        <label>
                            End Time:</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtEndDate" runat="server" Required="true" ShowTime="true" ShowDate="false" />
                    </td>
                </tr>  


            </table>
            
            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                
                <% if (Entity == null)
                { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save & Next" ImagePath="tick.png"
                    Class="positive" />
              <%} %>
              <% if (Entity != null)
                 { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save & Next" ImagePath="tick.png"
                    Class="positive" />
                    
                   
                <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Calendar Item" ImagePath="cross.png"
                    Class="negative" />
                    <%} %> 
           
            </div>
            <div class="back">
                <a href='/admin/calendar'>
                    Back to Calendar</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>


    <ucUtil:LogHistory ID="ucLogHistory" runat="server" />
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" />
</asp:Content>
