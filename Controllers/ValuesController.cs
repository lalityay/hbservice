using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace hbservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddExtensions(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ModHeader.crx"));
            List<string> customPages = new List<string>();
            //customPages.Add("company/profile?id=100369");
            //customPages.Add("company/stock?id=100369");
            //customPages.Add("company/corporateStructure?Id=100369");
            //customPages.Add("company/capitalOfferings?ID=100369");
            //customPages.Add("company/rankingReport?ID=100369");
            customPages.Add("company/detailedRatesReport?ID=100369");
            customPages.Add("company/rateSpecials?ID=100369");
            //customPages.Add("company/rateSpecials?ID=100369");
            //customPages.Add("company/branchCompetitors?ID=100369");
            var watch = System.Diagnostics.Stopwatch.StartNew();

            RemoteWebDriver driver = new RemoteWebDriver(new Uri("http://172.17.0.2:4445/wd/hub/"), options.ToCapabilities(), TimeSpan.FromSeconds(600));
            driver.Navigate().GoToUrl("chrome-extension://cilllgpaahohggfdioeinnncmaecaeni/icon.png");
            var UserCookie = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cookie.txt"));

            (driver).ExecuteScript("localStorage.setItem('profiles', JSON.stringify({Cookie:'" + UserCookie + "'}));");
            List<string> manifestList = new List<string>();
            for (int i = 0; i < customPages.Count; i++)
            {
                var blankURL = "about:blank";
                driver.Navigate().GoToUrl(blankURL);
                //driver.Navigate().GoToUrl(string.Format("https://platform.midevcld.spglobal.com/web/client?auth=inherit#{0}&rbExportType=Pdf&kss=&ReportBuilderQuery=1", customPages[i]));
                driver.Navigate().GoToUrl(string.Format("https://www.snlnet.com/web/client?auth=inherit#{0}&rbExportType=Pdf&kss=&ReportBuilderQuery=1", customPages[i]));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                string manifest = "Not Set";
                try
                {
                    manifest = driver.FindElement(By.Name("reportManifest")).GetAttribute("innerHTML");
                    manifestList.Add(manifest);
                }
                catch(Exception ex)
                {
                    manifestList.Add("Failed for " + customPages[i]);
                }
            }
            driver.Close();
            driver.Dispose();
            watch.Stop();
            manifestList.Add(", Total Elapsed seconds:" + watch.Elapsed.TotalSeconds);
            return manifestList;
        }


    }
}
