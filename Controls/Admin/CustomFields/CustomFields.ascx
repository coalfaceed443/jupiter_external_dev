<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomFields.ascx.cs" Inherits="CRM.Controls.Admin.CustomFields.CustomFields" %>

<table class="details" style="float:left;margin-top:20px;width:960px;">
    <tr>
        <td colspan="2"><label><strong>Custom Fields</strong></label></td>
    </tr>
    <asp:Repeater ID="rptQuestions" runat="server">
        <ItemTemplate>
                    
                <ucUtil:CustomField ID="ucFormQuestion" runat="server" CRM_FormField='<%# (CRM.Code.Models.CRM_FormField)Container.DataItem %>' />
                            <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("ID") %>'/>
                               
                      
        </ItemTemplate>
    </asp:Repeater>
</table>