<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Person.Relation.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

    <style type="text/css">

        .rblist input {
            float:left;
        }
        .rblist label{
            float:right !important;
            width:255px;
        }

        .rblist td {
        width:295px !important;
        }

    </style>

      <script type="text/javascript" src="/_assets/scripts/DYMO.Label.Framework.2.0.2.js"></script>
    <script type="text/javascript" src="/_assets/scripts/dymo.js"></script>

    <script type="text/javascript">

        dymo.label.framework.trace = 1; //true
        dymo.label.framework.init(startupCode);
        function startupCode() {
            console.log("starting...");
        }

        function checkDymo() {
            /* access DYMO Label Framework Library */

            var printers = dymo.label.framework.getPrinters();
            if (printers.length == 0) {                
                return false;
            }

        }

        //window.onload = startupCode;

        function printDymo() {

            if (checkDymo() == false)
            {
                alert("No printer found");
            }

            dymoExtension.print($("#<%= txtLabel.ClientID%>").val());
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    
 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Relationship Record :
                <%if (PersonRelationship != null)
                  {%>
                Edit this relationship for <%= Entity.Fullname %>
                <%}
                  else
                  {%>
                Add <%= Entity.Fullname %> to a new relationship
                <%} %></h3>
        </div>        
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navRelations"  />
        
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false" runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                           Person A : *</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucAcPerson" runat="server" Name="Person A" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Relation to B : *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRelationCodetoB" runat="server" DataTextField="Name" DataValueField="ID" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Person B :</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucAcPersonB" runat="server" Name="Person B" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Relation to A : *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRelationCodeToA" runat="server" DataTextField="Name" DataValueField="ID" />
                    </td>
                </tr>

                        
            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                           Salutation : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtSalutation" runat="server" Required="true" Name="Salutation" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Address to use for this relationship
                        </label>
                    </td>
                    <td class="rblist">
                        <asp:RadioButtonList ID="rbPersonAddress" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label><a href="#" onclick="printDymo();return false;">Print Label</a></label>
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtLabel" runat="server" TextMode="MultiLine" Height="80" style="width:280px;" />
                    </td>
                </tr>
            </table>

            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (PersonRelationship == null)
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
                <a href='<%= Entity.RelationListURL %>'>
                    Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
        <ucUtil:LogHistory ID="ucLogHistory" runat="server" />   
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" />   

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
