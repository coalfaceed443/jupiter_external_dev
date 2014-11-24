using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;

namespace CRM.Code.Utils.WebControl
{
    /// <summary>
    /// Summary description for TitledDropDownList
    /// </summary>
    public class TitledDropDownList : DropDownList
    {
        public interface ICustomStyle
        {
            Guid ID { get; set; }
            string Style { get; set; }
            string Name { get; set; }
            string Description { get; set; }
            bool InUse { get; set; }
            string UsedBy { get; set; }
            void SetStyle(System.Web.UI.Control control);
        }

        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            IList styles = this.GetStyles();
            bool flag = false;
            for (int i = 0; i < this.Items.Count; i++)
            {
                writer.WriteBeginTag("Option");
                if (Items[i].Selected)
                {
                    if (flag)
                    {
                        this.VerifyMultiSelect();
                    }
                    flag = true;
                    writer.WriteAttribute("selected", "selected");
                }
                writer.WriteAttribute("value", Items[i].Value);
                writer.WriteAttribute("title", Items[i].Text);
                if (styles != null)
                {
                    //writer.WriteAttribute("Class", styles[i].Style);
                }
                Items[i].Attributes.Render(writer);
                if (this.Page != null)
                {
                    this.Page.ClientScript.RegisterForEventValidation(this.UniqueID, Items[i].Value);
                }

                writer.Write(HtmlTextWriter.TagRightChar);
                System.Web.HttpUtility.HtmlEncode(Items[i].Text, writer);
                writer.WriteEndTag("Option");
                writer.WriteLine();
            }
        }
        private IList GetStyles()
        {
            if (this.DataSource == null)
                return null;
            ArrayList styles = null;
            if (this.DataSource is IList)
            {
                styles = (ArrayList)this.DataSource;
            }
            else
            {
                if (this.DataSource.GetType().GetInterface("IList") != null)
                {
                    System.Collections.IList originalList = (System.Collections.IList)this.DataSource;
                    if (styles == null)
                        styles = new ArrayList();
                    foreach (Object obj in originalList)
                    {
                        ICustomStyle style = null;
                        if (obj.GetType().GetInterface("ICustomStyle") != null)
                            style = (ICustomStyle)obj;
                        if (style == null)
                        {
                            styles = null;
                            break;
                        }
                        styles.Add(style);
                    }
                }
            }
            if (styles != null && styles.Count != this.Items.Count)
                styles = null;
            return styles;
        }
    }
}