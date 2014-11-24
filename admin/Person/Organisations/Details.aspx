<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Person.Organisation.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Personal Organisation Record :
                <%if (PersonOrganisation != null)
                  {%>
                Edit this organisational record for <%= Entity.Fullname %>
                <%}
                  else
                  {%>
                Add <%= Entity.Fullname %> to a new organisation
                <%} %></h3>
        </div>        
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navOrganisations"  />
        
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                 runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                           Organisation : *</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucACOrganisation" runat="server" Required="true" Name="Organisation" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Role : *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRole" runat="server" DataTextField="Name" DataValueField="ID" />
                    </td>
                </tr>
            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                           Email :</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtEmail" runat="server" IsEmail="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Telephone :</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtTelephone" runat="server" />
                    </td>
                </tr>

                        
            </table>
            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (PersonOrganisation == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Item" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else if (PersonOrganisation.IsArchived)
                  { %>

                <ucUtil:Button ID="btnReinstate" runat="server" ButtonText="Reinstate Archived Item" ImagePath="refresh_small.png"
                    Class="negative" />
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
                <a href='<%= Entity.OrganisationListURL %>'>
                    Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
        <ucUtil:LogHistory ID="ucLogHistory" runat="server" />   
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" />   
</asp:Content>
