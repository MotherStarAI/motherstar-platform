using Google.Apis.PagespeedInsights.v5;
using Google.Apis.PagespeedInsights.v5.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using RCommon;
using MotherStar.Platform.Application.Contracts.SEO.Lighthouse;

namespace MotherStar.Platform.Application.SEO.Lighthouse.Services
{
    /// <summary>
    /// Uses Node version of lighthouse tool to run the lighthouse report locally (no need to make Google API call). 
    /// </summary>
    /// <remarks>This will NOT work unless Node is installed on the machine, and Chrome is available. This probably won't work in the cloud
    /// unless we can find an embedded browser - maybe web driver?</remarks>
    public class LighthouseNodeReportGenerator : ILighthouseReportGenerator
    {
        private readonly ILogger<LighthouseNodeReportGenerator> _logger;

        public LighthouseNodeReportGenerator(ILogger<LighthouseNodeReportGenerator> logger)
        {
            _logger = logger;
        }



        public async Task<PagespeedApiPagespeedResponseV5> RunLighthousePageAudits(Guid pageAuditRequestId, IList<LighthouseAuditTypeEnum> auditTypes,
            string url, BrowserOptions browserOptions, string lighthouseApiKey, CancellationToken cancellationToken)
        {
            string browserStrategy = "--emulated-form-factor -none";
            var auditJobs = new List<string>();

            switch (browserOptions)
            {
                case BrowserOptions.Desktop:
                    browserStrategy = "--emulated-form-factor -desktop";
                    break;
                case BrowserOptions.Mobile:
                    browserStrategy = "--emulated-form-factor -mobile";
                    break;
                case BrowserOptions.Unspecified:
                    browserStrategy = "--emulated-form-factor -none";
                    break;
                default:
                    browserStrategy = "--emulated-form-factor -none";
                    break;
            }

            foreach (var auditType in auditTypes)
            {
                switch (auditType)
                {
                    case LighthouseAuditTypeEnum.Accessbility:
                        auditJobs.Add("--only-categories=accessibility");
                        break;
                    case LighthouseAuditTypeEnum.Performance:
                        auditJobs.Add("--only-categories=performance");
                        break;
                    case LighthouseAuditTypeEnum.SEO:
                        auditJobs.Add("--only-categories=seo");
                        break;
                    case LighthouseAuditTypeEnum.BestPractices:
                        auditJobs.Add("--only-categories=best-practices");
                        break;
                    default:
                        break;
                }
            }

            // Beware file system permission issues
            string outputPath = @".\lighthouse-reports";
            string fileName = pageAuditRequestId + ".report.json";
            Directory.CreateDirectory(outputPath);
            //File.Create(outputPath);
            string args = @"/C lighthouse {url} --quiet --chrome-flags= --headless --output=json --output-path=" + outputPath + fileName + browserStrategy + auditJobs.GetDelimitedString(' ');
            ProcessStartInfo cmd = new ProcessStartInfo("cmd.exe");
            cmd.Arguments = args;
            cmd.WindowStyle = ProcessWindowStyle.Hidden;
            Process process = Process.Start(cmd);
            process.WaitForExit();

            _logger.LogDebug("Lighthouse Command line tool: writing file to path: {ouptputPath}", outputPath + fileName);

            string jsonReport = File.ReadAllText(outputPath);

            return await Task.FromResult(JsonConvert.DeserializeObject<PagespeedApiPagespeedResponseV5>(jsonReport));
        }


    }
}
