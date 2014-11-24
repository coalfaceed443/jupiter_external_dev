using System;
using System.Web.UI.WebControls;
using System.IO;
using CRM.Code.Utils.Files;
using System.Web;
namespace CRM.Controls.Forms
{
    public partial class UserControlFileUpload : System.Web.UI.UserControl
    {
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

        public FileUpload FileUpload
        {
            get
            {
                return FileUpload1;
            }
        }

        public string RequiredExtension { get; set; }


        private bool isImage = false;
        public bool IsImage
        {
            get
            {
                return isImage;
            }
            set
            {
                isImage = value;
            }
        }

        public string CurrentFile
        {
            get;
            set;
        }

        public string Name { get; set; }

        public bool HasFile
        {
            get
            {
                return FileUpload1.HasFile;
            }
        }

        public string FileName
        {
            get
            {
                return FileUpload1.FileName;
            }
        }
        public string Extension
        {
            get
            {
                return Files.GetFileExtension(FileUpload1.FileName);
            }
        }

        public double MaxSize { get; set; }

        public double FileSize
        {
            get
            {
                return (FileUpload1.FileContent.Length / 1000);
            }
        }

        public void SaveAs(string filePath)
        {
            FileUpload1.SaveAs(filePath);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cusFile.ErrorMessage = Name + " is required";
            cusFile.Enabled = required;

            if (File.Exists(MapPath(CurrentFile)))
            {
                lnkView.NavigateUrl = CurrentFile;
                divView.Visible = true;
            }

        }

        protected void cusFile_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = FileUpload1.HasFile;
        }
        protected void cusType_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!String.IsNullOrEmpty(RequiredExtension))
            {
                string extension = RequiredExtension;

                if (RequiredExtension.Substring(0, 1) != ".")
                {
                    extension = "." + extension;
                }

                if (FileUpload1.HasFile)
                {
                    if (Files.GetFileExtension(FileUpload1.FileName) == extension)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        ((CustomValidator)source).ErrorMessage = "The upload extension for '" + Name + "' must be " + extension;
                        args.IsValid = false;
                    }
                }
            }
        }
        protected void cusSize_ServerValidate(object source, ServerValidateEventArgs args)
        {
            double maxSize = 20000 * 1024;

            if (MaxSize > 0)
            {
                maxSize = MaxSize * 1024;
            }

            if (FileUpload1.HasFile)
            {
                if (FileUpload1.FileContent.Length > maxSize)
                {
                    ((CustomValidator)source).ErrorMessage = "The maximum filesize for the file '" + Name + "' is " + maxSize / 1024 + "Kb";
                    args.IsValid = false;
                }
            }
        }
    }

}