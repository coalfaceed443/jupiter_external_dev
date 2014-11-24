using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM
{
    public partial class Swap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtOutput.Text = "";
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            
            string insert = txtInsert.Text;

            string[] lines = txtInsert.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line.Contains("="))
                {
                    string left = Clean(line.Split('=')[1]).Trim();
                    string right = Clean(line.Split('=')[0]).Trim();

                    txtOutput.Text += left + " = " + right + ";" + Environment.NewLine;
                }
            }
         }

        protected string Clean(string input)
        {
            return input.Replace(";", "");
        }
    }
}