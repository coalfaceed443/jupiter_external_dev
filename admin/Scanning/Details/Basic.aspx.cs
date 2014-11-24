using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin;
using CRM.Code.BasePages.Admin.Persons;
using CRM.Code.Models;

namespace CRM.admin.Scanning.Details
{
    public partial class Basic : CRM_PersonPage<CRM_Person>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucNavScan.Entity = Entity;
            if (!Page.IsPostBack)
            {
                PopulateFields();
            }
        }

        protected void PopulateFields()
        {
            imgPhoto.Src = Entity.Photo;
            txtID.Text = Entity.ID.ToString();
            lblDoB.Text = Entity.DateOfBirthOutput;
            lblPrimaryEmail.Text = Entity.PrimaryEmail;
            lblPrimaryAddress.Text = Entity.PrimaryAddress.FormattedAddressBySep("<br/>");
            lblPrimaryTel.Text = Entity.PrimaryTelephone;
            chkIsChild.Checked = Entity.IsChild;
            chkIsConcession.Checked = Entity.IsConcession;
            chkIsCarer.Checked = Entity.IsCarerMinder;
        }
    }
}