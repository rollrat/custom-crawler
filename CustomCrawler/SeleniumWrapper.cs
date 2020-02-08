/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCrawler
{
    public class SeleniumWrapper
    {
        ChromeDriver driver;

        public SeleniumWrapper()
        {
            var driver_path = Path.Combine(Directory.GetCurrentDirectory(), "chromedriver.exe");
            var driver_zip_path = Path.Combine(Directory.GetCurrentDirectory(), "chromedriver.zip");
            if (!File.Exists(driver_path))
            {
                NetCommon.GetDefaultClient().DownloadFile("https://chromedriver.storage.googleapis.com/72.0.3626.7/chromedriver_win32.zip", driver_zip_path);

                var zip = ZipFile.Open(driver_zip_path, ZipArchiveMode.Read);
                zip.Entries[0].ExtractToFile(driver_path);
                zip.Dispose();
                File.Delete(driver_zip_path);
            }
            var chromeDriverService = ChromeDriverService.CreateDefaultService($"{Directory.GetCurrentDirectory()}");
            chromeDriverService.HideCommandPromptWindow = true;
            var chrome = new ChromeOptions();
            chrome.AddArgument("--headless");
            driver = new ChromeDriver(chromeDriverService, chrome);
        }

        public void Navigate(string url, int _wait = 3)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_wait);
            driver.Url = url;
        }

        public string GetHeight()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            return js.ExecuteScript("return document.body.scrollHeight").ToString();
        }

        public void ScrollDown()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("window.scrollTo(0,document.body.scrollHeight);");
        }

        public void Scroll(int position)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript($"window.scrollTo(0,{position});");
        }

        public void ClickXPath(string path)
        {
            driver.FindElementByXPath(path).Click();
        }

        public void ClickName(string path)
        {
            driver.FindElementByName(path).Click();
        }

        public void SendKeyId(string id, string content)
        {
            driver.FindElementById(id).SendKeys(content);
        }

        public void WaitComplete(int _wait = 3)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(_wait)).Until(
                d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public byte[] CanvasToPng(string path)
        {
            var canvas = driver.FindElementByXPath(path);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            var base64 = (string)js.ExecuteScript($"return arguments[0].toDataURL('image/png').substring(21);", canvas);
            return Convert.FromBase64String(base64);
        }

        public string GetHtml()
        {
            return driver.PageSource;
        }

        public void Close()
        {
            driver.Quit();
        }
    }
}
