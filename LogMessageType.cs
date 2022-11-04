/******************************************************************************
**  Copyright(c) 2022 ignackoo. All rights reserved.
**
**  Licensed under the MIT license.
**  See LICENSE file in the project root for full license information.
**
**  This file is a part of the C# Library Log.
** 
**  Log message types for filters
**
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public enum LogMessageType
    {
        Off = 0,
        Trace = 1, 
        Debug = 2, 
        Information = 4, 
        Warning = 8, 
        Error = 16, 
        Critical = 32,
        All = Trace + Debug + Information + Warning + Error + Critical
    };
}
