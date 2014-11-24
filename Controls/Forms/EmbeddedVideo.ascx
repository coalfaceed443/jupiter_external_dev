<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Forms.UserControlEmbeddedVideo" Codebehind="EmbeddedVideo.ascx.cs" %>

<script type="text/javascript">
    $(document).ready(function () {
        $('.video-overlay').css('opacity', '0.3');
    });
    function checkVideo(el) {
        $('.video-overlay').show();
        if (el.value.length > 10) {
            // parse URL to see whether it is from youtube or vimeo
            if (el.value.indexOf('youtube') > -1) {
                // extract id
                var exp = /v=(.+?)(&|$)/;
                var values = exp.exec(el.value);
                if (values != null && values.length > 1) {
                    var id = values[1];
                    $("#<%= imgVideo.ClientID %>").attr('src', 'http://i4.ytimg.com/vi/' + id + '/1.jpg');
                    $('.video-overlay').hide();
                }
                else {
                    $('.video-overlay').hide();
                }
            }
            else if (el.value.indexOf('vimeo') > -1) {
                try {
                    var id = el.value.split('vimeo.com/')[1];
                    $.getJSON("http://vimeo.com/api/v2/video/" + id + ".json?callback=?", function (data) {
                        $("#<%= imgVideo.ClientID %>").attr('src', data[0]['thumbnail_small']);
                        $('.video-overlay').hide();
                    });
                }
                catch (e) { $('.video-overlay').hide(); }
            }
            else {
                $("#<%= imgVideo.ClientID %>").attr('src', '/_assets/images/admin/novideo.jpg');
                $('.video-overlay').hide();
            }
        }
        else {
            $("#<%= imgVideo.ClientID %>").attr('src', '/_assets/images/admin/novideo.jpg');
            $('.video-overlay').hide();
        }
    }

</script>
<div style="position:relative;">
<div class="video-overlay">&nbsp;</div>
<asp:TextBox id="txtVideoURL" runat="server" CssClass="textbox" onkeyup="checkVideo(this);"></asp:TextBox><br />
<asp:Image ID="imgVideo" runat="server" ImageUrl="~/_assets/images/admin/novideo.jpg" style="margin: 12px 0 0 12px;" />
<% if (!String.IsNullOrEmpty(this.Text))
   { %>
       <script type="text/javascript">
           checkVideo(document.getElementById('<%= txtVideoURL.ClientID %>'));
       </script>
<% } %>
</div>
