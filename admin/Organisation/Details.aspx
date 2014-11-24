<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Organisation.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Organisation :
                <%if (Entity != null)
                  {%>
                Edit
                <%}
                  else
                  {%>
                Add
                <%} %></h3>
        </div>

        
        <ucUtil:NavigationOrganisation ID="ucNavOrganisation" runat="server" Section="navDetails"  />
        

        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"  runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Name :</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtOrganisationName" runat="server" Required="true" />
                    </td>
                </tr>            
                <tr>
                    <td>
                        <label>
                            Organisation Type :</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOrgType" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>                    
            </table>

            <table class="details searchTableRight">


                <ucUtil:Address ID="ucAddress" runat="server" />
            </table>


            <ucUtil:CustomFields ID="ucCustomFields" runat="server" />


            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (Entity == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Item" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else if (Entity.IsArchived)
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
                <a href='list.aspx'>
                    Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
    
    <ucUtil:LogHistory ID="ucLogHistory" runat="server" />  
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" /> 

</asp:Content>
