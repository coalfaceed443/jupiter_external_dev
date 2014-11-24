using System;

namespace CRM.Controls.Forms
{
    public partial class UserControlTextEditor : System.Web.UI.UserControl
    {
        private int width = 940;

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

        private int height = 300;

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
                return fckBody.Text;
            }
            set
            {
                fckBody.Text = value;
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

        public string ToolbarSet { get; set; }

        private bool startExpanded = true;
        public bool ToolbarStartExpanded
        {
            get
            {
                return startExpanded;
            }
            set
            {
                startExpanded = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            fckBody.Width = width;
            fckBody.Height = height;

            reqTextBox.ErrorMessage = Name + " is required";
            reqTextBox.Enabled = required;

            fckBody.ForcePasteAsPlainText = true;

            fckBody.StylesSet = @"[
        { name : 'Large Text', element : 'p', attributes : { 'class' : 'large-text' } },
        { name : 'Sub Heading', element : 'p', attributes : { 'class' : 'subheading' } },
        { name : 'Uppercase', element : 'p', attributes : { 'class' : 'uppercase' } },
        { name : 'Regular', element : 'p', attributes : { 'class' : '' } }
        ]";

            if (ToolbarSet == "basic")
            {
                fckBody.config.toolbar = new object[]
		    {
                new object[] { "Bold", "Italic", "Underline",  "NumberedList", "BulletedList" ,"Link", "Unlink" }
		    };
            }

            fckBody.FilebrowserBrowseUrl = "/_assets/ckeditor/browsers/Filemanager-master/index.html";


        }
    }

}