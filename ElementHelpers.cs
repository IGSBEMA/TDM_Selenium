using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Diagnostics;

public class ElementHelper
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public ElementHelper(IWebDriver driver)
	{
        this.driver = driver;
        this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        if (driver == null)
        {
            throw new ArgumentNullException(nameof(driver), "Der WebDriver darf nicht null sein.");
        }
    }

    public void ClickWhenVisible(By locator)
    {
        try
        {
            // Warte darauf, dass das Element sichtbar ist
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));

            string elementName = locator.ToString();
            element.Click();
            // Klicke auf das Element, nachdem es sichtbar ist
            Console.WriteLine("ClickWhenVisible: " + elementName +" gefunden und geklickt.");
        }
        catch (WebDriverTimeoutException)
        {
            //Console.WriteLine("ClickWhenVisible: Das Element wurde nicht gefunden oder ist nicht sichtbar.");
            string elementName = locator.ToString();
            int currentLineNumber = new StackFrame(0, true).GetFileLineNumber();
            Assert.Warn("ClickWhenVisible: Zeile: "+currentLineNumber+" : " + elementName + " NICHT gefunden.");
        }
    }

    public void ClickOptional(By locator)
    {
        try
        {
            // Warte darauf, dass das Element sichtbar ist
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
            string elementName = locator.ToString();
            element.Click();
            // Klicke auf das Element, nachdem es sichtbar ist
            Console.WriteLine("ClickOptional: " + elementName + " gefunden und geklickt.");
        }
        catch (WebDriverTimeoutException)
        {

        }
    }

    public void waitUntilvisible(By locator)
    {
        try
        {
            string elementName = locator.ToString();
            // Warte darauf, dass das Element sichtbar ist
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));

            Console.WriteLine("waitUntilvisible: "+ elementName+" wurde gefunden und geklickt.");
        }
        catch (WebDriverTimeoutException)
        {
            string elementName = locator.ToString();
            //Console.WriteLine("waitUntilvisible: Das Element wurde nicht gefunden oder ist nicht sichtbar.");
            Assert.Warn("waitUntilvisible: "+ elementName+"nicht gefunden oder ist nicht sichtbar.");
        }
    }

    public void senKeyWhenVisible(By locator, string text)
    {
        try
        {
            // Warte darauf, dass das Element sichtbar ist
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
            element.SendKeys(text);

           Console.WriteLine("senKeyWhenVisible: Das Element wurde gefunden und der Wert: "+text+" wurde eingegeben");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("senKeyWhenVisible: Das Element wurde nicht gefunden oder ist nicht sichtbar.");
            Assert.Fail();
        }
    }

    public void senKeyAdvancedSearch(By locator, string text)
    {
        try
        {
            // Warte darauf, dass das Element sichtbar ist
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));

            foreach (char c in text)
            {
                element.SendKeys(c.ToString());
            }

            Console.WriteLine("c: Das Element wurde gefunden und der Wert: " + text + " wurde eingegeben");

        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("senKeyWhenVisible: Das Element wurde nicht gefunden oder ist nicht sichtbar.");
            Assert.Fail();
        }
    }



    public void WaitForCarouselToLoad(By locator)
    {
        try
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            //By carouselItemLocator = By.CssSelector(".carousel-item");
            By carouselItemLocator = locator;
            wait.Until(ExpectedConditions.ElementExists(carouselItemLocator));
        }
        catch (WebDriverTimeoutException)
        {
            //((Console.WriteLine("WaitForCarouselToLoad: Das Element wurde nicht gefunden oder ist nicht sichtbar."));
            Assert.Warn("WaitForCarouselToLoad: Das Element wurde nicht gefunden oder ist nicht sichtbar.");
        }

    }

    public void checkObjectExist(By locator)
    {
        try
        {
            // Warte darauf, dass das Element sichtbar ist
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
            
            Console.WriteLine("checkObjectExist: Das Element wurde gefunden");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("checkObjectExist: Das Element wurde nicht gefunden");
            Assert.Fail();
        }
    }

    public void checkObjectExistWithoutFail(By locator)
    {
        try
        {
            // Warte darauf, dass das Element sichtbar ist
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));

            Console.WriteLine("checkObjectExist: Das Element wurde gefunden");
        }
        catch (WebDriverTimeoutException)
        {
            string elementName = locator.ToString();
            int currentLineNumber = new StackFrame(0, true).GetFileLineNumber();
            Assert.Warn("checkObjectExistWithoutFail: Zeile: " + currentLineNumber + " : " + elementName + " NICHT gefunden.");
        }
    }

    /// <summary>
    /// Führt einen Mouseover-Effekt über das angegebene Element aus.
    /// </summary>
    /// <param name="driver">Der WebDriver, der für die Interaktion mit dem Browser verwendet wird.</param>
    /// <param name="locator">Der By-Locator, der das Element identifiziert, über dem der Mouseover-Effekt ausgeführt werden soll.</param>
    public void PerformMouseOver(IWebDriver driver, By locator)
    {
        try
        {
            string elementName = locator.ToString();

            // Erstelle eine Instanz des JavascriptExecutor
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            //Ergebnis prüfen -> meh als 10 Ergebnisse
            IWebElement element = driver.FindElement(locator);

            // Führe das JavaScript zum Auslösen des MouseOver-Ereignisses aus
            js.ExecuteScript("arguments[0].dispatchEvent(new MouseEvent('mouseover', { bubbles: true }));", element);

        Console.WriteLine("PerformMouseOver: "+ elementName +" wurde gefunden");

        }
        catch(WebDriverException)
        {
        Assert.Warn("PerformMouseOver: Das Element wurde nicht gefunden");
       

        }
    }

    public void PerformMouseClickByJs(IWebDriver driver, By locator)
    {
        try
        {
            string elementName = locator.ToString();
            //Javascript zum Klicken auf Element
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", driver.FindElement(locator));
            Console.WriteLine("PerformMouseClickByJs: " + elementName + " wurde gefunden");
        }
        catch (WebDriverException)
        {
            Assert.Warn("PerformMouseClickByJs: Das Element wurde nicht gefunden");
        }
     
    }

    public void CheckJqueryActive(IWebDriver driver)
    {
        try
        {
            // Warte darauf, dass jQuery zu Ende ausgeführt wird
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => (bool)((IJavaScriptExecutor)driver).ExecuteScript("return jQuery != undefined && jQuery.active == 0"));

            // jQuery wurde abgeschlossen
            Console.WriteLine("jQuery wurde abgeschlossen.");
        }
        catch (WebDriverException)
        {
            Console.WriteLine("jQuery wurde nicht abgeschlossen.");
        }
    }

}

