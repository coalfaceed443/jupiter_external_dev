using System;
using System.Text.RegularExpressions;
namespace CRM.Controls.Forms
{
    public partial class UserControlEmbeddedVideo : System.Web.UI.UserControl
    {
        public string Text
        {
            get
            {
                return txtVideoURL.Text;
            }
            set
            {
                txtVideoURL.Text = value;
            }
        }

        private int width = 286;

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

        public bool TryParseYouTubeReference(out string youtubeRef)
        {
            youtubeRef = "";
            try
            {
                Regex r = new Regex("v=(.+?)(&|$)");
                Match m = r.Match(Text);
                youtubeRef = m.Captures[0].Value.Replace("v=", "").Replace("&", "");
            }
            catch { return false; }
            return true;
        }

        public bool TryParseVimeoReference(out string vimeoRef)
        {
            vimeoRef = "";
            if (!Text.Contains("vimeo.com"))
                return false;
            try
            {
                vimeoRef = Text.Replace("vimeo.com/", "").Replace("http://", "").Replace("https://", "").Replace("www.", "");
            }
            catch { return false; }
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtVideoURL.Width = width;
        }
    }

}
