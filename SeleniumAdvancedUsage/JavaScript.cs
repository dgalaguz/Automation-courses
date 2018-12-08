using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumAdvancedUsage
{
    [TestFixture]
    class JavaScript
    {
        IWebDriver driver;
        IJavaScriptExecutor jexec;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            jexec = (IJavaScriptExecutor)driver;
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Quit();
        }

        [Test]
        public void BasicActions()
        {
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.google.com/";

            var searchField = driver.FindElement(By.Name("q"));
            var searchButton = driver.FindElements(By.Name("btnK"))[1];

            string searchRequest = "42";

            jexec.ExecuteScript($"arguments[0].value = \"{searchRequest}\"", searchField);
            jexec.ExecuteScript($"arguments[0].click()", searchButton);

            var firstGoogleResultText = driver.FindElement(By.ClassName("g")).Text;
            Assert.That(firstGoogleResultText, Does.Contain("42 — ответ на «главный вопрос Жизни, Вселенной и Всего Остального»."));

        }

        [Test]
        public void ScrollIntoView()
        {
            driver.Manage().Window.Maximize();
            driver.Url = "https://demos.telerik.com/aspnet-core/menu/scrollable";
            driver.FindElement(By.XPath("//a[text() = 'Accept Cookies']")).Click();

            var promotionslabel = driver.FindElement(By.XPath("//span[text() = 'Promotions']"));

            jexec.ExecuteScript("arguments[0].scrollIntoView()", promotionslabel);

            Assert.True(promotionslabel.Displayed);
        }

        [Test]
        public void WindowedTable()
        {
            driver.Manage().Window.Maximize();
            driver.Url = "https://bvaughn.github.io/react-virtualized/#/components/List";

            var listContainer = driver.FindElement(By.CssSelector("div[role = 'grid']"));

            var nameToSearch = By.XPath("//div[text() = 'Felix Sailer']/ancestor::div[2]");

            IWebElement elementFound;            

            while (true)
            {
                if (driver.FindElements(nameToSearch).Count != 0)
                {
                    elementFound = driver.FindElement(nameToSearch);
                    break;
                }

                long prevScrollTop = (long)jexec.ExecuteScript("return arguments[0].scrollTop", listContainer);

                jexec.ExecuteScript("arguments[0].scrollBy(0, 100)", listContainer);

                long scrollTop = (long)jexec.ExecuteScript("return arguments[0].scrollTop", listContainer);

                if (prevScrollTop == scrollTop)
                    throw new NoSuchElementException("List does not contain this element");
            }

            jexec.ExecuteScript("arguments[0].scrollIntoView()", elementFound);
        }

    }
}
