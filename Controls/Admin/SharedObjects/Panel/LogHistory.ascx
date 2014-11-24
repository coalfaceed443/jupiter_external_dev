<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="LogHistory.ascx.cs" Inherits="CRM.Controls.Admin.SharedObjects.Panel.LogHistory" %>

  
<a id="simple-menu<%= this.ClientID %>" class="simple-menu" href="#sidr" title="Logs">
    <img src="/_assets/images/admin/icons/record.png" />
</a>

<div id="sidr" class="sidr-large">

  <h3>Logs</h3>

  <div id="log">
  
  </div>

  <a href="#" onclick="closeSidr('sidr');">
    <img src="/_assets/images/admin/icons/tick_b.png" class="ok-icon" />
  </a>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#simple-menu<%= this.ClientID %>').sidr({
            source: populate,
            name: "sidr",
            body: ""
            
        });
    });


    function populate() {
        $("#log").load("/admin/history/frames/history.aspx?name=<%= TableName %>&id=<%= OverrideID %>&parentID=<%= ParentID %>");
    }
    
</script>
