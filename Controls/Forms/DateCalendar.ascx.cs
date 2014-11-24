using System;
using CRM.Code.Utils;
using CRM.Code.Utils.Time;
using System.Web.UI.WebControls;

namespace CRM.Controls.Forms
{
    public partial class UserControlDateCalendar : System.Web.UI.UserControl
    {
        public string Text
        {
            get
            {
                return txtDate.Text;
            }
            set
            {
                txtDate.Text = value;
            }
        }

        public TextBox TextBox { get { return txtDate; } }

        private bool required = false;
        public bool Required
        {
            get
            {
                return required;
            }
            set
            {
                required = value;
            }
        }

        public override string ClientID
        {
            get
            {
                return txtDate.ClientID;
            }

        }

        private bool showTime = false;
        public bool ShowTime
        {
            get
            {
                return showTime;
            }
            set
            {
                showTime = value;
            }
        }
        private bool showDate = true;
        public bool ShowDate
        {
            get
            {
                return showDate;
            }
            set
            {
                showDate = value;
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public DateTime Value
        {
            get
            {
                return CRM.Code.Utils.Text.Text.FormatInputDateTime(txtDate.Text + " " + txtTime.Text);
            }
            set
            {
                txtDate.Text = value.ToString("dd/MM/yyyy");
                txtTime.Text = value.ToString("HH:mm");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            reqDate.ErrorMessage = Name + " is required";
            reqDate.Enabled = required;

            regDate.ErrorMessage = Name + " is invalid";
            regDate.Enabled = required;

            reqTime.ErrorMessage = Name + " is required";
            reqTime.Enabled = required;

            regTime.ErrorMessage = Name + " is invalid";
            regTime.Enabled = required;

            if (showTime)
            {
                txtTime.Visible = true;
                reqTime.Enabled = true;
                reqTime.Enabled = true;
            }
            if (!showDate)
            {
                txtDate.Visible = false;
                reqDate.Enabled = false;
                regDate.Enabled = false;
                txtDate.Text = Value.ToString("dd/MM/yyyy");
                imbDate.Visible = false;
            }
        }
    }
}