using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace SeleniumAdvancedUsage
{
    [TestFixture]
    class Cookies
    {
        [Test]
        public void ManagingCookies()
        {
            var driver = new ChromeDriver();
            driver.Url = "https://www.w3schools.com/js/js_cookies.asp";

            var displayAllCookiesButton = By.XPath("//button[text() = 'Display All Cookies']");

            driver.FindElement(By.XPath("//button[text() = 'Create Cookie 1']")).Click();
            driver.FindElement(By.XPath("//button[text() = 'Create Cookie 2']")).Click();

            driver.FindElement(displayAllCookiesButton).Click();

            var alert = driver.SwitchTo().Alert();

            Assert.AreEqual("firstname=John lastname=Smith", alert.Text);

            alert.Accept();

            // Getting all cookies.
            var cookies = driver.Manage().Cookies.AllCookies;

            // Deleting specific cookie.
            driver.Manage().Cookies.DeleteCookieNamed("firstname");

            driver.FindElement(displayAllCookiesButton).Click();

            alert = driver.SwitchTo().Alert();

            Assert.AreEqual(" lastname=Smith", alert.Text);

            alert.Accept();

            // Adding my cookie.

            var serializedCookie = new Dictionary<string, object>
                {
                    {"secure", false},
                    {"isHttpOnly", false},
                    {"name", "firstname"},
                    {"value", "John"},
                    {"domain", "www.w3schools.com"},
                    {"path", "/js"},
                };
            
            Cookie myCustomcookie = Cookie.FromDictionary(serializedCookie);
            driver.Manage().Cookies.AddCookie(myCustomcookie);

            driver.FindElement(displayAllCookiesButton).Click();

            alert = driver.SwitchTo().Alert();

            Assert.AreEqual("firstname=John lastname=Smith", alert.Text);

            alert.Accept();

            // Deleting all cookies.
            driver.Manage().Cookies.DeleteAllCookies();

            driver.FindElement(displayAllCookiesButton).Click();
            alert = driver.SwitchTo().Alert();

            Assert.AreEqual(" ", alert.Text);
            driver.Quit();
        }
    }
}
