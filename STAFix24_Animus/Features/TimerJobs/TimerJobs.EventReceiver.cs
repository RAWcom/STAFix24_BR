using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Stafix.Features.TimerJobs
{

        [Guid("E6892FD1-BC1A-49D8-8229-45C461243B80")]
        public class TimerJob_ObslugaWiadomosciEventReceiver : SPFeatureReceiver
        {
            public override void FeatureActivated(SPFeatureReceiverProperties properties)
            {
                //SPSite site = properties.Feature.Parent as SPSite;

                //try
                //{
                //    Stafix.TimerJobs.ObslugaWiadomosciTJ.CreateTimerJob(site);
                //    Stafix.TimerJobs.PrzygotowanieWiadomosciTJ.CreateTimerJob(site);
                //}
                //catch (Exception ex)
                //{
                //    ElasticEmail.EmailGenerator.ReportError(ex, site.Url);
                //}
            }

            public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
            {
                //var site = properties.Feature.Parent as SPSite;
                //Stafix.TimerJobs.ObslugaWiadomosciTJ.DelteTimerJob(site);
                //Stafix.TimerJobs.PrzygotowanieWiadomosciTJ.DelteTimerJob(site);
            }
        }
  
}
