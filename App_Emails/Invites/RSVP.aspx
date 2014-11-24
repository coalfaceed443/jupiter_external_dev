<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RSVP.aspx.cs" Inherits="CRM.App_Emails.Invites.RSVP" %>


             <p>Hi @NAME@,</p>

                        <p>@RESPONDER@ has replied to your invite:</p>
<table width="660">
    
    <tr>
        <td colspan="3" style="background-color:#005BAA;height:20px;"></td>
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
                                <td><strong>Status</strong></td>
                                <td>&nbsp;</td>
                                <td>@STATUS@</td>                            
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
        <td colspan="3" style="background-color:#005BAA;height:20px;"></td>
    </tr>
        
</table>

