using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mob.Core
{
    public static class PictureUtility
    {

        //contentType is not always available 
        //that's why we manually update it here
        //http://www.sfsu.edu/training/mimetype.htm
        public static string GetContentType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".bmp":
                    return "image/bmp";
                case ".gif":
                    return "image/gif";
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                case ".jfif":
                case ".pjpeg":
                case ".pjp":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                default:
                    return string.Empty;
            }
        }

        public static string GetFileExtensionFromContentType(string contentType)
        {
            switch (contentType)
            {
                case "image/bmp":
                    return ".bmp";
                case "image/gif":
                    return ".gif";
                case "image/jpeg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                case "image/tiff":
                    return ".tiff";
                case "image/x-icon":
                    return ".ico";
            }

            return "";
        }

        /// <summary>
        /// Gets image format from the content type
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormatFromContentType(string contentType)
        {
            switch (contentType)
            {
                case "image/bmp":
                    return ImageFormat.Bmp; ;
                case "image/gif":
                    return ImageFormat.Gif;
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "image/png":
                    return ImageFormat.Png;
                case "image/tiff":
                    return ImageFormat.Tiff;
                case "image/x-icon":
                    return ImageFormat.Icon;
            }
            //default jpeg
            return ImageFormat.Jpeg; ;
        }
    }
}