/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log to sql provider
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public class LogToSql : ILogToDestination
    {
        /**********************************************************************
        ** Constructors and destructors
        */
        #region Constructors and destructors

        /// <summary>
        /// Log to Sql server.
        /// </summary>
        public LogToSql()
        {
            return;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~LogToSql()
        {
           Dispose(false);
        }

        #endregion // Constructors and destructors


        /**********************************************************************
        ** Properties
        */
        #region Properties

        /// <summary>
        /// Connection string to Sql server database.
        /// Example: "Server=localhost\\SQLEXPRESS; Database=LogTest; Trusted_Connection=yes;"
        /// </summary>
        public string ConnectionString { get; set; }

        #endregion // Properties


        private System.Data.SqlClient.SqlConnection sqlserverconnection = null;


        /**********************************************************************
        ** API methods
        */
        #region API

        /// <summary>
        /// Open sql server database log stream.
        /// </summary>
        public void Open()
        {
            this.sqlserverconnection = new System.Data.SqlClient.SqlConnection(this.ConnectionString);
            this.sqlserverconnection.Open();
            return;
        }

        /// <summary>
        /// Is sql server database log stream open.
        /// </summary>
        /// <returns>True if is open and without bugs.</returns>
        public bool IsOpen()
        {
            if (this.sqlserverconnection == null) return (false);
            if (this.sqlserverconnection.State == System.Data.ConnectionState.Closed || this.sqlserverconnection.State == System.Data.ConnectionState.Broken) return (false);
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
            if (this.sqlserverconnection != null)
            {
                System.Data.SqlClient.SqlCommand command =
                    new System.Data.SqlClient.SqlCommand(
                        "INSERT INTO LogTable (Datetime, Type, CategoryId, EventId, Message, Data) VALUES (@Datetime, @Type, @CategoryId, @EventId, @Message, @Data)");

                command.CommandType = System.Data.CommandType.Text;

                // In the command, there are some parameters denoted by @, you can change their value on a condition
                command.Parameters.AddWithValue("@Datetime", datetime);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@CategoryId", categoryid);
                command.Parameters.AddWithValue("@EventId", eventid);
                command.Parameters.AddWithValue("@Message", message);
                command.Parameters.AddWithValue("@Data", rawdata);

                // Set the Connection to the new OleDbConnection.
                command.Connection = this.sqlserverconnection;

                // Open the connection and execute the insert command.
                command.ExecuteNonQuery();
            }
            return;
        }

        /// <summary>
        /// Close sql server database log stream.
        /// </summary>
        public void Close()
        {
            if (this.sqlserverconnection != null)
            {
                this.sqlserverconnection.Close();
                this.sqlserverconnection = null;
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
