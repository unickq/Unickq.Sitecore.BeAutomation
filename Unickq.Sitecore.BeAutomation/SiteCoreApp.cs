using System;
using System.Diagnostics;
using NLog;
using OpenQA.Selenium;
using Seleniq.Attributes;
using Seleniq.Core;
using Unickq.Sitecore.BeAutomation.Components;
using Unickq.Sitecore.BeAutomation.Utils;
using LogLevel = NLog.LogLevel;

namespace Unickq.Sitecore.BeAutomation
{
    public class SiteCoreApp : SeleniqBase, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetLogger("Sc:Main");

        static SiteCoreApp()
        {
#if DEBUG
            CommonUtils.SetLogger(LogLevel.Trace);
#else
            CommonUtils.SetLogger(LogLevel.Info);
#endif
        }

        private bool IsLoggedIn => Driver.FindElements(By.CssSelector(".sc-list")).Count == 1;


        /// <summary>Initializes a new instance of the <see cref="SiteCoreApp"/> class.</summary>
        /// <param name="driver">WebDriver.</param>
        /// <param name="baseUrl">The base URL of Sitecore app.</param>
        public SiteCoreApp(IWebDriver driver, string baseUrl)
        {
            BaseUrl = baseUrl;
            CommonUtils.ValidateUrlIsRelatedToSitecore(baseUrl);
            InitWebDriver(driver);
            Driver.Manage().Window.Maximize();
            Navigate("/sitecore");
        }

        protected override string BaseUrl { get; }

        /// <summary>Initializes a new instance of the <see cref="SiteCoreApp"/> class.</summary>
        /// <param name="wedDriverFunc">  Function to initialize WebDriver.</param>
        /// <param name="baseUrl">The base URL of Sitecore app.</param>
        public SiteCoreApp(Func<IWebDriver> wedDriverFunc, string baseUrl)
        {
            BaseUrl = baseUrl;
            CommonUtils.ValidateUrlIsRelatedToSitecore(baseUrl);
            InitWebDriver(wedDriverFunc.Invoke());
            Logger.Debug($"Initialized {Driver.GetType().Name}");
            Driver.Manage().Window.Maximize();
            Navigate("/sitecore");
        }

        /// <summary>Logs user in Sitecore.</summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="ScException">Unable to login. Page title - {Driver.Title}</exception>
        public SiteCoreApp LogIn(string username, string password)
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Navigate("/sitecore/login");
                    Logger.Info($"Logging in as {username}");
                    Driver.FindElement(By.Id("UserName")).SendKeys(username);
                    Driver.FindElement(By.Id("Password")).SendKeys(password);
                    Driver.FindElement(By.CssSelector("#login > input")).Click();
                }
            }
            catch (Exception)
            {
                throw new ScException($"Unable to login. Page title - {Driver.Title}");
            }


            return this;
        }

        /// <summary>Goes to specific Sitecore page.</summary>
        /// <typeparam name="T">SeleniqBasePage</typeparam>
        /// <returns>T SeleniqBasePage</returns>
        public T GoTo<T>() where T : SeleniqBasePage, new()
        {
            return PerformWithTimer(() => InstanceOf<T>(NavigateBy.PageUrl), $"{typeof(T).Name} initialization time");
        }


        /// <summary>Logs user out.</summary>
        public void LogOut()
        {
            if (IsLoggedIn)
            {
                Logger.Debug($"Logging out");
                new GlobalHeader().ClickLogOut();
            }
        }

        private void Navigate(string part)
        {
            if (!Driver.Url.EndsWith(part))
            {
                Logger.Debug($"Navigating to {part}");
                PerformWithTimer(() => Driver.Navigate().GoToUrl(string.Concat(BaseUrl, part)), $"Load time of {part}");
            }
        }

        /// <summary>Performs Action the with timer.</summary>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        public void PerformWithTimer(Action action, string message)
        {
            var timer = new Stopwatch();
            timer.Start();
            action.Invoke();
            timer.Stop();
            Logger.Debug($"{message} is {timer.ElapsedMilliseconds} milliseconds");
        }

        /// <summary>Performs Func&lt;T&gt; with timer.</summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="action">The Func.</param>
        /// <param name="message">The message.</param>
        /// <returns>Type</returns>
        public T PerformWithTimer<T>(Func<T> action, string message)
        {
            var timer = new Stopwatch();
            timer.Start();
            var t = action.Invoke();
            timer.Stop();
            Logger.Debug($"{message} is {timer.ElapsedMilliseconds} milliseconds");
            return t;
        }

        /// <summary>Kills browser.</summary>
        public void Dispose()
        {
            Logger.Debug("Killing WebDriver");
            Driver.Quit();
        }
    }
}
