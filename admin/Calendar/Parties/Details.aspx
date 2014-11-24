<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Calendar.Parties.Details" %>
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
        
        <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navParties"  />

        <div class="innerContentForm">


            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                 runat="server" />

            <table class="details searchTableLeft">
                              
                <tr>
                    <td>
                        <label>
                            Theme: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtTheme" runat="server" Name="Theme" Required="true" />
                    </td>
                </tr>
                              
                <tr>
                    <td>
                        <label>
                            Age on Birthday: 
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAgeOnBirthday" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>                         
                <tr>
                    <td>
                        <label>
                            Number of Children: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtChildren" runat="server" Name="Number of Children" DataType="int" Required="true" />
                    </td>
                </tr>        
                <tr>
                    <td>
                        <label>
                            Additional Adults: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAdults" runat="server" Name="Additional Adults" Required="true" />
                    </td>
                </tr>            




            </table>
            <table class="details searchTableRight">

      
                <tr>
                    <td>
                        <label>
                            Agreed to Terms?: 
                        </label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkAgreedToTerms" runat="server" />
                    </td>
                </tr>                            
                <tr>
                    <td>
                        <label>
                            Additional Email address:
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAdditionalEmail" runat="server" Name="Additional Email" Required="false" IsEmail="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Catering Price: 
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtCateringPrice" runat="server" Name="Catering Price" Required="true" Text="0.00" />
                    </td>
                </tr>      

      
            </table>



            <div class="editor">
                <label>Catering Requirements</label>
                <ucUtil:TextEditor ID="txtCateringRequirements" runat="server" />
            </div>


            <br class="clearFix" />
            <br class="clearFix" />

            <div class="buttons">
                
                <% if (CRM_CalendarParty == null)
                { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save & Next" ImagePath="tick.png"
                    Class="positive" />
              <%} %>
              <% if (CRM_CalendarParty != null)
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
