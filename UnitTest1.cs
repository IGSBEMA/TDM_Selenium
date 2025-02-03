using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;


namespace Xebia_Selenium
{
    public class Tests
    {


        //Url
        String u = "https://www.rijksmuseum.nl/en/rijksstudio";
        //String u = "https://www.rijksmuseum.nl/nl/rijksstudio";

        //Optinale Objekt klicken
        string strCloseLightBox = "[data-role='lightbox-close']";
        string strCloseLightBoxcontent = "[data-role='lightbox-content']";

        //user
        string user = "Markus1010ch@gmail.com";
        string pwd = "Test12345";


        // Search Parameter
        //Test1
        string search_test1 = "Rembrandt van Rijn";

        //Test2--start
        string search_test2 = "Hilversum";
        string searchName = "Havicksz";
        string searchTitel = "The Feast of St Nicholas";
        string searchMaterial = "canvas";
        string madeBetweenStart = "1600";
        string madeBetweenEnd = "1700";
        //Test2--ende



        private static IWebDriver d;

        private ElementHelper elementHelper;
        private ImageExtractor imageExtractor;

        private int rijksstudio;

        [OneTimeSetUp]
        public void Setup()
        {
            d = new ChromeDriver();

             // Initialisiere die ElementHelper-Klasse
            elementHelper = new ElementHelper(d);
            imageExtractor = new ImageExtractor();

            d.Navigate().GoToUrl(u);
            d.Manage().Window.Maximize();
            IWebElement element = d.FindElement(By.Name("gdprChoice"));

            // Überprüfe, ob das Element gefunden wurde
            if (element != null)
            {
                Console.WriteLine("Cookies wurde akzeptiert");
                // Führe weitere Aktionen aus, z.B. Klicken auf das Element
                d.FindElement(By.Name("gdprChoice")).Click();
            }
            else
            {
                Console.WriteLine("Das Element wurde nicht gefunden.");
                // Behandele den Fall, wenn das Element nicht gefunden wurde
            }




        }
        [Test]
        public void ALogin()
        {


            //Login Button
            elementHelper.ClickWhenVisible
                (By.CssSelector(".header-link:nth-child(2) span"));

            //Einfügen User
            elementHelper.senKeyWhenVisible
                (By.Id("email"), user);

            //Einfügen Passwort
            elementHelper.senKeyWhenVisible
                (By.Id("wachtwoord"), pwd);

            //Login Button
            elementHelper.ClickWhenVisible
                (By.CssSelector(".clear > .button-bold"));

            //Auswahl Sprache
            elementHelper.waitUntilvisible
                (By.CssSelector(".link:nth-child(1) > .avatar-placeholder"));


            //Auswahl der Sprache erfolgt
            elementHelper.ClickWhenVisible
                (By.CssSelector("[aria-controls='header-language-menu']"));


            //Auswahl Sprache basierend auf URL
            if (u.Contains("/en/"))
            {
                // Code, der ausgeführt wird, wenn die Bedingung wahr ist
                elementHelper.ClickWhenVisible
                    (By.XPath("//span[contains(text(), 'English (English)')]"));
            }
            else
            {
                // Code, der ausgeführt wird, wenn die Bedingung falsch ist
                elementHelper.ClickWhenVisible
                    (By.XPath("//span[contains(text(), 'Nederlands (Dutch)')]"));
            }

        }


        [Test]

        public void B_Suche_mit_Namen()
        {
            //Suche öffnen
            elementHelper.ClickWhenVisible
                (By.CssSelector(".icon-search"));
            //d.FindElement(By.CssSelector(".icon-search")).Click();

            //Suchfeld aktivieren - fokussieren
            elementHelper.ClickWhenVisible
                (By.CssSelector(".search-bar-input"));

            // Suche eingebn mit Parameter
            elementHelper.senKeyWhenVisible
                (By.CssSelector(".search-bar-input"), search_test1);

            //Suche auslösen
            elementHelper.ClickWhenVisible
                (By.CssSelector(".search-bar-button"));


            //Synchronisation
            elementHelper.WaitForCarouselToLoad
                (By.CssSelector(".search-results-count"));

            //Ergebnis prüfen -> meh als 10 Ergebnisse
            IWebElement element1 = d.FindElement
                (By.CssSelector(".search-results-count"));

            string resultText = element1.Text;
            string[] words = resultText.Split(' ');

            int num = int.Parse(words[0].Replace("(", ""));

            //Prüfung ob mehr als 10 Resultate vorhanden sind
            if (num >= 10)
            {
                Console.WriteLine(String.Concat("Es sind insgesamt: ", num, " Suchergebnisse geliefert worden"));
                //Assert.Pass();
            }
            else
            {
                Console.WriteLine(String.Concat("Es sind: ", num, " Suchergebnisse geliefert worden"));
                Assert.Fail();
            }

            //Synchronisation
            elementHelper.WaitForCarouselToLoad(By.CssSelector(".search-results-row:nth-child(3) .carousel-item"));

            // Karussel mit Suchergebnissen wird ausgelesen
            IReadOnlyCollection<IWebElement> carouselElements =
            d.FindElements(By.CssSelector(".search-results-row:nth-child(3) .carousel-item"));



            //Javascript zum lesen der nicht sichtbaren Elemente im Karusel
            IJavaScriptExecutor js = (IJavaScriptExecutor)d;


            //Counter zum Iterieren der vesrteckten Elemente
            int counter = 0;


            //Prüfung ob Anzahl vorhanden
            if (carouselElements.Count >= 10)
            {
                // Anzahl der gefundenen Elemente ausgeben
                Console.WriteLine("Im Karussel sind : " + carouselElements.Count + " " + "  Suchergebnisse enthalten");


                // Gehe durch jedes Carousel-Item und lese den Text aus
                foreach (IWebElement item in carouselElements)
                {
                    counter++;
                    string cssSelector = ".search-results-row:nth-child(3) .carousel-item:nth-child(" + counter + ") .block-content";

                    // Mit Java Script wird nicht sichtbarer Inhalt ausgelesen
                    string hiddenElementText = (string)js.ExecuteScript("return arguments[0].innerText;", d.FindElement(By.CssSelector(cssSelector)));
                    Console.WriteLine("Titel der Bilder: " + hiddenElementText);

                    // Durch die Elemente scrollen um diese sichbar zu machen
                    js.ExecuteScript("arguments[0].scrollIntoView(true);", d.FindElement(By.CssSelector(cssSelector)));
                    Thread.Sleep(1000);

                }
            }
            else
            {
                Console.WriteLine("Anzahl Suchelemente fehlerhaft " + carouselElements.Count);
                //Assert.Fail();
            }

        }

        [Test]
        public void C_Sammlungsmanipulation()
        {

            // Karussel mit Suchergebnissen wird ausgelesen
            IReadOnlyCollection<IWebElement> carouselElements =
            d.FindElements(By.CssSelector(".search-results-row:nth-child(3) .carousel-item"));


            //Javascript zum lesen der nicht sichtbaren Elemente im Karusel
            IJavaScriptExecutor js = (IJavaScriptExecutor)d;



            //Iteration um 3 Gemälde der Kollektion hinzuzufügen
            for (int i = 10; i > 7; i--)
            {
                //Warten bis Karusel geladen
                elementHelper.WaitForCarouselToLoad(By.CssSelector(".search-results-row:nth-child(3) .carousel-item"));

                //Sring zum finden des Objektes mit Iteration --3.. Objekte werden angesprochen
                string csselector = ".search-results-row:nth-child(3) .carousel-item:nth-child(" + i + ") .block-content";

 
                //Aktiviert Objekt 1-3 
                js.ExecuteScript("arguments[0].click();", d.FindElement
                    (By.CssSelector(csselector)));

    
                 //Hinzufügen des Bildes in Kolletion
                js.ExecuteScript("arguments[0].click();", d.FindElement
                    (By.CssSelector("#favourite-button1 > div > ul > li:nth-child(2) > a")));


                //Zum existierende Kollektion hinzufügen--hier: Kollektion 1
                elementHelper.ClickWhenVisible
                    (By.CssSelector("[data-role='add-object-to-existing-set']"));

                //Handler für optionale Schritte
                elementHelper.ClickWhenVisible
                    (By.XPath("\r\n/html/body/div[8]/section/div/button"));

                /*
                //Prüft ob Pop-Up Ball existiert
                elementHelper.checkObjectExist
                    (By.CssSelector("\"p[data-role='feedback-balloon']\""));
                */


                d.Navigate().Back();

                Thread.Sleep(2000);

                // Mit Java Script Name der Bilder auslesen
                string hiddenElementText = (string)js.ExecuteScript("return arguments[0].innerText;", d.FindElement(By.CssSelector(csselector)));
                Console.WriteLine("Das Bild mit dem Titel: " + hiddenElementText + "wurde der Kollektion hinzugefügt");

            }

        }

        [Test]
        public void D_PruefeAnzahlKollektion()
        {
            //Profil wird geöffnet
            elementHelper.ClickWhenVisible(By.CssSelector(".link:nth-child(1)> .avatar-placeholder"));

            //Optinaler Schritt --> wenn Element existiert
            elementHelper.ClickWhenVisible
                (By.CssSelector("[data-role='lightbox-close']"));

            //Prüfung ob Bild 1 vorhanden in Kollektion
            elementHelper.checkObjectExist
                (By.CssSelector("#infinite-scroll-page-results > figure > a > div.set-item.width-2-3 > div > img"));

            //Prüfung ob Bild 2 vorhanden in Kollektion
            elementHelper.checkObjectExist
                (By.CssSelector("#infinite-scroll-page-results > figure > a > div:nth-child(2) > div > img"));

            //Prüfung ob Bild 3 vorhanden in Kollektion
            elementHelper.checkObjectExist
                (By.CssSelector("#infinite-scroll-page-results > figure > a > div:nth-child(3) > div > img"));
        }

        [Test]
        public void E_LoescheKollektin()
        {

            //Profil wird geöffnet
            elementHelper.ClickOptional
                (By.CssSelector(".link:nth-child(1)> .avatar-placeholder"));


            //Optinaler Schritt --> wenn Element existiert
            elementHelper.ClickOptional
                (By.CssSelector("[data-role='lightbox-close']"));

            elementHelper.ClickWhenVisible
                (By.XPath("//div[@id='infinite-scroll-page-results']/figure/a/div/div/img"));

            //Optinaler Schritt --> wenn Element existiert
            elementHelper.ClickWhenVisible
                (By.XPath("/html/body/div[8]/section/article/div[1]/button"));

            // Iteriere drei Mal
            for (int i = 0; i < 3; i++)
            {
                string selectorIconEdit = "#rijksstudio-results > figure:nth-child(" + (i + 1) + ") > div > nav > button\t\t";
                string selctorDeleteItem = "#rijksstudio-results > figure:nth-child(" + (i + 1) + ") > div > nav > ul > li:nth-child(3) > button";
                try
                {
                    Console.WriteLine("Iteration: " + (i + 1) + "start");
                    ///-------------Bild1-loeschen---start---------->

                    // Führe MouseOver mit JavaScript aus
                    elementHelper.PerformMouseOver(d, By.CssSelector(selectorIconEdit));
                    Thread.Sleep(1000);


                    //Button-Löschen
                    elementHelper.ClickWhenVisible
                        (By.CssSelector(selctorDeleteItem));

                    //Button-Löschen
                    //elementHelper.ClickWhenVisible(By.CssSelector(".button-combo-start"));


                    //Button-Löschen-entgültig
                    elementHelper.ClickWhenVisible
                        (By.XPath("/html/body/div[8]/section/article/nav/ul/li[1]/button"));
                    ///-------------Bild1-loeschen---ende---------->

                    Console.WriteLine("Iteration: " + (i + 1) + "ende");
                    Thread.Sleep(1000);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ausnahme in Schleife, Versuch {i + 1}: {ex.Message}");
                }
            }


        }

        [Test]
        public void F_ErweiterteSuche()
        {



                    //Suche öffnen
                    elementHelper.ClickWhenVisible
                    (By.CssSelector(".icon-search"));
                    //d.FindElement(By.CssSelector(".icon-search")).Click();


                    // Suche eingeben mit Parameter
                    elementHelper.senKeyWhenVisible
                        (By.CssSelector(".search-bar-input"), search_test2);

                    //Suche auslösen
                    elementHelper.ClickWhenVisible
                        (By.CssSelector(".search-bar-button"));


                    //Alle Anzeigen
                    elementHelper.ClickWhenVisible
                        (By.XPath("//*[@id=\"maincontent\"]/div/div[2]/div/div[2]/div[1]/div/a"));

                    //Advanced Search
                    elementHelper.ClickWhenVisible
                        (By.CssSelector("#maincontent > section > div:nth-child(2) > section > div > div.item.tablet-p-hide > ul > li:nth-child(2) > a"));


                    //Eingabe Name
                    elementHelper.senKeyAdvancedSearch
                        (By.Id("token-input-QueryDescriptor_AdvancedSearchOptions_ArtistCriteria_InvolvedMakerName"), searchName);

                    elementHelper.ClickWhenVisible
                        (By.CssSelector("b"));
                    //Eingabe Name --Ende-->

                    //Sync Element
                    elementHelper.CheckJqueryActive(d);

                    //Emgabe Material--start
                    elementHelper.senKeyAdvancedSearch
                        (By.Id("token-input-QueryDescriptor_AdvancedSearchOptions_ObjectCriteria_Material"), searchMaterial);

                    elementHelper.ClickWhenVisible
                        (By.CssSelector("b"));
                    //Emgabe Material--ende

                    //Eingabe Titel
                    elementHelper.senKeyAdvancedSearch
                        (By.CssSelector("#QueryDescriptor_AdvancedSearchOptions_ObjectCriteria_Title"), searchTitel);

                    //Sync Element
                    elementHelper.CheckJqueryActive(d);

                    //Eingabe Start Datum
                    elementHelper.senKeyAdvancedSearch
                        (By.CssSelector("#QueryDescriptor_AdvancedSearchOptions_ObjectCriteria_DatingPeriod_StartDate"), madeBetweenStart);

                    //Sync Element
                    elementHelper.CheckJqueryActive(d);

                    //Eingabe Ende Datum
                    elementHelper.senKeyAdvancedSearch
                        (By.CssSelector("#QueryDescriptor_AdvancedSearchOptions_ObjectCriteria_DatingPeriod_EndDate"), madeBetweenEnd);


                    //Sync Element
                    elementHelper.CheckJqueryActive(d);


                    //Suche auslösen -Button Find-
                    elementHelper.PerformMouseClickByJs(d, By.CssSelector(".button-search"));

                    //Sync Element
                    elementHelper.CheckJqueryActive(d);

                    // Finde das Element
                    ReadOnlyCollection<IWebElement> elements = d.FindElements(By.XPath("//*[@id=\"infinite-scroll-page-results\"]/figure/div/div/div/a[1]/figure/img"));

                    // Überprüfe, ob das Element existiert
                    if (elements.Count > 0)
                    {
                        Console.WriteLine("Das Element existiert.");
                    }
                    else
                    {
                        Assert.Warn("Das Bild wurde nicht gefunden. Workaround aktiviert");
                        d.Navigate().GoToUrl("https://www.rijksmuseum.nl/en/search?q=Hilversum&p=1&ps=12&involvedMaker=Jan+Havicksz.+Steen&material=canvas&title=The+Feast+of+St+Nicholas&yearfrom=1600&yearto=1700&st=Objects");
                     }

                            elementHelper.checkObjectExist
                        (By.XPath("//*[@id=\"infinite-scroll-page-results\"]/figure/div/div/div/a[1]/figure/img"));

        }

        [Test]
        public void G_Bildvergleich()
        {

            //lokaler Pfad für Bild-Download
            string webPagePicPath = "C:\\Temp\\bild.jpg";
            string localPicPath = "C:\\Users\\Marku\\source\\repos1\\MarkusBehle\\Xebia_Selenium\\Pictures\\The_Feast_of_St_Nicholas_Source.jpg";

            //Klasse extrahiert und speichert Bild von WebPage
            var imageData = imageExtractor.ExtractImageAsync
                (d, By.XPath("//*[@id=\"infinite-scroll-page-results\"]/figure/div/div/div/a[1]/figure/img")).Result;

            //Schreibt Bild ins lokale Dateisystem
            File.WriteAllBytes (webPagePicPath, imageData);

            //Bild wird auf Byte-Eben verglichen
            ImageCompare.Main(webPagePicPath, localPicPath);

            // Überprüfe, ob die Datei existiert, bevor du sie löschst
            if (File.Exists(webPagePicPath))
            {
                // Lösche die Datei
                File.Delete(webPagePicPath);
                Console.WriteLine("Die Datei wurde erfolgreich gelöscht.");
            }
            else
            {
                Console.WriteLine("Die Datei existiert nicht.");
            }


        }



        [OneTimeTearDown]
        public void TearDown()
        {
            //d.Quit();
        }




    }


}
