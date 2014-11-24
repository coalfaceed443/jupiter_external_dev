<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="List.ascx.cs" Inherits="CRM.Controls.Admin.SharedObjects.Notes.List" %>

  
<a id="simple-menu<%= this.ClientID %>" class="simple-menu-notes" href="#sidr2" title="Notes">
    <img src="/_assets/images/admin/icons/notes.png" />    
    <span class="unread">
        <span class="unread-count"><asp:Literal ID="litUnreadCount" runat="server" /></span>
        <img src="/_assets/images/admin/icons/unread.png" alt="unread icon" />
    </span>
</a>


<div id="sidr2" class="sidr-large">

  <h3>Notes : <asp:Literal ID="litNote" runat="server" /></h3>

  <div id="notes">
    <iframe id="iNotes" width="100%" height="100%">
        
    </iframe>
  </div>

  <a href="#" onclick="closeSidr('sidr2');">
    <img src="/_assets/images/admin/icons/tick_b.png" class="ok-icon" />
  </a>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('#simple-menu<%= this.ClientID %>').sidr({
            source: populateNotes,
            name: "sidr2",
            body: ""
        });
    });

    function updateCounter(count) {
        $(".unread-count").html(count);
    }

    function populateNotes() {
        $("#iNotes").attr("src", "/admin/notes/frames/list.aspx?references=<%= Reference %>");
        $("#iNotes").css("height", $("#sidr2").height() + "px");
    }
    
</script>
