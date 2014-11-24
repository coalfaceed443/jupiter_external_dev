﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Fundraising.Funds.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Fund :
                <%if (Entity != null)
                  {%>
                Edit Fund
                <%}
                  else
                  {%>
                Add a new Fund
                <%} %></h3>
        </div>
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false" runat="server" />
            <table class="details">
                <tr>
                    <td>
                        <label>
                           Name : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtName" runat="server" Name="Name" Required="true"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Is Active : *</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" />
                    </td>
                </tr>
            
            </table>
            
            <br class="clearFix" />
            <label>Thank you Template</label>
                    <ucUtil:TextEditor ID="txtTemplate" runat="server" Required="true" />
           
            <br class="clearFix" />
            <br class="clearFix" />
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
