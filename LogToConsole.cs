/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log to console provider
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public class LogToConsole : ILogToDestination
    {
        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log to console.
        /// </summary>
        public LogToConsole()
        {
            return;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogToConsole()
        {
           Dispose(false);
        }

        #endregion


        /**********************************************************************
        ** Properties
        */
        #region Properties

        /// <summary>
        /// Unused in Log to console
        /// </summary>
        public string ConnectionString { get; set; }

        #endregion


        /**********************************************************************
        ** API methods
        */
        #region API

        /// <summary>
        /// Open console log stream.
        /// </summary>
        /// 
        public void Open()
        {
            return;
        }

        /// <summary>
        /// Is console log stream open.
        /// </summary>
        /// <returns>True</returns>
        public bool IsOpen()
        {
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
            Console.WriteLine(sb.ToString());
            return;
        }

        /// <summary>
        /// Close console log stream.
        /// </summary>
        public void Close()
        {
            return;
        }

        #endregion // API methods


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
