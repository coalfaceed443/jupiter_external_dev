<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="CustomField.ascx.cs"
    Inherits="CRM.Controls.Admin.CustomFields.Form.CustomField" %>
    <asp:HiddenField ID="hdnID" runat="server" />
<tr>
    <td>
        <label>
            <asp:Literal ID="litName" runat="server" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
        </label>
    </td>
    <td class="custom-options">
        <asp:TextBox ID="txtSingleLineTextBox" runat="server" Visible="false" CssClass="textbox"></asp:TextBox>
        <asp:TextBox ID="txtMultiLineTextBox" runat="server" Height="120px" TextMode="MultiLine"
            CssClass="input text" Visible="false"></asp:TextBox>
        <asp:DropDownList ID="ddlDropDownList" runat="server" Visible="false" CssClass="select last" AppendDataBoundItems="false">
        </asp:DropDownList>
        <asp:CheckBox ID="chkSingleCheckBox" runat="server" Visible="false" />
        <asp:Panel ID="pnlMultipleCheckBox" runat="server" Visible="false">
            <div id="divMultipleCheckBoxSubText" runat="server" visible="false"   style="width: 263px;
                border-bottom: 2px solid #c5c5c6; padding: 0 5px 3px 5px;">
                Please select</div>
            <div class="chklist-container">
                <asp:CheckBoxList ID="chkBoxList" runat="server" RepeatLayout="Table" RepeatColumns="4" RepeatDirection="Horizontal" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMultipleRadioButton" runat="server" CssClass="" Visible="false">
            <div id="divMultipleRadioButtonSubText" runat="server" visible="false" style="padding: 0 5px 3px 5px;">
                Please select</div>
            <div class="radio-container">
                <asp:RadioButtonList ID="radBtnList" runat="server" RepeatLayout="Table" />
            </div>
        </asp:Panel>
    </td>
</tr>
