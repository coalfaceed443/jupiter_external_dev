using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.BasePages.Admin.AnnualPass;
using CRM.Code.Managers;
using CRM.Code.Models;

namespace CRM.admin.AnnualPassCard
{
    public partial class Details : AnnualPassCardPage<CRM_AnnualPass>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnSubmitChanges.Visible = false;

            ucNav.Entity = Entity;
            CRMContext = Entity;

            if (!Page.IsPostBack)
            {
                if (Entity != null)
                {
                    PopulateFields();
                }
                else
                {
                    txtMembershipNumber.Text = new AnnualPassManager().GetNewMemberID().ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Card Created, start adding passes", "details.aspx?id=" + Entity.ID);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Card Updated");
            }
        }

        protected void PopulateFields()
        {
            txtMembershipNumber.Text = Entity.MembershipNumber.ToString();
            txtMembershipNumber.ShowText = true;
        }

        protected void SaveRecord(bool newRecord)
        {
            if (newRecord)
            {
                Entity = new CRM_AnnualPassCard();
                db.CRM_AnnualPassCards.InsertOnSubmit(Entity);
                Entity.MembershipNumber = Convert.ToInt32(txtMembershipNumber.Text);                
            }

            db.SubmitChanges();
        }

        protected void cusMemberNo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Entity == null)
            {
                args.IsValid = !db.CRM_AnnualPassCards.Any(a => a.MembershipNumber.ToString() == txtMembershipNumber.Text);
            }
            else
            {
                args.IsValid = !db.CRM_AnnualPassCards.Any(a => a.MembershipNumber.ToString() == txtMembershipNumber.Text && a.ID != Entity.ID);            
            }
        }
    }
}