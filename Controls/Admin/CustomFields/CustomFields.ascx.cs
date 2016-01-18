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
                
                CRM.Controls.Admin.CustomFields.Form.CustomField formQuestionControl = (CRM.Controls.Admin.CustomFields.Form.CustomField)item.FindControl("ucFormQuestion");

                IEnumerable<CRM_FormFieldResponse> answers = db.CRM_FormFieldResponses.Where(r => r.CRM_FormFieldID == formField.ID && r.TargetReference == targetReference);
                
                formQuestionControl.Populate(answers); 
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

                        IEnumerable<CRM_FormFieldResponse> answers = db.CRM_FormFieldResponses.Where(r => r.TargetReference == TargetReference && r.CRM_FormFieldID == formField.ID);

                        db.CRM_FormFieldResponses.DeleteAllOnSubmit(answers);
                        db.SubmitChanges();

                        if (formField.Type == (byte)CRM_FormField.Types.DropDownList)
                        {
                            CRM_FormFieldResponse response = new CRM_FormFieldResponse()
                            {
                                Answer = "",
                                CRM_FormFieldItemID = db.CRM_FormFieldItems.Single(s => s.ID.ToString() == selectedValue).ID,
                                CRM_FormFieldID = formField.ID,
                                TargetReference = TargetReference
                            };

                            db.CRM_FormFieldResponses.InsertOnSubmit(response);
                            db.SubmitChanges();
                        }
                        else if (formField.Type == (byte)CRM_FormField.Types.MultiLineTextBox || formField.Type == (byte)CRM_FormField.Types.SingleLineTextBox || formField.Type == (byte)CRM_FormField.Types.SingleCheckBox
                             || formField.Type == (byte)CRM_FormField.Types.MultipleRadioButtons)
                        {

                            CRM_FormFieldResponse response = new CRM_FormFieldResponse()
                            {
                                Answer = selectedValue,
                                CRM_FormFieldItemID = null,
                                CRM_FormFieldID = formField.ID,
                                TargetReference = TargetReference
                            };

                            db.CRM_FormFieldResponses.InsertOnSubmit(response);
                            db.SubmitChanges();
                        }
                        else if (formField.Type == (byte)CRM_FormField.Types.MultipleCheckBoxes)
                        {
                            string[] IDs = selectedValue.Split(',');

                            foreach (string id in IDs)
                            {
                                CRM_FormFieldResponse response = new CRM_FormFieldResponse()
                                {
                                    Answer = "",
                                    CRM_FormFieldItemID = db.CRM_FormFieldItems.Single(s => s.ID.ToString() == id).ID,
                                    CRM_FormFieldID = formField.ID,
                                    TargetReference = TargetReference
                                };

                                db.CRM_FormFieldResponses.InsertOnSubmit(response);
                                db.SubmitChanges();
                            }
                        }

                       
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