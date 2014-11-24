<%@ Page Language="C#" AutoEventWireup="true" Inherits="CRM.Admin.Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Admin Panel</title>
    
    <link href="/_assets/images/admin/favicon.ico" rel="icon" type="image/x-icon" />
    <link href="/_assets/css/admin/default.css" rel="stylesheet" type="text/css" />
    <link href="/_assets/css/admin/buttons.css" rel="stylesheet" type="text/css" />

    <script src="/_assets/scripts/jquery-1.4.2.min.js" type="text/javascript"></script>
    
    <!-- Can't use a panel, need to use JS to submit -->
    <script language="javascript" type="text/javascript">

        document.onkeydown = checkKeycode
        function checkKeycode(e) {
            var keycode;
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            if (keycode == 13) 
            {
                __doPostBack('btnLogin', '');
            }
        }

    </script>

</head>
<body>
    
    <form id="form1" runat="server">

    <div id="lContainer">

        <div id="loginLogo">&nbsp;</div>
              
            <h2><%= CRM.Code.Constants.WebsiteName %> Login</h2>

        <div id="loginForm">
      
            <div id="loginContainer">
                
                <asp:MultiView ID="mvLogin" runat="server" ActiveViewIndex="0">
                    <asp:View ID="viewLogin" runat="server">
                        
                            <asp:Label ID="lblMessage" CssClass="notice" runat="server" visible="false" />

                            <p id="user">
                                Username:<br />
                                <label class="smallInput">
                                    <asp:TextBox CssClass="textbox" ID="txtUsername" Style="width: 100%;" runat="server" />
                                </label>
                            </p>
                            <p id="pass">
                                Password:
                                <br />
                                <label class="smallInput">
                                    <asp:TextBox CssClass="textbox" Style="width: 100%;" ID="txtPassword" runat="server" TextMode="Password" />
                                </label>
                            </p>

                          
                            <div id="loginbut">
                                <label>
                                    <asp:Button ID="btnLogin" runat="server" Text="Submit" style="border-width:0px;" onclick="btnLogin_Click" />
                                </label>
                            </div>
                     

                    </asp:View>
                    <asp:View ID="viewForgotten" runat="server">

                            <asp:CustomValidator ID="cusNoLogin" runat="server" Display="Dynamic" 
                                ErrorMessage="We did not recognise those details.  Please contact a Master Admin." 
                                ValidationGroup="reset" onservervalidate="cusNoLogin_ServerValidate" />

                            <p>Please enter your username or email address below.</p>
                            <br />
                            <p id="user" style="width:100%;">
                                Email / Username:
                               
                                    <asp:TextBox CssClass="textbox" ID="txtEmailUsername" Style="width: 96%;" runat="server" />
                              
                            </p>

                            <div id="loginbut">
                                <label>
                                    <asp:Button ID="btnForgotten" runat="server" Text="Send me a reset link" 
                                    style="border-width:0px;" onclick="btnForgotten_Click" 
                                    ValidationGroup="reset"  />
                                </label>
                            </div>
                     

                    </asp:View>
                    <asp:View ID="viewSent" runat="server">
                            <p>A reset link has been sent to <asp:Literal ID="litEmail" runat="server" />.  It will expire at <%= CRM.Code.Utils.Time.UKTime.Now.AddMinutes(5).ToString("HH:mm") %>.</p>
                            <br />
                      <div id="loginbut">
                                <label>
                                    <asp:Button ID="btnResend" runat="server" Text="Re-send me a reset link" 
                                    style="border-width:0px;" onclick="btnForgotten_Click" 
                                    ValidationGroup="reset"  />
                                </label>
                            </div>

                    </asp:View>
                    <asp:View ID="viewReset" runat="server">
                        
                            <asp:ValidationSummary ID="valReset" runat="server" ValidationGroup="resetme" />

                            <p>Please enter your new password below.</p>
                            <br />
                            <p id="user" style="width:100%;">
                                Password:
                               
                                    <ucUtil:TextBox CssClass="textbox" ID="txtNewPassword" TextMode="Password" Style="width: 96%;" runat="server" Required="true" ValidationGroup="resetme" />
                              
                            </p>

                            <div id="loginbut">
                                <label>
                                    <asp:Button ID="btnReset" runat="server" Text="Reset" 
                                    style="border-width:0px;" onclick="btnReset_Click" ValidationGroup="resetme"  />
                                </label>
                            </div>

                    </asp:View>

                    <asp:View ID="viewExpired" runat="server">
                            
                        <p>Your link has expired.  <a href="/admin">Click here to reset your password again</a></p>

                    </asp:View>

                    <asp:View ID="viewDone" runat="server">
                        
                        <p>Your password has been reset.  <a href="/admin">Click here to login with your new details.</a></p>

                    </asp:View>

                </asp:MultiView>


         
                <div style="clear:both;" />

            </div>

        </div>
               
        
    </div>
    
         <p id="forgotten"><asp:LinkButton ID="lnkForgotten" runat="server" Visible="false" 
                 Text="Forgotten your login?" onclick="lnkForgotten_Click" /></p>

    </form>

</body>
</html>
