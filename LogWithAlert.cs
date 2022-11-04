/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log with alerts
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class LogWithAlert : ILogToDestination
    {
        private ILogToDestination ToLogDestination { get; set; }
        private ILogToDestination ToAlertDestination { get; set; }


        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log to destination sync.
        /// </summary>
        public LogWithAlert(ILogToDestination tologdestination, ILogToDestination toalertdestination)
        {
            this.ToLogDestination = tologdestination;
            this.ToAlertDestination = toalertdestination;
            return;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogWithAlert()
        {
            this.ToLogDestination.Dispose();
            this.ToAlertDestination.Dispose();
            Dispose(false);
            return;
        }

        #endregion


        /**********************************************************************
        ** Properties
        */
        #region Properties

        /// <summary>
        /// Which message types are enabled to alert.
        /// </summary>
        public LogMessageType EnabledAlertMessages { get; set; }

        #endregion


        /**********************************************************************
        ** API methods
        */
        #region API

        /// <summary>
        /// Open log stream.
        /// </summary>
        public void Open()
        {
            this.ToLogDestination.Open();
            this.ToAlertDestination.Open();
            return;
        }

        /// <summary>
        /// Is Open unused in this layer.
        /// </summary>
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
            this.ToLogDestination.WriteMessage(datetime, type, categoryid, eventid, message, rawdata);
            if ((this.EnabledAlertMessages & type) != 0)
            {
                this.ToAlertDestination.WriteMessage(datetime, type, categoryid, eventid, message, rawdata);
            }
            return;
        }

        /// <summary>
        /// Close log stream.
        /// </summary>
        public void Close()
        {
            this.ToLogDestination.Close();
            this.ToAlertDestination.Close();
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
