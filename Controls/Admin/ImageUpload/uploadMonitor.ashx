<%@ WebHandler Language="C#" Class="uploadMonitor" %>
/*
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
using System.Web;
using System.IO;

using CodeCarvings.Piczard.Web;

public class uploadMonitor : IHttpHandler 
{
    
    // STATES:
    // 1 = File not found -> No error (upload in progress...)
    // 2 = upload success
    // 3 = upload error
    
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "application/xml";

        string temporaryFileId = context.Request["tfid"];
        string key = context.Request["k"];

        if (!this.ValidateTemporaryFileId(temporaryFileId))
        {
            this.WriteEmptyState(context);
            return;
        }
        if (!this.ValidateKey(key, ""))
        {
            this.WriteEmptyState(context);
            return;
        }

        string uploadMonitorFilePath = this.GetUploadMonitorFilePath(temporaryFileId);
        if (File.Exists(uploadMonitorFilePath))
        {
            // File found -> An error has occurred
            string xml = File.ReadAllText(uploadMonitorFilePath, System.Text.Encoding.UTF8);
            context.Response.Write(xml);
        }
        else
        {
            // File not found -> No error
            this.WriteState(context, 1);
        }
    }
    
    public bool IsReusable 
    {
        get 
        {
            return false;
        }
    }

    protected bool ValidateTemporaryFileId(string temporaryFileId)
    {
        if (string.IsNullOrEmpty(temporaryFileId))
        {
            return false;
        }
        if (!TemporaryFileManager.ValidateTemporaryFileId(temporaryFileId, false))
        {
            return false;
        }

        return true;
    }
    protected bool ValidateKey(string key, string additionalData)
    {
        if (string.IsNullOrEmpty(key))
        {
            return false;
        }
        string decodedKey = CodeCarvings.Piczard.Helpers.SecurityHelper.DecryptString(key);
        string[] keyVett = decodedKey.Split('&');
        if (keyVett.Length != 4)
        {
            return false;
        }
        DateTime timestamp = new DateTime(long.Parse(keyVett[1]));
        if (timestamp < DateTime.Now.AddDays(-1))
        {
            return false;
        }
        if (keyVett[2] != additionalData)
        {
            return false;
        }

        return true;
    }

    protected string GetUploadMonitorFilePath(string temporaryFileId)
    {
        return TemporaryFileManager.GetTemporaryFilePath(temporaryFileId, "_um.xml");
    }
    
    protected void WriteState(HttpContext context, int state)
    {
        string xml = string.Format("<?xml version=\"1.0\"?><uploadMonitor state=\"{0}\"></uploadMonitor>", state);
        context.Response.Write(xml);
    }

    protected void WriteEmptyState(HttpContext context)
    {
        this.WriteState(context, 0);
    }
}