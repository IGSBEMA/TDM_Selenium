using OpenQA.Selenium;

public class ImageExtractor
{
    private readonly HttpClient _httpClient;

    public ImageExtractor()
    {
        _httpClient = new HttpClient();
    }

    public async Task<byte[]> ExtractImageAsync(IWebDriver driver, By locator)
    {
        // Finde das Element, das das Bild enthält
        IWebElement element = driver.FindElement(locator);

        // Extrahiere den Wert des "src" Attributes, das die URL zum Bild enthält
        string imageUrl = element.GetAttribute("src");

        // Lade das Bild von der URL herunter
        try
        {
            using (var response = await _httpClient.GetAsync(imageUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    throw new Exception($"Failed to download image from {imageUrl}. Status code: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while downloading the image: {ex.Message}");
            throw;
        }
    }
}
