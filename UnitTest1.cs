using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using OpenQA.Selenium.Support.Extensions;
using System.IO;
using System.Collections.Generic;

namespace syxsence
{
    [TestClass]
    public class UnitTest1
    {
        public IWebDriver driver;
        public readonly string URL = "https://testteamdev.cloudmanagementsuite.com/";
        public TestContext TestContext;
        public Dictionary<string, int> taskType = new Dictionary<string, int>() 
        { 
            { "Discovery", 1 },
            {"Feature Update", 2},
            { "Software Deploy", 3 },
            { "Patch Scan", 4 },
            { "Patch Deploy", 5 },
            { "Security Scan", 6 },
            { "Reboot", 7 },
            { "Cortex Jobs", 8 } 
        };
       
        [TestInitialize]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(URL);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
        }

        [TestCleanup]
        public void CleanUp()
        {
            //var takeScreenshot = driver.TakeScreenshot();

            //if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
            //{
            //    var filePathToScreenshot = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Screenshot " + DateTime.Now.ToString().Replace(".", "_").Replace(":", "_") + ".png";
            //    takeScreenshot.SaveAsFile(filePathToScreenshot);

            //    if (File.Exists(filePathToScreenshot))
            //    {
            //        TestContext.AddResultFile(filePathToScreenshot);
            //    }
            //}
            driver.Close();
        }
        
        public void SignInSite()
        {
            IWebElement loginField = driver.FindElement(By.XPath("//input[@placeholder='Email Address']"));
            loginField.SendKeys("sergeyr@syxsense.com");

            IWebElement passwordField = driver.FindElement(By.XPath("//input[@placeholder='Password']"));
            passwordField.SendKeys("1");

            IWebElement signInButton = driver.FindElement(By.XPath("//div[@name='btnSignIn']"));
            signInButton.Click();
        }
        public void ChooseTaskTypeToCreate(string task)
        {
            IWebElement createTaskButton = driver.FindElement(By.XPath("//div[@id='id_807']"));
            createTaskButton.Click();

            int x = 0;
            
            foreach (var a in taskType)
            {
                if (taskType.Keys.Contains(task))
                {
                    while (x < taskType[task])
                    {
                        createTaskButton.SendKeys(Keys.ArrowDown);
                        x++;
                    }
                    createTaskButton.SendKeys(Keys.Enter);
                    break;
                }
            }
        }
        
        public void SelectLeftMenuTab(string tabName)
        {
            IWebElement tasksButton = driver.FindElement(By.XPath($"//div[contains(text(), '{ tabName }')]"));
            tasksButton.Click();
        }
        [TestMethod]
        public void FirstTest()
        {
            SignInSite();

            SelectLeftMenuTab("Tasks");

            ChooseTaskTypeToCreate("Reboot");

            IWebElement specialDevicesRB = driver.FindElement(By.XPath("//div[@name='rbSpecificAssets']/div[@name='icon']"));
            specialDevicesRB.Click();

            IWebElement firstTestDevices = driver.FindElement(By.XPath("//div[contains(text(), 'first test')]"));
            firstTestDevices.Click();

            //var firstTestSite = driver.FindElements(By.XPath("//div[contains(text(), 'first test')]"));
            //Assert.IsTrue(firstTestSite[0].Text.Contains("first test"));
            

            var checkBox = driver.FindElements(By.XPath("//div[@role='checkbox']"));
            checkBox[0].Click();

            IWebElement saveButton = driver.FindElement(By.XPath("//div[@id='id_2879']"));
            saveButton.Click();

            IWebElement nextButton = driver.FindElement(By.XPath("//div[@id='id_2706']"));
            nextButton.Click();

            nextButton.Click();

            nextButton.Click();

            nextButton.Click();

            if (driver.FindElement(By.XPath("//div[@id='id_3418']")).Displayed)
            {
                driver.FindElement(By.XPath("//div[@name='ok']")).Click();
            }

            var lastTask = driver.FindElements(By.XPath("//span[contains(text(), 'Reboot')]"));
            lastTask[0].Click();

            

            Assert.AreEqual("Reboot progress", driver.FindElement(By.XPath("//div[@id='id_4396']")).Text);
            
        }
    }
}
