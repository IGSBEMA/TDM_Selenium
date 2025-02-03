using System.IO;
using System.Net;

public static class ImageHelper
{
    // Methode zum Herunterladen des Bildes von der URL
    public static byte[] DownloadImage(string imageUrl)
    {
        using (var client = new WebClient())
        {
            return client.DownloadData(imageUrl);
        }
    }

    // Methode zum Speichern eines Bilds als Datei
    public static void SaveImage(byte[] imageBytes, string imagePath)
    {
        File.WriteAllBytes(imagePath, imageBytes);
    }
}

