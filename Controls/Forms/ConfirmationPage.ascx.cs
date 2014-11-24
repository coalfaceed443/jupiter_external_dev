using System;
using System.Web.UI;
using System.Collections.Generic;
using CRM.Code;

namespace CRM.Controls.Forms
{
    public partial class UserControlConfirmationPage : System.Web.UI.UserControl
    {
        private bool showConfirmation = false;
        public bool ShowConfirmation
        {
            get
            {
                return showConfirmation;
            }
            set
            {
                showConfirmation = value;
            }
        }

        private string header = "";
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
            }
        }

        private string message = "";
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

        public UserControl AddButton(string text, string icon, string type, string URL)
        {
            UserControlButton button = ((Page)Context.CurrentHandler).LoadControl(Constants.ButtonUCPath) as UserControlButton;
            button.ButtonText = text;
            button.Class = type;
            button.ImagePath = icon;
            button.NavigateUrl = URL;

            plhButtons.Controls.Add(button);
            return button;
        }

        public UserControl AddButton(string text, string icon, string type, EventHandler EventHandler)
        {
            UserControlButton button = ((Page)Context.CurrentHandler).LoadControl(Constants.ButtonUCPath) as UserControlButton;
            button.ButtonText = text;
            button.Class = type;
            button.ImagePath = icon;
            button.EventHandler = EventHandler;

            plhButtons.Controls.Add(button);
            return button;
        }


        public void CannotDelete(string type, string children, bool showNow)
        {
            Header = "You cannot delete this " + type;

            Message = "<p>You cannot delete this " + type + " as it has still contains " + children + ".  Please delete the " + children + " before proceeding.";

            AddButton("OK", "tick.png", "positive", "javascript:$.fancybox.close();");

            ShowConfirmation = showNow;
        }

        public void StandardDeleteHidden(string type, EventHandler eventx)
        {
            StandardDelete(type, eventx, false);
        }

        public void PermanentDeleteHidden(string type, EventHandler eventx)
        {
            PermanentDelete(type, eventx, false);
        }

        public void StandardDeleteVisible(string type, EventHandler eventx)
        {
            StandardDelete(type, eventx, true);
        }

        private void StandardDelete(string type, EventHandler eventx, bool showNow)
        {
            Header = "Are you sure?";

            Message = "<p>The " + type + " will be marked as archived!</p>";

            UserControl button = AddButton("Delete " + type, "cross.png", "negative", eventx);

            AddButton("Cancel", "tick.png", "positive", "javascript:$.fancybox.close();");

            ShowConfirmation = showNow;
            CreateChildControls();
        }

        private void PermanentDelete(string type, EventHandler eventx, bool showNow)
        {
            Header = "Are you sure?";

            Message = "<p>The " + type + " will be deleted permanently!</p>";

            UserControl button = AddButton("Delete " + type, "cross.png", "negative", eventx);

            AddButton("Cancel", "tick.png", "positive", "javascript:$.fancybox.close();");

            ShowConfirmation = showNow;
            CreateChildControls();
        }

        public void StandardAddUpdate(string type, string listPage, string detailsPage, bool newRecord)
        {
            StandardAddUpdate(type, listPage, detailsPage, newRecord, true, type);
        }

        public void StandardAddUpdate(string type, string listPage, string detailsPage, bool newRecord, bool showAdd)
        {
            StandardAddUpdate(type, listPage, detailsPage, newRecord, showAdd, type);
        }

        public void StandardAddUpdate(string type, string listPage, string detailsPage, bool newRecord, bool showAdd, string buttonText)
        {
            if (newRecord)
            {
                Header = "The " + type + " has been added successfully";
            }
            else
            {
                Header = "The " + type + " has been updated successfully";
            }

            Message = "<p>Please choose one of the following options:</p>";

            AddButton("View " + buttonText + " List", "table.png", "neutral", listPage);

            if (showAdd)
            {
                AddButton("Add another " + buttonText, "add.png", "neutral", detailsPage);
            }

            ShowConfirmation = true;
            CreateChildControls();
        }

        public void ShowCustomPopup(string headerText, string message, List<UserControl> buttons)
        {
            Header = headerText;

            if (!message.StartsWith("<p>"))
                message = "<p>" + message + "</p>";

            Message = message;

            foreach (UserControl button in buttons)
            {
                plhButtons.Controls.Add(button);
            }

            ShowConfirmation = true;
            CreateChildControls();
        }

        public void ShowPopup()
        {
            ShowConfirmation = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Target = "_parent";
        }
    }
}