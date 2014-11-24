<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Forms.UserControlFileUpload" Codebehind="FileUpload.ascx.cs" %>

<div class="buttons">

    <asp:FileUpload ID="FileUpload1" runat="server" />

    <br style="clear:both;" />

    <div style="margin:10px 0px 0px 10px;" runat="server" visible="false" id="divView">

        <asp:HyperLink CssClass="neutral" ID="lnkView" runat="server" Target="_blank">
            <img src="/_assets/images/admin/icons/page_link.png" alt=""/>
            View File
        </asp:HyperLink>

    </div>

    <% if (IsImage) { %>

    <script type="text/javascript">
        $(document).ready(function () {

            $('#<%=lnkView.ClientID%>').fancybox({
                'autoScale': false,
                'autoDimensions': true
            });
        });
	</script>

    <% } %>

</div>

<asp:CustomValidator ID="cusSize" runat="server" Display="None" 
    EnableClientScript="false" onservervalidate="cusSize_ServerValidate" />
<asp:CustomValidator ID="cusType" runat="server" Display="None" 
    EnableClientScript="false" onservervalidate="cusType_ServerValidate" />
<asp:CustomValidator ID="cusFile" runat="server" Enabled="false" Display="None" EnableClientScript="false" onservervalidate="cusFile_ServerValidate" />