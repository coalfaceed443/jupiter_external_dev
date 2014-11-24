<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlButton.ascx.cs" Inherits="CRM.Controls.Forms.UserControlButton" %>

<span ID="pnlButton" runat="server">
    <asp:LinkButton ID="btnButton" runat="server">
    <%=!String.IsNullOrEmpty(this.ImagePath) ? "<img src=\"/_assets/images/admin/icons/" + ImagePath + "\" alt=\"\" />" : "" %>
    <%=ButtonText %>
    </asp:LinkButton>
</span>

<span ID="pnlLink" runat="server" Visible="false">
    <a href="<%=NavigateUrl %>"<%=!String.IsNullOrEmpty(Target) ? " target=\"" + Target + "\"" : " onclick=\"parent.$.fancybox.close();parent.window.location='" + NavigateUrl + "'\""%> class="<%=Class %>" <%=!String.IsNullOrEmpty(Style) ? "style=\"" + Style + "\"" : ""%>>
    <%=!String.IsNullOrEmpty(ImagePath) ? "<img src=\"/_assets/images/admin/icons/" + ImagePath + "\" alt=\"\" />" : "" %>
    <%=ButtonText %>
    </a>
</span>