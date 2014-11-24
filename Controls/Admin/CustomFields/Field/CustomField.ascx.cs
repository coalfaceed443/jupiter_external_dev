using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;

namespace CRM.Controls.Admin.CustomFields.Form
{
    public partial class CustomField : System.Web.UI.UserControl
    {

        public CRM_FormField CRM_FormField { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }

        }

        protected void OrganiseForm()
        {
            if (CRM_FormField != null)
            {
                hdnID.Value = CRM_FormField.ID.ToString();
                litName.Text = CRM_FormField.Name;

                switch (CRM_FormField.Type)
                {
                    case (byte)CRM_FormField.Types.SingleLineTextBox:
                        txtSingleLineTextBox.Visible = true;
                        break;
                    case (byte)CRM_FormField.Types.MultiLineTextBox:
                        txtMultiLineTextBox.Visible = true;
                        break;
                    case (byte)CRM_FormField.Types.DropDownList:
                        ddlDropDownList.Visible = true;

                        foreach (CRM_FormFieldItem item in CRM_FormField.CRM_FormFieldItems.OrderBy(a => a.OrderNo))
                        {
                            if (!ddlDropDownList.Items.Contains(new ListItem(item.Label, item.Label)))
                            {
                                ddlDropDownList.Items.Add(new ListItem(item.Label, item.Label));
                            }
                        }

                        if (ddlDropDownList.Items.Count == 0)
                            ddlDropDownList.Items.Add(new ListItem("---", "Not Entered"));

                        break;
                    case (byte)CRM_FormField.Types.SingleCheckBox:
                        chkSingleCheckBox.Visible = true;
                        break;
                    case (byte)CRM_FormField.Types.MultipleCheckBoxes:
                        pnlMultipleCheckBox.Visible = true;
                        foreach (CRM_FormFieldItem item in CRM_FormField.CRM_FormFieldItems.OrderBy(a => a.OrderNo))
                        {
                            chkBoxList.Items.Add(new ListItem(item.Label, item.Label));
                        }

                        break;
                    case (byte)CRM_FormField.Types.MultipleRadioButtons:
                        pnlMultipleRadioButton.Visible = true;
                        foreach (CRM_FormFieldItem item in CRM_FormField.CRM_FormFieldItems.OrderBy(a => a.OrderNo))
                        {
                            radBtnList.Items.Add(new ListItem(item.Label, item.Label));
                        }

                        break;
                }
            }
        }

        public void Populate(string answer)
        {
            OrganiseForm();
            if (CRM_FormField != null)
            {
                switch (CRM_FormField.Type)
                {
                    case (byte)CRM_FormField.Types.SingleLineTextBox:
                        txtSingleLineTextBox.Text = answer;
                        break;
                    case (byte)CRM_FormField.Types.MultiLineTextBox:
                        txtMultiLineTextBox.Text = answer.Replace("<br/>", "\n");
                        break;
                    case (byte)CRM_FormField.Types.DropDownList:
                        ddlDropDownList.SelectedValue = answer;
                        break;
                    case (byte)CRM_FormField.Types.SingleCheckBox:
                        chkSingleCheckBox.Checked = answer == "Yes" ? true : false;
                        break;
                    case (byte)CRM_FormField.Types.MultipleCheckBoxes:
                        foreach (ListItem item in chkBoxList.Items)
                        {
                            string[] items = answer.Split(new string[] { "<br/>" }, StringSplitOptions.RemoveEmptyEntries);

                            if (items.Any(i => i.Trim().ToLower() == item.Text.Trim().ToLower()))
                                item.Selected = true;
                        }
                        break;
                    case (byte)CRM_FormField.Types.MultipleRadioButtons:
                        radBtnList.SelectedValue = answer;
                        break;
                }
            }
        }

        public string SelectedValue()
        {
            string returnValue = "";

            if (CRM_FormField != null)
            {
                switch (CRM_FormField.Type)
                {
                    case (byte)CRM_FormField.Types.SingleLineTextBox:
                        returnValue = txtSingleLineTextBox.Text;
                        break;
                    case (byte)CRM_FormField.Types.MultiLineTextBox:
                        returnValue = txtMultiLineTextBox.Text.Replace("\n", "<br/>");
                        break;
                    case (byte)CRM_FormField.Types.DropDownList:
                        returnValue = ddlDropDownList.SelectedValue;
                        break;
                    case (byte)CRM_FormField.Types.SingleCheckBox:
                        returnValue = chkSingleCheckBox.Checked ? "Yes" : "No";
                        break;
                    case (byte)CRM_FormField.Types.MultipleCheckBoxes:
                        foreach (ListItem item in chkBoxList.Items)
                        {
                            if (item.Selected && !String.IsNullOrEmpty(returnValue))
                                returnValue += "<br/>";
                            if (item.Selected)
                                returnValue += item.Value;
                        }
                        break;
                    case (byte)CRM_FormField.Types.MultipleRadioButtons:
                        returnValue = radBtnList.SelectedValue;
                        break;
                }
            }

            return returnValue;
        }
    
    }
}