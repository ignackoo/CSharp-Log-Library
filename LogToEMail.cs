/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log to email provider
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public class LogToEMail : ILogToDestination
    {
        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log to eMail.
        /// </summary>
        public LogToEMail()
        {
            return;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogToEMail()
        {
           Dispose(false);
        }

        #endregion // Constructors and destructors


        /**********************************************************************
        ** Properties
        */
        #region Properties

        /// <summary>
        /// Connection string to eMail .
        /// Example: "Smtp=smtp.emailprovider.tld;Ssl=true;Port=587;Sender=youremail@emailprovider.tld;Password=abcdefgh;Recipient=youremail@emailprovider.tld;PauseMS=10"
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Smtp address from connection string.
        /// </summary>
        private string Smtp
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                return ((string)csb["Smtp"]);
            }
        }

        /// <summary>
        /// Ssl from connection string
        /// </summary>
        private bool Ssl
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                string str = ((string)csb["Ssl"]);
                bool value;
                bool b = bool.TryParse(str, out value);
                if (b == true) return (value);
                return (false);
            }
        }

        /// <summary>
        /// Port from connection string.
        /// </summary>
        private int Port
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                string str = ((string)csb["Port"]);
                int value;
                bool b = int.TryParse(str, out value);
                if (b == true) return (value);
                return (25);
            }
        }

        /// <summary>
        /// Sender from connection string.
        /// </summary>
        private string Sender
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                return ((string)csb["Sender"]);
            }
        }

        /// <summary>
        /// Password from connection string.
        /// </summary>
        private string Password
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                return ((string)csb["Password"]);
            }
        }

        /// <summary>
        /// Recipient from connection string.
        /// </summary>
        private string Recipient
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                return ((string)csb["Recipient"]);
            }
        }

        /// <summary>
        /// Pause in miliseconds from connection string.
        /// </summary>
        private int PauseMS
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                string str = ((string)csb["PauseMS"]);
                int value;
                bool b = int.TryParse(str, out value);
                if (b == true) return (value);
                return (0);
            }
        }

        #endregion // Properties


        private System.Net.Mail.SmtpClient smtpclient = null;


        /**********************************************************************
        ** API methods
        */
        #region API

        /// <summary>
        /// Open eMail log stream.
        /// </summary>
        public void Open()
        {
            // Use the SMTP Host
            this.smtpclient = new System.Net.Mail.SmtpClient(this.Smtp);
            // Enable SSL for encryption across channels
            this.smtpclient.EnableSsl = this.Ssl;
            // Port 25, 2525 and 465, 587 for ssl/tls communication
            this.smtpclient.Port = this.Port;
            // Provide authentication information with SMTP server to authenticate your sender account
            this.smtpclient.Credentials = new System.Net.NetworkCredential(this.Sender, this.Password);
            return;
        }

        /// <summary>
        /// Is eMail log stream open.
        /// </summary>
        /// <returns>True/False</returns>
        public bool IsOpen()
        {
            if (this.smtpclient == null) return (false);
            return (true);
        }

        /// <summary>
        /// Write Message to log.
        /// </summary>
        /// <param name="datetime">DateTime log creation.</param>
        /// <param name="type">Log entry type.</param>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void WriteMessage(DateTime datetime, LogMessageType type, Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if (this.smtpclient != null)
            {
                System.Net.Mail.MailMessage newMail = new System.Net.Mail.MailMessage();
                // Follow the RFS 5321 Email Standard
                newMail.From = new System.Net.Mail.MailAddress(this.Sender);
                // Declare the email subject
                newMail.To.Add(this.Recipient);
                // email subject
                newMail.Subject = type.ToString() + " email";
                // Use HTML for the email body
                newMail.IsBodyHtml = true;
                // Email body
                StringBuilder sb = new StringBuilder();
                sb.Append("<h3><b>From:</b> " + this.Sender + "</h3>");
                sb.Append("<h3><b>Email:</b> " + type.ToString() + "</h3>");
                sb.Append(string.Format("<h3><b>Date:</b> {0:0000}-{1:00}-{2:00}</h3>", datetime.Year, datetime.Month, datetime.Day));
                sb.Append(string.Format("<h3><b>Time:</b> {0:00}:{1:00}:{2:00}:{3:000}</h3>", datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond));
                sb.Append(string.Format("<h3><b>Category:</b> {0}</h3>", categoryid));
                sb.Append(string.Format("<h3><b>Event:</b> {0}</h3>", eventid));
                sb.Append("<h3><b>Message:</b> " + message + "</h3>");
                sb.Append("<h3>");
                if (rawdata != null && rawdata.Length > 0)
                {
                    sb.Append(string.Format("<b>Hexdata:</b> "));
                    for (int i = 0; i < rawdata.Length; i++)
                    {
                        if (i > 0) sb.Append(",");
                        sb.Append(string.Format("{0:X2}", rawdata[i]));
                    }
                }
                sb.Append("</h3>");
                newMail.Body = sb.ToString();
                // Send the constructed mail
                this.smtpclient.Send(newMail);
                // Pause
                System.Threading.Thread.Sleep(this.PauseMS);
            }
            return;
        }

        /// <summary>
        /// Close eMail log stream.
        /// </summary>
        public void Close()
        {
            if (this.smtpclient != null)
            {
                this.smtpclient.Dispose();
                this.smtpclient = null;
            }
            return;
        }

        #endregion // API


        /**********************************************************************
        ** IDisposable implementation
        */
        #region IDisposable

        // To detect redundant calls Dispose
        private bool _disposed;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this.Close();

                _disposed = true;
            }
        }

        #endregion // IDisposable

    }
}
