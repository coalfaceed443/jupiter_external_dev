<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Forms.UserControlTextBox" Codebehind="TextBox.ascx.cs" %>

<asp:TextBox ID="txtTextBox" runat="server" CssClass="textbox"></asp:TextBox>
<asp:Literal ID="litText" runat="server"></asp:Literal>
<%if(!String.IsNullOrEmpty(DefaultValue)){ %>
<script type="text/javascript">

    $(function () {
        var defaultValue = '<%= DefaultValue %>';
        if (defaultValue != null) {
            $('#<%=txtTextBox.ClientID %>').focus(function () {
                if ($(this).val().toLowerCase() == defaultValue.toLowerCase())
                    $(this).val("");
            });
            $('#<%=txtTextBox.ClientID %>').blur(function () {
                if ($(this).val() == "")
                    $(this).val(defaultValue);
            });
        }
    });

</script>
<%} %>
<asp:RequiredFieldValidator ID="reqTextBox" ControlToValidate="txtTextBox" ErrorMessage="" runat="server" Display="None" EnableClientScript="false"></asp:RequiredFieldValidator>

<asp:CustomValidator ID="cusEmail" runat="server" Enabled="false" ErrorMessage="Your email address is invalid" Display="None" EnableClientScript="false" onservervalidate="cusEmail_ServerValidate" />

<asp:CustomValidator ID="cusDefault" runat="server" Enabled="false" ErrorMessage="" Display="None" EnableClientScript="false" onservervalidate="cusDefault_ServerValidate" />
    
<asp:CompareValidator runat="server" ID="cmpDataType" ErrorMessage="" Type="Double" ControlToValidate="txtTextBox" Operator="DataTypeCheck" EnableClientScript="false" Display="None"></asp:CompareValidator>
<asp:CustomValidator ID="cusMaxLength" runat="server" Display="None" OnServerValidate="cusMaxLength_Validate"></asp:CustomValidator>