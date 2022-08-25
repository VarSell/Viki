using System.Net;
using System.Net.Mail;
namespace Viki
{
    internal class Record
    {
        internal static string CompileBugReport(Exception ex, System.Diagnostics.StackTrace st)
        {
            string report = String.Empty;
            try
            {
                string date = DateTime.Now.ToString("MMMM-dd-yyyy HH:mm:ss");
                var x = st.GetFrame(0);

                string classCrash = x.GetMethod().DeclaringType.ToString() ?? "null";
                string methodCrash = x.GetMethod().ToString() ?? "null";
                string lnCrash = x.GetFileLineNumber().ToString();
                string clnCrash = x.GetFileColumnNumber().ToString();

                //report = $"\n{new string('*', date.Length+4)}\n* {date} *\n{new string('*', date.Length+4)}\n\nClass  : {classCrash}\nMethod : {methodCrash}\nLine   : {lnCrash}\nColumn : {clnCrash}\n\nMessage\n{ex.Message}\n\nStackTrace\n{ex.StackTrace}".Trim();
                report = $@"<h1>An exception occurred</h1>
<h2>Environment</h2>
<ul><b>
<li>MachineName: {Environment.MachineName}</li>
<li>OSVersion: {Environment.OSVersion}</li>
<li>Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}</li>
</ul></b>

<h2>Exception Information</h2>
<ul><b>
<li>Class: {classCrash}</li>
<li>Method: {methodCrash}</li>
<li>Line: {lnCrash}</li>
<li>Column: {clnCrash}</li>
<li>Message:</li>
<ol>{ex.Message}</ol>
<li>Stacktrace:</li>
<ol>{ex.StackTrace}</ol>
</ul></b>
";
                return report;
            }
            catch
            {
                return report;
            }
        }
        internal static async void SendBugReport(Exception ex, System.Diagnostics.StackTrace st, RichTextBox rtf)
        {
            await Task.Run(() =>
            {
                try
                {
                    void Log(string message)
                    {
                        rtf.AppendText(" - "+message+Environment.NewLine);
                        rtf.ScrollToCaret();
                    }
                    Log("Error:" + ex.Message);
                    if (!Form1.feedback)
                        return;
                    Log("Compiling bug report.");
                    var fromAddress = new MailAddress("PowerShell.ODiUM@gmail.com", Environment.MachineName);
                    var toAddress = new MailAddress("AcaelusThorne6430@gmail.com", "Reciever");
                    const string fromPassword = "znebxalsvnrhmtvx";
                    string body = Record.CompileBugReport(ex, st).Replace(Environment.UserName, "User");

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = "[DEBUG]",
                        Body = body,
                        IsBodyHtml = true
                    })
                    {
                        try
                        {
                            Log("Sending bug report.");
                            smtp.Send(message);
                            Log("Success.");
                        }
                        catch (Exception ex)
                        {
                            Log("Error sending bug report.");
                            MessageBox.Show(message.Body);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
        }
    }
}
