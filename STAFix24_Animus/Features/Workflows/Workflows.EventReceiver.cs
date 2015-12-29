using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Stafix.Features.Workflows
{
    [Guid("c14b0f66-3569-4106-a516-2f818f0a5b86")]
    public class WorkflowsEventReceiver : SPFeatureReceiver
    {
        private string workFlowHistoryListName = "Workflow History";
        private string workFlowTaskListName = "Workflow Tasks";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

            SPSite site = properties.Feature.Parent as SPSite;
            SPWeb web = site.RootWeb;

            try
            {
                //swfStratyZLatUbieglych
                BLL.Workflows.AssociateSiteWorkflow(web, "AB72115A-F93D-44CE-A21C-6B386482643F", "Generator rekordów - Straty z lat ubiegłych", workFlowTaskListName, workFlowHistoryListName);

                //swfObslugaKolejkiWiadomosci
                BLL.Workflows.AssociateSiteWorkflow(web, "37286101-2D91-4114-972A-D2C3CCB2F78C", "Obsługa kolejki wiadomości", workFlowTaskListName, workFlowHistoryListName);

                //swfObslugaKartKontrolnych
                BLL.Workflows.AssociateSiteWorkflow(web, "54743C0D-B49A-495C-A3DE-F46094E60195", "Obsługa kart kontrolnych", workFlowTaskListName, workFlowHistoryListName);

            }
            catch (Exception ex)
            {
                var result = ElasticEmail.EmailGenerator.ReportError(ex, site.Url);
            }
        }
    }
}
