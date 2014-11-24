<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataQuery.ascx.cs" Inherits="CRM.Controls.Admin.SharedObjects.List.Query.DataQuery" EnableViewState="true" %>

<div class="data-query">
<asp:DropDownList ID="ddlFilterColumn" runat="server" DataTextField="Text" DataValueField="Value" />

<asp:DropDownList ID="ddlFilter" runat="server">
    <asp:ListItem Text="Contains"></asp:ListItem>
    <asp:ListItem Text="Does not Contain" Value="!Contains"></asp:ListItem>
    <asp:ListItem Text="Is less than" Value="<"></asp:ListItem>
    <asp:ListItem Text="Is less than or equal to" Value="<="></asp:ListItem>
    <asp:ListItem Text="Is the same as" Value="=="></asp:ListItem>
    <asp:ListItem Text="Is greater than" Value=">"></asp:ListItem>
    <asp:ListItem Text="Is greater than or equal to" Value=">="></asp:ListItem>
    <asp:ListItem Text="Is not" Value="!="></asp:ListItem>
</asp:DropDownList>

<asp:TextBox ID="txtFilter" runat="server" CssClass="textbox" Width="200"  />

<asp:DropDownList ID="ddlConcat" runat="server" style="min-width:0;">
    <asp:ListItem Text="OR"></asp:ListItem>
    <asp:ListItem Value="AND"></asp:ListItem>
</asp:DropDownList>

<asp:HiddenField ID="hdnID" runat="server" />

<asp:MultiView ID="mvOptions" runat="server" ActiveViewIndex="0">
    <asp:View ID="viewAdd" runat="server">
        <asp:LinkButton id="lnkAdd" runat="server" onclick="lnkAdd_Click"><img src="/_assets/images/admin/icons/new.png" alt="Add" /></asp:LinkButton>    
    </asp:View>
    <asp:View ID="viewEdit" runat="server">
        <asp:LinkButton id="lnkSave" runat="server" onclick="lnkSave_Click"><img src="/_assets/images/admin/icons/tick_b.png" alt="Save"  /></asp:LinkButton>    
        <asp:LinkButton id="lnkDelete" runat="server" onclick="lnkDelete_Click"><img src="/_assets/images/admin/icons/remove.png" alt="Delete" style="margin-right:5px;" /></asp:LinkButton>    
    </asp:View>
</asp:MultiView>
</div>