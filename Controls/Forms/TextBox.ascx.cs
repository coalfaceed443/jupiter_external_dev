using System;
using System.Web.UI.WebControls;
using System.Net.Mail;
using CRM.Code.WebControl;
namespace CRM.Controls.Forms
{
    public partial class UserControlTextBox : UserControlBase
    {
        private int width = 286;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        public string OnKeyDown { get; set; }
        public string OnKeyPress { get; set; }
        public string OnKeyUp { get; set; }
        public string OnBlur { get; set; }
        public string OnFocus { get; set; }

        private int height = 80;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public string Text
        {
            get
            {
                return txtTextBox.Text;
            }
            set
            {
                base.BaseValue = value;
                txtTextBox.Text = value;
            }
        }

        private bool showText = false;
        public bool ShowText
        {
            get
            {
                return showText;
            }
            set
            {
                showText = value;
            }
        }

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

        public TextBoxMode TextMode { get; set; }
        public override string ClientID
        {
            get
            {
                return txtTextBox.ClientID;
            }

        }
        private string datatype;
        public string DataType
        {
            get
            {
                return datatype;
            }
            set
            {
                datatype = value;
            }
        }

        private string defaultValue;
        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
            }
        }

        private int maxLength = 250;
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        private bool email;
        public bool IsEmail
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        public string CssClass { get; set; }

        public string Name { get; set; }

        public string ValidationGroup { get; set; }

        public string Style { get; set; }

        private bool _AutoHideValue = false;
        public bool AutoHideValue { get { return _AutoHideValue; } set { _AutoHideValue = value; } }

        public TextBox TextBox { get { return txtTextBox; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(defaultValue))
                {
                    if (String.IsNullOrEmpty(Text))
                        txtTextBox.Text = defaultValue;
                }
            }

            if (!String.IsNullOrEmpty(defaultValue))
            {
                cusDefault.Enabled = true;
                cusDefault.ErrorMessage = this.Name + " is still the default value";
            }

            if (Name == "")
                Name = ID.Replace("txt", "");

            base.BaseValue = txtTextBox.Text;

            txtTextBox.Width = width;

            if (TextMode == TextBoxMode.MultiLine)
                txtTextBox.Height = height;

            txtTextBox.TextMode = TextMode;

            if (!String.IsNullOrEmpty(ValidationGroup))
            {
                reqTextBox.ValidationGroup = ValidationGroup;
                cusEmail.ValidationGroup = ValidationGroup;
                cmpDataType.ValidationGroup = ValidationGroup;
            }

            if (IsEmail)
                cusEmail.Enabled = true;

            txtTextBox.Width = width;

            reqTextBox.ErrorMessage = Name + " is required";
            reqTextBox.Enabled = required;

            cmpDataType.Visible = true;
            string userType = "";
            switch (DataType)
            {
                case "double":
                    cmpDataType.Type = ValidationDataType.Double;
                    userType = "a number";
                    this.Text = this.Text.Replace(",", "");
                    break;

                case "int":
                    cmpDataType.Type = ValidationDataType.Integer;
                    userType = "a number";
                    break;

                case "string":
                    cmpDataType.Type = ValidationDataType.String;
                    userType = "text";
                    break;

                default:
                    cmpDataType.Visible = false;
                    break;
            }

            cmpDataType.ErrorMessage = this.Name + " must be " + userType;

            if (!String.IsNullOrEmpty(Style))
            {
                bool add = true;
                if (txtTextBox.Attributes["style"] != null)
                {
                    add = !txtTextBox.Attributes["style"].Contains(Style);
                }

                if (add)
                    txtTextBox.Attributes["style"] += Style;
            }

            if (CssClass != null)
            {
                txtTextBox.CssClass = CssClass;
            }

            if (showText)
            {
                txtTextBox.Visible = false;
                reqTextBox.Enabled = false;
                cusEmail.Enabled = false;
                cmpDataType.Enabled = false;

                litText.Text = "<span>" + Text + "</span>";
            }

            if (!String.IsNullOrEmpty(OnKeyDown))
                txtTextBox.Attributes["onkeydown"] = OnKeyDown;
            if (!String.IsNullOrEmpty(OnKeyUp))
                txtTextBox.Attributes["onkeyup"] = OnKeyUp;
            if (!String.IsNullOrEmpty(OnKeyPress))
                txtTextBox.Attributes["onkeypress"] = OnKeyPress;
            if (!String.IsNullOrEmpty(OnBlur))
                txtTextBox.Attributes["onblur"] = OnBlur;
            if (!String.IsNullOrEmpty(OnFocus))
                txtTextBox.Attributes["onfocus"] = OnFocus;

            if (!String.IsNullOrEmpty(defaultValue))
            {
                txtTextBox.Attributes["onfocus"] += "if(this.value == '" + defaultValue + "'){this.value = '';}";
                txtTextBox.Attributes["onblur"] += "if(this.value == ''){this.value = '" + defaultValue + "';}";
            }
        }

        public void addStyle(string style)
        {
            txtTextBox.Attributes["style"] += style;
        }

        public void removeStyle(string styleTag)
        {
            if (txtTextBox.Attributes["style"] != null)
            {
                int indexOfTag = -1;
                do
                {
                    //find the tag
                    indexOfTag = txtTextBox.Attributes["style"].IndexOf(styleTag);
                    if (indexOfTag > -1)
                    {
                        //Get the rest of the style attribute so we can find the correct ';'
                        string subString = txtTextBox.Attributes["style"].Substring(indexOfTag);
                        int indexOfSemi = subString.IndexOf(';');

                        //If there is a semi colon remove the tag, otherwise just remove everything from the start of the tag
                        if (indexOfSemi > -1)
                        {
                            string first = txtTextBox.Attributes["style"].Substring(0, indexOfTag);
                            string second = txtTextBox.Attributes["style"].Substring(indexOfSemi + indexOfTag + 1,
                                txtTextBox.Attributes["style"].Length - (indexOfSemi + indexOfTag + 1));

                            txtTextBox.Attributes["style"] = first + second;
                        }
                        else
                        {
                            txtTextBox.Attributes["style"] = txtTextBox.Attributes["style"].Substring(0, indexOfTag);
                        }
                    }
                } while (indexOfTag >= 0);
            }
        }

        protected void cusEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Required == true && txtTextBox.Text != "")
            {
                try
                {
                    MailAddress message = new MailAddress(txtTextBox.Text);
                    args.IsValid = true;
                }
                catch
                {
                    args.IsValid = false;
                }
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void cusDefault_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!String.IsNullOrEmpty(defaultValue) && txtTextBox.Text.ToLower() == defaultValue.ToLower() && required)
                args.IsValid = false;
            else
                args.IsValid = true;
        }

        protected void cusMaxLength_Validate(object source, ServerValidateEventArgs args)
        {
            if (txtTextBox.Text.Length > MaxLength)
            {
                cusMaxLength.ErrorMessage = Name + " has too many characters, the maximum amount of characters is " + MaxLength.ToString() 
                                                 + " and you have input " + txtTextBox.Text.Length + " characters.";
                args.IsValid = false;
            }
            else
                args.IsValid = true;

        }
    }

}