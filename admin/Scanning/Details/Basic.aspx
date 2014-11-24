<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Basic.aspx.cs" Inherits="CRM.admin.Scanning.Details.Basic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <ucUtil:Scripts ID="ucScripts" runat="server" />
</head>
<body id="ajaxtab">
    <form id="form1" runat="server">
    <br class="clearFix" /> 
    <div id="LHSDetails">
        
        <table>
            <tr>
                <td rowspan="3" width="225">
                    <img src="#" alt="User Photo" id="imgPhoto" runat="server" width="200" />
                </td>                    
                <td width="250">
                    <table>
                        <tr>
                            <td>
                                <label>ID</label>
                                <ucUtil:TextBox ID="txtID" runat="server" ShowText="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>                           
                                <%= Entity.Fullname %>                   
                            </td>
                        </tr>
                        <tr>
                            <td> 
                                <asp:Label ID="lblDoB" runat="server" />                              
                            </td>
                        </tr>
                    </table>    
                </td>
                <td  width="250">
                    <table id="contact">
                        <tr>
                            <td>
                                <label>Primary Email</label>
                                <asp:Label ID="lblPrimaryEmail" runat="server" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Primary Address</label>
                                <asp:Label ID="lblPrimaryAddress" runat="server" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Primary Tel</label>
                                <asp:Label ID="lblPrimaryTel" runat="server" />
                            </td>
                        </tr>                        
                    </table>
                </td>
                <td width="200">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsChild" runat="server" Text="Is Child" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsConcession" runat="server" Text="Is Concession" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <asp:CheckBox ID="chkIsCarer" runat="server" Text="Is Carer/Minder" Enabled="false" />
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            

        </table>

        <div class="buttons">
        <a href="<%= Entity.DetailsURL %>" target="_blank">Edit this person record <img src="/_assets/images/admin/icons/new-window-icon-gray.png" alt="open in a new window" width="10px" /></a>
        </div>

    </div>

    <ucUtil:NavigationScan ID="ucNavScan" runat="server" section="navDetails" />
    
    </form>
</body>
</html>
