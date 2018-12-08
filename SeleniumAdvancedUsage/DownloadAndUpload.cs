using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace SeleniumAdvancedUsage
{
    [TestFixture]
    class DownloadAndUpload
    {
        IWebDriver driver;
        string downloadDirectoryPath;

        [SetUp]
        public void SetUp()
        {
            // Changing the default download directory is optional.
            downloadDirectoryPath = PrepareDownloadDirectory();
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", downloadDirectoryPath);

            // Creating the driver.
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void UploadSingleFile()
        {
            // Locators that we're going to use.
            By fileInputLocator = By.XPath("(//input[@type='file'])[2]");
            By uploadedfileNamesLocator = By.CssSelector("span.filename");            

            // Opening the page.
            driver.Navigate().GoToUrl("https://files.fm/");

            // Getting the path to file that you want to upload.
            var pathToFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\FilesForUpload\\File1.txt");

            // Sending the path to input.
            driver.FindElement(fileInputLocator).SendKeys(pathToFile);

            Thread.Sleep(500);

            // Verifying that file was successfully added to upload queue.
            Assert.That(driver.FindElement(uploadedfileNamesLocator).Text.Trim(), Does.Contain("File1.txt"));
        }

        [Test]
        public void UploadMultipleFiles()
        {
            // Locators that we're going to use.
            By fileInputLocator = By.XPath("(//input[@type='file'])[2]");
            By uploadedfileNamesLocator = By.CssSelector("span.filename");

            // Opening the page.
            driver.Navigate().GoToUrl("https://files.fm/");

            // Getting the path to folder containing the files you want to upload.
            string resourcesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "FilesForUpload");

            // Getting the path to each file you want to upload.
            string[] filePathes = Directory.GetFiles(resourcesPath);

            // Joining file pathes with new line into single string.
            string filePathesInCorrectFormat = string.Join("\n", filePathes);

            // Sending the path to input
            driver.FindElement(fileInputLocator).SendKeys(filePathesInCorrectFormat);

            Thread.Sleep(500);

            var uploadedFilesElements = driver.FindElements(uploadedfileNamesLocator);

            // Verifying that count of files is correct.
            Assert.That(uploadedFilesElements.Count(), Is.EqualTo(filePathes.Count()));

            // Verifying that each file was added with correct name.
            for (int i = 0; i < filePathes.Count(); i++)
            {
                var uploadedFileElementText = uploadedFilesElements[i].Text.Trim();
                var expectedFileName = Path.GetFileName(filePathes[i]);
                Assert.That(uploadedFileElementText, Does.Contain(expectedFileName));
            }
        }

        [Test]
        public void DownloadFile()
        {
            // Locators that we're going to use.
            By fileDownloadLinkLocator = By.LinkText("Download xls");

            // Opening the page.
            driver.Navigate().GoToUrl("http://www.leafground.com/pages/download.html");

            // Clicking on link to Download the file.
            driver.FindElement(fileDownloadLinkLocator).Click();

            // There are more advanced ways to wait for file to download, but we will use Sleep() for now.
            Thread.Sleep(1000);

            // Checking that file is downloaded.
            Assert.That(Directory.GetFiles(downloadDirectoryPath, "download.xls"), Is.Not.Empty);
        }

        private string PrepareDownloadDirectory()
        {
            var downloadDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Downloads", DateTime.Now.ToString("yy-MM-dd HH-mm-ss"));

            Directory.CreateDirectory(downloadDirectoryPath);

            return downloadDirectoryPath;
        }

    }
}
