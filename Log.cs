/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log api
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public class Log
    {
        private ILogToDestination ToDestination { get; set; }


        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log Constructor
        /// </summary>
        /// <param name="todestination"></param>
        public Log(ILogToDestination todestination)
        {
            this.ToDestination = todestination;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Log()
        {
            this.ToDestination.Dispose();
            Dispose(false);
            return;
        }

        #endregion


        /**********************************************************************
        ** Properties
        */
        #region Properties

        /// <summary>
        /// Which message types are enabled to log.
        /// </summary>
        public LogMessageType EnabledLogMessages { get; set; }

        #endregion


        /**********************************************************************
        ** API methods
        */
        #region API

        /// <summary>
        /// Open log stream
        /// </summary>
        public void Open()
        {
            lock (this)
            {
                this.ToDestination.Open();
            }
        }

        /// <summary>
        /// Write Trace Message to log if is enabled.
        /// </summary>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void Trace(Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if ((this.EnabledLogMessages & LogMessageType.Trace) == LogMessageType.Trace)
            {
                lock (this)
                {
                    this.ToDestination.WriteMessage(DateTime.Now, LogMessageType.Trace, categoryid, eventid, message, rawdata);
                }
            }
            return;
        }

        /// <summary>
        /// Write Debug Message to log if is enabled.
        /// </summary>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void Debug(Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if ((this.EnabledLogMessages & LogMessageType.Debug) == LogMessageType.Debug)
            {
                lock (this)
                {
                    this.ToDestination.WriteMessage(DateTime.Now, LogMessageType.Debug, categoryid, eventid, message, rawdata);
                }
            }
            return;
        }

        /// <summary>
        /// Write Information Message to log if is enabled.
        /// </summary>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void Information(Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if ((this.EnabledLogMessages & LogMessageType.Information) == LogMessageType.Information)
            {
                lock (this)
                {
                    this.ToDestination.WriteMessage(DateTime.Now, LogMessageType.Information, categoryid, eventid, message, rawdata);
                }
            }
            return;
        }

        /// <summary>
        /// Write Warning Message to log if is enabled.
        /// </summary>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void Warning(Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if ((this.EnabledLogMessages & LogMessageType.Warning) == LogMessageType.Warning)
            {
                lock (this)
                {
                    this.ToDestination.WriteMessage(DateTime.Now, LogMessageType.Warning, categoryid, eventid, message, rawdata);
                }
            }
            return;
        }

        /// <summary>
        /// Write Error Message to log if is enabled.
        /// </summary>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void Error(Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if ((this.EnabledLogMessages & LogMessageType.Error) == LogMessageType.Error)
            {
                lock (this)
                {
                    this.ToDestination.WriteMessage(DateTime.Now, LogMessageType.Error, categoryid, eventid, message, rawdata);
                }
            }
            return;
        }

        /// <summary>
        /// Write Critical Message to log if is enabled.
        /// </summary>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void Critical(Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if ((this.EnabledLogMessages & LogMessageType.Critical) == LogMessageType.Critical)
            {
                lock (this)
                {
                    this.ToDestination.WriteMessage(DateTime.Now, LogMessageType.Critical, categoryid, eventid, message, rawdata);
                }
            }
            return;
        }

        /// <summary>
        /// Close log stream
        /// </summary>
        public void Close()
        {
            lock (this)
            {
                this.ToDestination.Close();
            }
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
