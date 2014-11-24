<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Address.ascx.cs" Inherits="CRM.Controls.Admin.SharedObjects.Address" %>

<tr>
    <td>
        <label>
            Address Line 1 : <%= Required(Address1Required) %></label>
    </td>
    <td>
        <ucUtil:TextBox ID="txtAddress1" runat="server" Required="false" Name="Address Line 1" />
    </td>
</tr>
<tr>
    <td>
        <label>
            Address Line 2 : <%= Required(Address2Required) %></label>
    </td>
    <td>
        <ucUtil:TextBox ID="txtAddress2" runat="server" Required="false" Name="Address Line 2" />
    </td>
</tr>
<tr>
    <td>
        <label>
            Address Line 3 : <%= Required(Address3Required) %></label>
    </td>
    <td>
        <ucUtil:TextBox ID="txtAddress3" runat="server" Required="false" Name="Address Line 3" />
    </td>
</tr>
<tr>
    <td>
        <label>
            Address Line 4 :</label>
    </td>
    <td>
        <ucUtil:TextBox ID="txtAddress4" runat="server" Required="false" Name="Address Line 4" />
    </td>
</tr>
<tr>
    <td>
        <label>
            Address Line 5 :</label>
    </td>
    <td>
        <ucUtil:TextBox ID="txtAddress5" runat="server" Required="false" Name="Address Line 5" />
    </td>
</tr>
<tr>
    <td>
        <label>
            Town : <%= Required(TownRequired) %></label>
    </td>
    <td>
        <ucUtil:TextBox ID="txtTown" runat="server" Required="false" Name="Town" />
    </td>
</tr>
<tr>
    <td>
        <label>
            County : <%= Required(CountyRequired) %></label>
    </td>
    <td>
        <ucUtil:TextBox ID="txtCounty" runat="server" Required="false" Name="County" />
    </td>
</tr>
<tr>
    <td>
        <label>
            Postcode : <%= Required(PostcodeRequired) %></label>
    </td>
    <td>
        <ucUtil:TextBox ID="txtPostcode" runat="server" Required="false" Name="Postcode" />
    </td>
</tr>
<tr>
    <td>
        <label>
            Country : *</label>
    </td>
    <td>
        <asp:DropDownList ID="ddlCounty" runat="server" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID">
        </asp:DropDownList>
    </td>
</tr>