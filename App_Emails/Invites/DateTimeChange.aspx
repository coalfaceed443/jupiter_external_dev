<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DateTimeChange.aspx.cs" Inherits="CRM.App_Emails.Invites.DateTimeChange" %>


             <p>Hi @NAME@,</p>

                        <p>The following event you have been tagged in has been rescheduled:</p>
<table width="660">
    
    <tr>
        <td colspan="3" style="background-color:#EDAECE;height:20px;"></td>
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
                                <td><strong>Old Date Time</strong></td>
                                <td>&nbsp;</td>
                                <td>@OLDDATETIME@</td>                            
                            </tr>
                            <tr>
                                <td><strong>New Date Time</strong></td>
                                <td>&nbsp;</td>
                                <td>@NEWDATETIME@</td>                            
                            </tr>
                            <tr>
                                <td><strong>Date Changed By</strong></td>
                                <td>&nbsp;</td>
                                <td>@CHANGED@</td>                            
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        <table width="620">
                            <tr>
                                <td width="50"><a href="" style="font-size:20px;color:#6EB340;">ACCEPT</a></td>
                                <td width="350">&nbsp;</td>
                                <td width="50"><a href="" style="font-size:20px;color:#DF006E;">DECLINE</a></td>
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
        <td colspan="3" style="background-color:#EDAECE;height:20px;"></td>
    </tr>
        
</table>

