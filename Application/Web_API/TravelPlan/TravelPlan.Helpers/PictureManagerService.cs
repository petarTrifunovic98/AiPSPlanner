using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TravelPlan.Helpers
{
    public class PictureManagerService
    {
        private readonly static string _folderLocation = @"..\..\";
        public static string SaveImageToFile(string base64Image, string folderName, int id)
        {
            byte[] bytes = Convert.FromBase64String(base64Image);
            string path = folderName + "Images\\" + id.ToString() + ".png";
            FileInfo file = new System.IO.FileInfo(_folderLocation + path);
            file.Directory.Create();
            File.WriteAllBytes(file.FullName, bytes);
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
