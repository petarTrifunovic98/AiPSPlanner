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

        public static string SaveImageToFileGenerateId(string base64Image, string folderName, int idPart)
        {
            byte[] bytes = Convert.FromBase64String(base64Image);
            string counterValue = "0";

            string counterPath = folderName + "Images\\" + "idCounter.txt";
            FileInfo file = new System.IO.FileInfo(_folderLocation + counterPath);
            file.Directory.Create();
            if (File.Exists(_folderLocation + counterPath))
                counterValue = File.ReadAllText(_folderLocation + counterPath);
            else
                File.Create(_folderLocation + counterPath).Close();

            File.WriteAllText(_folderLocation + counterPath, (int.Parse(counterValue) + 1).ToString());

            string picturePath = folderName + "Images\\" + idPart.ToString() + counterValue + ".png";
            File.WriteAllBytes(_folderLocation + picturePath, bytes);
            return picturePath;
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
