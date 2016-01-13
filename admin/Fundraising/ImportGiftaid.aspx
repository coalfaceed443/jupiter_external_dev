<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportGiftaid.aspx.cs" Inherits="CRM.admin.Fundraising.ImportGiftaid" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ucUtil:Scripts ID="ucScripts" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
 <div class="topContentBox" style="width: 500px;">
        <h2>
            Log Gift Aid Claim</h2>
        <asp:MultiView ID="mvLog" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewEnter" runat="server">
                <p class="lead">
                    Use this form to upload any fundraising exports you have made in CSV form. If the original
                    'Donation ID' field from the CRM export is still available as a column, it
                    will be logged against that record as having gift aid claimed aganist it.</p>

                    <asp:ValidationSummary ID="val" runat="server" />
                <div class="innerContentForm" style="width: 400px;">
                    <table class="details">
                        <tr>
                            <td>
                                <label>
                                    Claim name
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Upload CSV
                                </label>
                            </td>
                            <td>
                                <ucUtil:FileUpload ID="fuExcel" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div class="buttons">
                        <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Upload .csv" ImagePath="add.png" />
                    </div>
                </div>
            </asp:View>
            <asp:View ID="viewDone" runat="server">

                <p class="lead">
                    Claim logged [<asp:Label ID="lblLogResult" runat="server" />].<br />
                    You may now close this window.</p>

            </asp:View>

            <asp:View ID="viewEmpty" runat="server">
                <p>No records could be imported - please check the layout of your CSV file</p>

                <div class="buttons">
                    
                        <ucUtil:Button ID="btnAgain" runat="server" ButtonText="Try again" ImagePath="left.png" />
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    </form>
</body>
</html>
