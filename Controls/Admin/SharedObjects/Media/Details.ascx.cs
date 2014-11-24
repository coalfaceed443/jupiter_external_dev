using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Utils;
using CRM.Code.Utils.Ordering;
using CRM.Code.Models;
using CodeCarvings.Piczard;
using System.IO;
using CodeCarvings.Piczard.Filters.Watermarks;
using CRM.Code.Managers;
using CRM.Code;

namespace CRM.Controls.Admin.SharedObjects.Media
{
    public partial class Details : System.Web.UI.UserControl
    {
        public CRM.Code.Models.Media CurrentMedia;
        public string Reference { get; set; }
        protected MainDataContext db;
        protected int ParentID = 0;
        public int[] MediaDimensions { get; set; }
        public string BackURL { get; set; }
        public bool ShowCategories { get; set; }
        public bool HideDescription { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            db = MainDataContext.CurrentContext;
            // load data //

            int entityID = 0, parentID = 0;

            Int32.TryParse(Request.QueryString["mid"], out entityID);

            CurrentMedia = db.Medias.SingleOrDefault(p => p.ID == entityID);

            if (Reference == "")
                NoticeManager.SetMessage("Unable to locate Media Owner Item", "/admin");
            else
                ParentID = parentID;

            // buttons //

            btnSubmit.EventHandler = btnSubmit_Click;
            btnSubmitChanges.EventHandler = btnSubmitChanges_Click;
            btnDelete.EventHandler = btnDelete_Click;

            // confirmations //
            confirmationDelete.StandardDeleteHidden("media", btnRealDelete_Click);

            // process //

            if (!Page.IsPostBack)
            {
                imgUpload.CropConstraint = new FixedCropConstraint(MediaDimensions[0], MediaDimensions[1]);
                imgUpload.PreviewResizeConstraint = new FixedResizeConstraint(MediaDimensions[0] / 2, MediaDimensions[1] / 2);

                txtShortDescription.Visible = !HideDescription;

                if (CurrentMedia != null)
                {
                    pnlAdd.Visible = false;
                    pnlEdit.Visible = true;

                    PopulateFields();
                }
                else
                {
                    pnlAdd.Visible = true;
                    pnlEdit.Visible = false;
                }
            }

        }

        private void PopulateFields()
        {
            txtName.Text = CurrentMedia.Name;
            chkIsActive.Checked = CurrentMedia.IsActive;
            imgUpload.LoadImageFromFileSystem(MapPath(CurrentMedia.ImageURL));
            txtShortDescription.Text = CurrentMedia.Description;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(true);

                NoticeManager.SetMessage("Media Added", BackURL);
            }
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveRecord(false);

                NoticeManager.SetMessage("Media Updated");
            }
        }

        protected void SaveRecord(bool newRecord)
        {
            // new record / exiting record //

            var medias = db.Medias;

            if (newRecord)
            {
                CurrentMedia = new CRM.Code.Models.Media();
                CurrentMedia.Reference = Reference;
                CurrentMedia.OrderNo = Ordering.GetNextOrderID(medias);

                db.Medias.InsertOnSubmit(CurrentMedia);
            }

            // common //

            CurrentMedia.Name = txtName.Text;
            CurrentMedia.IsActive = chkIsActive.Checked;
            CurrentMedia.Description = txtShortDescription.Text;


            db.SubmitChanges();

            if (imgUpload.HasNewImage)
            {
                CRM.Code.Utils.Files.Files.CheckDirectory(Code.Models.Media.folderPath);

                imgUpload.SaveProcessedImageToFileSystem(MapPath(CurrentMedia.ImageURL));
                new FixedCropConstraint(CRM.Code.Models.Media.DefaultDimensions[0], CRM.Code.Models.Media.DefaultDimensions[1]).SaveProcessedImageToFileSystem(CurrentMedia.ImageURL, CurrentMedia.ImageOriginalURL);

                ScaledResizeConstraint resizeFilter = new ScaledResizeConstraint(1024, 768);
                resizeFilter.SaveProcessedImageToFileSystem(CurrentMedia.ImageOriginalURL, CurrentMedia.ImageOriginalURL);

                try
                {
                    File.Copy(imgUpload.TemporarySourceImageFilePath, MapPath(CurrentMedia.ImageOriginalURL), true);
                }
                catch
                {
                    imgUpload.SaveProcessedImageToFileSystem(MapPath(CurrentMedia.ImageOriginalURL));
                }

                new FreeCropConstraint(GfxUnit.Pixel, null, 800, null, null).SaveProcessedImageToFileSystem(CurrentMedia.ImageOriginalURL, CurrentMedia.ImageOriginalURL);

                new FixedCropConstraint(CRM.Code.Models.Media.Thumbs[0], CRM.Code.Models.Media.Thumbs[1]).SaveProcessedImageToFileSystem(CurrentMedia.ImageURL, CurrentMedia.ImageThumbURL);

                imgUpload.ClearTemporaryFiles();
            }
        }

  
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            confirmationDelete.ShowPopup();
        }

        protected void btnRealDelete_Click(object sender, EventArgs e)
        {
            string backList = BackURL;
            db.Medias.DeleteOnSubmit(CurrentMedia);
            db.SubmitChanges();

            NoticeManager.SetMessage("Media Deleted", backList);

        }
    }
}