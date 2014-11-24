<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Calendar.Invite.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">

        <div class="contentBoxTop"><h3><%= Entity.DisplayName %> Invites & Attendance</h3></div>
        
        
        <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navInvites"  />

        <div class="innerContentForm">
                    
        <table class="details searchTableLeft">
            <tr>
                <td>
                    <label>Contact to link to this calendar Item:</label>
                </td>
                <td>
                    <ucUtil:AutoComplete ID="acContact" runat="server" />  
                </td>
            </tr>
          
        </table>

        <div class="editor-nolabel">
        <ucUtil:ListView ID="ucList" runat="server" />
        </div>
        </div>

    </div>

</asp:Content>
