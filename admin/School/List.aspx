<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.School.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">

        <div class="contentBoxTop"><h3>School Records</h3></div>

        <div class="innerContentForm">

        <p class="top"><a href="details.aspx">Add new School</a></p>       
        
                    
        <table class="details">
            <tr>
                <td>
                    <label>Find Record:</label>
                </td>
                <td>
                    <ucUtil:AutoComplete ID="acSchool" runat="server" />    
                </td>
            </tr>
        </table>
        <br class="clearFix" />
        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>

</asp:Content>
