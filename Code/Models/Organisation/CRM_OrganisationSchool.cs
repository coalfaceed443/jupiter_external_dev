using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Controls.Forms.Handlers;

namespace CRM.Code.Models
{
    public partial class CRM_OrganisationSchool
    {
        [IsListData("Organisation Name")]
        public string OrganisationName
        {
            get
            {
                return this.CRM_Organisation.Name;
            }
        }

        [IsListData("School Name")]
        public string SchoolName
        {
            get
            {
                return this.CRM_School.Name;
            }
        }

        [IsListData("School Address")]
        public string SchoolAddress
        {
            get
            {
                return this.CRM_School.FormattedAddress;
            }
        }


        [IsListData("View School")]
        public string ViewSchoolURL
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(this.ViewSchool, "View");
            }
        }

        public string ViewSchool
        {
            get
            {
                return this.CRM_School.DetailsURL;
            }
        }

        [IsListData("View Organisation")]
        public string ViewOrganisationURL
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(this.ViewOrganisation, "View");
            }
        }


        public string ViewOrganisation
        {
            get
            {
                return this.CRM_Organisation.DetailsURL;
            }
        }

        [IsListData("Organisation Address")]
        public string OrganisationAddress
        {
            get
            {
                return this.CRM_Organisation.CRM_Address.FormattedAddress;
            }
        }


        [IsListData("Delete")]
        public string RemoveFromTask
        {
            get
            {
                return Utils.Text.Text.ConvertUrlsToLinks(this.DeleteURL, "Delete");
            }
        }

        private string DeleteURL
        {
            get
            {
                return ActionLink.FormURL(ActionLink.Route.DeleteOrganisationSchool, this.ID, HttpContext.Current.Request.RawUrl);
            }
        }


    }
}