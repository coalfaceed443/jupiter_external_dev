using CRM.Code.BasePages.Admin;
using CRM.Code.Models;
using CRM.Code.Utils.Time;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.admin.Fundraising
{
    public partial class ImportGiftaid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmit.EventHandler = btnSubmit_Click;
            btnAgain.EventHandler = btnAgain_Click;


        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                byte[] fileArray = new byte[fuExcel.FileUpload.FileContent.Length + 1];

                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = fuExcel.FileUpload.FileContent.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    fileArray = ms.ToArray();
                }

                CRM_GiftAidLog communication = new CRM_GiftAidLog();
                communication.AdminID = ((AdminPage)Page).AdminUser.ID;
                communication.Name = txtName.Text;
                communication.IncomingFileType = fuExcel.Extension;
                communication.FileStore = fileArray;
                communication.DataStore = new byte[0];
                communication.Timestamp = UKTime.Now;
                db.CRM_GiftAidLogs.InsertOnSubmit(communication);
                db.SubmitChanges();

                int lineNumber = 0;

                fuExcel.FileUpload.FileContent.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(fuExcel.FileUpload.FileContent);
                DataTable dt = new DataTable();
                dt.TableName = txtName.Text;

                int ObjectReference = -1;


                while (!reader.EndOfStream)
                {
                    lineNumber++;

                    string contents = ReadNextMultiline(reader);
                    Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                    string[] contentsSplit = CSVParser.Split(contents);


                    if (lineNumber == 1)
                    {
                        for (int i = 0; i < contentsSplit.Length; i++)
                        {
                            if (contentsSplit[i] == "ID" || contentsSplit[i] == "Donation ID")
                            {
                                ObjectReference = i;
                            }

                            dt.Columns.Add(contentsSplit[i], typeof(string));
                        }
                    }
                    else
                    {
                        if (ObjectReference != -1)
                        {
                            CRM_Fundraising Donation = db.CRM_Fundraisings.FirstOrDefault(f => f.ID.ToString() == contentsSplit[ObjectReference]);

                            if (Donation != null)
                            {
                                CRM_GiftAidLogItem link = new CRM_GiftAidLogItem();

                                link.CRM_FundraisingID = Donation.ID;
                                communication.CRM_GiftAidLogItems.Add(link);

                                Donation.GiftClaimed = UKTime.Now;

                                db.SubmitChanges();
                            }
                        }

                        DataRow row = dt.NewRow();
                        for (int i = 0; i < contentsSplit.Length; i++)
                        {
                            try
                            {
                                row[i] = contentsSplit[i];
                            }
                            catch (IndexOutOfRangeException ex) { }
                        }
                        dt.Rows.Add(row);
                    }
                }

                BinaryFormatter bformatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                bformatter.Serialize(stream, dt);
                communication.DataStore = stream.ToArray();
                stream.Close();
                dt = null;
                db.SubmitChanges();


                mvLog.SetActiveView(viewDone);
                lblLogResult.Text = communication.Name + " - " + communication.CRM_GiftAidLogItems.Count() + " records matched up";

            }
        }

        protected void btnAgain_Click(object sender, EventArgs e)
        {
            mvLog.SetActiveView(viewEnter);
        }

        protected string ReadNextMultiline(StreamReader mlReader)
        {
            bool MultilineDetected;
            string res = "", mLine = "";
            do
            {
                MultilineDetected = false;
                mLine = mlReader.ReadLine();
                res = String.Concat(
                                        res,
                                        (res.Length > 0 ? " " : ""),    // add a space where there was a linebreak.
                                        mLine);
                string[] broken = res.Split(';');
                // if the RES features unfinished multiliner, then the LAST element will contain exactly 1 " symbol:
                if ((broken[broken.Length - 1].IndexOf('\"') >= 0) &&               // there's some " symbol
                    (broken[broken.Length - 1].IndexOf('\"') == broken[broken.Length - 1].LastIndexOf('\"'))    // there's exactly 1 " on that row.
                   )
                {
                    MultilineDetected = true;
                }
            } while (MultilineDetected);
            return res;
        }

        protected string RemoveLinebreaks(string item)
        {
            return item.Replace("\\\"", " ");
        }

        protected const string delimiter = "[*]";
        protected string ParseCSVInput(string item)
        {
            return item.Replace(delimiter, ",").Replace("\\\"", " ").Replace("\"", "");
        }

    }
}