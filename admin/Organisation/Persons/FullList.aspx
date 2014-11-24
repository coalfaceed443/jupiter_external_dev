<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="FullList.aspx.cs" Inherits="CRM.admin.Organisation.Persons.FullList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Employee Records</h3></div>
        
        <div class="innerContentForm">

        <p class="top"><a href="/admin/person/list.aspx">Search for or Add a person to create a new employee</a></p>       
                           


        <table class="details">
            <tr>
                <td>
                    <label>Find Record:</label>
                </td>
                <td>
                    <ucUtil:AutoComplete ID="acOrgPerson" runat="server" />    
                </td>
            </tr>
        </table>

        <br class="clearFix" />
        
        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>

</asp:Content>
