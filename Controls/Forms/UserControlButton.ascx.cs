using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.BasePages.Admin.Calendar;

namespace CRM.Controls.Forms
{
    public partial class UserControlButton : System.Web.UI.UserControl
    {
        public string ButtonText { get; set; }
        public string ImagePath { get; set; }
        public string Class { get; set; }
        public string NavigateUrl { get; set; }
        public string Style { get; set; }
        public string OnClick { get; set; }
        public string Target { get; set; }

        //The event fired from the button
        public EventHandler EventHandler { get; set; }

        //Constructors for the control, used to dynamically create controls
        public UserControlButton() { }

        public UserControlButton(string text, string imagePath, string cssClass)
        {
            ButtonText = text;
            ImagePath = imagePath;
            Class = cssClass;
        }

        public UserControlButton(string text, string imagePath, string cssClass, string navUrl)
        {
            ButtonText = text;
            ImagePath = imagePath;
            Class = cssClass;
            NavigateUrl = navUrl;
        }

        public UserControlButton(string text, string imagePath, string cssClass, EventHandler eventHandler)
        {
            ButtonText = text;
            ImagePath = imagePath;
            Class = cssClass;

            this.EventHandler = eventHandler;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page is AdminPage)
            {
                CRM_SystemAccessAdmin AdminPermission = ((AdminPage)Page).AdminPermission;
                if (AdminPermission != null && this.ID != null)
                {
                    if (this.ID.Contains("btnSubmitChanges"))
                    {
                        this.Visible = AdminPermission.IsWrite;
                    }
                    else if (this.ID.Contains("btnSubmit") && !this.ID.Contains("Changes"))
                    {
                        this.Visible = AdminPermission.IsAdd;
                    }
                    else if (this.ID == "btnDelete")
                    {
                        this.Visible = AdminPermission.IsDelete;
                    }
                }
            }

            if (Page is CRM_CalendarPage<CRM_Calendar>)
            {
                CRM_Calendar entity = ((CRM_CalendarPage<CRM_Calendar>)Page).Entity;


                if (entity != null)
                {
                    bool IsOwner = ((CRM_CalendarPage<CRM_Calendar>)Page).AdminUser.ID == entity.CreatedByAdminID;
                    CRM_SystemAccessAdmin AdminPermission = ((AdminPage)Page).AdminPermission;



                    if (AdminPermission != null && this.ID != null)
                    {
                        if (entity.PrivacyStatus == (byte)CRM_Calendar.PrivacyTypes.Editable)
                        {
                            if (!IsOwner)
                            {
                                if (this.ID.Contains("btnSubmitChanges"))
                                {
                                    this.Visible = AdminPermission.IsWrite;
                                }
                                else if (this.ID.Contains("btnSubmit") && !this.ID.Contains("Changes"))
                                {
                                    this.Visible = AdminPermission.IsAdd;
                                }
                                else if (this.ID == "btnDelete")
                                {
                                    this.Visible = AdminPermission.IsDelete;
                                }
                            }
                        }
                        else if (entity.PrivacyStatus == (byte)CRM_Calendar.PrivacyTypes.Private || entity.PrivacyStatus == (byte)CRM_Calendar.PrivacyTypes.Viewable)
                        {
                            if (!IsOwner)
                            {
                                if (this.ID.Contains("btnSubmitChanges"))
                                {
                                    this.Visible = false;
                                }
                                else if (this.ID.Contains("btnSubmit") && !this.ID.Contains("Changes"))
                                {
                                    this.Visible = false;
                                }
                                else if (this.ID == "btnDelete")
                                {
                                    this.Visible = false;
                                }
                            }
                        }
                    }
                }
            }

            btnButton.CssClass = Class;
            if (!String.IsNullOrEmpty(Style))
                btnButton.Attributes["style"] = Style;

            if (!String.IsNullOrEmpty(OnClick))
            {
                pnlLink.Attributes["onclick"] = OnClick;
                btnButton.Attributes["onclick"] = OnClick;
            }

            if (EventHandler != null)
            {
                //ButtonText = EventHandler.Method.Name;
                btnButton.Click += EventHandler;
            }
            else
            {
                pnlButton.Visible = false;
                pnlLink.Visible = true;
            }
        }
    }
}