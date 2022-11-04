/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log to file provider
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public class LogToFile : ILogToDestination
    {
        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log to file.
        /// </summary>
        public LogToFile()
        {
            return;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogToFile()
        {
           Dispose(false);
        }

        #endregion // Constructors and destructors


        /**********************************************************************
        ** Properties
        */
        #region Properties

        /// <summary>
        /// Connection string to file.
        /// "Path=C:\\A;Period=second/minute/hour/day/month/year;"
        /// Default period is empty, use only Path part.
        /// Example: "Path=C:\\A;Period=minute;" creates new log file every minute.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// File path from connection string.
        /// </summary>
        private string Path
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                return ((string)csb["Path"]);
            }
        }

        /// <summary>
        /// File period from connection string.
        /// </summary>
        private string Period
        {
            get
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                string period = (string)csb["Period"];
                if (period.ToLower() == "second") return ("second");
                else if (period.ToLower() == "minute") return ("minute");
                else if (period.ToLower() == "hour") return ("hour");
                else if (period.ToLower() == "day") return ("day");
                else if (period.ToLower() == "month") return ("month");
                else if (period.ToLower() == "year") return ("year");
                else return (""); // without period
            }
        }

        /// <summary>
        /// FileName according to period in connection string.
        /// </summary>
        private string FileName
        {
            get
            {
                string logfilename;
                DateTime dt = DateTime.Now;
                if (this.Period.ToLower() == "second")
                {
                    logfilename = string.Format("Log-per-second-{0:0000}-{1:00}-{2:00}-{3:00}-{4:00}-{5:00}.txt", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
                }
                else if (this.Period.ToLower() == "minute")
                {
                    logfilename = string.Format("Log-per-minute-{0:0000}-{1:00}-{2:00}-{3:00}-{4:00}.txt", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute);
                }
                else if (this.Period.ToLower() == "hour")
                {
                    logfilename = string.Format("Log-per-hour-{0:0000}-{1:00}-{2:00}-{3:00}.txt", dt.Year, dt.Month, dt.Day, dt.Hour);
                }
                else if (this.Period.ToLower() == "day")
                {
                    logfilename = string.Format("Log-per-day-{0:0000}-{1:00}-{2:00}.txt", dt.Year, dt.Month, dt.Day);
                }
                else if (this.Period.ToLower() == "month")
                {
                    logfilename = string.Format("Log-per-month-{0:0000}-{1:00}.txt", dt.Year, dt.Month);
                }
                else if (this.Period.ToLower() == "year")
                {
                    logfilename = string.Format("Log-per-year-{0:0000}.txt", dt.Year);
                }
                else // without period
                {
                    logfilename = string.Format("Log.txt");
                }
                return (logfilename);
            }
        }

        #endregion // Properties


        private System.IO.StreamWriter streamwritter = null;


        /**********************************************************************
        ** API methods
        */
        #region API

        /// <summary>
        /// Open file log stream.
        /// </summary>
        public void Open()
        {
            // Create a file to write to if file doesnt exist
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Path);
            if (this.Path.EndsWith("\\") == false) sb.Append("\\");
            sb.Append(this.FileName);
            this.streamwritter = System.IO.File.AppendText(sb.ToString());
            return;
        }

        /// <summary>
        /// Is file log stream open.
        /// </summary>
        /// <returns>True/False</returns>
        public bool IsOpen()
        {
            if (this.streamwritter == null) return (false);
            return (true);
        }

        /// <summary>
        /// Write Message to log.
        /// </summary>
        /// <param name="datetime">DateTime log creation.</param>
        /// <param name="type">Log type.</param>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void WriteMessage(DateTime datetime, LogMessageType type, Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if (this.streamwritter != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("{0:0000}-{1:00}-{2:00} ", datetime.Year, datetime.Month, datetime.Day));
                sb.Append(string.Format("{0:00}:{1:00}:{2:00}:{3:000} ", datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond));
                if (type == LogMessageType.Trace) sb.Append("      TRACE ");
                else if (type == LogMessageType.Debug) sb.Append("      DEBUG ");
                else if (type == LogMessageType.Information) sb.Append("INFORMATION ");
                else if (type == LogMessageType.Warning) sb.Append("    WARNING ");
                else if (type == LogMessageType.Error) sb.Append("      ERROR ");
                else if (type == LogMessageType.Critical) sb.Append("   CRITICAL ");
                sb.Append(string.Format("{0:00000} ", categoryid));
                sb.Append(string.Format("{0:00000000} ", eventid));
                sb.Append(string.Format("'{0}' ", message));
                if (rawdata != null && rawdata.Length > 0)
                {
                    sb.Append(string.Format("hexdata["));
                    for (int i = 0; i < rawdata.Length; i++)
                    {
                        if (i > 0) sb.Append(",");
                        sb.Append(string.Format("{0:X2}", rawdata[i]));
                    }
                    sb.Append(string.Format("]"));
                }
                this.streamwritter.WriteLine(sb.ToString());
                this.streamwritter.Flush();
            }
            return;
        }

        /// <summary>
        /// Close file log stream.
        /// </summary>
        public void Close()
        {
            if (this.streamwritter != null)
            {
                this.streamwritter.Close();
                this.streamwritter = null;
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
