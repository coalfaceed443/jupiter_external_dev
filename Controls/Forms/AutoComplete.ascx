<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoComplete.ascx.cs" Inherits="CRM.Controls.Forms.AutoComplete" %>


<script type="text/javascript">

    function onOpened() {
        alert("beep");
    }

    $(document).ready(function () {

        var ctrlname = 'relatedcat-<%= DateTime.Now.Second %><%= this.ClientID %>';

        $('.auto-<%= this.ClientID %>').typeahead({
            name: 'relatedcat-<%= DateTime.Now.Second %><%= this.ClientID %>',
            remote: 
            { 
                url: "/Controls/Forms/Handlers/AutoCompleteJSON.ashx?<asp:Literal ID='litset' runat='server' />&query=%QUERY", 
                cache:  false
            },
            limit: 35,
            template: [
                    '<div style="height:40px;" title="{{title}}" class="tt-suggestion-<%= this.ClientID %>"><input type="hidden" value="{{objreference}}" /><img src="{{image}}" alt="" style="vertical-align:middle;float:left;padding-right:20px;width:40px;height:30px;">',
                    '<p class="repo-name"><span class="name">{{name}}</span> <span class="alt-reference">{{typeName}}</p></div>'
                ].join(''),
            engine: Hogan
        });

        $(".auto-<%= this.ClientID %>").on("keypress", function () {
            $("#typeahead-load<%= this.ClientID %>").fadeIn();

            setInterval(function () {
                if ($(".tt-dropdown-menu").is(":visible")) {
                    $("#typeahead-load<%= this.ClientID %>").fadeOut();
                }
            }, 500);

        });

        $(document).on("click", ".tt-suggestion-<%= this.ClientID %>", function () {

            var selectedID = $(this).find("input:hidden").val();
            $("#<%= hdnSelectedID.ClientID %>").val(selectedID);
            eval($("#<%= lnkSelect.ClientID %>").attr("href"));
            $(".auto-<%= this.ClientID%>").val("");
            $(".auto-<%= this.ClientID%>").focus();
        });


        $("#<%= hdnTextValue.ClientID %>").val("");

        $(".auto-<%= this.ClientID%>").on("keyup", function () {
            $("#<%= hdnTextValue.ClientID %>").val($(this).val());
        })


    });
  

</script>


<asp:MultiView ID="mvInput" runat="server" ActiveViewIndex="0">
    <asp:View ID="viewInput" runat="server">
    
                        <asp:HiddenField ID="hdnTextValue" runat="server" Value="" />
                        <asp:HiddenField ID="hdnSelectedID" runat="server" />
                        <asp:LinkButton ID="lnkSelect" runat="server" onclick="lnkSelect_Click" CausesValidation="false" />

                         <asp:CustomValidator ID="cusSelected" runat="server" 
                 onservervalidate="cusSelected_ServerValidate" Display="None" EnableClientScript="false" />
        
        <div style="position:relative;">
        <input type="text" class="typeahead auto-<%= this.ClientID %> tt-query textbox" style="width:230px;" />
        <img src="/_assets/images/admin/typeahead-load-3.gif" alt="loading" id="typeahead-load<%= this.ClientID %>" style="position:absolute;left:168px;bottom:2px;width:137px;display:none;" />
        </div>
    </asp:View>
    <asp:View ID="viewLabel" runat="server">
        <asp:LinkButton ID="lnkSwitch" 
            runat="server" onclick="lnkSwitch_Click" CausesValidation="false">
            <img src="/_assets/images/admin/icons/switch.png" alt="switch" title="switch" height="30px" style="float:left;margin-left:10px;vertical-align:middle;" />
        </asp:LinkButton>
        <label>
            <a href="#" target="_blank" ID="lnkRecord" runat="server"></a> 
        </label>
    </asp:View>
</asp:MultiView>
                
               
             