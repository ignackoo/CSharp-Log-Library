/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log sync
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public class LogSync : ILogToDestination
    {
        private ILogToDestination ToDestination { get; set; }


        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log to destination sync.
        /// </summary>
        public LogSync(ILogToDestination todestination)
        {
            this.ToDestination = todestination;
            return;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogSync()
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
            this.ToDestination.Open();
            return;
        }

        /// <summary>
        /// Is log stream open.
        /// </summary>
        /// <returns>True/False</returns>
        public bool IsOpen()
        {
            return (this.ToDestination.IsOpen());
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
            if (this.IsOpen() == false)
            {
                this.Close();
                this.Open();
            }
            this.ToDestination.WriteMessage(datetime, type, categoryid, eventid, message, rawdata);
            return;
        }

        /// <summary>
        /// Close log stream.
        /// </summary>
        public void Close()
        {
            this.ToDestination.Close();
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
