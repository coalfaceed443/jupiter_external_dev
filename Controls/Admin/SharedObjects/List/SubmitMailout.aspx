<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubmitMailout.aspx.cs"
    Inherits="CRM.Controls.Admin.SharedObjects.List.SubmitMailout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ucUtil:Scripts ID="ucScripts" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="topContentBox" style="width: 500px;">
        <h2>
            Log Communication</h2>
        <asp:MultiView ID="mvLog" runat="server" ActiveViewIndex="0">
            <asp:View ID="viewEnter" runat="server">
                <p class="lead">
                    Use this form to upload any mailouts you have made in CSV form. If the original
                    'Reference' field from the CRM export is still available as a column, it
                    will be logged against that record as having received this mailout.</p>

                    <asp:ValidationSummary ID="val" runat="server" />
                    <asp:CustomValidator ID="cusType" runat="server" 
                    ErrorMessage="Please select a communication type" Display="None" 
                    EnableClientScript="false" onservervalidate="cusType_ServerValidate" />
                <div class="innerContentForm" style="width: 400px;">
                    <table class="details">
                        <tr>
                            <td>
                                <label>
                                    Mailout name
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Type
                                </label>                                
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="true" DataTextField="Value" DataValueField="Key">
                                    <asp:ListItem Text="[Please Select]" Value="" />
                                </asp:DropDownList>
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
                    Communication logged [<asp:Label ID="lblLogResult" runat="server" />].<br />
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
