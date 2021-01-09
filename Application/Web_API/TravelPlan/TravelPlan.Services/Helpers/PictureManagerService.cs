using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TravelPlan.Services.Helpers
{
    public class PictureManagerService
    {
        public static void SaveImageToFile(string base64Image, string path)
        {
            byte[] bytes = Convert.FromBase64String(base64Image);
            File.WriteAllBytes(path, bytes);
        }

        public static string LoadImageFromFile(string path)
        {
            string base64Image = "";
            if (!string.IsNullOrEmpty(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                base64Image = Convert.ToBase64String(bytes);
            }
            return base64Image;
        }
    }
}
