<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetLink.aspx.cs" Inherits="CRM.App_Emails.ResetLink" %>

        <p>Hi @DISPLAYNAME@,</p>

        <p>Your reset link is as follows:</p>

        <p><a href="@RESETLINK@">@RESETLINK@</a></p>

        <p>This link will expire at @EXPIRY@.  Upon clicking this link you will be able to reset your password.</p>
