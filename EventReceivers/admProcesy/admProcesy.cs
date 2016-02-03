using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using BLL;

namespace EventReceivers.admProcesy
{
    using System.Collections.Generic;

    public class admProcesy : SPItemEventReceiver
    {
        private string _ZAKONCZONY = "Zakończony";
        private string _ANULOWANY = "Anulowany";

        public override void ItemAdded(SPItemEventProperties properties)
        {
            this.EventFiringEnabled = false;

            try
            {
                switch (properties.ListItem.ContentType.Name)
                {
                    case "Generowanie formatek rozliczeniowych":
                        BLL.Workflows.StartWorkflow(properties.ListItem, "Generuj formatki rozliczeniowe");
                        break;
                    case "Generowanie formatek rozliczeniowych dla klienta":
                        BLL.Workflows.StartWorkflow(properties.ListItem, "Generuj formatki rozliczeniowe dla klienta");
                        break;
                    case "Obsługa wiadomości":
                        BLL.Workflows.StartSiteWorkflow(properties.Web.Site, "Obsługa wiadomości oczekujących", properties.ListItemId.ToString());
                        break;
                    case "Przygotuj wiadomości z kart kontrolnych":
                        BLL.Workflows.StartSiteWorkflow(properties.Web.Site, "Obsługa kart kontrolnych", properties.ListItemId.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                BLL.Logger.LogEvent(properties.WebUrl, ex.ToString());
                var result = ElasticEmail.EmailGenerator.ReportError(ex, properties.WebUrl.ToString());
                BLL.Tools.Set_Text(properties.ListItem, "enumStatusZlecenia", _ANULOWANY);
                properties.ListItem.Update();
            }

            this.EventFiringEnabled = true;

        }
    }
}
