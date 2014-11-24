<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Forms.DropDownNotice" EnableViewState="false" Codebehind="DropDownNotice.ascx.cs" %>

<% if (!String.IsNullOrEmpty(this.Message))
   { %>
<script type="text/javascript">
    
    $(document).ready(function() { 

        $("#putMessageHere").html('<%= this.Message.Replace("'", "\'")%>');
        
        setTimeout(function() {
            $("#alert-message").animate({top: '+=57'}, 200);
        }, 500);
                    
        setTimeout(function() {
            $("#alert-message").animate({top: '-=57'}, 200);
        }, 4000);
    });

</script>

<div id="alert-message">

    <span style="position: relative; top: 5px;" id="putMessageHere"></span>

</div>

<% } %>