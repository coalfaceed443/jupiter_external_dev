﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.Managers;
using CRM.Code.Models;

namespace CRM.admin.AnnualPassCard.AnnualPass.Type
{
    public partial class Details : AdminPage
    {
        protected CRM.Code.Models.CRM_AnnualPassType Entity = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            RunSecurity(CRM.Code.Models.Admin.AllowedSections.NotSet);
            Entity = db.CRM_AnnualPassTypes.SingleOrDefault(c => c.ID.ToString() == Request.QueryString["id"]);
            
            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;

            // Security //

            btnSubmitChanges.Visible = PermissionManager.CanUpdate;
            btnDelete.Visible = PermissionManager.CanDelete;
            if (!PermissionManager.CanAdd && Entity == null)
                Response.Redirect("list.aspx");

            // confirmations //

            confirmationDelete.StandardDeleteHidden("annual pass type", btnRealDelete_Click);

            // process //

            CRMContext = Entity;

            if (!IsPostBack)
            {
                if (Entity != null)
                    PopulateFields();
            }
        }



        private void PopulateFields()
        {
            txtName.Text = Entity.Name;
            txtPrice.Text = Entity.DefaultPrice.ToString("N2");
            ddlType.SelectedValue = Entity.Type.ToString();
            txtGroupSize.Text = Entity.GroupSize.ToString();
            chkIsWebsite.Checked = Entity.IsWebsite;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Item Added", "details.aspx?id=" + Entity.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Item Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //

            object oldEntity = null;

            if (newRecord)
            {
                Entity = new CRM_AnnualPassType();
                db.CRM_AnnualPassTypes.InsertOnSubmit(Entity);
            }

            Entity.Name = txtName.Text;
            Entity.DefaultPrice = Convert.ToDecimal(txtPrice.Text);
            Entity.Type = Convert.ToByte(ddlType.SelectedValue);
            Entity.GroupSize = Convert.ToInt32(txtGroupSize.Text);
            Entity.IsWebsite = chkIsWebsite.Checked;
            db.SubmitChanges();

            if (oldEntity != null)
            {
                CRM.Code.History.History.RecordLinqUpdate(AdminUser, oldEntity, Entity);
                db.SubmitChanges();
            }
            else
            {
                CRM.Code.History.History.RecordLinqInsert(AdminUser, Entity);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }


        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            db.CRM_AnnualPassTypes.DeleteOnSubmit(Entity);
            db.SubmitChanges();

            NoticeManager.SetMessage("Item removed", "list.aspx");
        }


    }
}