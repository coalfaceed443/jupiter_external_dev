<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Person.Families.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Personal Family Record :
                <%if (PersonFamily != null)
                  {%>
                Edit this family record for <%= Entity.Fullname %>
                <%}
                  else
                  {%>
                Add <%= Entity.Fullname %> to a new family
                <%} %></h3>
        </div>        
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navFamilies"  />
        
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                 runat="server" />
            <table class="details">
                <tr>
                    <td>
                        <label>
                           Family : *</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucACFamily" runat="server" Required="true" AtLeastTextRequired="true" Name="Family" />
                    </td>
                </tr>
               </table>
           
            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (PersonFamily == null)
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

            <br class="clearFix" />
            <br class="clearFix" />
            <ucUtil:ListView ID="ucFamilyList" runat="server" />

            <div class="back">
                <a href='<%= Entity.FamilyListURL %>'>
                    Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
        <ucUtil:LogHistory ID="ucLogHistory" runat="server" />   
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" />   
</asp:Content>
