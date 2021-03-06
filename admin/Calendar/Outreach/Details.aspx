﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Calendar.Outreach.Details" %>
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
        
        <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navOutreach"  />

        <div class="innerContentForm">


            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                 runat="server" />

            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                           School or Organisation: *</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="acSchoolOrg" runat="server" Required="true" Name="School/Organisation" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Facilitator:
                        </label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="acFacilitator" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Offer: 
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOffer" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true">
                            <asp:ListItem Text="None" Value="" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Price: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPrice" runat="server" Name="Price" Required="true" DataType="decimal" Width="100" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Pupils: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPupils" runat="server" Name="Pupils" Required="true" DataType="int" Width="100" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Adults: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAdults" runat="server" Name="Adults" DataType="int" Required="true" Text="0" Width="100"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Year Group: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtYearGroup" runat="server" Name="Year Group" Required="false" />
                    </td>
                </tr>



            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                           Special needs children: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtSpecialNeeds" Required="true" runat="server" Name="Number of special needs children (0 for none)" Text="0" Width="100" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Special needs details: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtSpecialNeedsDetails" Required="false" runat="server" Name="Additional details regarding special needs" TextMode="MultiLine" Width="280" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Attended Children: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAttendedChildren" runat="server" Name="Attended Children" DataType="int" Required="true" Width="100" Text="0"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Attended Adults: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAttendedAdults" runat="server" Name="Attended Adults" DataType="int" Required="true" Width="100" Text="0"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Attended Teachers: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAttendedTeachers" runat="server" Name="Attended Teachers" DataType="int" Required="true" Width="100" Text="0"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Confirmation Sent: 
                        </label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkConfirmationSent" runat="server" />
                    </td>
                </tr>   
                               <tr>
                    <td>
                        <label>
                            Date Confirmation Sent: 
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

            <div class="editor">
                <label>Workshop Times</label>
                <ucUtil:TextEditor ID="txtWorkshopTimes" runat="server" Name="Workshop Details" />
            </div>

           <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                
                <% if (CRM_CalendarOutreach == null)
                { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save & Next" ImagePath="tick.png"
                    Class="positive" />
              <%} %>
              <% if (CRM_CalendarOutreach != null)
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
