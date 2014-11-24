<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True"
    CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Calendar.Duplicate.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

<style type="text/css">

    .innerContentForm .details td:first-child
    {
        width:auto !important;
    }

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Calendar : Duplicate</h3>
        </div>
        <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navDuplicate" />
        <div class="innerContentForm">
            <ul style="padding-bottom:10px;line-height:20px;">
                <li>Enter a start time and end time for your calendar item.</li>
                <li>For calendar items that run continually please use the 'Runs Until' textbox. The CMS will
                    then create your calendar entry every day between your start and runs until date.</li>
                <li>Once created there will be no link between these calendar events. Editing one will
                    not affect another.</li>
            </ul>
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                HeaderText="The form could not be submitted for the following reasons:" runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Start Date & Time:</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtDate" runat="server" Width="250" ShowTime="true" Required="true"
                            Name="Start Date" />
                        <br class="clearFix" />
                        </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            End Time</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtEndDate" runat="server" Width="250" ShowDate="false"
                            ShowTime="true" />
                    </td>
                </tr>
            </table>
            <table class="details searchTableRight">
            <asp:Panel ID="pnlEnd" runat="server">
                <tr>
                    <td>
                        <label>
                            Runs Until:</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtEndRun" runat="server" Width="250" ShowTime="false" ShowDate="true"
                            Name="Runs Until" />
                </tr>
                </asp:Panel>
                <tr>
                    <td>
                        <label>
                            Apply</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="Single Date" Value="" />
                            <asp:ListItem Text="Every Day" Value="day" />
                            <asp:ListItem Text="Every Week" Value="week" />
                            <asp:ListItem Text="Every Month" Value="month" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div class="chklist-container custom-options">
                <table class="details" style="float:left;width:950px;">
                    <tr>
                        <td>
                            <label>
                                Days to include
                            </label>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="chkDayList" runat="server" DataTextField="Value" DataValueField="Key" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                </table>
            </div>
            <br style="clear: both;" />
            <div class="buttons">
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Duplicate" ImagePath="add.png"
                    Class="neutral" />
            </div>
            <br style="clear: both;" />
            <br style="clear: both;" />
           
            <ucUtil:ListView ID="lvDuplicated" runat="server" />
        </div>
    </div>
</asp:Content>
