using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace MyKanban.test
{
    public class IntegrationTest1
    {
        [Test]
        public void Test_AvvioApp()
        {
            AppiumOptions options = new();
            options.PlatformName = "Windows";
            options.AutomationName = "Windows";
            options.DeviceName = "WindowsPC";
            options.App = "Root";

            var driver = new WindowsDriver(
                new Uri("http://127.0.0.1:4723/"),
                options);

            Assert.That(driver, Is.Not.Null);

            driver.Quit();
        }
    }
}