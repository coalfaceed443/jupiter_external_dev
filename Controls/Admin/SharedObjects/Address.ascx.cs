using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRM.Code.Models;
using CRM.Code.Interfaces;

namespace CRM.Controls.Admin.SharedObjects
{
    public partial class Address : System.Web.UI.UserControl
    {
        public bool Address1Required { get; set; }
        public bool Address2Required { get; set; }
        public bool Address3Required { get; set; }
        public bool TownRequired { get; set; }
        public bool CountyRequired { get; set; }
        public bool PostcodeRequired { get; set;}
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtAddress1.Required = Address1Required;
                txtAddress2.Required = Address2Required;
                txtAddress3.Required = Address3Required;
                txtTown.Required = TownRequired;
                txtCounty.Required = CountyRequired;
                txtPostcode.Required = PostcodeRequired;

                using (MainDataContext db = new MainDataContext())
                {
                    ddlCounty.DataSource = from c in db.Countries
                                           orderby c.ID == 1 descending, c.Name
                                           select c;
                    ddlCounty.DataBind();
                    db.Dispose();
                }
            }
        }

        public string Required(bool required)
        {
            return required ? "*" : "";
        }

        public void Populate(IAddress address)
        {
            txtAddress1.Text = address.AddressLine1;
            txtAddress2.Text = address.AddressLine2;
            txtAddress3.Text = address.AddressLine3;
            txtAddress4.Text = address.AddressLine4;
            txtAddress5.Text = address.AddressLine5;
            txtTown.Text = address.Town;
            txtCounty.Text = address.County;
            txtPostcode.Text = address.Postcode;
            ddlCounty.SelectedValue = address.CountryID.ToString();
        }

        public IAddress Save(IAddress address)
        {
            address.AddressLine1 = txtAddress1.Text;
            address.AddressLine2 = txtAddress2.Text;
            address.AddressLine3 = txtAddress3.Text;
            address.AddressLine4 = txtAddress4.Text;
            address.AddressLine5 = txtAddress5.Text;
            address.Town = txtTown.Text;
            address.County = txtCounty.Text;
            address.CountryID = Convert.ToInt32(ddlCounty.SelectedValue);
            address.Postcode = txtPostcode.Text;

            return address;
        }
    }


}