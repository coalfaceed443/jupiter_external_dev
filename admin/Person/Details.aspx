<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True"
    CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Person.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            if ($("#<%= hdnBaseImage.ClientID%>").val() != "") {
                $("#viewTempCapture").show();
                $("#imgTempPerson").attr("src", $("#<%= hdnBaseImage.ClientID%>").val());
                $("#imgTempPerson").show();
                ShowResetControl();

            } else {
                $("#viewCapture").show();
                ShowCaptureControl();
            }

            navigator.getUserMedia = (navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia);
            window.URL = (window.URL || window.mozURL || window.webkitURL);

            if (navigator.getUserMedia == undefined) {
                if (confirm("Please upgrade to a browser that supports use of webcams \r(e.g. Chrome). \r\rClick OK below to visit the Chrome website.\rClick Cancel to continue without webcam support.")) {
                    window.open("http://www.google.com/chrome");
                }
            }
            else {
                navigator.getUserMedia({ video: true }, function (stream) {

                    var video = document.getElementById("webcam");
                    video.src = window.URL.createObjectURL(stream);

                    var canvas = document.getElementById("canvas");

                    setInterval(function () {

                        canvas.width = video.videoWidth;
                        canvas.height = video.videoHeight;
                        canvas.getContext('2d').drawImage(video, 0, 0);
                    }, 10);
                }, function (err) {
                    console.log("The following error occured: " + err);
                });
            }
        });

        function ShowCaptureControl() {
            if ($("#viewCapture") != undefined) {
                $("#viewCapture").show();
                $("#viewTempCapture").hide();
                $("#captureControl").show();
                $("#resetControl").hide();
            } 
        }

        function ShowResetControl() {
            $("#captureControl").hide();
            $("#resetControl").show();
            $("#viewCapture").hide();
            $("#viewTempCapture").show();
        }

        function TakePhoto() {
            var canvas = document.getElementById("canvas");
            var imgData = canvas.toDataURL("img/png");
            imgData = imgData.replace('data:image/png;base64,', '');
            $.ajax({
                url: '/controls/admin/captureimagestream.ashx?admin=<%= AdminUser.ID %>',
                type: 'POST',
                data: imgData,
                contentType: "img/png",
                success: Processed
            });


        }

        function Processed() {
            $("#imgTempPerson").attr("src", "<%= AdminUser.TempPhotoFile %>");
            $("#imgTempPerson").fadeIn();
            ShowResetControl();

            $("#<%= hdnCaptured.ClientID %>").val("1");
        }

        function Reset() {
            ShowCaptureControl();          
            $("#<%= hdnCaptured.ClientID %>").val("0");
        }

    </script>


    <script type="text/javascript" src="/_assets/scripts/DYMO.Label.Framework.2.0.2.js"></script>
    <script type="text/javascript" src="/_assets/scripts/dymo.js"></script>

    <script type="text/javascript">

        dymo.label.framework.trace = 1; //true
        dymo.label.framework.init(startupCode);
        function startupCode() {
            console.log("starting...");
        }

        function checkDymo() {
            /* access DYMO Label Framework Library */

            var printers = dymo.label.framework.getPrinters();
            if (printers.length == 0) {                
                return false;
            }

        }

        //window.onload = startupCode;

        function printDymo() {

            if (checkDymo() == false)
            {
                alert("No printer found");
            }

            dymoExtension.print($("#<%= txtLabel.ClientID%>").val());
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Person :
                <%if (Entity != null)
                  {%>
                Edit <%= Entity.IsArchived ? "[ARCHIVED]" : "" %>
                <%}
                  else
                  {%>
                Add
                <%} %></h3>
        </div>
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navDetails" />
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />

                <!--
    <ucUtil:Duplicate ID="ucDuplicate" runat="server" />
            -->
            <table class="details searchTableLeft">
                <% if (Entity != null)
                    {%>

                
                <tr>
                    <td>
                        <label>
                            Addresses :</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLabelAddresses" runat="server" DataTextField="Text" DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddlLabelAddresses_SelectedIndexChanged" Width="290">
                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Label :</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLabel" runat="server" TextMode="MultiLine" Width="290" Height="100" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Print :</label>
                    </td>
                    <td><label>
                        <a href="#" onclick="printDymo();">Print</a></label>
                    </td>
                </tr>

                <%} %>
                <tr>
                    <td>
                        <label>
                            Title :</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTitles" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Firstname : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtFirstname" runat="server" Required="true" Name="Firstname" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Lastname : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtLastname" runat="server" Required="true" Name="Lastname" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Previous Names :</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPrevious" runat="server" Required="false" />
                    </td>
                </tr>
                <asp:Panel ID="pnlAdultLeft" runat="server">
                <tr>
                    <td>
                        <label>
                            Primary Telephone :</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtTelephone" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Secondary Telephone :</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtTelephone2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Primary Email :</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPrimaryEmail" runat="server" Required="false" IsEmail="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Do Not Mail:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDoNotMail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Do Not Email:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDoNotEmail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Prefer Email:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsContactEmail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Is Donor:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDonor" runat="server" Enabled="false" />
                    </td>
                </tr>
                </asp:Panel>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnCaptured" runat="server" />
                        <asp:HiddenField ID="hdnBaseImage" runat="server" />
                        <label>
                            <span class="help" title="Please click allow when prompted to use your webcam">Photo</span>
                        </label>
                          <label style="clear:left;margin-top:20px;">
                            <a href="#" onclick="TakePhoto();return false;" id="captureControl">
                                <img src="/_assets/images/admin/icons/capture.png" alt="capture" title="Take photo"
                                    width="50px" /></a>

                                    <a href="#" onclick="Reset();return false;" id="resetControl" style="display:none;">
                                     <img src="/_assets/images/admin/icons/switch.png" alt="Reset" title="Reset"
                                    width="50px" /
                                    </a>
                        </label>
                    </td>
                    <td>

                        <div id="viewCapture" style="display:none;">
                            
                            <canvas id="canvas">                                
                               
                            </canvas>
                            <video id="webcam" autoplay>                           
                            </video>
                        </div>

                        <div id="viewTempCapture" style="display:none;">
                              <img src="#" alt="" id="imgTempPerson" width="252" style="padding-left:20px;display: none;" />
                        </div>

         
                      
                    </td>
                </tr>
            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                            Date of Birth :</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtDateOfBirth" runat="server" />
                    </td>
                </tr>                
                <tr>
                    <td>
                        <label>
                            Is Child:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsChild" runat="server" AutoPostBack="true" />
                    </td>
                </tr>
                <asp:Panel ID="pnlAdultInfo" runat="server">
                <tr>
                    <td>
                        <label>
                            Is Concession:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsConcession" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Is Carer/Minder:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsCarerMinder" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Is Deceased:</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsDeceased" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <span title="Mark record as Automatic Gift Aid" class="help">Gift Aid :</span></label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsGiftAid" runat="server" />
                    </td>
                </tr>

                <ucUtil:Address ID="ucAddress" runat="server" />


                </asp:Panel>
            </table>

            <table class="details" style="width:960px;float:left;margin:20px 0;">

                <tr>
                    <td>                       
                        <label>
                            Primary Address
                        </label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlPrimaryAddress" runat="server" DataTextField="InternalName" DataValueField="ID" />
                    </td>

                </tr>

            </table>

            <ucUtil:CustomFields ID="ucCustomFields" runat="server" />

            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (Entity == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Person" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else if (Entity.IsArchived)
                  { %>

                <ucUtil:Button ID="btnReinstate" runat="server" ButtonText="Reinstate Archived Item" ImagePath="refresh_small.png"
                    Class="negative" />
                  <%}
                    else
                    { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Person" ImagePath="tick.png"
                    Class="positive" />
              
                <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Archive Person" ImagePath="cross.png"
                    Class="negative" />
                <%} %>
            </div>
            <div class="back">
                <a href='list.aspx'>Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
    <ucUtil:LogComms ID="ucLogComms" runat="server" Visible="false" />
    <ucUtil:LogHistory ID="ucLogHistory" runat="server" />
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" />
</asp:Content>
