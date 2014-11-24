<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VenueChange.aspx.cs" Inherits="CRM.App_Emails.Invites.VenueChange" %>

<p>
    Hi @NAME@,</p>
<p>
    The following event has had it's venues updated:</p>
<table width="660">
    <tr>
        <td colspan="3" style="background-color: #648BAD; height: 20px;">
        </td>
    </tr>
    <tr>
        <td style="width: 20px;">
        </td>
        <td>
            <table>
                <tr>
                    <td class="style1">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        <table>
                            <tr>
                                <td>
                                    <strong>Name</strong>
                                </td>
                                <td style="width: 40px;">
                                    &nbsp;
                                </td>
                                <td>
                                    @EVENTNAME@
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Date Time</strong>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    @DATETIME@
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Venues amended by</strong>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    @AMENDED@
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        <strong>Sender's message</strong>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        @SENDERMESSAGE@
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style2">&nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        <table width="620">
                            <tr>
                                <td width="500" colspan="3"><a href="@VENUEDETAILS@" style="font-size:20px;color:#6EB340;">VIEW VENUES</a></td>
                            </tr>
                        </table>
                    </td>                    
                </tr>
            </table>
        </td>
        <td style="width: 20px;">
        </td>
    </tr>
    <tr>
        <td colspan="3" style="background-color: #648BAD; height: 20px;">
        </td>
    </tr>
</table>
