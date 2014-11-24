<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="CommsLog.ascx.cs" Inherits="CRM.Controls.Admin.SharedObjects.Panel.CommsLog" %>

  
<a id="simple-menu<%= this.ClientID %>" class="simple-menu-comms simple-menu" href="#sidr" title="Logs">
    <img src="/_assets/images/admin/icons/mail.png" />
</a>

<div id="sidrcomms" class="sidr-large">

  <h3>Communications</h3>

  <div id="commslog">
  
  </div>

  <a href="#" onclick="closeSidr('sidrcomms');">
    <img src="/_assets/images/admin/icons/tick_b.png" class="ok-icon" />
  </a>
</div>

<script type="text/javascript">
    $(document).ready(function () {

        $('#simple-menu<%= this.ClientID %>').sidr({
            source: populatecomms,
            name: "sidrcomms",
            body: ""

        });

    });


    function populatecomms() {
        $("#commslog").load("/admin/communications/frames/comms.aspx?reference=<%= IContact.Reference %>", function () {
        
        });
    }
    
</script>
