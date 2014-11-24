<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.School.Organisations.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Organisation Schools</h3></div>
        
        
        <ucUtil:NavigationSchool ID="ucNavSchool" runat="server" Section="navOrgs"  />

        <div class="innerContentForm">

        <p class="top">Link an organisation to this school</p>       
        
                    
        <table class="details searchTableLeft">
            <tr>
                <td>
                    <label>Organisation to link to this school:</label>
                </td>
                <td>
                    <ucUtil:AutoComplete ID="acOrganisation" runat="server" />  
                </td>
            </tr>
          
        </table>

        <div class="editor-nolabel">
        <ucUtil:ListView ID="ucList" runat="server" />
        </div>
        </div>

    </div>

</asp:Content>
