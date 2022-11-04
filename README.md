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

To use this library see the readme.txt file for more details.
