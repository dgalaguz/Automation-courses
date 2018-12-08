using System;
using System.Collections.Generic;
using System.IO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using NUnit.Framework;

namespace SeleniumAdvancedUsage
{
    [TestFixture]
    public class VariousUsefulFeatures
    {
        [Test]
        public void TakingAScreenshot()
        {
            // Creating the driver.
            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            // Opening the page.
            driver.Navigate().GoToUrl("http://www.google.com");

            // Taking Screenshot.
            var screenshot = driver.GetScreenshot();

            // Saving a screenshoot to the desktop. You can save them to any directory you want.          
            var destinationPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var screenshotPath = Path.Combine(destinationPath, "screenshot.png");
            screenshot.SaveAsFile(screenshotPath);

            // Adding screenshot to the test output.
            TestContext.AddTestAttachment(screenshotPath);

            driver.Quit();
        }

        [Test]
        public void AlertHandling()
        {
            // Locators that we're going to use.
            By confirmBoxButtonLocator = By.XPath("//button[text() = 'Confirm Box']");
            By alertBoxButtonLocator = By.XPath("//button[text() = 'Alert Box']");
            By resultLocator = By.Id("result");

            // Creating the driver.
            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            // Opening the page.
            driver.Navigate().GoToUrl("http://www.leafground.com/pages/Alert.html");

            // Clicking on 'Alert Box' button.
            driver.FindElement(alertBoxButtonLocator).Click();

            // Switching to alert.
            var simpleAlert = driver.SwitchTo().Alert();

            // Verifying the alert text.
            Assert.That(simpleAlert.Text, Is.EqualTo("I am an alert box!"));

            // Accepting the alert.
            simpleAlert.Accept();

            // Clicking on 'Confirm Box' button.
            driver.FindElement(confirmBoxButtonLocator).Click();

            // Switching to alert.
            var confirmationAlert = driver.SwitchTo().Alert();

            // Verifying the alert text.
            Assert.That(confirmationAlert.Text, Is.EqualTo("Press a button!"));

            // Accepting the alert.
            confirmationAlert.Accept();

            var resultText = driver.FindElement(resultLocator).Text;

            // Verifying the result.
            Assert.That(resultText, Is.EqualTo("You pressed OK!"));            

            driver.Quit();
        }

        // Headless Testing is a testing which is performed on Headless Browser which is a browser simulation program that does not have a graphical user interface.

        [Test]
        public void HeadlessTestExecution()
        {
            // Locators that we're going to use.
            By searchFieldLocator = By.Name("q");
            By googleResultsLocator = By.ClassName("g");

            // Setting the options to start the browser with.
            var chromeOptions = new ChromeOptions();

            // Uncomment the line below to run headless.
            //chromeOptions.AddArguments("--headless");
            
            // Creating the driver.
            var driver = new ChromeDriver(chromeOptions);
            driver.Manage().Window.Maximize();

            // Opening the page.
            driver.Navigate().GoToUrl("http://www.google.com");

            // Enter the value to the search field.
            driver.FindElement(searchFieldLocator).SendKeys("42");
            driver.FindElement(searchFieldLocator).SendKeys(Keys.Enter);

            // Reading the text from the first google result.
            var firstGoogleResultText = driver.FindElement(googleResultsLocator).Text;

            // Verifying that first google result contains expected text.
            Assert.That(firstGoogleResultText, Does.Contain("42 — ответ на «главный вопрос Жизни, Вселенной и Всего Остального»."));

            driver.Quit();
       }
    }
}
