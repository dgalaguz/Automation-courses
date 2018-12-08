using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Threading;

namespace SeleniumAdvancedUsage
{
    [TestFixture]
    class BrowserOptions
    {
        // Full list of chromium command line switches: https://peter.sh/experiments/chromium-command-line-switches/
        // Chrome Capabilities page: http://chromedriver.chromium.org/capabilities
        // In this example included only the most usefull and most common.

        [Test]
        public void UsingChromeArgs()
        {
            var options = new ChromeOptions();
            options.AddArguments
                (
                    "--start-fullscreen",
                    "--start-maximized",
                    "--disable-infobars",
                    "--disable-extensions",
                    "--allow-running-insecure-content"
                    //"--window-size=1038,735"
                );

            var driver = new ChromeDriver(options);
            driver.Url = "https://mail.fortegrp.net/";
        }

        // List of full FireFox command line options: https://developer.mozilla.org/en-US/docs/Mozilla/Command_Line_Options#Browser
        // List of full FireFox profile options: http://kb.mozillazine.org/About:config_entries

        [Test]
        public void UsingFirefoxArgs()
        {
            var options = new FirefoxOptions();

            options.AddArguments
                (
                    "-headless"
                );

            options.Profile = new FirefoxProfile();
            options.Profile.SetPreference("browser.download.dir", TestContext.CurrentContext.TestDirectory);
            options.Profile.SetPreference("browser.download.folderList", 2);
            options.Profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "image/jpeg");

            var driver = new FirefoxDriver(options);
            driver.Url = "https://unsplash.com/search/photos/test";
            new Actions(driver).MoveToElement(driver.FindElement(By.CssSelector("a[title='Download photo']"))).Click().Perform();

            Thread.Sleep(3000);

            driver.Quit();
        }

        [Test]
        public void AddingExtentionToChrome()
        {
            var options = new ChromeOptions();

            var pathToExtention = Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "Resources",
                "BrowserExtensions",
                "Google Translate.crx");

            options.AddExtension(pathToExtention);
            var driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.google.com/";
            driver.Quit();
        }

        [Test]
        public void MobileDeviceEmulationOnChrome()
        {
            var options = new ChromeOptions();
            options.EnableMobileEmulation("iPhone X");
            //options.EnableMobileEmulation(new ChromeMobileEmulationDeviceSettings());
            var driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.google.com/";
        }

        [Test]
        public void UsingChromeWithProxy()
        {           
            var options = new ChromeOptions();

            var port = 8080;
            var proxy = new Proxy
            {
                HttpProxy = $"localhost:{port}",
                SslProxy = $"localhost:{port}",
                FtpProxy = $"localhost:{port}"
            };

            options.Proxy = proxy;
            options.AddArgument("--disable-web-security");

            var driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Url = "https://mail.fortegrp.net/";

            driver.Quit();
        }

        [Test]
        public void UsingFireFoxWithProxy()
        {
            var options = new FirefoxOptions();

            var port = 8080;
            var proxy = new Proxy
            {
                HttpProxy = $"localhost:{port}",
                SslProxy = $"localhost:{port}",
                FtpProxy = $"localhost:{port}"
            };

            options.Proxy = proxy;
            options.AcceptInsecureCertificates = true;

            var driver = new FirefoxDriver(options);
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.google.com/";

            driver.Quit();
        }
    }
}
