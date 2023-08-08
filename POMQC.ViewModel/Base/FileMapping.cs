using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POMQC.ViewModels.Base
{
    public static class FileMapping
    {
        public static string GetMimeType(string extension)
        {
            var mimeType = string.Empty;
            switch (extension)
            {
                case "doc":
                    mimeType = "application/msword";
                    break;
                case "docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "xls":
                    mimeType = "application/ms-excel";
                    break;
                case "xlsx":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "ppt":
                    mimeType = "application/ms-powerpoint";
                    break;
                case "pptx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case "odt":
                    mimeType = "application/vnd.oasis.opendocument.text";
                    break;
                case "ods":
                    mimeType = "application/vnd.oasis.opendocument.spreadsheet";
                    break;
                case "odp":
                    mimeType = "application/vnd.oasis.opendocument.presentation";
                    break;
                default:
                    mimeType = "application/pdf";
                    break;
            }

            return mimeType;
        }
    }
}
