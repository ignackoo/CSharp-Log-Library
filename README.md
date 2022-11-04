# CSharp-Log-Library
free c# log library to console, debug, email, eventlog, file, xmlfile, sql

I believe that the software development must be simple as is possible.

Library.Log is thus small, modular log system with alerts.

It allows recording exceptions, errors and events in a very simple way.
It can be used both for software development and software in use.
It can be used synchronously and asynchronously.

Events can be logged to the console, debug, eventlog, sqlserver, file, xml file, email.
Alerts can be sent, for example, by email, but also stored separately using other providers.
Logs and alerts have the same timestamp in case of occurrence for easier search
in the system of logs and alerts by time stamp and for simple analysis of logs and alerts.

It is possible to set filters which events according to type will be saved as logs 
and which will be saved as alerts.

WARNING! In the case of sending logs and alerts using an email client
it is good to use a well-configured email account on the email server,
which will not restrict messaging using limits.
Unreliable storage or sending may occur in
logged events or alerts, or can be triggered an exception
and subsequent stopping of sending events and alerts !!

Therefore, it is good to use an email provider only for sending alerts and not all logs
and it is good to use a mail account and a mail server operated in-house.

If necessary, it is easy to modify individual LogTo providers for storing and sending events,
or write a new provider according to the existing ones, depending on the need for sending and storing messages.

Message formats are defined in the providers' code.

ignackoo/CSharp-ConnectionStringBuilder-Library from github must be included in project.


------------------------------------------------------------------------------------------------------------------------------

Log and Alert Stack
------------------------------------------------------------------------------------------------------------------------------
+                                                Log.cs (with log filter)                                                    +
------------------------------------------------------------------------------------------------------------------------------
|                                           LogWithAlert.cs (with alert filter)                                              |
------------------------------------------------------------------------------------------------------------------------------
|                                           (log part)                                         |  |       (alert part)       |
------------------------------------------------------------------------------------------------  ----------------------------
|                                     LogSync.cs/LogAsync.cs                                   |  |  LogSync.cs/LogAsync.cs  |
------------------------------------------------------------------------------------------------  ----------------------------
| LogToConsole / LogToDebug / LogToEventLog / LogToSql / LogToFile / LogToXmlFile / LogToEMail |  |       LogToEMail.cs      |
------------------------------------------------------------------------------------------------  ----------------------------

------------------------------------------------------------------------------------------------------------------------------

Logged Event types
Trace, Debug, Information, Warning, Error, Critical

------------------------------------------------------------------------------------------------------------------------------

LogToFile.cs storage format
                                    cate  event id
date       time         type        gory  code     message           data
YYYY-MM-DD HH:MM:SS:MSC       TRACE XXXXX XXXXXXXX 'message to log.' hexdata[01,02,03]\n\r
YYYY-MM-DD HH:MM:SS:MSC       DEBUG XXXXX XXXXXXXX 'message to log.' hexdata[01,02,03]\n\r
YYYY-MM-DD HH:MM:SS:MSC INFORMATION XXXXX XXXXXXXX 'message to log.' hexdata[01,02,03]\n\r
YYYY-MM-DD HH:MM:SS:MSC     WARNING XXXXX XXXXXXXX 'message to log.' hexdata[01,02,03]\n\r
YYYY-MM-DD HH:MM:SS:MSC       ERROR XXXXX XXXXXXXX 'message to log.' hexdata[01,02,03]\n\r
YYYY-MM-DD HH:MM:SS:MSC    CRITICAL XXXXX XXXXXXXX 'message to log.' hexdata[01,02,03]\n\r

Storage files are created according to time period in seconds, minutes, hours, days, months, years or without period.
If you need to go through the resulting log files, it is good to sort them by date of creation or by
the date of the last modification from the point of view of the operating system.

------------------------------------------------------------------------------------------------------------------------------

LogToXmlFile.cs storage format
                                                                                  
date                   time                     type                    category                  event id code          message                           data
<Date>YYYY-MM-DD</Date><Time>HH:MM:SS:MSC</Time><Type>      TRACE</Type><Category>XXXXX</Category><Event>XXXXXXXX</Event><Message>message to log.</Message><HexData>01,02,03</HexData>\n\r
<Date>YYYY-MM-DD</Date><Time>HH:MM:SS:MSC</Time><Type>      DEBUG</Type><Category>XXXXX</Category><Event>XXXXXXXX</Event><Message>message to log.</Message><HexData>01,02,03</HexData>\n\r
<Date>YYYY-MM-DD</Date><Time>HH:MM:SS:MSC</Time><Type>INFORMATION</Type><Category>XXXXX</Category><Event>XXXXXXXX</Event><Message>message to log.</Message><HexData>01,02,03</HexData>\n\r
<Date>YYYY-MM-DD</Date><Time>HH:MM:SS:MSC</Time><Type>    WARNING</Type><Category>XXXXX</Category><Event>XXXXXXXX</Event><Message>message to log.</Message><HexData>01,02,03</HexData>\n\r
<Date>YYYY-MM-DD</Date><Time>HH:MM:SS:MSC</Time><Type>      ERROR</Type><Category>XXXXX</Category><Event>XXXXXXXX</Event><Message>message to log.</Message><HexData>01,02,03</HexData>\n\r
<Date>YYYY-MM-DD</Date><Time>HH:MM:SS:MSC</Time><Type>   CRITICAL</Type><Category>XXXXX</Category><Event>XXXXXXXX</Event><Message>message to log.</Message><HexData>01,02,03</HexData>\n\r

Storage files are created according to time period in seconds, minutes, hours, days, months, years or without period.
If you need to go through the resulting log files, it is good to sort them by date of creation or by
the date of the last modification from the point of view of the operating system.

------------------------------------------------------------------------------------------------------------------------------

Setting log providers LogToConsole, LogToDebug, LogToEMail, LogToEventLog, LogToFile, LogToSql, LogToXmlFile
is done using connection strings.

LogToConsole  - writes messages to the console screen, therefore it does not need any connection string.

LogToDebug    - writes messages to the visual studio diagnostic debug, therefore it does not need any connection string.

LogToEMail    - sends messages using email
		    Connection String Example = "Smtp=smtp.gmail.com;Ssl=true;Port=587;Sender=EMAIL@gmail.com;Password=abcdefghijklmnop;Recipient=EMAIL@mail.com;PauseMS=10;";
		    Smtp - smtp address
		    Ssl - determines whether secure ssl communication with the smtp server is used
		    Sender - the sender's email for communication with the smtp server
		    Password - password to the sender's email for communication with the smtp server
		    Recipient - email of the recipient of the message
		    PauseMS - delay between sending individual messages in milliseconds

LogToEventLog - saves messages to the Windows event log
      	Connection String Example = "MachineName=.;LogName=LogName;Source=AppName";
		    MachineName - the name of the computer where the reports will be saved
		    LogName - the name of the event log file where the messages will be saved
		    Source - the name of the application that saves the records
     		Windows event log must be initialized before use.

LogToFile     - saves messages in a formatted file
		    Connection String Example = "Path=C:\\Log;Period=second;";
		    Path - is the path to the directory where the log files will be created
		    Period - time interval in which new log files will always be created
			           it can be in seconds, minutes, hours, days, months, years
                 or without a period when only one log file is created

LogToXmlFile  - saves messages in a formatted xml file
		    Connection String Example = "Path=C:\\Log;Period=second;";
		    Path - is the path to the directory where the log files will be created
		    Period - time interval in which new log files will always be created
			           it can be in seconds, minutes, hours, days, months, years
                 or without a period when only one log file is created

LogToSql      - saves the report to the Sql Server database (can be Sql Server or Sql Server localdb file database)
		    Connection String Example = "Server=localhost\\SQLEXPRESS; Database=LogDatabase; Trusted_Connection=yes;";
		    uses a standard connection string to connect to the logging database
		    see https://www.connectionstrings.com/ for sql server
		    Sql Server database must be initialized before use.


According to the existing LogToDestination providers, the user of the Library.Log System
can easily create own providers for storing messages
according to the needs of the user or developer.

------------------------------------------------------------------------------------------------------------------------------

Example of initialization and use of the log system

Library.LogToXmlFile logtoxmlfile = new Library.LogToXmlFile();           // log to xml file
logtoxmlfile.ConnectionString = "Path=C:\\A;Period=second";               // file creation period is every second
Library.LogAsync logtoxmlfileasync = new Library.LogAsync(logtoxmlfile);  // log to xml file async

Library.LogToEMail alerttoemail = new Library.LogToEMail();               // alert to email
alerttoemail.ConnectionString = "Smtp=smtp.gmail.com;Ssl=true;Port=587;Sender=test@gmail.com;Password=abcdefghijklmnop;Recipient=test@mail.com;PauseMS=10;";  // email config
Library.LogAsync alerttoemailasync = new Library.LogAsync(alerttoemail);  // alert to email async

Library.LogWithAlert logwithalert = new Library.LogWithAlert(logtoxmlfileasync, alerttoemailasync);   // alerttoemailasync will be used as alerts, second parameter
logwithalert.EnabledAlertMessages = Library.LogMessageType.Error | Library.LogMessageType.Critical; // message filter for alerts

Library.Log log = new Library.Log(logwithalert);				  // log will be used with alerts
log.EnabledLogMessages = Library.LogMessageType.All;                      // message filter for logs

log.Open()                                                                // open log
log.Trace(1, 10001, "log message", bytearray);                            // log trace
log.Debug(1, 10001, "log message", bytearray);                            // log debug
log.Information(1, 10001, "log message", bytearray);                      // log information
log.Warning(1, 10001, "log message", bytearray);                          // log warning
log.Error(1, 10001, "log message", bytearray);                            // log error with alert error
log.Critical(1, 10001, "log message", bytearray);                         // log critical with alert critical
log.Close()                                                               // close log

