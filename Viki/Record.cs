using System.Net;
using System.Net.Mail;
using static Viki.Config;

namespace Viki
{
    /// <summary>
    /// Compiles and sends a simple bug report.
    /// </summary>
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
                    void Log(string message, Color color)
                    {
                        rtf.SelectionStart = rtf.TextLength;
                        rtf.SelectionLength = 0;

                        rtf.SelectionColor = color;
                        rtf.AppendText(" - "+message+Environment.NewLine);
                        rtf.SelectionColor = rtf.ForeColor;
                        rtf.ScrollToCaret();
                    }
                    Log("Error: " + ex.Message, Color.DarkRed);
                    if (!Feedback)
                        return;
                    Log("Compiling bug report.", Color.Green);
                    var fromAddress = new MailAddress("INVALID_DATA", Environment.MachineName);
                    var toAddress = new MailAddress("INVALID_DATA", "Reciever");
                    const string fromPassword = "INVALID_DATA";
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
                            Log("Sending bug report.", Color.DarkGreen);
                            smtp.Send(message);
                            Log("Success.", Color.DarkGreen);
                        }
                        catch (Exception ex)
                        {
                            Log("Error sending bug report. Message:", Color.DarkRed);
                            Log(ex.Message, Color.Red);
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
