﻿/*
 * Piczard Examples | ExampleSet -A- C#
 * Copyright 2011 Sergio Turolla - All Rights Reserved.
 * Author: Sergio Turolla
 * <codecarvings.com>
 *  
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF 
 * ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
 * TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT 
 * SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
 * ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN 
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
 * OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Xml;
using System.ComponentModel;

using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Serialization;
using CodeCarvings.Piczard.Web;
using CodeCarvings.Piczard.Web.Helpers;

namespace CRM.Controls.Admin.ImageUpload
{
    /// <summary>
    /// A ready-to-use ASCX control that provides advanced image uploading features.</summary>
    public partial class SimpleImageUpload
        : System.Web.UI.UserControl, IPostBackDataHandler
    {

        #region Consts

        protected static readonly Size DefaultImageEditPopupSize = new Size(800, 520);
        protected static readonly Size DefaultButtonSize = new Size(110, 26);
        protected const bool PerformTemporaryFolderWriteTestOnPageLoad = true;

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PerformTemporaryFolderWriteTestOnPageLoad)
            {
                // Check if the application can write on the temporary folder
                this.TemporaryFolderWriteTest();
            }

            // Load the JS file
            Type t = this.Page.GetType();
            string scriptKey = "SimpleImageUpload.js";
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered(t, scriptKey))
            {
                this.Page.ClientScript.RegisterClientScriptInclude(t, scriptKey, this.ResolveUrl("SimpleImageUpload.js"));
            }

            this.cusRequired.ErrorMessage = Name + " is required";
        }

        #endregion

        #region Overrides

        #region ControlState

        protected override object SaveControlState()
        {
            List<object> values = new List<object>();
            values.Add(base.SaveControlState());

            values.Add(this.TemporaryFileId);
            values.Add(this._OutputResolution);
            values.Add(JSONSerializer.SerializeToString(this._CropConstraint));
            values.Add(JSONSerializer.SerializeToString(this._PreviewResizeConstraint));
            values.Add(this._ImageUploaded);
            values.Add(this._ImageEdited);
            values.Add(this._SourceImageClientFileName);
            values.Add(this._Configurations);

            values.Add(this._DebugUploadProblems);

            return values.ToArray();
        }

        protected override void LoadControlState(object savedState)
        {
            if ((savedState != null) && (savedState is object[]))
            {
                object[] values = (object[])savedState;
                int i = 0;
                base.LoadControlState(values[i++]);

                this._TemporaryFileId = (string)values[i++];
                this._OutputResolution = (float)values[i++];
                this._CropConstraint = CropConstraint.FromJSON((string)values[i++]);
                this._PreviewResizeConstraint = ResizeConstraint.FromJSON((string)values[i++]);
                this._ImageUploaded = (bool)values[i++];
                this._ImageEdited = (bool)values[i++];
                this._SourceImageClientFileName = (string)values[i++];
                this._Configurations = (string[])values[i++];

                this._DebugUploadProblems = (bool)values[i++];
            }
        }

        #endregion

        #region Viewstate

        protected override object SaveViewState()
        {
            List<object> values = new List<object>();
            values.Add(base.SaveViewState());

            values.Add(this._Width);
            values.Add(this._AutoOpenImageEditPopupAfterUpload);
            values.Add(this._ImageEditPopupSize);
            values.Add(this._ButtonSize);

            values.Add(this._EnableEdit);
            values.Add(this._EnableRemove);
            values.Add(this._EnableUpload);
            values.Add(this._EnableCancelUpload);

            values.Add(this._Text_EditButton);
            values.Add(this._Text_RemoveButton);
            values.Add(this._Text_BrowseButton);
            values.Add(this._Text_CancelUploadButton);
            values.Add(this._Text_ConfigurationLabel);
            values.Add(this._StatusMessage_NoImageSelected);
            values.Add(this._StatusMessage_UploadError);
            values.Add(this._StatusMessage_InvalidImage);
            values.Add(this._StatusMessage_Wait);

            return values.ToArray();
        }

        protected override void LoadViewState(object savedState)
        {
            if ((savedState != null) && (savedState is object[]))
            {
                object[] values = (object[])savedState;
                int i = 0;
                base.LoadViewState(values[i++]);

                this._Width = (Unit)values[i++];
                this._AutoOpenImageEditPopupAfterUpload = (bool)values[i++];
                this._ImageEditPopupSize = (Size)values[i++];
                this._ButtonSize = (Size)values[i++];

                this._EnableEdit = (bool)values[i++];
                this._EnableRemove = (bool)values[i++];
                this._EnableUpload = (bool)values[i++];
                this._EnableCancelUpload = (bool)values[i++];

                this._Text_EditButton = (string)values[i++];
                this._Text_RemoveButton = (string)values[i++];
                this._Text_BrowseButton = (string)values[i++];
                this._Text_CancelUploadButton = (string)values[i++];
                this._Text_ConfigurationLabel = (string)values[i++];
                this._StatusMessage_NoImageSelected = (string)values[i++];
                this._StatusMessage_UploadError = (string)values[i++];
                this._StatusMessage_InvalidImage = (string)values[i++];
                this._StatusMessage_Wait = (string)values[i++];
            }
        }

        #endregion

        #region Render

        protected override void OnInit(EventArgs e)
        {
            this.Page.RegisterRequiresControlState(this);
            this.Page.RegisterRequiresPostBack(this);

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            #region Dynamic load CSS and JS files

            string crlf = ""; // "\r\n";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type=\"text/javascript\">\r\n");
            //Google Chrome & Safari Ajax Bug
            bool isInAjaxPostBack = AjaxHelper.IsInAjaxPostBack(this.Page);
            if (!isInAjaxPostBack)
            {
                sb.Append("//<![CDATA[\r\n");
            }

            // JS function executed after the JS library is loaded
            sb.Append("function " + this.InitFunctionName2 + "()" + crlf);
            sb.Append("{" + crlf);
            sb.Append("var loadData={" + crlf);
            sb.Append("popupPictureTrimmerClientId:\"" + JSHelper.EncodeString(this.popupPictureTrimmer1.ClientID) + "\"" + crlf);
            sb.Append(",btnEditClientId:\"" + JSHelper.EncodeString(this.btnEdit.ClientID) + "\"" + crlf);
            sb.Append(",btnRemoveClientId:\"" + JSHelper.EncodeString(this.btnRemove.ClientID) + "\"" + crlf);
            sb.Append(",btnBrowseDisabledClientId:\"" + JSHelper.EncodeString(this.btnBrowseDisabled.ClientID) + "\"" + crlf);
            sb.Append(",btnCancelUploadClientId:\"" + JSHelper.EncodeString(this.btnCancelUpload.ClientID) + "\"" + crlf);
            sb.Append(",hfActClientId:\"" + JSHelper.EncodeString(this.hfAct.ClientID) + "\"" + crlf);
            sb.Append(",ddlConfigurationsClientId:\"" + JSHelper.EncodeString(this.ddlConfigurations.ClientID) + "\"" + crlf);

            sb.Append(",uploadUrl:\"" + JSHelper.EncodeString(this.UploadUrl) + "\"" + crlf);
            sb.Append(",uploadMonitorUrl:\"" + JSHelper.EncodeString(this.UploadMonitorUrl) + "\"" + crlf);
            sb.Append(",btnPostBack_PostBackEventReference:\"" + JSHelper.EncodeString(this.Page.ClientScript.GetPostBackEventReference(this.btnPostBack, "")) + "\"" + crlf);
            sb.Append(",imageEditPopupSize_width:" + this.ImageEditPopupSize.Width.ToString(System.Globalization.CultureInfo.InvariantCulture) + crlf);
            sb.Append(",imageEditPopupSize_height:" + this.ImageEditPopupSize.Height.ToString(System.Globalization.CultureInfo.InvariantCulture) + crlf);
            sb.Append(",autoOpenImageEditPopup:" + JSHelper.EncodeBool(this._AutoOpenImageEditPopup) + crlf);
            sb.Append(",buttonSize_width:" + this.ButtonSize.Width.ToString(System.Globalization.CultureInfo.InvariantCulture) + crlf);
            sb.Append(",buttonSize_height:" + this.ButtonSize.Height.ToString(System.Globalization.CultureInfo.InvariantCulture) + crlf);
            sb.Append(",enableCancelUpload:" + JSHelper.EncodeBool(this._EnableCancelUpload) + crlf);
            sb.Append(",dup:" + JSHelper.EncodeBool(this._DebugUploadProblems) + crlf);
            sb.Append(",statusMessage_Wait:\"" + JSHelper.EncodeString(this.StatusMessage_Wait) + "\"" + crlf);
            sb.Append("};" + crlf);
            sb.Append("var control = CodeCarvings.Wcs.Piczard.Upload.SimpleImageUpload.loadControl(\"" + JSHelper.EncodeString(this.ClientID) + "\", loadData);");
            sb.Append("}" + crlf);

            // Dynamic load JS / CSS (required for Ajax)
            sb.Append("function " + this.InitFunctionName + "()" + crlf);
            sb.Append("{" + crlf);
            sb.Append("if (typeof(window.__ccpz_siu_lt) === \"undefined\")" + crlf);
            sb.Append("{" + crlf);
            // The variable (window.__ccpz_siu_lt) (configured in SimpleImageUpload.js) is undefined...
            sb.Append(JSHelper.GetLoadScript(this.ResolveUrl("SimpleImageUpload.js"), this.InitFunctionName + "_load_js", this.InitFunctionName2 + "();") + crlf);
            sb.Append("}" + crlf);
            sb.Append("else" + crlf);
            sb.Append("{" + crlf);
            sb.Append(this.InitFunctionName2 + "();" + crlf);
            sb.Append("}" + crlf);
            sb.Append("}" + crlf);

            if (!isInAjaxPostBack)
            {
                sb.Append("\r\n//]]>\r\n");
            }
            sb.Append("</script>");

            string scriptToRegister = sb.ToString();
            if (!AjaxHelper.RegisterClientScriptBlockInAjaxPostBack(this.Page, "CCPZ_SIU_DAI_" + this.ClientID, scriptToRegister, false))
            {
                this.litScript.Text = scriptToRegister;
            }
            else
            {
                this.litScript.Text = "";
            }

            // Setup the initialization function
            if (this.Visible)
            {
                this.popupPictureTrimmer1.OnClientControlLoadFunction = this.InitFunctionName;
            }
            else
            {
                this.popupPictureTrimmer1.OnClientControlLoadFunction = "";
            }

            #endregion

            // Update the layout

            this.btnEdit.Enabled = this.HasImage;
            this.btnRemove.Enabled = this.HasImage;

            this.btnEdit.Visible = this.EnableEdit;
            this.btnEdit.OnClientClick = "CodeCarvings.Wcs.Piczard.Upload.SimpleImageUpload.openImageEditPopup(\"" + JSHelper.EncodeString(this.ClientID) + "\"); return false;";
            this.hlPictureImageEdit.Attributes["onclick"] = this.btnEdit.OnClientClick;
            this.btnRemove.Visible = this.EnableRemove;
            this.btnRemove.OnClientClick = "CodeCarvings.Wcs.Piczard.Upload.SimpleImageUpload.removeImage(\"" + JSHelper.EncodeString(this.ClientID) + "\"); return false;";

            this.btnEdit.Width = this.ButtonSize.Width;
            this.btnEdit.Height = this.ButtonSize.Height;

            this.btnRemove.Width = this.ButtonSize.Width;
            this.btnRemove.Height = this.ButtonSize.Height;

            this.btnBrowse.Width = this.btnBrowseDisabled.Width = this.ButtonSize.Width;
            this.btnBrowse.Height = this.btnBrowseDisabled.Height = this.ButtonSize.Height;

            this.btnCancelUpload.OnClientClick = "CodeCarvings.Wcs.Piczard.Upload.SimpleImageUpload.cancelUpload(\"" + JSHelper.EncodeString(this.ClientID) + "\"); return false;";

            this.btnCancelUpload.Width = this.ButtonSize.Width;
            this.btnCancelUpload.Height = this.ButtonSize.Height;

            this.phEditCommands.Visible = this.EnableEdit || this.EnableRemove;

            this.phUploadCommands.Visible = this.EnableUpload;
            this.btnCancelUpload.Visible = this.EnableCancelUpload;

            // Update the texts
            this.litStatusMessage.Text = this.CurrentStatusMessage;

            this.btnEdit.Text = this.Text_EditButton;
            this.btnRemove.Text = this.Text_RemoveButton;
            this.btnBrowse.Text = this.Text_BrowseButton;
            this.btnBrowseDisabled.Text = this.Text_BrowseButton;
            this.btnCancelUpload.Text = this.Text_CancelUploadButton;

            if (this.HasImage)
            {
                if (!File.Exists(this.PreviewImageFilePath))
                {
                    // The preview file does not exists -> create it
                    this._UpdatePreview = true;
                }

                if (this._UpdatePreview)
                {
                    // Get the processing job (Default resolution = 96DPI)
                    ImageProcessingJob job = this.GetImageProcessingJob();
                    job.OutputResolution = CommonData.DefaultResolution;

                    // Add the resize constraint
                    if (this.PreviewResizeConstraint != null)
                    {
                        job.Filters.Add(this.PreviewResizeConstraint);
                    }

                    if (File.Exists(TemporarySourceImageFilePath))
                    {
                        // Save the preview image
                        job.SaveProcessedImageToFileSystem(this.TemporarySourceImageFilePath, this.PreviewImageFilePath, new JpegFormatEncoderParams());
                    }

                }
                // Force the reload of the preivew
                this.imgPreview.ImageUrl = this.PreviewImageUrl;
            }

            if (this._CropConstraint != null)
            {
                // Crop Enabled
                this.popupPictureTrimmer1.ShowZoomPanel = true;
            }
            else
            {
                // Crop disabled
                this.popupPictureTrimmer1.ShowZoomPanel = false;
            }

            // Update the configuration UI
            this.litSelectConfiguration.Text = this.Text_ConfigurationLabel;
            this.ddlConfigurations.Items.Clear();
            string[] configurations = this.Configurations;
            if (configurations != null)
            {
                for (int i = 0; i < configurations.Length; i++)
                {
                    this.ddlConfigurations.Items.Add(new ListItem(configurations[i], i.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                }
                this.ddlConfigurations.SelectedIndex = this.SelectedConfigurationIndex.Value;
            }
            this.ddlConfigurations.Attributes["onchange"] = "CodeCarvings.Wcs.Piczard.Upload.SimpleImageUpload.onConfigurationChange(\"" + JSHelper.EncodeString(this.ClientID) + "\");";

            base.OnPreRender(e);
        }

        #endregion

        #endregion

        #region IPostBackDataHandler Members

        bool IPostBackDataHandler.LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            this._LastAct = this.hfAct.Value;
            // Reset the action
            this.hfAct.Value = "";

            string selectedConfigurationIndex = postCollection[this.ddlConfigurations.UniqueID];
            if (!string.IsNullOrEmpty(selectedConfigurationIndex))
            {
                // Int value
                this._SelectedConfigurationIndex = int.Parse(selectedConfigurationIndex, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                // Null value
                this._SelectedConfigurationIndex = null;
            }

            switch (this._LastAct)
            {
                case "upload":
                    // A new file has been uploaded
                    if (File.Exists(this.UploadMonitorFilePath))
                    {
                        XmlDocument doc = new XmlDocument();
                        using (StreamReader reader = File.OpenText(this.UploadMonitorFilePath))
                        {
                            // Load the document
                            doc.Load(reader);
                        }

                        XmlNodeList nodes = doc.GetElementsByTagName("uploadMonitor");
                        if (nodes.Count > 0)
                        {
                            XmlNode uploadMonitorNode = nodes[0];

                            XmlAttribute stateAttribute = uploadMonitorNode.Attributes["state"];
                            if (stateAttribute != null)
                            {
                                if (!string.IsNullOrEmpty(stateAttribute.Value))
                                {
                                    this._UploadMonitorStatus = int.Parse(stateAttribute.Value);

                                    if (this._UploadMonitorStatus.Value == 2)
                                    {
                                        // Upload success
                                        // Get the file name

                                        this._SourceImageClientFileName = uploadMonitorNode.FirstChild.Value;
                                    }

                                    // Image upload (success / error)
                                    return true;
                                }
                            }
                        }
                    }
                    break;
                case "edit":
                    // Image edit
                    return true;
                case "remove":
                    // Image remove
                    return true;
                case "configuration":
                    // Selected confiugration index changed
                    return true;
            }

            // No event
            return false;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            // Process the control events

            switch (this._LastAct)
            {
                case "upload":
                    // Image upload (success / error)
                    if (this._UploadMonitorStatus.HasValue)
                    {
                        switch (this._UploadMonitorStatus.Value)
                        {
                            case 2:
                                // Upload success
                                this.ProcessUploadSuccess();
                                break;
                            case 3:
                                // Upload error
                                this.ProcessUploadError();
                                break;
                        }
                    }
                    break;
                case "edit":
                    // Image edit
                    this.ProcessEdit();
                    break;
                case "remove":
                    // Image remove
                    this.ProcessRemove();
                    break;
                case "configuration":
                    // Selected confiugration index changed
                    this.ProcessSelectedConfigurationIndexChanged();
                    break;
            }
        }

        #endregion

        #region Properties

        #region Settings

        #region Misc

        protected Unit _Width = Unit.Empty;
        /// <summary>
        /// Gets or sets the control width.</summary>
        public Unit Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                this._Width = value;
            }
        }

        protected float _OutputResolution = CommonData.DefaultResolution;
        /// <summary>
        /// Gets or sets the output resolution (DPI - default value = 96).</summary>
        public float OutputResolution
        {
            get
            {
                return this._OutputResolution;
            }
            set
            {
                if (this.HasImage)
                {
                    throw new Exception("Cannot change the OutputResolution after an image has been loaded.");
                }

                // Validate the new resolution
                CodeCarvings.Piczard.Helpers.ImageHelper.ValidateResolution(value, true);

                this._OutputResolution = value;
            }
        }

        protected CropConstraint _CropConstraint = null;
        /// <summary>
        /// Gets or sets the crop constraint.</summary>
        public CropConstraint CropConstraint
        {
            get
            {
                return this._CropConstraint;
            }
            set
            {
                if (this.HasImage)
                {
                    throw new Exception("Cannot change the CropConstraint after an image has been loaded.");
                }
                this._CropConstraint = value;
            }
        }

        protected ResizeConstraint _PreviewResizeConstraint = null;
        /// <summary>
        /// Gets or sets the preview resize constraint.</summary>
        public ResizeConstraint PreviewResizeConstraint
        {
            get
            {
                if (_PreviewResizeConstraint == null)
                    _PreviewResizeConstraint = new FixedResizeConstraint(CRM.Code.Constants.DetailsPreviewDimensions[0], CRM.Code.Constants.DetailsPreviewDimensions[1]);

                return this._PreviewResizeConstraint;
            }
            set
            {
                if (this.HasImage)
                {
                    throw new Exception("Cannot change the PreviewResizeConstraint after an image has been loaded.");
                }

                this._PreviewResizeConstraint = value;
            }
        }

        protected Size _ImageEditPopupSize = DefaultImageEditPopupSize;
        /// <summary>
        /// Gets or sets the image edit popup size.</summary>
        public Size ImageEditPopupSize
        {
            get
            {
                return this._ImageEditPopupSize;
            }
            set
            {
                this._ImageEditPopupSize = value;
            }
        }

        protected bool _AutoOpenImageEditPopupAfterUpload = false;
        /// <summary>
        /// Gets or sets a value indicating whether to automatically open the image edit popup after the upload process.</summary>
        public bool AutoOpenImageEditPopupAfterUpload
        {
            get
            {
                return this._AutoOpenImageEditPopupAfterUpload;
            }
            set
            {
                this._AutoOpenImageEditPopupAfterUpload = value;
            }
        }

        protected Size _ButtonSize = DefaultButtonSize;
        /// <summary>
        /// Gets or sets the size of the buttons.</summary>
        public Size ButtonSize
        {
            get
            {
                return this._ButtonSize;
            }
            set
            {
                this._ButtonSize = value;
            }
        }

        protected string[] _Configurations = null;
        /// <summary>
        /// Gets or sets the available configuration names.</summary>
        public string[] Configurations
        {
            get
            {
                return this._Configurations;
            }
            set
            {
                this._Configurations = value;
            }
        }

        protected int? _SelectedConfigurationIndex = null;
        /// <summary>
        /// Gets or sets the index of the selected configuration.</summary>
        public int? SelectedConfigurationIndex
        {
            get
            {
                if (this._Configurations == null)
                {
                    // No configuration available
                    return null;
                }
                if (this._Configurations.Length == 0)
                {
                    // No configuration available
                    return null;
                }

                if (!this._SelectedConfigurationIndex.HasValue)
                {
                    // First configuration selected by defaul
                    return 0;
                }
                else
                {
                    if (this._SelectedConfigurationIndex.Value >= this._Configurations.Length)
                    {
                        // Use the last configuration available
                        return this._Configurations.Length - 1;
                    }
                }

                return this._SelectedConfigurationIndex;
            }
            set
            {
                if (value.HasValue)
                {
                    if (this._Configurations == null)
                    {
                        // No configuration available
                        throw new Exception("Cannot set the SelectedConfigurationIndex because no configuration has been set yet.");
                    }
                    if (this._Configurations.Length == 0)
                    {
                        // No configuration available
                        throw new Exception("Cannot set the SelectedConfigurationIndex because there is no configuration set.");
                    }

                    if (value.Value < 0)
                    {
                        throw new Exception("SelectedConfigurationIndex cannot be < 0.");
                    }
                    if (value.Value >= this._Configurations.Length)
                    {
                        throw new Exception("SelectedConfigurationIndex must be < Configurations.length.");
                    }
                }

                this._SelectedConfigurationIndex = value;
            }
        }

        protected bool _DebugUploadProblems = false;
        /// <summary>
        /// Gets or sets a value indicating whether to show details when an upload error occurs.</summary>
        public bool DebugUploadProblems
        {
            get
            {
                return this._DebugUploadProblems;
            }
            set
            {
                this._DebugUploadProblems = value;
            }
        }

        #endregion

        #region Globalization

        #region UI elements

        protected string _Text_EditButton = "Edit...";
        /// <summary>
        /// Gets or sets the text of the "Edit" button.</summary>
        public string Text_EditButton
        {
            get
            {
                return this._Text_EditButton;
            }
            set
            {
                this._Text_EditButton = value;
            }
        }

        protected string _Text_RemoveButton = "Remove";
        /// <summary>
        /// Gets or sets the text of the "Remove" button.</summary>
        public string Text_RemoveButton
        {
            get
            {
                return this._Text_RemoveButton;
            }
            set
            {
                this._Text_RemoveButton = value;
            }
        }

        protected string _Text_BrowseButton = "Browse...";
        /// <summary>
        /// Gets or sets the text of the "Browse" button.</summary>
        public string Text_BrowseButton
        {
            get
            {
                return this._Text_BrowseButton;
            }
            set
            {
                this._Text_BrowseButton = value;
            }
        }

        protected string _Text_CancelUploadButton = "Cancel upload";
        /// <summary>
        /// Gets or sets the text of the "Cancel Upload" button.</summary>
        public string Text_CancelUploadButton
        {
            get
            {
                return this._Text_CancelUploadButton;
            }
            set
            {
                this._Text_CancelUploadButton = value;
            }
        }

        protected string _Text_ConfigurationLabel = "Configuration:";
        /// <summary>
        /// Gets or sets the text of the "Configuration" label.</summary>
        public string Text_ConfigurationLabel
        {
            get
            {
                return this._Text_ConfigurationLabel;
            }
            set
            {
                this._Text_ConfigurationLabel = value;
            }
        }

        #endregion

        #region Status messages

        protected string _CurrentStatusMessage = null;
        protected string CurrentStatusMessage
        {
            get
            {
                if (this._CurrentStatusMessage != null)
                {
                    // Last status set
                    return this._CurrentStatusMessage;
                }

                // By default return the "No image selected" text.
                return this.StatusMessage_NoImageSelected;
            }
            set
            {
                this._CurrentStatusMessage = value;
            }
        }
        /// <summary>
        /// Sets the current satus message.</summary>
        /// <param name="text">The message to display.</param>
        public void SetCurrentStatusMessage(string text)
        {
            this.CurrentStatusMessage = text;
        }

        protected string _StatusMessage_NoImageSelected = "No image selected.";
        /// <summary>
        /// Gets or sets the text displayed when no image has been selected.</summary>
        public string StatusMessage_NoImageSelected
        {
            get
            {
                return this._StatusMessage_NoImageSelected;
            }
            set
            {
                this._StatusMessage_NoImageSelected = value;
            }
        }

        protected string _StatusMessage_UploadError = "<span style=\"color:#cc0000;\">A server error has occurred during the upload process.<br/>Please ensure that the file is smaller than {0} KBytes.</span>";
        /// <summary>
        /// Gets or sets the text displayed when a (generic) upload error has occurred.</summary>
        public string StatusMessage_UploadError
        {
            get
            {
                return string.Format(this._StatusMessage_UploadError, this.MaxRequestLength);
            }
            set
            {
                this._StatusMessage_UploadError = value;
            }
        }

        protected string _StatusMessage_InvalidImage = "<span style=\"color:#cc0000;\">The uploaded file is not a valid image.</span>";
        /// <summary>
        /// Gets or sets the text displayed when the uploaded image file is invalid.</summary>
        public string StatusMessage_InvalidImage
        {
            get
            {
                return this._StatusMessage_InvalidImage;
            }
            set
            {
                this._StatusMessage_InvalidImage = value;
            }
        }

        protected string _StatusMessage_Wait = "<span style=\"color:#aaaaaa;\">Please wait...</span>";
        /// <summary>
        /// Gets or sets the text displayed when the user has to wait (e.g. a postback has been delayed).</summary>
        public string StatusMessage_Wait
        {
            get
            {
                return this._StatusMessage_Wait;
            }
            set
            {
                this._StatusMessage_Wait = value;
            }
        }

        #endregion

        #endregion

        #region Features

        protected bool _EnableEdit = true;
        /// <summary>
        /// Gets or sets a value indicating whether it's possible to edit the image.</summary>
        public bool EnableEdit
        {
            get
            {
                return this._EnableEdit;
            }
            set
            {
                this._EnableEdit = value;
            }
        }

        protected bool _EnableRemove = false;
        /// <summary>
        /// Gets or sets a value indicating whether it's possible to remove the image.</summary>
        public bool EnableRemove
        {
            get
            {
                return this._EnableRemove;
            }
            set
            {
                this._EnableRemove = value;
            }
        }

        protected bool _EnableUpload = true;
        /// <summary>
        /// Gets or sets a value indicating whether it's possible to upload an image.</summary>
        public bool EnableUpload
        {
            get
            {
                return this._EnableUpload;
            }
            set
            {
                this._EnableUpload = value;
            }
        }

        protected bool _EnableCancelUpload = true;
        /// <summary>
        /// Gets or sets a value indicating whether it's possible to cancel an upload in progess.</summary>
        public bool EnableCancelUpload
        {
            get
            {
                return this._EnableCancelUpload;
            }
            set
            {
                this._EnableCancelUpload = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the image is required to be uploaded.</summary>
        public bool Required
        {
            get
            {
                return this.cusRequired.Enabled;
            }
            set
            {
                this.cusRequired.Enabled = value;
            }
        }

        protected string _Name = "";
        /// <summary>
        /// Gets or sets a value indicating whether the image is required to be uploaded.</summary>
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
            }
        }
        #endregion

        #endregion

        #region Current state

        protected string _TemporaryFileId = null;
        /// <summary>
        /// Gets or sets the current temporary file id.</summary>
        protected string TemporaryFileId
        {
            get
            {
                if (this._TemporaryFileId == null)
                {
                    // Get e new temporary file id
                    this._TemporaryFileId = TemporaryFileManager.GetNewTemporaryFileId();
                }

                return this._TemporaryFileId;
            }
            set
            {
                // Validate the value
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Invalid TemporaryFileId (null).");
                }
                if (!TemporaryFileManager.ValidateTemporaryFileId(value, false))
                {
                    throw new Exception("Invalid TemporaryFileId.");
                }

                this._TemporaryFileId = value;
            }
        }

        protected string _RenderId = null;
        protected string RenderId
        {
            get
            {
                if (this._RenderId == null)
                {
                    this._RenderId = Guid.NewGuid().ToString("N");
                }
                return this._RenderId;
            }
        }

        protected string InitFunctionName
        {
            get
            {
                return "CCPZ_SIU_" + this.RenderId + "_Init";
            }
        }

        protected string InitFunctionName2
        {
            get
            {
                return "CCPZ_SIU_" + this.RenderId + "_Init2";
            }
        }

        // If true, the control must regenerate the preview image (in the prerender method)
        protected bool _UpdatePreview = false;
        protected string _LastAct = "";
        protected int? _UploadMonitorStatus = null;
        protected bool _AutoOpenImageEditPopup = false;

        /// <summary>
        /// Gets a value indicating whether the control has an image.</summary>
        public bool HasImage
        {
            get
            {
                return this.popupPictureTrimmer1.ImageLoaded;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control has an image uploaded or edited by the user.</summary>
        public bool HasNewImage
        {
            get
            {
                if (this.HasImage)
                {
                    if (this.ImageUploaded)
                    {
                        return true;
                    }

                    if (this.ImageEdited)
                    {
                        return true;
                    }
                }

                // Image not loaded or already persisted
                return false;
            }
        }

        protected bool _ImageUploaded = false;
        /// <summary>
        /// Gets a value indicating whether the control has an image uploaded by the user.</summary>
        public bool ImageUploaded
        {
            get
            {
                if (!this.HasImage)
                {
                    return false;
                }

                return this._ImageUploaded;
            }
        }

        protected bool _ImageEdited = false;
        /// <summary>
        /// Gets a value indicating whether the control has an image edited by the user.</summary>
        public bool ImageEdited
        {
            get
            {
                if (!this.HasImage)
                {
                    return false;
                }

                return this._ImageEdited;
            }
        }

        protected string _SourceImageClientFileName = null;
        /// <summary>
        /// Gets or sets the original image file name (before upload).</summary>
        public string SourceImageClientFileName
        {
            get
            {
                return this._SourceImageClientFileName;
            }
            set
            {
                if (value != null)
                {
                    // Get the file name
                    value = Path.GetFileName(value);
                }
                this._SourceImageClientFileName = value;
            }
        }

        #endregion

        #region PictureTrimmer properties

        /// <summary>
        /// Gets the current <see cref="PictureTrimmerUserState"/> of the <see cref="PictureTrimmer"/> control.</summary>
        public PictureTrimmerUserState UserState
        {
            get
            {
                if (!this.HasImage)
                {
                    return null;
                }

                return this.popupPictureTrimmer1.UserState;
            }
        }

        /// <summary>
        /// Gets the current <see cref="PictureTrimmerValue"/> of the <see cref="PictureTrimmer"/> control.</summary>
        public PictureTrimmerValue Value
        {
            get
            {
                if (!this.HasImage)
                {
                    return null;
                }

                return this.popupPictureTrimmer1.Value;
            }
        }

        /// <summary>
        /// Gets or sets the current culture.</summary>
        public string Culture
        {
            get
            {
                return this.popupPictureTrimmer1.Culture;
            }
            set
            {
                this.popupPictureTrimmer1.Culture = value;
            }
        }

        /// <summary>
        /// Gets or sets the CanvasColor used by the <see cref="PictureTrimmer"/> control.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public BackgroundColor CanvasColor
        {
            get
            {
                return this.popupPictureTrimmer1.CanvasColor;
            }
            set
            {
                this.popupPictureTrimmer1.CanvasColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the ImageBackColor used by the <see cref="PictureTrimmer"/> control.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public BackgroundColor ImageBackColor
        {
            get
            {
                return this.popupPictureTrimmer1.ImageBackColor;
            }
            set
            {
                this.popupPictureTrimmer1.ImageBackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="PictureTrimmer.ImageBackColorApplyMode"/> property of the <see cref="PictureTrimmer"/> control.</summary>
        public PictureTrimmerImageBackColorApplyMode ImageBackColorApplyMode
        {
            get
            {
                return this.popupPictureTrimmer1.ImageBackColorApplyMode;
            }
            set
            {
                this.popupPictureTrimmer1.ImageBackColorApplyMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the UIUnit used by the <see cref="PictureTrimmer"/> control.</summary>
        public GfxUnit UIUnit
        {
            get
            {
                return this.popupPictureTrimmer1.UIUnit;
            }
            set
            {
                this.popupPictureTrimmer1.UIUnit = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="PictureTrimmer.CropShadowMode"/> property of the <see cref="PictureTrimmer"/> control.</summary>
        public PictureTrimmerCropShadowMode CropShadowMode
        {
            get
            {
                return this.popupPictureTrimmer1.CropShadowMode;
            }
            set
            {
                this.popupPictureTrimmer1.CropShadowMode = value;
            }
        }

        /// <summary>
        /// Gets the size (pixel) of the source image.</summary>
        public Size SourceImageSize
        {
            get
            {
                if (!this.HasImage)
                {
                    throw new Exception("Image not loaded.");
                }

                return this.popupPictureTrimmer1.SourceImageSize;
            }
        }

        #endregion

        #region File paths / urls

        /// <summary>
        /// Gets the path of the temporary file containing the source image.</summary>
        public string TemporarySourceImageFilePath
        {
            get
            {
                return TemporaryFileManager.GetTemporaryFilePath(this.TemporaryFileId, "_s.tmp");
            }
        }

        protected string UploadFilePath
        {
            get
            {
                return TemporaryFileManager.GetTemporaryFilePath(this.TemporaryFileId, "_u.tmp");
            }
        }

        protected string PreviewImageFilePath
        {
            get
            {
                return TemporaryFileManager.GetTemporaryFilePath(this.TemporaryFileId, "_p.jpg");
            }
        }

        protected string TemporaryWriteTestFilePath
        {
            get
            {
                return TemporaryFileManager.GetTemporaryFilePath(this.TemporaryFileId, "_wt.tmp");
            }
        }

        protected string UploadMonitorFilePath
        {
            get
            {
                return TemporaryFileManager.GetTemporaryFilePath(this.TemporaryFileId, "_um.xml");
            }
        }

        protected string PreviewImageUrl
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(this.ResolveUrl("preview.ashx"));
                sb.Append("?tfid=" + HttpUtility.UrlEncode(this.TemporaryFileId.ToString()));
                sb.Append("&k=" + HttpUtility.UrlEncode(this.GetQueryKey("")));
                sb.Append("&ts=" + HttpUtility.UrlEncode(DateTime.UtcNow.Ticks.ToString()));

                return sb.ToString();
            }
        }

        protected string UploadUrl
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(this.ResolveUrl("upload.aspx"));
                sb.Append("?tfid=" + HttpUtility.UrlEncode(this.TemporaryFileId.ToString()));
                string keyAdditionalData = "dup=" + (this._DebugUploadProblems ? "1" : "0");
                sb.Append("&" + keyAdditionalData);
                sb.Append("&k=" + HttpUtility.UrlEncode(this.GetQueryKey(keyAdditionalData)));
                sb.Append("&cid=" + HttpUtility.UrlEncode(this.ClientID));
                sb.Append("&bsw=" + HttpUtility.UrlEncode(this.ButtonSize.Width.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                sb.Append("&bsh=" + HttpUtility.UrlEncode(this.ButtonSize.Height.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                sb.Append("&ts=" + HttpUtility.UrlEncode(DateTime.UtcNow.Ticks.ToString()));

                return sb.ToString();
            }
        }

        protected string UploadMonitorUrl
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(this.ResolveUrl("uploadMonitor.ashx"));
                sb.Append("?tfid=" + HttpUtility.UrlEncode(this.TemporaryFileId.ToString()));
                sb.Append("&k=" + HttpUtility.UrlEncode(this.GetQueryKey("")));

                return sb.ToString();
            }
        }

        protected string MaxRequestLength
        {
            get
            {
                // Default value = 4 MB
                string result = "4096";

                try
                {
                    System.Web.Configuration.HttpRuntimeSection section = (System.Web.Configuration.HttpRuntimeSection)System.Configuration.ConfigurationManager.GetSection("system.web/httpRuntime");
                    if (section != null)
                    {
                        result = section.MaxRequestLength.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                catch
                {
                    // An error has occurred (Medium trust ?)
                    result = "?";
                }

                return result;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Base class that provides data for the ImageUpload and the SelectedConfigurationIndexChanged events.</summary>
        public class ConfigurationEventArgs
            : EventArgs
        {

            /// <summary>
            /// Intializes new instace of the <see cref="ConfigurationEventArgs"/> class.</summary>
            /// <param name="outputResolution">The resolution (DPI) of the image that is generated by the control.</param>
            /// <param name="cropConstraint">The constraints that have to be satisfied by the cropped image.</param>
            /// <param name="previewResizeConstraint">The size of the preview image.</param>
            public ConfigurationEventArgs(float outputResolution, CropConstraint cropConstraint, ResizeConstraint previewResizeConstraint)
            {
                this._OutputResolution = outputResolution;
                this._CropConstraint = cropConstraint;
                this._PreviewResizeConstraint = previewResizeConstraint;

                this._OriginalOutputResolution = this._OutputResolution;
                this._OriginalCropConstraintString = JSONSerializer.SerializeToString(this._CropConstraint, true);
                this._OriginalPreviewResizeConstraintString = JSONSerializer.SerializeToString(this._PreviewResizeConstraint, true);
            }

            private float _OutputResolution;
            /// <summary>
            /// Gets or sets the resolution (DPI) of the image that is generated by the control.</summary>        
            public float OutputResolution
            {
                get
                {
                    return this._OutputResolution;
                }
                set
                {
                    // Validate the resolution
                    CodeCarvings.Piczard.Helpers.ImageHelper.ValidateResolution(value, true);

                    this._OutputResolution = value;
                }
            }

            private CropConstraint _CropConstraint;
            /// <summary>
            /// Gets the constraints that have to be satisfied by the cropped image.</summary>        
            public CropConstraint CropConstraint
            {
                get
                {
                    return this._CropConstraint;
                }
                set
                {
                    this._CropConstraint = value;
                }
            }

            private ResizeConstraint _PreviewResizeConstraint;
            /// <summary>
            /// Gets or sets the preview resize constraint.</summary>
            public ResizeConstraint PreviewResizeConstraint
            {
                get
                {
                    return this._PreviewResizeConstraint;
                }
                set
                {
                    this._PreviewResizeConstraint = value;
                }
            }

            private float _OriginalOutputResolution;
            internal bool OutputResolutionChanged
            {
                get
                {
                    return this._OutputResolution != this._OriginalOutputResolution;
                }
            }

            private string _OriginalCropConstraintString;
            internal bool CropConstraintChanged
            {
                get
                {
                    return JSONSerializer.SerializeToString(this._CropConstraint, true) != this._OriginalCropConstraintString;
                }
            }

            private string _OriginalPreviewResizeConstraintString;
            internal bool PreviewResizeConstraintChanged
            {
                get
                {
                    return JSONSerializer.SerializeToString(this._PreviewResizeConstraint, true) != this._OriginalPreviewResizeConstraintString;
                }
            }
        }

        /// <summary>
        /// Provides data for the ImageUpload event.</summary>
        public class ImageUploadEventArgs
            : ConfigurationEventArgs
        {
            /// <summary>
            /// Intializes new instace of the <see cref="ImageUploadEventArgs"/> class.</summary>
            /// <param name="outputResolution">The resolution (DPI) of the image that is generated by the control.</param>
            /// <param name="cropConstraint">The constraints that have to be satisfied by the cropped image.</param>
            /// <param name="previewResizeConstraint">The size of the preview image.</param>
            public ImageUploadEventArgs(float outputResolution, CropConstraint cropConstraint, ResizeConstraint previewResizeConstraint)
                : base(outputResolution, cropConstraint, previewResizeConstraint)
            {
            }
        }
        /// <summary>
        /// Represents the method that handles the ImageUpload event.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The argument of type ImageUploadEventArgs that contains the event data.</param>
        public delegate void ImageUploadEventHhander(object sender, ImageUploadEventArgs args);

        /// <summary>
        /// Occurs when a new image is uploaded.</summary>
        public event ImageUploadEventHhander ImageUpload = null;
        protected void OnImageUpload(ImageUploadEventArgs e)
        {
            if (this.ImageUpload != null)
            {
                this.ImageUpload(this, e);
            }
        }

        /// <summary>
        /// Occurs when an upload process does not complete successfully.</summary>
        public event EventHandler UploadError = null;
        protected void OnUploadError(EventArgs e)
        {
            if (this.UploadError != null)
            {
                this.UploadError(this, e);
            }
        }

        /// <summary>
        /// Occurs when the image is edited.</summary>
        public event EventHandler ImageEdit = null;
        protected void OnImageEdit(EventArgs e)
        {
            if (this.ImageEdit != null)
            {
                this.ImageEdit(this, e);
            }
        }

        /// <summary>
        /// Occurs when the image is removed.</summary>
        public event EventHandler ImageRemove = null;
        protected void OnImageRemove(EventArgs e)
        {
            if (this.ImageRemove != null)
            {
                this.ImageRemove(this, e);
            }
        }

        /// <summary>
        /// Provides data for the SelectedConfigurationIndexChanged event.</summary>
        public class SelectedConfigurationIndexChangedEventArgs
            : ConfigurationEventArgs
        {
            /// <summary>
            /// Intializes new instace of the <see cref="SelectedConfigurationIndexChangedEventArgs"/> class.</summary>
            /// <param name="outputResolution">The resolution (DPI) of the image that is generated by the control.</param>
            /// <param name="cropConstraint">The constraints that have to be satisfied by the cropped image.</param>
            /// <param name="previewResizeConstraint">The size of the preview image.</param>
            public SelectedConfigurationIndexChangedEventArgs(float outputResolution, CropConstraint cropConstraint, ResizeConstraint previewResizeConstraint)
                : base(outputResolution, cropConstraint, previewResizeConstraint)
            {
            }
        }
        /// <summary>
        /// Represents the method that handles the SelectedConfigurationIndexChanged event.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The argument of type SelectedConfigurationIndexChangedEventArgs that contains the event data.</param>
        public delegate void SelectedConfigurationIndexChangedEventHhander(object sender, SelectedConfigurationIndexChangedEventArgs args);

        /// <summary>
        /// Occurs when a the ConfigurationIndex has been changed by the user.</summary>
        public event SelectedConfigurationIndexChangedEventHhander SelectedConfigurationIndexChanged = null;
        protected void OnSelectedConfigurationIndexChanged(SelectedConfigurationIndexChangedEventArgs e)
        {
            if (this.SelectedConfigurationIndexChanged != null)
            {
                this.SelectedConfigurationIndexChanged(this, e);
            }
        }

        #endregion

        #region Methods

        #region Load

        protected void LoadImageFromFileSystem_Internal(string sourceImageFilePath, PictureTrimmerValue value)
        {
            // Calculate the client file name
            using (LoadedImage image = ImageArchiver.LoadImage(sourceImageFilePath))
            {
                this.SourceImageClientFileName = "noname" + ImageArchiver.GetFormatEncoderParams(image.FormatId).FileExtension;

                if (CodeCarvings.Piczard.Configuration.WebSettings.PictureTrimmer.UseTemporaryFiles)
                {
                    // The picture trimmer can use temporary files -> Load the image now
                    // This generates a new temporary files, however saves CPU and RAM
                    this.popupPictureTrimmer1.LoadImage(image.Image, this._OutputResolution, this._CropConstraint);
                }
            }

            if (!CodeCarvings.Piczard.Configuration.WebSettings.PictureTrimmer.UseTemporaryFiles)
            {
                // The picture trimmer cannot use temporary files -> Load the image now
                this.popupPictureTrimmer1.LoadImageFromFileSystem(sourceImageFilePath, this._OutputResolution, this._CropConstraint);
            }

            if (value != null)
            {
                // Optional: Set the picture trimmer value
                this.popupPictureTrimmer1.Value = value;
            }

            // The new image has been loaded
            this._ImageUploaded = false;
            this._ImageEdited = false;

            // Update the preview
            this._UpdatePreview = true;
        }

        /// <summary>
        /// Loads an image stored in the file system and applies a specific <see cref="PictureTrimmerValue"/>.</summary>
        /// <param name="sourceImageFilePath">The path of the image to load.</param>
        /// <param name="value">The <see cref="PictureTrimmerValue"/> to apply.</param>
        public void LoadImageFromFileSystem(string sourceImageFilePath, PictureTrimmerValue value)
        {
            //If the file does not exist do not try to load it
            if (File.Exists(sourceImageFilePath))
            {
                // Copy the source image into the temporary folder
                // So there is no problem il the original source image is deleted (e.g. when a record is updated...)
                System.IO.File.Copy(sourceImageFilePath, this.TemporarySourceImageFilePath, true);

                // Load the image
                this.LoadImageFromFileSystem_Internal(this.TemporarySourceImageFilePath, value);

                // Use the original file name as source client file name
                this.SourceImageClientFileName = sourceImageFilePath;
            }
        }

        /// <summary>
        /// Loads an image stored in the file system and auto-calculates the <see cref="PictureTrimmerValue"/> to use.</summary>
        /// <param name="sourceImageFilePath">The path of the image to load.</param>
        public void LoadImageFromFileSystem(string sourceImageFilePath)
        {
            this.LoadImageFromFileSystem(sourceImageFilePath, null);
        }

        /// <summary>
        /// Loads an image from a <see cref="Stream"/> and applies a specific <see cref="PictureTrimmerValue"/>.</summary>
        /// <param name="sourceImageStream">The <see cref="Stream"/> containing the image to load.</param>
        /// <param name="value">The <see cref="PictureTrimmerValue"/> to apply.</param>
        public void LoadImageFromStream(Stream sourceImageStream, PictureTrimmerValue value)
        {
            // Save the stream
            using (Stream writer = File.Create(this.TemporarySourceImageFilePath))
            {
                if (sourceImageStream.Position != 0)
                {
                    sourceImageStream.Seek(0, SeekOrigin.Begin);
                }

                byte[] buffer = new byte[4096];
                int readBytes;
                while (true)
                {
                    readBytes = sourceImageStream.Read(buffer, 0, buffer.Length);
                    if (readBytes <= 0)
                    {
                        break;
                    }
                    writer.Write(buffer, 0, readBytes);
                }

                writer.Close();
            }

            this._ImageEdited = true;

            // Load the image from the temporary file
            this.LoadImageFromFileSystem_Internal(this.TemporarySourceImageFilePath, value);
        }

        /// <summary>
        /// Loads an image from a <see cref="Stream"/> and auto-calculates the <see cref="PictureTrimmerValue"/> to use.</summary>
        /// <param name="sourceImageStream">The <see cref="Stream"/> containing the image to load.</param>
        public void LoadImageFromStream(Stream sourceImageStream)
        {
            this.LoadImageFromStream(sourceImageStream, null);
        }

        /// <summary>
        /// Loads an image from an array of bytes and applies a specific <see cref="PictureTrimmerValue"/>.</summary>
        /// <param name="sourceImageBytes">The array of bytes to load.</param>
        /// <param name="value">The <see cref="PictureTrimmerValue"/> to apply.</param>
        public void LoadImageFromByteArray(byte[] sourceImageBytes, PictureTrimmerValue value)
        {
            //Save the byte array
            using (Stream writer = File.Create(this.TemporarySourceImageFilePath))
            {
                writer.Write(sourceImageBytes, 0, sourceImageBytes.Length);
                writer.Close();
            }

            // Load the image from the temporary file
            this.LoadImageFromFileSystem_Internal(this.TemporarySourceImageFilePath, value);
        }

        /// <summary>
        /// Loads an image from an array of bytes and auto-calculates the <see cref="PictureTrimmerValue"/> to use.</summary>
        /// <param name="sourceImageBytes">The array of bytes to load.</param>
        public void LoadImageFromByteArray(byte[] sourceImageBytes)
        {
            this.LoadImageFromByteArray(sourceImageBytes, null);
        }

        /// <summary>
        /// Unloads the current image.</summary>
        /// <param name="clearTemporaryFiles">If true, delete the temporary files.</param>
        protected void UnloadImage(bool clearTemporaryFiles)
        {
            this.popupPictureTrimmer1.UnloadImage();

            if (clearTemporaryFiles)
            {
                // Delete the temporary files
                this.ClearTemporaryFiles();
            }

            // No image
            this._ImageUploaded = false;
            this._ImageEdited = false;
            this._SourceImageClientFileName = null;
        }

        /// <summary>
        /// Unloads the current image and clears the temporary files.</summary>
        public void UnloadImage()
        {
            this.UnloadImage(true);
        }

        #endregion

        #region Image Processing

        /// <summary>
        /// Returns the <see cref="ImageProcessingJob"/> that can be used to process the source image.</summary>
        /// <returns>An <see cref="ImageProcessingJob"/> ready to be used to process imagess.</returns>
        public ImageProcessingJob GetImageProcessingJob()
        {
            return this.popupPictureTrimmer1.GetImageProcessingJob();
        }

        /// <summary>
        /// Returns the output image processed by the control.</summary>
        /// <returns>A <see cref="Bitmap"/> image processed by the control.</returns>
        public Bitmap GetProcessedImage()
        {
            return this.popupPictureTrimmer1.GetProcessedImage();
        }

        /// <summary>
        /// Processes  the source image and saves the output in a <see cref="Stream"/> with a specific image format.</summary>
        /// <param name="destStream">The <see cref="Stream"/> in which the image will be saved.</param>
        /// <param name="formatEncoderParams">The image format of the saved image.</param>
        public void SaveProcessedImageToStream(Stream destStream, FormatEncoderParams formatEncoderParams)
        {
            this.popupPictureTrimmer1.SaveProcessedImageToStream(destStream, formatEncoderParams);
        }

        /// <summary>
        /// Processes the source image and saves the output in a <see cref="Stream"/> with the default image format.</summary>
        /// <param name="destStream">The <see cref="Stream"/> in which the image will be saved.</param>
        public void SaveProcessedImageToStream(Stream destStream)
        {
            this.popupPictureTrimmer1.SaveProcessedImageToStream(destStream);
        }

        /// <summary>
        /// Processes the source image and saves the output in the file system with a specific image format.</summary>
        /// <param name="destFilePath">The file path of the saved image.</param>
        /// <param name="formatEncoderParams">The image format of the saved image.</param>
        /// <returns>Wether or not the save was successful</returns>
        public bool SaveProcessedImageToFileSystem(string destFilePath, FormatEncoderParams formatEncoderParams)
        {
            try
            {
                this.popupPictureTrimmer1.SaveProcessedImageToFileSystem(destFilePath, formatEncoderParams);
                return true;
            }
            catch (FileNotFoundException e)
            {
                //On the odd occasion this throws a file not found exception, in this case return false to indicate failure to save
                return false;
            }
        }

        /// <summary>
        /// Processes the source image and save the output in the file system with the default image format.</summary>
        /// <param name="destFilePath">The file path of the saved image.</param>
        /// <returns>Wether or not the save was successful</returns>
        public bool SaveProcessedImageToFileSystem(string destFilePath)
        {
            try
            {
                this.popupPictureTrimmer1.SaveProcessedImageToFileSystem(destFilePath);
                return true;
            }
            catch (FileNotFoundException e)
            {
                //On the odd occasion this throws a file not found exception, in this case return false to indicate failure to save
                return false;
            }
        }

        /// <summary>
        /// Processes the source image and retuns a byte array containing the processed image encoded with a specific image format.</summary>
        /// <param name="formatEncoderParams">The image format of the saved image.</param>
        /// <returns>An array of bytes containing the processed image.</returns>
        public byte[] SaveProcessedImageToByteArray(FormatEncoderParams formatEncoderParams)
        {
            return this.popupPictureTrimmer1.SaveProcessedImageToByteArray(formatEncoderParams);
        }

        /// <summary>
        /// Processes the source image and retuns a byte array containing the processed image encoded with the default image format.</summary>
        /// <returns>An array of bytes containing the processed image.</returns>
        public byte[] SaveProcessedImageToByteArray()
        {
            return this.popupPictureTrimmer1.SaveProcessedImageToByteArray();
        }

        #endregion

        #region Misc

        /// <summary>
        /// Deletes the internal temporary files generated by the control.</summary>
        public void ClearTemporaryFiles()
        {
            this.ClearTemporaryFile(this.TemporarySourceImageFilePath);
            this.ClearTemporaryFile(this.UploadFilePath);
            this.ClearTemporaryFile(this.PreviewImageFilePath);
            this.ClearTemporaryFile(this.TemporaryWriteTestFilePath);
            this.ClearTemporaryFile(this.UploadMonitorFilePath);
        }

        /// <summary>
        /// Opens the image edit popup window.</summary>
        public void OpenImageEditPopup()
        {
            if (!this.HasImage)
            {
                throw new Exception("Image not loaded");
            }

            // Open the image edit popup
            this._AutoOpenImageEditPopup = true;
        }

        #endregion

        #region Protected

        protected void ClearTemporaryFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        protected string GetSubElementId(string subId)
        {
            return this.ClientID + "_" + subId;
        }

        protected void TemporaryFolderWriteTest()
        {
            // Check if the application can write on the temporary folder
            if (!File.Exists(this.TemporaryWriteTestFilePath))
            {
                File.WriteAllText(this.TemporaryWriteTestFilePath, "write test", System.Text.Encoding.UTF8);
            }
        }

        protected string GetRenderStyleWidth()
        {
            if (!this._Width.IsEmpty)
            {
                return "width:" + this._Width.ToString(System.Globalization.CultureInfo.InvariantCulture) + ";";
            }
            else
            {
                // Do not render the value
                return "";
            }
        }

        protected string GetQueryKey(string additionalData)
        {
            Random rnd = new Random();
            string keyLeft = Guid.NewGuid().ToString("N").Substring(0, rnd.Next(1, 10));
            string keyRight = Guid.NewGuid().ToString("N").Substring(0, rnd.Next(1, 10));
            long timeStamp = DateTime.Now.Ticks + Convert.ToInt64(rnd.Next(500, 500000));
            string key = keyLeft + "&" + timeStamp.ToString(System.Globalization.CultureInfo.InvariantCulture) + "&" + additionalData + "&" + keyRight;
            string encodedKey = CodeCarvings.Piczard.Helpers.SecurityHelper.EncryptString(key);

            return encodedKey;
        }

        protected void ProcessUploadSuccess()
        {
            string sourceImageClientFileName = this._SourceImageClientFileName;

            if (this.HasImage)
            {
                // Unload the current image
                this.UnloadImage(false);
            }

            // Copy the uploaded file
            File.Copy(this.UploadFilePath, this.TemporarySourceImageFilePath, true);

            try
            {
                // Load the image in the PictureTrimmer control
                this.popupPictureTrimmer1.LoadImageFromFileSystem(this.TemporarySourceImageFilePath, this._OutputResolution, this.CropConstraint);
            }
            catch
            {
                // Invalid image

                // Display the invalid image message;
                this.CurrentStatusMessage = this.StatusMessage_InvalidImage;

                // EVENT: Upload error (invalid image)
                this.OnUploadError(EventArgs.Empty);
            }

            if (this.HasImage)
            {
                // Restore the source image client file name (changed in the UnloadImage method)
                this._SourceImageClientFileName = sourceImageClientFileName;

                // The new image has been uploaded
                this._ImageUploaded = true;
                this._ImageEdited = false;

                // Update the preview
                this._UpdatePreview = true;

                if (this.ImageUpload != null)
                {
                    // EVENT: Image upload
                    ImageUploadEventArgs args = new ImageUploadEventArgs(this._OutputResolution, this.CropConstraint, this.PreviewResizeConstraint);
                    this.OnImageUpload(args);
                    bool reloadImage = false;
                    if (args.OutputResolutionChanged)
                    {
                        this._OutputResolution = args.OutputResolution;
                        reloadImage = true;
                    }
                    if (args.CropConstraintChanged)
                    {
                        this._CropConstraint = args.CropConstraint;
                        reloadImage = true;
                    }
                    if (args.PreviewResizeConstraintChanged)
                    {
                        this._PreviewResizeConstraint = args.PreviewResizeConstraint;
                        // No need to reload if only the preview resize constraint has changed
                        // AND - the updatePrevie is surely already TRUE
                    }

                    if (reloadImage)
                    {
                        // Reload the image - Use the current source image size to save memory
                        this.popupPictureTrimmer1.SetLoadImageData_ImageSize(this.SourceImageSize);
                        this.popupPictureTrimmer1.LoadImageFromFileSystem(this.TemporarySourceImageFilePath, this._OutputResolution, this.CropConstraint);
                    }
                }

                // Invoke the OpenImageEditPopup after the event, so the eventhandler may change
                // the AutoOpenImageEditPopupAfterUpload property
                if (this.AutoOpenImageEditPopupAfterUpload)
                {
                    // Open the image edit popup
                    this.OpenImageEditPopup();
                }
            }
        }

        protected void ProcessUploadError()
        {
            if (this.HasImage)
            {
                // Unload the current image so we can display the error message
                this.UnloadImage(false);
            }

            // Display the error message;
            this.CurrentStatusMessage = this.StatusMessage_UploadError;

            // EVENT: Upload error
            this.OnUploadError(EventArgs.Empty);
        }

        protected void ProcessEdit()
        {
            // The new image has been edited
            this._ImageUploaded = false;
            this._ImageEdited = true;

            // Update the preview
            this._UpdatePreview = true;

            // EVENT: Image edit
            this.OnImageEdit(EventArgs.Empty);
        }

        protected void ProcessRemove()
        {
            // Unload the image
            this.UnloadImage(true);

            // EVENT: Image removed
            this.OnImageRemove(EventArgs.Empty);
        }

        protected void ProcessSelectedConfigurationIndexChanged()
        {
            // The new image has been edited
            this._ImageUploaded = false;
            this._ImageEdited = false;

            // Update the preview
            this._UpdatePreview = true;

            // Open the image edit popup
            this._AutoOpenImageEditPopup = true;

            if (this.SelectedConfigurationIndexChanged != null)
            {
                /// EVENT: Configuration index changed
                SelectedConfigurationIndexChangedEventArgs args = new SelectedConfigurationIndexChangedEventArgs(this._OutputResolution, this.CropConstraint, this.PreviewResizeConstraint);
                this.OnSelectedConfigurationIndexChanged(args);
                bool reloadImage = false;
                if (args.OutputResolutionChanged)
                {
                    this._OutputResolution = args.OutputResolution;
                    reloadImage = true;
                }
                if (args.CropConstraintChanged)
                {
                    this._CropConstraint = args.CropConstraint;
                    reloadImage = true;
                }
                if (args.PreviewResizeConstraintChanged)
                {
                    this._PreviewResizeConstraint = args.PreviewResizeConstraint;
                    // No need to reload if only the preview resize constraint has changed
                    // AND - the updatePrevie is surely already TRUE
                }

                if (reloadImage)
                {
                    // Reload the image - Use the current source image size to save memory
                    this.popupPictureTrimmer1.SetLoadImageData_ImageSize(this.SourceImageSize);
                    this.popupPictureTrimmer1.LoadImageFromFileSystem(this.TemporarySourceImageFilePath, this._OutputResolution, this.CropConstraint);
                }
            }
        }

        #endregion

        #endregion

        protected void cusRequired_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = this.HasNewImage || this.HasImage;
        }
    }
}