<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Calendar.Task.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Tasks</h3></div>

        <div class="innerContentForm">

        <p class="top">Use the <a href="/admin/calendar">calendar</a> to add a new task</p>       
        
                    
        <table class="details searchTableLeft">
            <tr>
                <td>
                    <label>Name:</label>
                </td>
                <td>
                    <ucUtil:AutoComplete ID="acTaskName" runat="server" />  
                </td>
            </tr>
            <tr>
                <td>
                    <label>Hide Completed Tasks:</label>
                </td>
                <td>
                    <asp:CheckBox ID="chkHideCompleted" runat="server" />
                </td>
            </tr>
        </table>

        <table class="details searchTableRight">
            <tr>
                <td>
                    <label>Due Date:</label>
                </td>
                <td>
                    <ucUtil:DateCalendar ID="txtDueDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <label>Only show tasks I'm involved in:</label>
                </td>
                <td>
                    <asp:CheckBox ID="chkOwnTasks" runat="server" />
                </td>
            </tr>
        </table>

        <div class="buttons">
                <ucUtil:Button ID="btnSubmitChanges" OnClick="btnSubmitChanges_Click" runat="server" ButtonText="Search" ImagePath="search.png"
                Class="neutral" />
        </div>

        <br class="clearFix" />
        
        <div class="editor-nolabel">
        <ucUtil:ListView ID="ucList" runat="server" />
        </div>
        </div>

    </div>

</asp:Content>
