using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumAdvancedUsage
{
    [TestFixture]
    class SwitchingBetweenWindowsTabsAndFrames
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

        // Working with tabs is the same as with windows. WebDriver doesn't see any difference between them.
        [Test]
        public void SwitchingBetweenWindowsAndTabs()
        {
            // Locators that we're going to use.
            By openMultipleWindowsButtonLocator = By.XPath("//button[text() = 'Open Multiple Windows']");
            By pageHeadingLocator = By.CssSelector("h1.wp-heading");

            // Opening the page.
            driver.Navigate().GoToUrl("http://www.leafground.com/pages/Window.html");

            // Clicking on button. Two other windows should open.
            driver.FindElement(openMultipleWindowsButtonLocator).Click();

            // Switching to second window.
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            driver.Manage().Window.Maximize();

            // Verifying page heading.
            Assert.That(driver.FindElement(pageHeadingLocator).Text, Is.EqualTo("Play with HyperLinks"));

            // Switching to third window.
            driver.SwitchTo().Window(driver.WindowHandles[2]);
            driver.Manage().Window.Maximize();

            // Verifying page heading.
            Assert.That(driver.FindElement(pageHeadingLocator).Text, Is.EqualTo("Bond with Buttons"));

            // Closing the third window.
            driver.Close();

            // Closing the second window.
            driver.SwitchTo().Window(driver.WindowHandles[1]);
            driver.Close();

            // Closing the first(original) window.
            driver.SwitchTo().Window(driver.WindowHandles[0]);
            driver.Close();
        }

        /* 
         * An IFrame (Inline Frame) is an HTML document embedded inside another HTML document on a website.
         * The IFrame HTML element is often used to insert content from another source, such as an advertisement, into a Web page.
         * A Web designer can change an IFrame's content without requiring the user to reload the surrounding page.
         */

        [Test]
        public void SwitchingBetweenFrames()
        {
            // Locators that we're going to use.
            By firstFrameLocator = By.CssSelector("iframe[src='default.html']");
            By secondFrameLocator = By.CssSelector("iframe[src='page.html']");
            string frameNestedInSecondFrameName = "frame2";

            By bodyLocator = By.TagName("body");
            By buttonLocator = By.TagName("button");

            // Opening the page.
            driver.Navigate().GoToUrl("http://www.leafground.com/pages/frame.html");

            /*
             * In order to intercat with elements inside the frame you need to switch to that frame first,
             * otherwise element that you looking for will not be found because its not 'visible' to the WebDriver from the 'outside' of the frame.
             * WebDriwer threats each frame as a separate web page.
             */
            driver.SwitchTo().Frame(driver.FindElement(firstFrameLocator));

            // Now we can verify the text inside the frame. As well as interact with any element within this frame.
            Assert.That(driver.FindElement(bodyLocator).Text, Does.Contain("I am inside a frame"));

            var buttonElement = driver.FindElement(buttonLocator);
            buttonElement.Click();

            Assert.That(buttonElement.Text, Is.EqualTo("Hurray! You Clicked Me."));

            // You can return back(out) to original(parent) page like this.
            driver.SwitchTo().DefaultContent();

            /*
             * Nesting the frames is also a common practice.
             * In current example there are two frames contained withit one another.
             * To access the child frame you need switch to parent first then to child.        
             */
            driver.SwitchTo().Frame(driver.FindElement(secondFrameLocator));
            driver.SwitchTo().Frame(frameNestedInSecondFrameName);

            // Now we can access the child frame elements.
            Assert.That(driver.FindElement(bodyLocator).Text, Does.Contain("I am inside a nested frame"));

            buttonElement = driver.FindElement(buttonLocator);
            buttonElement.Click();

            Assert.That(buttonElement.Text, Is.EqualTo("Hurray! You Clicked Me."));
        }

    }
}
