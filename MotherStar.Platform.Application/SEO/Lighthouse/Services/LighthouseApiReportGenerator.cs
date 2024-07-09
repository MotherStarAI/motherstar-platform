using Google.Apis.PagespeedInsights.v5;
using Google.Apis.PagespeedInsights.v5.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotherStar.Platform.Application.SEO.Lighthouse.Services
{
    /// <summary>
    /// This generates a lighthouse report using the Google Pagespeed Insights API. An API key is required in the appsettings under {Lighthouse:{ApiKey:"string"}} section.
    /// </summary>
    /// <remarks>There may be limits of around 10,000 requests per month on this API unless the license gets upgraded from free version.</remarks>
    public class LighthouseApiReportGenerator : ILighthouseReportGenerator
    {
        private readonly ILogger<LighthouseApiReportGenerator> _logger;

        public LighthouseApiReportGenerator(ILogger<LighthouseApiReportGenerator> logger)
        {
            _logger = logger;
        }

        private PagespeedapiResource.RunpagespeedRequest PrepareRequest(string url, BrowserOptions browserOptions, IList<PagespeedapiResource.RunpagespeedRequest.CategoryEnum> categories, string lighthouseApiKey)
        {
            var init = new BaseClientService.Initializer
            {
                ApiKey = lighthouseApiKey
            };

            var browserStrategy = PagespeedapiResource.RunpagespeedRequest.StrategyEnum.MOBILE;

            switch (browserOptions)
            {
                case BrowserOptions.Desktop:
                    browserStrategy = PagespeedapiResource.RunpagespeedRequest.StrategyEnum.DESKTOP;
                    break;
                case BrowserOptions.Mobile:
                    browserStrategy = PagespeedapiResource.RunpagespeedRequest.StrategyEnum.MOBILE;
                    break;
                case BrowserOptions.Unspecified:
                    browserStrategy = PagespeedapiResource.RunpagespeedRequest.StrategyEnum.STRATEGYUNSPECIFIED;
                    break;
                default:
                    break;
            }

            var pagespeedService = new PagespeedInsightsService(init);
            //pagespeedService.
            var pagespeedRequest = pagespeedService.Pagespeedapi.Runpagespeed(url);
            //pagespeedRequest.Url = url;
            pagespeedRequest.Strategy = browserStrategy;
            pagespeedRequest.CategoryList = new Google.Apis.Util.Repeatable<PagespeedapiResource.RunpagespeedRequest.CategoryEnum>(categories);
            return pagespeedRequest;
        }

        public async Task<PagespeedApiPagespeedResponseV5> RunLighthousePageAudits(Guid pageAuditRequestId, IList<LighthouseAuditTypeEnum> auditTypes, string url,
            BrowserOptions browserOptions, string lighthouseApiKey, CancellationToken cancellationToken)
        {
            var auditJobs = new List<PagespeedapiResource.RunpagespeedRequest>();
            var categories = new List<PagespeedapiResource.RunpagespeedRequest.CategoryEnum>();

            foreach (var auditType in auditTypes)
            {
                switch (auditType)
                {
                    case LighthouseAuditTypeEnum.Accessbility:
                        categories.Add(PagespeedapiResource.RunpagespeedRequest.CategoryEnum.ACCESSIBILITY);
                        break;
                    case LighthouseAuditTypeEnum.Performance:
                        categories.Add(PagespeedapiResource.RunpagespeedRequest.CategoryEnum.PERFORMANCE);
                        break;
                    case LighthouseAuditTypeEnum.SEO:
                        categories.Add(PagespeedapiResource.RunpagespeedRequest.CategoryEnum.SEO);
                        break;
                    case LighthouseAuditTypeEnum.BestPractices:
                        categories.Add(PagespeedapiResource.RunpagespeedRequest.CategoryEnum.BESTPRACTICES);
                        break;
                    default:
                        break;
                }
            }

            var results = await PrepareRequest(url, browserOptions, categories, lighthouseApiKey)
                .ExecuteAsync(cancellationToken);
            return results;
        }

        /*
        public async Task RunBotForSite(Site site)
        {
            // Get the sitemap
            var pageCmd = await _pageService.GetSiteMapPagesAsync(site.SiteId);

            foreach (var page in pageCmd.DataResult)
            {

                var performance = await _pagespeedService.RunPerformanceAuditAsync((site.UseHttps ? "https://" : "http://") + site.DomainName + page.Path,
                      PagespeedV5AppService.BrowserOptions.Desktop);
                Console.WriteLine("Performance Audit for Page: " + performance.LighthouseResult.FinalUrl);
                foreach (var audit in performance.LighthouseResult.Audits)
                {
                    Console.WriteLine("Audit Key: " + audit.Key);
                    Console.WriteLine("Description: " + audit.Value.Description);
                    Console.WriteLine("Details: " + audit.Value.Details);
                    Console.WriteLine("Display Value: " + audit.Value.DisplayValue);
                    Console.WriteLine("Error Message: " + audit.Value.ErrorMessage);
                    Console.WriteLine("Explanation: " + audit.Value.Explanation);
                    Console.WriteLine("Numeric Value: " + audit.Value.NumericValue.ToString());
                    Console.WriteLine("Scope: " + audit.Value.Score);
                    Console.WriteLine("Score Display mode: " + audit.Value.ScoreDisplayMode);
                    Console.WriteLine("Title: " + audit.Value.Title);
                    Console.WriteLine("Warnings: " + audit.Value.Warnings);
                }

                var seo = await _pagespeedService.RunSeoAuditAsync((site.UseHttps ? "https://" : "http://") + site.DomainName + page.Path,
                    PagespeedV5AppService.BrowserOptions.Desktop);
                Console.WriteLine("SEO Audit for Page: " + seo.LighthouseResult.FinalUrl);
                foreach (var audit in seo.LighthouseResult.Audits)
                {
                    Console.WriteLine("Audit Key: " + audit.Key);
                    Console.WriteLine("Description: " + audit.Value.Description);
                    Console.WriteLine("Details: " + audit.Value.Details);
                    Console.WriteLine("Display Value: " + audit.Value.DisplayValue);
                    Console.WriteLine("Error Message: " + audit.Value.ErrorMessage);
                    Console.WriteLine("Explanation: " + audit.Value.Explanation);
                    Console.WriteLine("Numeric Value: " + audit.Value.NumericValue.ToString());
                    Console.WriteLine("Scope: " + audit.Value.Score);
                    Console.WriteLine("Score Display mode: " + audit.Value.ScoreDisplayMode);
                    Console.WriteLine("Title: " + audit.Value.Title);
                    Console.WriteLine("Warnings: " + audit.Value.Warnings);
                }



                var bestPractices = await _pagespeedService.RunBestPracticesAuditAsync((site.UseHttps ? "https://" : "http://") + site.DomainName + page.Path,
                    PagespeedV5AppService.BrowserOptions.Desktop);
                Console.WriteLine("Best Practices Audit for Page: " + bestPractices.LighthouseResult.FinalUrl);
                foreach (var audit in bestPractices.LighthouseResult.Audits)
                {
                    Console.WriteLine("Audit Key: " + audit.Key);
                    Console.WriteLine("Description: " + audit.Value.Description);
                    Console.WriteLine("Details: " + audit.Value.Details);
                    Console.WriteLine("Display Value: " + audit.Value.DisplayValue);
                    Console.WriteLine("Error Message: " + audit.Value.ErrorMessage);
                    Console.WriteLine("Explanation: " + audit.Value.Explanation);
                    Console.WriteLine("Numeric Value: " + audit.Value.NumericValue.ToString());
                    Console.WriteLine("Scope: " + audit.Value.Score);
                    Console.WriteLine("Score Display mode: " + audit.Value.ScoreDisplayMode);
                    Console.WriteLine("Title: " + audit.Value.Title);
                    Console.WriteLine("Warnings: " + audit.Value.Warnings);
                }

                var accessibility = await _pagespeedService.RunAccessibilityAuditAsync((site.UseHttps ? "https://" : "http://") + site.DomainName + page.Path,
                    PagespeedV5AppService.BrowserOptions.Desktop);
                Console.WriteLine("Accessibility Audit for Page: " + accessibility.LighthouseResult.FinalUrl);
                foreach (var audit in accessibility.LighthouseResult.Audits)
                {
                    Console.WriteLine("Audit Key: " + audit.Key);
                    Console.WriteLine("Description: " + audit.Value.Description);
                    Console.WriteLine("Details: " + audit.Value.Details);
                    Console.WriteLine("Display Value: " + audit.Value.DisplayValue);
                    Console.WriteLine("Error Message: " + audit.Value.ErrorMessage);
                    Console.WriteLine("Explanation: " + audit.Value.Explanation);
                    Console.WriteLine("Numeric Value: " + audit.Value.NumericValue.ToString());
                    Console.WriteLine("Scope: " + audit.Value.Score);
                    Console.WriteLine("Score Display mode: " + audit.Value.ScoreDisplayMode);
                    Console.WriteLine("Title: " + audit.Value.Title);
                    Console.WriteLine("Warnings: " + audit.Value.Warnings);
                }

            }
        }*/
    }
}
