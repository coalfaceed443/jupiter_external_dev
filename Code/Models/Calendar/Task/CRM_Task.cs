using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Code.Interfaces;
using CRM.Code.Helpers;

namespace CRM.Code.Models
{
    public partial class CRM_Task : IHistory
    {
        public object ShallowCopy()
        {
            return (CRM_Task)this.MemberwiseClone();
        }

        public string OutputTableName
        {
            get
            {
                return "Task Record";
            }
        }

        public string TableName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        [IsListData("Due Date")]
        public string DueDateOutput
        {
            get
            {
                return this.DueDate.ToString("dd/MM/yyyy");
            }
        }


        [IsListData("Owner")]
        public string Owner
        {
            get
            {
                return this.Admin.DisplayName;
            }
        }


        [IsListData("Is Complete")]
        public string IsCompleteDisplay
        {
            get
            {
                return this.IsComplete ? "Yes" : "No";
            }
        }


        [IsListData("Is Cancelled")]
        public string IsCancelledDisplay
        {
            get
            {
                return this.IsCancelled ? "Yes" : "No";
            }
        }


        public List<string> Tokens
        {
            get
            {
                List<string> tokens = new List<string>();
                tokens.AddRange(JSONSet.ConvertToTokens(this.Name));
                return tokens;
            }
        }


        [IsListData("Participants including archived")]
        public int ParticipantsandArchived
        {
            get
            {
                return this.CRM_TaskParticipants.Count();
            }
        }

        [IsListData("Archived participants")]
        public int ArchivedParticipants
        {
            get
            {
                return this.CRM_TaskParticipants.Where(c => c.IsArchived).Count();
            }
        }

        [IsListData("Participants")]
        public int Participants
        {
            get
            {
                return this.CRM_TaskParticipants.Where(c => !c.IsArchived).Count();
            }
        }


        public int ParentID
        {
            get
            {
                return 0;
            }
        }

        [IsListData("View")]
        public string DetailsURLLink
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(DetailsURL, "View");
            }
        }

        public string DetailsURL
        {
            get
            {
                return "/admin/calendar/task/details.aspx?id=" + this.CRM_CalendarID + "&pid=" + this.ID;
            }
        }

        public bool IsVisible(int AdminUserID)
        {
            return this.OwnerAdminID == AdminUserID || this.CRM_TaskParticipants.Any(t => t.AdminID == AdminUserID) || this.IsPublic;
        }

    }
}