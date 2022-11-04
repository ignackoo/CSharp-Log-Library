/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log to event log provider
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public class LogToEventLog : ILogToDestination
    {
        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log to EventLog.
        /// </summary>
        public LogToEventLog()
        {
            return;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogToEventLog()
        {
           Dispose(false);
        }

        #endregion // Constructors and destructors


        /**********************************************************************
        ** Properties
        */
        #region Properties

        /// <summary>
        /// Connection string to EventLog.
        /// Example: "MachineName=.;LogName=MyLog;Source=AppName"
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// MachineName from connection string.
        /// </summary>
        private string MachineName 
        { 
            get 
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                return ((string)csb["MachineName"]);
            } 
        }

        /// <summary>
        /// LogName from connection string.
        /// </summary>
        private string LogName 
        { 
            get 
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                return ((string)csb["LogName"]);
            } 
        }

        /// <summary>
        /// Source from connection string.
        /// </summary>
        private string Source 
        { 
            get 
            {
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                csb.ConnectionString = this.ConnectionString;
                return ((string)csb["Source"]);
            } 
        }

        #endregion // Properties


        private System.Diagnostics.EventLog eventlog = null;


        /**********************************************************************
        ** API methods
        */
        #region API

        /// <summary>
        /// Open log EventLog stream.
        /// </summary>
        public void Open()
        {
            // Create an EventLog instance and assign its source.
            this.eventlog = new System.Diagnostics.EventLog(this.LogName, this.MachineName, this.Source);
            return;
        }

        /// <summary>
        /// Is EventLog stream open.
        /// </summary>
        /// <returns>True/False</returns>
        public bool IsOpen()
        {
            if (this.eventlog == null) return (false);
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
            if (this.eventlog != null)
            {
                // Write an entry to the log.
                if (type == LogMessageType.Trace)
                {
                    message = "TRACE INFORMATION: " + message;
                    this.eventlog.WriteEntry(message, System.Diagnostics.EventLogEntryType.Information, eventid, categoryid, rawdata);
                }
                else if (type == LogMessageType.Debug)
                {
                    message = "DEBUG INFORMATION: " + message;
                    this.eventlog.WriteEntry(message, System.Diagnostics.EventLogEntryType.Information, eventid, categoryid, rawdata);
                }
                else if (type == LogMessageType.Information)
                {
                    message = "INFORMATION: " + message;
                    this.eventlog.WriteEntry(message, System.Diagnostics.EventLogEntryType.Information, eventid, categoryid, rawdata);
                }
                else if (type == LogMessageType.Warning)
                {
                    message = "WARNING: " + message;
                    this.eventlog.WriteEntry(message, System.Diagnostics.EventLogEntryType.Warning, eventid, categoryid, rawdata);
                }
                else if (type == LogMessageType.Error)
                {
                    message = "ERROR: " + message;
                    this.eventlog.WriteEntry(message, System.Diagnostics.EventLogEntryType.Error, eventid, categoryid, rawdata);
                }
                else // LogMessageType.Critical
                {
                    message = "CRITICAL ERROR: " + message;
                    this.eventlog.WriteEntry(message, System.Diagnostics.EventLogEntryType.Error, eventid, categoryid, rawdata);
                }
            }
            return;
        }

        /// <summary>
        /// Close EventLog log stream.
        /// </summary>
        public void Close()
        {
            if (this.eventlog != null)
            {
                this.eventlog.Close();
                this.eventlog = null;
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
