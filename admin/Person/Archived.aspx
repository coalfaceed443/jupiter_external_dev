<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Archived.aspx.cs" Inherits="CRM.admin.Person.Archived" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Archived Person Records</h3></div>

        <div class="innerContentForm">

                    
        <table class="details">
            <tr>
                <td>
                    <label>Find Record:</label>
                </td>
                <td>
                    <ucUtil:AutoComplete ID="acPerson" runat="server" />    
                </td>
            </tr>
        </table>


        <br class="clearFix" />
        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
