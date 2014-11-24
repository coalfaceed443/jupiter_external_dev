﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Admin.ImageUpload.upload" Codebehind="upload.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CodeCarvings Piczard - SimpleImageUpload</title>
    <style type="text/css">
        html, body, form
        {
	        width: 100%;
	        height: 100%;
	        margin: 0px;
	        padding: 0px;
	        overflow: hidden;
        }  
        
        .containerDiv
        {
        	width:100%;
        	height: 100%;
        }
        
        .hiddenDiv
        {
        	display:none;
        }
        
        .InputFile
        {
        	font-size: <% =this.ButtonSize.Height.ToString(System.Globalization.CultureInfo.InvariantCulture) %>px;
        	height: <% =this.ButtonSize.Height.ToString(System.Globalization.CultureInfo.InvariantCulture) %>px;
        	width: <% =this.ButtonSize.Width.ToString(System.Globalization.CultureInfo.InvariantCulture) %>px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server" ID="phMainContainer">
            <div class="containerDiv">
                <asp:FileUpload runat="server" ID="fuFile" CssClass="InputFile" />
                <div class="hiddenDiv">
                    <asp:DropDownList runat="server" ID="ddlDummyForceDoPostBackCreation" AutoPostBack="true">
                        <asp:ListItem Text="Dummy" Value="Dummy" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button runat="server" ID="btnUpload" Text="Upload" OnClick="btnUpload_Click" />
                </div>
            </div>
            
            <script type="text/javascript">
                //<![CDATA[
                                                              
                function getSimpleImageUploadControl()
                {
                    var id = "<% =CodeCarvings.Piczard.Web.Helpers.JSHelper.EncodeString(this.SimpleImageUploadControlId) %>";
                    return window.parent.CodeCarvings.Wcs.Piczard.Upload.SimpleImageUpload.getControl(id);
                }

                function onUploadStart()
                {
                    var control = getSimpleImageUploadControl();
                    if (control != null)
                    {
                        control.onUploadStart();
                    }                    
                }
                function onUploadSuccess()
                {
                    var control = getSimpleImageUploadControl();
                    if (control != null)
                    {
                        control.onUploadSuccess();
                    }
                }
                
                // Auto-postback when a file is selected
                function fuFile_Monitor()
                {
                    var oFuFile = document.getElementById("<% =this.fuFile.ClientID %>");
                    if (oFuFile)
                    {
                        var value = oFuFile.value
                        if (oFuFile.value)
                        {
                            window.clearInterval(intervalId);
                            onUploadStart();
 
                            <% =this.Page.ClientScript.GetPostBackEventReference(this.btnUpload, "") %>
                         }
                    }
                }
                var intervalId = window.setInterval(fuFile_Monitor, 250);
                //]]>
            </script>        
        </asp:PlaceHolder>        
    </form>
</body>
</html>
