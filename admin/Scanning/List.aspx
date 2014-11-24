<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="CRM.admin.Scanning.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

    <style type="text/css">
    
    #IFrameDetails,
    #IFrame
    {
        border:0;
        width:950px;
        overflow-y:hidden;
    }
    
    </style>
        <script type="text/javascript">
            $(document).ready(function () {

                window.open('frame/ScanLog.aspx', 'Seven Stories Activity Log', 'width=800,height=800,scrollbars=no,top=80, left=20');
                
            });

    </script>
    <script type="text/javascript">


        $(document).ready(function () {
            $(".scanner").keypress(function (event) {

                if (event.keyCode == 13)
                    return false;

            });

            $(".scanner").focus();

            $(".scanner").on("keyup", function () {
                FetchResults();
            })

            $(".scanner").on("click", function () {
                ClearScanner();
            })
        });

        function ClearScanner() {
            $(".scanner").val("");
            $(".scanner").focus();
        }

        function FetchResults() {
            $("#IFrame").attr("src", "ScanResultFrame.aspx?id=" + $(".scanner").val());
        }

        function ShowBasicDetails(id) {
            $("#IFrameDetails").attr("src", "/admin/scanning/Details/Basic.aspx?id=" + id)
        }
    
    </script>

    <script type="text/javascript">

        $(document).ready(function () {
        var items = ['IFrame', 'IFrameDetails'];
            PollIFrame(items);
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Scan Card</h3>
        </div>
        <div class="innerContentForm">
            <table class="details">
                <tr>
                    <td>
                        <label>
                            Scan Card:</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtScan" runat="server" CssClass="scanner" Width="150" />
                        <a href="#" onclick="ClearScanner();return false;">
                            <img src="/_assets/images/admin/icons/remove.png" alt="clear" width="20" style="vertical-align: middle;" />
                        </a>
                    </td>
                </tr>
            </table>
            <br class="clearFix" />

            <iframe id="IFrame"  style="width: 100%" scrolling="no">
            
            </iframe>

            
            <iframe id="IFrameDetails"  style="width: 100%" scrolling="no">
            
            </iframe>
        </div>
    </div>


</asp:Content>
