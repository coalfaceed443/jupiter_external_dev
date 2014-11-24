<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Families.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Family :
                <%if (Entity != null)
                  {%>
                Edit
                <%}
                  else
                  {%>
                Add
                <%} %></h3>
        </div>

        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"  runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Name : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtName" runat="server" Required="true" Name="Name" />
                    </td>
                </tr>     
                <% if (Entity != null)
                { %>       
                <tr>
                    <td>
                        <label>
                           Add member : *</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucACFamily" runat="server" Required="false" Name="Family" />
                    </td>
                </tr>             
                <%} %>
            </table>

            <table class="details searchTableRight">
            
            </table>

            <% if (Entity != null)
               { %>
            <br class="clearFix" />
            <br class="clearFix" />

            <ucUtil:ListView ID="ucFamilyPersons" runat="server" />
            <%} %>

            <div class="buttons">
                <%if (Entity == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Item" ImagePath="tick.png"
                    Class="positive" />
                <%}
                    else
                    { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Item" ImagePath="tick.png"
                    Class="positive" />

   
                <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Item" ImagePath="cross.png"
                    Class="negative" />
     
                <%} %>
            </div>
            <div class="back">
                <a href='list.aspx'>
                    Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
    
        <ucUtil:LogHistory ID="ucLogHistory" runat="server" />  
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" /> 


</asp:Content>
