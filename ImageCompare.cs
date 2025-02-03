using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public class ImageCompare
{
    public static void Main(string pathToImage1, string pathToImage2)
    {
        // Pfade zu den beiden Bildern
        //string pathToImage1 = "Pfad/zu/Bild1.jpg";
        //string pathToImage2 = "Pfad/zu/Bild2.jpg";

        // Lade die beiden Bilder als Byte-Arrays
        byte[] imageData1 = File.ReadAllBytes(pathToImage1);
        byte[] imageData2 = File.ReadAllBytes(pathToImage2);

        // Vergleiche die Byte-Arrays
        bool areEqual = CompareByteArrays(imageData1, imageData2);

        if (areEqual)
        {
            Console.WriteLine("Die Bilder sind identisch.");
        }
        else
        {
            Console.WriteLine("Die Bilder sind unterschiedlich.");
        }
    }

    public static bool CompareByteArrays(byte[] array1, byte[] array2)
    {
        // Wenn die Byte-Arrays unterschiedliche Längen haben, sind sie definitiv unterschiedlich
        if (array1.Length != array2.Length)
            return false;

        // Vergleiche die einzelnen Bytes in den Arrays
        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }

        // Alle Bytes sind gleich, daher sind die Arrays gleich
        return true;
    }

    public static void WaitForPageLoad(IWebDriver driver)
    {
        // Warte, bis das Dokument bereit ist
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        bool isJavaScriptComplete = wait.Until(drv => (bool)((IJavaScriptExecutor)drv).ExecuteScript("return (typeof jQuery !== 'undefined');"));

        // Ausgabe in der Konsole basierend auf dem Rückgabewert der JavaScript-Abfrage
        if (isJavaScriptComplete)
        {
            Console.WriteLine("WaitForPageLoad: Die Seite ist vollständig geladen.");
        }
        else
        {
            Console.WriteLine("WaitForPageLoad: Die Seite ist möglicherweise nicht vollständig geladen.");
        }
    }
    public static void ClearBrowserCache(IWebDriver driver)
    {
        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
        jsExecutor.ExecuteScript("window.localStorage.clear();"); // Leere den localStorage
        jsExecutor.ExecuteScript("window.sessionStorage.clear();"); // Leere den sessionStorage
        //jsExecutor.ExecuteScript("document.cookie.split(';').forEach(function(c) { document.cookie = c.replace(/^\\s+/,'') + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;'; });"); // Lösche alle Cookies
        driver.Navigate().Refresh(); // Aktualisiere die Seite, um sicherzustellen, dass der Cache geleert ist
    }
}
