<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Organisation.Schools.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Organisation Schools</h3></div>
        

        <ucUtil:NavigationOrganisation ID="ucNavOrganisation" runat="server" Section="navSchools"  />

        <div class="innerContentForm">

        <p class="top">Link a school to this organisation</p>       
        
                    
        <table class="details searchTableLeft">
            <tr>
                <td>
                    <label>School to link to this organisation:</label>
                </td>
                <td>
                    <ucUtil:AutoComplete ID="acSchool" runat="server" />  
                </td>
            </tr>
          
        </table>

        <div class="editor-nolabel">
        <ucUtil:ListView ID="ucList" runat="server" />
        </div>
        </div>

    </div>

</asp:Content>
