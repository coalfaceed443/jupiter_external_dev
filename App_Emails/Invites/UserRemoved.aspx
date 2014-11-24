<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRemoved.aspx.cs" Inherits="CRM.App_Emails.Invites.UserRemoved" %>


             <p>Hi @NAME@,</p>

                        <p>You have been removed from the following event:</p>
<table width="660">
    
    <tr>
        <td colspan="3" style="background-color:#F9D5C7;height:20px;"></td>
    </tr>
        
    <tr>
        <td style="width:20px;">
        
        </td>
        <td>
            <table> 
                <tr>
                    <td class="style1">&nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        <table>
                            <tr>
                                <td><strong>Name</strong></td>
                                <td style="width:40px;">&nbsp;</td>
                                <td>@EVENTNAME@</td>                            
                            </tr>
                            <tr>
                                <td><strong>Date Time</strong></td>
                                <td>&nbsp;</td>
                                <td>@DATETIME@</td>                            
                            </tr>
                            <tr>
                                <td><strong>Removed By</strong></td>
                                <td>&nbsp;</td>
                                <td>@REMOVED@</td>                            
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="style2"></td>
                </tr>
                <tr>
                                <td class="style2">
                                <strong>Sender's message</strong>
                                </td>
                            </tr>
              <tr>
                                <td class="style2">@SENDERMESSAGE@</td>
                            </tr>

                <tr>
                    <td class="style2">&nbsp;</td>
                </tr>
            </table>
        </td>
        <td style="width:20px;">
        
        </td>
    </tr>
    <tr>
        <td colspan="3" style="background-color:#F9D5C7;height:20px;"></td>
    </tr>
        
</table>

