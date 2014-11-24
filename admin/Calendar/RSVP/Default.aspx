<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CRM.admin.Calendar.RSVP.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

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
                <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section=""  />

        <div class="innerContentForm">


            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />

                <p>RSVP with the status of <asp:DropDownList ID="ddlStatus" runat="server" DataTextField="Value" DataValueField="Key" /></p>


            <div class="editor-nolabel">
                <label>Please enter a message to Event owner; <%= Admin.DisplayName %>:</label>
                <ucUtil:TextEditor ID="txtMessageToTags" runat="server" ToolbarSet="basic" Height="80" />
            </div>

            <div class="buttons">
                <label>
                    <ucUtil:Button ID="btnSendRSVP" runat="server" ButtonText="Send RSVP" ImagePath="email_go.png"
                        Class="positive" />
                </label>
            </div>

            <div class="back">
                <a href='/admin/calendar'>
                    Back to Calendar</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
    

</asp:Content>
