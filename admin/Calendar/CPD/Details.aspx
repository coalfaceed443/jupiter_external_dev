<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Calendar.CPD.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

<ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Calendar : <%= Entity.DisplayName %> : <%= Entity.CRM_CalendarType.Name %> :
                <%if (Entity != null)
                  {%>
                Edit Booking
                <%}
                  else
                  {%>
                Add new Booking details
                <%} %></h3>
        </div>
        
        <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navCDP"  />

        <div class="innerContentForm">


            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                 runat="server" />

            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            School or Organisation: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucACSchoolOrganisation" runat="server" Required="true" Name="School or Organisation" />
                    </td>
                </tr>                           
                <tr>
                    <td>
                        <label>
                            Package: 
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPackage" runat="server" DataTextField="Name" DataValueField="ID" />
                    </td>
                </tr>                          

                <tr>
                    <td>
                        <label>
                            Length: 
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLength" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>                          
            </table>
            <table class="details searchTableRight">                                             
                <tr>
                    <td>
                        <label>
                            Attendees: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAttendees" runat="server" DataType="int" Required="true" Name="Attendees" Width="100" />
                    </td>
                </tr>     
                <tr>
                    <td>
                        <label>
                            Confirmation Sent: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtConfirmationSent" runat="server" />
                    </td>
                </tr>    
                <tr>
                    <td>
                        <label>
                            Confirmation Initials: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtInitials" runat="server" Required="false" Name="Confirmation Initials" Width="100" />
                    </td>
                </tr>             
            </table>

            <br class="clearFix" />
            <br class="clearFix" />

            <div class="buttons">
                
                <% if (CRM_CalendarCPD == null)
                { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save & Next" ImagePath="tick.png"
                    Class="positive" />
              <%} %>
              <% if (CRM_CalendarCPD != null)
                 { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save & Next" ImagePath="tick.png"
                    Class="positive" />
            
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
