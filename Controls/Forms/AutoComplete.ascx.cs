using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Interfaces;
using CRM.Code.Utils.WebControl;

namespace CRM.Controls.Forms
{
    [PersistenceMode(PersistenceMode.Attribute)]
    public partial class AutoComplete : System.Web.UI.UserControl
    {
        public AutoCompleteConfig Config { get; set; }
        public EventHandler EventHandler { get; set; }
        public string Name { get; set; }
        public string Text
        {
            get
            {
                return hdnTextValue.Value;
            }
            set
            {
                lnkRecord.InnerText = value;
                mvInput.SetActiveView(viewLabel);
            }
        }

        // see Text Getter/Setter above, does actually do something...(switches UI view)
        public void SetText()
        {
            Text = Text;
        }

        public string SelectedID
        {
            get
            {
                return hdnSelectedID.Value;
            }
            set
            {
                hdnSelectedID.Value = value;
            }
        }


        public bool Required { get; set; }
        public bool AtLeastTextRequired { get; set; }
        public void Populate(IAutocomplete item)
        {
            if (item != null)
            {
                lnkRecord.InnerText = item.Name;
                lnkRecord.HRef = item.DetailsURL;
                SelectedID = item.Reference;
                mvInput.SetActiveView(viewLabel);
                hdnTextValue.Value = item.Name;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            cusSelected.ErrorMessage = Name + " is required";
            cusSelected.Enabled = Required;
            litset.Text = Config.QueryString;
            if (String.IsNullOrEmpty(lnkRecord.InnerText))
            {
                mvInput.SetActiveView(viewInput);
            }
            else
            {
                SetText();
            }
        }

        protected void lnkSwitch_Click(object sender, EventArgs e)
        {
            SwitchToInput();
        }

        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(SelectedID))
            {
                EventHandler.Invoke(sender, e);
            }
            else
                SelectedID = "";
        }

        public void SwitchToInput()
        {
            mvInput.SetActiveView(viewInput);
        }

        protected void cusSelected_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (AtLeastTextRequired)
            {
                args.IsValid = hdnTextValue.Value != String.Empty;
            }
            else
            {
                args.IsValid = !String.IsNullOrEmpty(SelectedID);
            }
        }



    }
}