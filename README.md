# Windows-Key-Logger-Service
Windows keylogger service (can store logs in file directory and send this data on email )

Written using “.net core5”. Very simple to use, just add your email's and SMTP port in SendEmailMessage method.

Every 100 characters send logs on email, you can change this by replacing condition on last “if” operator (Worker.cs)

If you want to change directory for file, do it in //Change directory

To compile this in Windows service, watch YouTube tutorial “https://www.youtube.com/watch?v=y64L-3HKuP0&ab_channel=IAmTimCorey”
