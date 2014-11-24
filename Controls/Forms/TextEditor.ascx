<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Forms.UserControlTextEditor" Codebehind="TextEditor.ascx.cs" %>

<CKEditor:CKEditorControl ID="fckBody" runat="server" BasePath="~/_assets/ckeditor"></CKEditor:CKEditorControl>

<asp:RequiredFieldValidator ID="reqTextBox" ControlToValidate="fckBody" ErrorMessage="" runat="server" Display="None" EnableClientScript="false"></asp:RequiredFieldValidator>