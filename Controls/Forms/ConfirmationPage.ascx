<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Forms.UserControlConfirmationPage" CodeBehind="ConfirmationPage.ascx.cs" %>
<%if (ShowConfirmation)
  { %>
<a href="#popup" id="aPopupDriver" style="display: none;">&nbsp;</a>
<script type="text/javascript">
    $(document).ready(function () {

        $('#aPopupDriver').fancybox({
            'autoDimensions': true,
            'modal': true,
            'margin':0,
            'padding':0
        });

        $('#aPopupDriver').click();

    });
</script>

<div style="display: none">

    <div id="popup">
        <h2 style="font-size: 14pt; color: #b37431; letter-spacing: -1px;">
            <%= Header %></h2>
        <p>
            <%= Message %></p>

        <div class="buttons">
            <asp:PlaceHolder ID="plhButtons" runat="server" />
        </div>

    </div>

</div>

<% } %>