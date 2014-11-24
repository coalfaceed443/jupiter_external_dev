using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.Utils.Error;

namespace CRM.Controls.Admin.CustomFields
{
    public partial class CustomFields : System.Web.UI.UserControl
    {
        public int _DataTableID { get; set; }
        public string TargetReference { get; set; }
        protected _DataTable _DataTable;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void Save(string targetReference)
        {
            TargetReference = targetReference;
            lnkSubmit_Click(null, null); 
        }

        public void Populate(string targetReference)
        {
            MainDataContext db = new MainDataContext();
            if (_DataTableID != 0)
            {
                _DataTable = db._DataTables.Single(f => f.ID == _DataTableID);


                if (!Page.IsPostBack)
                {
                    var questions = _DataTable.CRM_FormFields.Where(f => f.IsActive && !f.IsArchived).OrderBy(f => f.OrderNo);
                    rptQuestions.DataSource = questions;
                    rptQuestions.DataBind();
                }

            }
            else
                this.Visible = false;


            foreach (RepeaterItem item in rptQuestions.Items)
            {
                int fieldID = Int32.Parse(((HiddenField)item.FindControl("hdnID")).Value);
                CRM_FormField formField = db.CRM_FormFields.SingleOrDefault(a => a.ID == fieldID);
                CRM_FormFieldAnswer answer = formField.CRM_FormFieldAnswers.FirstOrDefault(f => f.TargetReference == targetReference);
                CRM.Controls.Admin.CustomFields.Form.CustomField formQuestionControl = (CRM.Controls.Admin.CustomFields.Form.CustomField)item.FindControl("ucFormQuestion");
                
                if (answer != null)
                    formQuestionControl.Populate(answer.Answer);
                else
                    formQuestionControl.Populate(String.Empty);    
            }
        }


        public bool IsValid()
        {
            MainDataContext db = new MainDataContext();
            bool isValid = true;
            foreach (RepeaterItem item in rptQuestions.Items)
            {
                int fieldID = Int32.Parse(((HiddenField)item.FindControl("hdnID")).Value);
                CRM_FormField formField = db.CRM_FormFields.SingleOrDefault(a => a.ID == fieldID);
                    
                CRM.Controls.Admin.CustomFields.Form.CustomField formQuestionControl = (CRM.Controls.Admin.CustomFields.Form.CustomField)item.FindControl("ucFormQuestion");
                formQuestionControl.CRM_FormField = formField;

                string selectedValue = formQuestionControl.SelectedValue(); 
                
                if ((selectedValue == "" || selectedValue == "-1") && formField.IsRequired)
                {
                    ValidationError.AddValidationError(formField.Name + " is required");
                    isValid = false;
                }
            }

            return isValid;
        }

        protected void lnkSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                MainDataContext db = new MainDataContext();
                bool isValid = true;


                foreach (RepeaterItem item in rptQuestions.Items)
                {
                    int fieldID = Int32.Parse(((HiddenField)item.FindControl("hdnID")).Value);
                    CRM_FormField formField = db.CRM_FormFields.SingleOrDefault(a => a.ID == fieldID);
                    
                    CRM.Controls.Admin.CustomFields.Form.CustomField formQuestionControl = (CRM.Controls.Admin.CustomFields.Form.CustomField)item.FindControl("ucFormQuestion");
                    formQuestionControl.CRM_FormField = formField;
                    string selectedValue = formQuestionControl.SelectedValue();


                    if ((selectedValue == "" || selectedValue == "-1") && formField.IsRequired)
                    {
                        isValid = false;
                        ValidationError.AddValidationError(formField.Name + " is required");
                    }
                    else
                    {
                        
                        CRM_FormFieldAnswer answer = formField.CRM_FormFieldAnswers.FirstOrDefault(f => f.TargetReference == TargetReference);

                        if (answer == null)
                        {
                            answer = new CRM_FormFieldAnswer();
                            db.CRM_FormFieldAnswers.InsertOnSubmit(answer);
                        }

                        answer.TargetReference = TargetReference;
                        answer.CRM_FormFieldID = formField.ID;
                        answer.Answer = selectedValue;
                        db.SubmitChanges();
                       
                    }
                }


                if (isValid)
                {

                    db.SubmitChanges();

                }
            }

        }

    }
}