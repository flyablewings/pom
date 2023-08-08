namespace POMQC.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class Utils
    {
        public static string Normalized(this string s)
        {
            s = s ?? string.Empty;
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex
                .Replace(temp, string.Empty)
                .Replace('\u0111', 'd')
                .Replace('\u0110', 'D')
                .Replace('/', '_')
                .Replace('#', '_')
                .Replace('?', '_')
                .Replace('%', '_');
        }

        public static string ImageFileTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageFileTypes"];
            }
        }

        public static string DocumentFileTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["DocumentFileTypes"];
            }
        }

        public static string Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void WriteOverride(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
        }

        public static void Write(string fileName, string content)
        {
            File.AppendAllText(fileName, content);
        }

        public static string[] Read(string fileName)
        {
            return File.ReadAllLines(fileName, Encoding.UTF8);
        }        

        public static T ConvertTo<T>(this object value)
        {
            if (!object.ReferenceEquals(value, null) && value != DBNull.Value)
            {
                if (Type.GetTypeCode(typeof(T)) == TypeCode.Boolean &&
                    Type.GetTypeCode(value.GetType()) == TypeCode.String)
                {                     
                    object tmp = value.ToString().Equals("1") ||
                        value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase);

                    return (T)tmp;
                }

                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return default(T);
                }
            }

            return default(T);
        }

        public static string ImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, image.RawFormat);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static string ImageToBase64(string imagePath)
        {
            return ImageToBase64(Image.FromFile(imagePath));
        }

        public static string ImageToBase64(Stream stream)
        {
            return ImageToBase64(Image.FromStream(stream, true));
        }

        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public static string Version
        {
            get
            {
                return ConfigurationManager.AppSettings["Version"];
            }
        }

        public static string MailServer
        {
            get
            {
                return ConfigurationManager.AppSettings["MailServer"];
            }
        }

        public static int MailPort
        {
            get
            {
                return ConfigurationManager.AppSettings["MailPort"].ConvertTo<int>();
            }
        }

        public static string MailAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["MailAdmin"];
            }
        }

        public static string MailPwd
        {
            get
            {
                return ConfigurationManager.AppSettings["MailPwd"];
            }
        }

        public static string MailSupport
        {
            get
            {
                return ConfigurationManager.AppSettings["MailSupport"];
            }
        }

        public static string MailError
        {
            get
            {
                return ConfigurationManager.AppSettings["MailError"];
            }
        }

        public static int CacheTime
        {
            get { return ConfigurationManager.AppSettings["CacheTime"].ConvertTo<int>(); }
        }

        public static bool SendMail(string mailServer, int port, string from, string pwd, string[] to, string subject, string content, string[] cc = null, string[] bcc = null, IList<System.Net.Mail.Attachment> attachments = null, bool ssl = false)
        {
            try
            {
                var mail = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient(mailServer);

                mail.From = new System.Net.Mail.MailAddress(from);
                foreach (var email in to)
                {
                    mail.To.Add(email);
                }

                if (cc != null)
                {
                    foreach (var email in cc)
                    {
                        mail.CC.Add(email);
                    }
                }

                if (bcc != null)
                {
                    mail.Bcc.Add(from);
                    foreach (var email in bcc)
                    {
                        mail.Bcc.Add(email);
                    }
                }

                if (attachments != null)
                {
                    foreach (var item in attachments)
                    {
                        item.TransferEncoding = TransferEncoding.QuotedPrintable;
                        mail.Attachments.Add(item);
                    }
                }

                smtp.EnableSsl = ssl;
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = content;
                smtp.Port = port;
                smtp.Credentials = new NetworkCredential(from, pwd);
                smtp.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}