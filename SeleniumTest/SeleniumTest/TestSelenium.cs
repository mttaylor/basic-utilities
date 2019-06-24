using System;
using System.Collections.Generic;
using System.Text;
using SeleniumTest.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTest
{
    class TestSelenium
    {
        static void Main (string[] args) {

            using (var driver = new ChromeDriver())
            {
                // Go to the home page
                driver.Navigate().GoToUrl("http://testing-ground.scraping.pro/login");

                // Get the page elements
                var userNameField = driver.FindElementById("usr");
                var userPasswordField = driver.FindElementById("pwd");
                var loginButton = driver.FindElementByXPath("//input[@value='Login']");

                // Type user name and password
                userNameField.SendKeys("admin");
                userPasswordField.SendKeys("12345");

                // and click the login button
                loginButton.Click();

                // Extract the text and save it into result.txt
                var result = driver.FindElementByXPath("//div[@id='case_login']/h3").Text;
                File.WriteAllText("result.txt", result);

                // Take a screenshot and save it into screen.png
                driver.GetScreenshot().SaveAsFile(@"screen.png", ImageFormat.Png);
            }
       
        }
    }
}
