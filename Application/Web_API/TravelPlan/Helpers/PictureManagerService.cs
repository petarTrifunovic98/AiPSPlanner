using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TravelPlan.Helpers
{
    public class PictureManagerService
    {
        private static string _folderLocation = "/../../";
        public static string SaveImageToFile(string base64Image, string folderName, int id)
        {
            byte[] bytes = Convert.FromBase64String(base64Image);
            string path = folderName + id.ToString() + ".png";
            File.WriteAllBytes(_folderLocation + path, bytes);
            return path;
        }

        public static string LoadImageFromFile(string path)
        {
            string base64Image = "";
            if (!string.IsNullOrEmpty(path))
            {
                byte[] bytes = File.ReadAllBytes(_folderLocation + path);
                base64Image = Convert.ToBase64String(bytes);
            }
            return base64Image;
        }
    }
}
