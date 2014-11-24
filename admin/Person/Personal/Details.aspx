<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Person.Personal.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Personal Record :
                <%if (PersonPersonal != null)
                  {%>
                Edit this personal record for <%= Entity.Fullname %>
                <%}
                  else
                  {%>
                Add new personal record for <%= Entity.Fullname %>
                <%} %></h3>
        </div>        
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navPersonal"  />
        
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false" runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                           Description : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtDescription" runat="server" Name="Description" Required="true" />
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
                           Make Primary :</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkMakePrimary" runat="server" />
                    </td>
                </tr>
                        
            </table>
            <table class="details searchTableRight">
                <ucUtil:Address ID="ucAddress" runat="server" />
            </table>
           
            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (PersonPersonal == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Item" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else if (PersonPersonal.IsArchived)
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
                <a href='<%= Entity.PersonalListURL %>'>
                    Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
        <ucUtil:LogHistory ID="ucLogHistory" runat="server" />   
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" />   
</asp:Content>
