using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Controls.Forms.Handlers;

namespace CRM.Code.Models
{
    public partial class CRM_TaskParticipant
    {
        [IsListData("Name")]
        public string Name
        {
            get
            {
                return this.Admin.DisplayName;
            }
        }

        [IsListData("Is Archived")]
        public string IsArchivedDisplay
        {
            get
            {
                return this.IsArchived ? "Yes" : "No";
            }
        }

        [IsListData("Remove from Task")]
        public string RemoveFromTask
        {
            get
            {
                if (this.IsArchived)
                    return Utils.Text.Text.ConvertUrlsToLinks(ReinstateFromTaskURL, "Reinstate");
                else
                {
                    if (this.CRM_Task.OwnerAdminID == this.AdminID)
                        return Utils.Text.Text.ConvertUrlsToLinks(RemoveFromTaskURL, "---");
                    else return Utils.Text.Text.ConvertUrlsToLinks(RemoveFromTaskURL, "Remove");

                }
            }
        }

        private string ReinstateFromTaskURL
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.ReinstateTaskParticipant, this.ID, this.CRM_Task.DetailsURL);
            }
        }

        private string RemoveFromTaskURL
        {
            get
            {
                if (this.CRM_Task.OwnerAdminID == this.AdminID)
                    return "#";
                else
                    return ActionLink.FormURL(ActionLink.Route.ArchiveTaskParticipant, this.ID, this.CRM_Task.DetailsURL);
            }
        }
    }
}