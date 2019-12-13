using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace eTutor.Core.Helpers
{
    public class FileValidations
    {
        public static bool CheckIfFileIsImage(string fileName)
        {
            string[] extensions = {"png", "jpg", "jpeg", "bmp", "gif", "tiff",".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tiff"};
            string fileExtension = Path.GetExtension(fileName);
            return extensions.Any(e => e == fileExtension);
        }
    }
}