using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using static System.Math;
using System.Linq;
using System.Threading;
using System.Drawing;

namespace SeleniumAdvancedUsage
{
    // Actions class reference https://seleniumhq.github.io/selenium/docs/api/dotnet/html/T_OpenQA_Selenium_Interactions_Actions.htm

    [TestFixture]
    class Interactions
    {
        IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            // Creating the driver.
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        /* Drag and Drop use cases:
         * https://demos.telerik.com/kendo-ui/calendar/selection
         * https://demos.telerik.com/kendo-ui/grid/selection
         * https://demos.telerik.com/kendo-ui/listbox/drag-and-drop
         * https://demos.telerik.com/kendo-ui/slider/index
         * https://demos.telerik.com/kendo-ui/sortable/index
         */

        [Test]
        public void DragAndDrop()
        {
            // Locators that we're going to use.
            By dropTargetLocator = By.Id("droptarget");
            By draggableLocator = By.Id("draggable");

            // Opening the page.
            driver.Navigate().GoToUrl("https://demos.telerik.com/aspnet-core/dragdrop/index");

            // Close annoying Cookies popup.
            driver.FindElement(By.XPath("//a[text() = 'Accept Cookies']")).Click();

            // Finding target elements.
            var dropTargetElement = driver.FindElement(dropTargetLocator);
            var draggableElement = driver.FindElement(draggableLocator);

            // Creating instance of Actions class. 
            var actions = new Actions(driver);

            // Drag & Drop small circle into a big one.
            // General way to do drag and drop.
            actions.DragAndDrop(draggableElement, dropTargetElement).Perform();

            // An alternative way.

            //actions.ClickAndHold(draggableElement).Perform();
            //Thread.Sleep(100);
            //actions.MoveToElement(dropTargetElement).Perform();
            //Thread.Sleep(100);
            //actions.Release().Perform();

            Assert.AreEqual("You did great!", dropTargetElement.Text);
        }

        [Test]
        public void OpenLinkInNewTab()
        {
            // Locators that we're going to use.
            By editCategoryLocator = By.CssSelector("img[alt = 'Edit / Text Fields']");
            By pageHeadingLocator = By.CssSelector("h1[itemprop = 'name']");

            // Opening the page.
            driver.Navigate().GoToUrl("http://www.leafground.com/home.html");

            // Clicking on link while holding CTRL key.
            new Actions(driver).KeyDown(Keys.Control).Click(driver.FindElement(editCategoryLocator)).Perform();

            // Switching to newly opened tab.
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            // Checking the heading for newly tab.
            Assert.That(driver.FindElement(pageHeadingLocator).Text.Trim(), Is.EqualTo("Work with Edit Fields"));
        }

        [Test]
        public void Hover()
        {
            // Locators that we're going to use.
            By nameInputLocator = By.Id("age");
            By toolTipLocator = By.CssSelector("div[role='tooltip']");

            // Opening the page.
            driver.Navigate().GoToUrl("http://www.leafground.com/pages/tooltip.html");

            // Hovering on input. Tooltip should appear.
            new Actions(driver).MoveToElement(driver.FindElement(nameInputLocator)).Perform();

            // Checking the tooltip text.
            Assert.That(driver.FindElement(toolTipLocator).Text, Is.EqualTo("Enter your Name"));
        }

        [Test]
        public void DoubleClick()
        {
            // Locators that we're going to use.
            By buttonLocator = By.XPath("//button[text() = 'Double-click me']");
            By messageLocator = By.Id("demo");

            // Opening the page.
            driver.Navigate().GoToUrl("https://www.w3schools.com/tags/tryit.asp?filename=tryhtml5_ev_ondblclick");

            // Switching to frame.
            driver.SwitchTo().Frame("iframeResult");

            // Finding 'Double-click me' button.
            var buttonElement = driver.FindElement(buttonLocator);

            // Performing double click.
            new Actions(driver).DoubleClick(buttonElement).Perform();

            // Message element should appear.
            var messageElement = driver.FindElement(messageLocator);

            // Verifying the message.
            Assert.That(messageElement.Text, Does.Contain("Hello World"));
        }

        // Bonus demonstration just for fun.
        [Test]
        public void SpinMeRightRound()
        {
            // Locators that we're going to use.
            By draggableLocator = By.CssSelector("div.sortable");

            // Opening the page.
            driver.Navigate().GoToUrl("https://marcojakob.github.io/dart-dnd/simple_sortable/");

            Thread.Sleep(1000);

            var draggableElement = driver.FindElement(draggableLocator);

            SpinIt(driver, draggableElement);
        }

        private void SpinIt(IWebDriver driver, IWebElement element)
        {
            new Actions(driver).ClickAndHold(element).MoveByOffset(500, 100).Perform();


            var radius = 100;
            Point oldPoint = new Point((int)Round(radius * Cos(0)), (int)Round(radius * Sin(0)));
            new Actions(driver).MoveByOffset(oldPoint.X, oldPoint.Y).Perform();

            for (int n = 0; n < 20; n++)
            {
                for (int i = 1; i <= 360; i += 5)
                {
                    Point newPoint;

                    int x = (int)Round(radius * Cos(i * PI / 180));
                    int y = (int)Round(radius * Sin(i * PI / 180));

                    newPoint = new Point(x, y);
                    new Actions(driver).MoveByOffset(newPoint.X - oldPoint.X, newPoint.Y - oldPoint.Y).Perform();
                    oldPoint = new Point(x, y);
                }
            }
        }
    }
}
