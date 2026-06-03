using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace MyKanban.test
{
    [TestFixture]
    public class LoginPageTests
    {
        private WindowsDriver driver;

        [SetUp]
        public void Setup()
        {
            AppiumOptions options = new();
            options.PlatformName = "Windows";
            options.AutomationName = "Windows";
            options.DeviceName = "WindowsPC";
            options.App = "Root";

            driver = new WindowsDriver(new Uri("http://127.0.0.1:4723/"), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            var finestra = driver.FindElement(MobileBy.Name("MyKanban"));
            driver.SwitchTo().Window(finestra.GetAttribute("NativeWindowHandle"));
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
        }

        [Test]
        public void Test_ElementiVisibili()
        {
            var username = driver.FindElement(MobileBy.AccessibilityId("EntNameAccount"));
            var password = driver.FindElement(MobileBy.AccessibilityId("EntPasswordAccount"));
            var btnLogin = driver.FindElement(MobileBy.AccessibilityId("btnLoginAccount"));

            Assert.That(username.Displayed, Is.True);
            Assert.That(password.Displayed, Is.True);
            Assert.That(btnLogin.Displayed, Is.True);
        }

        [Test]
        public void Test_CampiVuoti_MostraErrore()
        {
            var btnLogin = driver.FindElement(MobileBy.AccessibilityId("btnLoginAccount"));
            btnLogin.Click();

            var popup = driver.FindElement(MobileBy.Name("Errore"));
            Assert.That(popup.Displayed, Is.True);
        }

        [Test]
        public void Test_AccountNonEsistente()
        {
            driver.FindElement(MobileBy.AccessibilityId("EntNameAccount"))
                  .SendKeys("utente_che_non_esiste");
            driver.FindElement(MobileBy.AccessibilityId("EntPasswordAccount"))
                  .SendKeys("password123");

            driver.FindElement(MobileBy.AccessibilityId("btnLoginAccount")).Click();

            var popup = driver.FindElement(MobileBy.Name("Account non esistente"));
            Assert.That(popup.Displayed, Is.True);
        }

        [Test]
        public void Test_PasswordErrata()
        {
            driver.FindElement(MobileBy.AccessibilityId("EntNameAccount"))
                  .SendKeys("Account");
            driver.FindElement(MobileBy.AccessibilityId("EntPasswordAccount"))
                  .SendKeys("passwordsbagliata");

            driver.FindElement(MobileBy.AccessibilityId("btnLoginAccount")).Click();

            var popup = driver.FindElement(MobileBy.Name("Password errata"));
            Assert.That(popup.Displayed, Is.True);
        }

        [Test]
        public void Test_LoginCorretto()
        {
            driver.FindElement(MobileBy.AccessibilityId("EntNameAccount"))
                  .SendKeys("Account");
            driver.FindElement(MobileBy.AccessibilityId("EntPasswordAccount"))
                  .SendKeys("Password");

            driver.FindElement(MobileBy.AccessibilityId("btnLoginAccount")).Click();

            var popup = driver.FindElement(MobileBy.Name("Successo"));
            Assert.That(popup.Displayed, Is.True);
        }

        [Test]
        public void Test_StampaFinestre()
        {
            AppiumOptions options = new();
            options.PlatformName = "Windows";
            options.AutomationName = "Windows";
            options.DeviceName = "WindowsPC";
            options.App = "Root";

            driver = new WindowsDriver(new Uri("http://127.0.0.1:4723/"), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            var finestre = driver.WindowHandles;
            var nomi = string.Join("\n", finestre);

            Assert.Fail(nomi); // stampa tutti gli handle nel risultato
        }
    }
}