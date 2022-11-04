/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  ILogToDestination interface
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Library
{
    public interface ILogToDestination : IDisposable
    {
        // Constructors and destructors

        // Abstract Property signatures.

        // Abstract API Methods signatures.
        void Open();
        bool IsOpen();
        void WriteMessage(DateTime datetime, LogMessageType type, Int16 categoryid, Int32 eventid, string message, byte[] rawdata);
        void Close();
        
        // IDisposable implementation
    }
}
