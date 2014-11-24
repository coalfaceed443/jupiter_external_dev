<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Duplicate.ascx.cs" Inherits="CRM.Controls.Admin.SharedObjects.Duplicate" %>
<%@ Import Namespace="System.Linq" %>
<script type="text/javascript">
    var timer = <%= _Countdown %>;

    function LoadDuplicates() {
        var items = $.find("[data-duplicate]");

        var item = [];

        $(items).each(function()
        {
            item.push([$(this).data("duplicate"), $(this).val()]);
        });

        console.log(item);
        
        var strung = JSON.stringify(item); 

        $("#duplicates").attr("src", "/admin/Duplicate/List.aspx?namespace=<%= Namespace %>&compare=" + strung + "&id=<%= OriginalID %>");
    }




    $(document).ready(function () {
        $("[data-duplicate]").on("blur", LoadDuplicates);
    });


</script>

    <div id="check-duplicates">
    <iframe id="duplicates" style="border:0;width:960px;">
    
    </iframe>
    </div>
