<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewInvite.aspx.cs" Inherits="CRM.App_Emails.Invites.NewInvite" %>


             <p>Hi @NAME@,</p>

                        <p>You have been tagged against the following event:</p>
<table width="660">
    
    <tr>
        <td colspan="3" style="background-color:#CCE9B9;height:20px;"></td>
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
                                <td><strong>Invited By</strong></td>
                                <td>&nbsp;</td>
                                <td>@INVITED@</td>                            
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
                <tr>
                    <td class="style2">
                        <table width="620">
                            <tr>
                                <td width="50"><a href="@ACCEPT@" style="font-size:20px;color:#6EB340;">ACCEPT</a></td>
                                <td width="350">&nbsp;</td>
                                <td width="50"><a href="@DECLINE@" style="font-size:20px;color:#DF006E;">DECLINE</a></td>
                            </tr>
                        </table>
                    </td>                    
                </tr>
                <tr>
                    <td class="style2"></td>
                </tr>
            </table>
        </td>
        <td style="width:20px;">
        
        </td>
    </tr>
    <tr>
        <td colspan="3" style="background-color:#CCE9B9;height:20px;"></td>
    </tr>
        
</table>

