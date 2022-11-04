/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log async
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public class LogAsync : ILogToDestination
    {
        private ILogToDestination ToDestination { get; set; }

        private struct MessageStructure
        {
            public DateTime datetime;
            public LogMessageType type;
            public Int16 categoryid;
            public Int32 eventid;
            public string message;
            public byte[] rawdata;
        }


        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log to destination async.
        /// </summary>
        public LogAsync(ILogToDestination todestination)
        {
            this.ToDestination = todestination;
            return;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogAsync()
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


        private System.Collections.Concurrent.ConcurrentQueue<MessageStructure> concurentqueue = null;
        private bool closerequest = false;
        private System.Threading.Thread thread = null;


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

            // start thread
            this.concurentqueue = new System.Collections.Concurrent.ConcurrentQueue<MessageStructure>();
            this.closerequest = false;
            this.thread = new System.Threading.Thread(WriteMessageAsync);
            this.thread.Start();

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
        /// Write Message to log async from queue.
        /// </summary>
        private void WriteMessageAsync(object threadNumber)
        {
            bool closereq = false;

            while (true)
            {
                // read from concurent queue count
                int count = this.concurentqueue.Count;

                // write messages
                for (int i = 0; i < count; i++)
                {
                    MessageStructure ms;
                    bool b = concurentqueue.TryDequeue(out ms);
                    if (b == true)
                    {
                        if (this.IsOpen() == false)
                        {
                            this.Close();
                            this.Open();
                        }
                        this.ToDestination.WriteMessage(ms.datetime, ms.type, ms.categoryid, ms.eventid, ms.message, ms.rawdata);
                    }
                    System.Threading.Thread.Sleep(1);
                }
                 // if is req to close exit
                lock (this)
                {
                    closereq = this.closerequest;
                }
                if (closereq == true) break;

                System.Threading.Thread.Sleep(1);
            }

            return;
        }

        /// <summary>
        /// Write Message to log queue.
        /// </summary>
        /// <param name="datetime">DateTime log creation.</param>
        /// <param name="type">Log entry type.</param>
        /// <param name="categoryid">Category id.</param>
        /// <param name="eventid">Event id.</param>
        /// <param name="message">Message.</param>
        /// <param name="rawdata">Additional rawdata.</param>
        public void WriteMessage(DateTime datetime, LogMessageType type, Int16 categoryid, Int32 eventid, string message, byte[] rawdata)
        {
            if (this.thread != null)
            {
                // write to concurent queue
                MessageStructure ms = new MessageStructure();
                ms.datetime = datetime;
                ms.type = type;
                ms.categoryid = categoryid;
                ms.eventid = eventid;
                ms.message = message;
                ms.rawdata = rawdata;
                this.concurentqueue.Enqueue(ms);
            }
            return;
        }

        /// <summary>
        /// Close log stream.
        /// </summary>
        public void Close()
        {
            if (this.thread != null)
            {
                // wait to empty queue
                while (this.concurentqueue.Count > 0)
                {
                    System.Threading.Thread.Sleep(1);
                }

                // stop async write and exit thread
                lock (this)
                {
                    this.closerequest = true;
                }

                // wait for thread to complete before continuing
                thread.Join();

                this.ToDestination.Close();
            }
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
