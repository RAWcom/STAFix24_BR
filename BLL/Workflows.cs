using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System.Globalization;
using System.Xml.Linq;
using System.Diagnostics;

namespace BLL
{
    public class Workflows
    {
        public const string workflowHistoryListName = "Workflow History";
        public const string workflowTaskListName = "Workflow Tasks";

        public static SPWorkflow StartWorkflow(SPListItem listItem, string workflowName)
        {
            SPWorkflow wf = null;

            try
            {
                SPWorkflowManager manager = listItem.Web.Site.WorkflowManager;
                SPWorkflowAssociationCollection objWorkflowAssociationCollection = listItem.ParentList.WorkflowAssociations;
                foreach (SPWorkflowAssociation objWorkflowAssociation in objWorkflowAssociationCollection)
                {
                    if (String.Compare(objWorkflowAssociation.Name, workflowName, true) == 0)
                    {

                        //We found our workflow association that we want to trigger.

                        //Replace the workflow_GUID with the GUID of the workflow feature that you
                        //have deployed.

                        try
                        {
                            //result = manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, SPWorkflowRunOptions.SynchronousAllowPostpone);
                            wf = manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, true);

                            //ElasticEmail.EmailGenerator.SendMail("StartWorkflow: " + workflowName + " " + wf.InternalState.ToString(), "");

                            //manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, true);
                            //The above line will start the workflow...
                        }
                        catch (Exception)
                        { }


                        break;
                    }
                }
            }
            catch (Exception)
            { }

            return wf;
        }

        public static SPWorkflow StartSiteWorkflow(SPSite site, string workflowName, string initParameters)
        {
            SPWorkflow wf = null;

            using (SPWeb web = site.OpenWeb()) // get the web
            {
                //find workflow to start
                var assoc = web.WorkflowAssociations.GetAssociationByName(workflowName, CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(initParameters)) initParameters = string.Empty;

                //this is the call to start the workflow
                //result = site.WorkflowManager.StartWorkflow(null, assoc, initParameters.ToString() , SPWorkflowRunOptions.SynchronousAllowPostpone);
                wf = site.WorkflowManager.StartWorkflow(site, assoc, initParameters.ToString(), SPWorkflowRunOptions.SynchronousAllowPostpone);

                if (!wf.InternalState.ToString().Equals("Completed"))
                {
                    ElasticEmail.EmailGenerator.SendMail(string.Format(@"WorkflowManager({0})", site.RootWeb.ToString()), "InternalState=" + wf.InternalState.ToString());
                }
            }

            return wf;
        }

        public static void AssociateSiteWorkflow(SPWeb web, string workflowTemplateBaseGuid, string workflowAssociationName, string workFlowTaskListName, string workFlowHistoryListName)
        {
            SPWorkflowTemplateCollection workflowTemplates = web.WorkflowTemplates;
            SPWorkflowTemplate workflowTemplate = workflowTemplates.GetTemplateByBaseID(new Guid(workflowTemplateBaseGuid));

            if (workflowTemplate != null)
            {
                // Create the workflow association
                SPList taskList = web.Lists[workFlowTaskListName];
                SPList historyList = web.Lists[workFlowHistoryListName];
                SPWorkflowAssociation workflowAssociation = web.WorkflowAssociations.GetAssociationByName(workflowAssociationName, CultureInfo.InvariantCulture);

                if (workflowAssociation == null)
                {
                    workflowAssociation = SPWorkflowAssociation.CreateWebAssociation(workflowTemplate, workflowAssociationName, taskList, historyList);
                    workflowAssociation.AllowManual = true;
                    //workflowAssociation.Enabled = true;
                    web.WorkflowAssociations.Add(workflowAssociation);
                }
            }
        }

        /// <summary>
        /// przykładowe wywołania
        /// string enhancedAssociationData = AddVariableToWorkflowAssociationData(wrkFl.AssociationData, "MyVeryOwnAssociationParameter", "SomeCrazyValue!!");
        /// enhancedAssociationData = AddVariableToWorkflowAssociationData(enhancedAssociationData, "AnotherAssociationParameter", "AndYetAnotherCrazyValue");
        /// siteColl.WorkflowManager.StartWorkflow(record, wrkFl, enhancedAssociationData, true);
        /// 
        /// http://blog.degree.no/2011/08/sharepoint-2010-programmatically-set-workflow-initiation-parameters/
        /// </summary>
        /// <param name="originalAssociationData"></param>
        /// <param name="variableName"></param>
        /// <param name="variableValue"></param>
        /// <returns></returns>
        public static string AddVariableToWorkflowAssociationData(string originalAssociationData, string variableName, string variableValue)
        {
            XNamespace dfs = "http://schemas.microsoft.com/office/infopath/2003/dataFormSolution";
            XNamespace d = "http://schemas.microsoft.com/office/infopath/2009/WSSList/dataFields";

            var associationDataXml = XElement.Parse(originalAssociationData);
            if (associationDataXml == null
                || associationDataXml.Element(dfs + "dataFields") == null
                || associationDataXml.Element(dfs + "dataFields").Element(d + "SharePointListItem_RW") == null
                || associationDataXml.Element(dfs + "dataFields").Element(d + "SharePointListItem_RW").Element(d + variableName) == null)
            {
                Debug.WriteLine("AddVariableToWorkflowAssociationData() unable to find node '" + variableName + "' in association data");
                return originalAssociationData;
            }
            Debug.WriteLine("Setting workflow parameter '" + variableName + "' to '" + variableValue + "'");
            var nodeToChange = associationDataXml.Element(dfs + "dataFields").Element(d + "SharePointListItem_RW").Element(d + variableName);
            nodeToChange.SetValue(variableValue);
            return associationDataXml.ToString();
        }

        public static void EnsureWorkflowAssociation(SPList list, string workflowTemplateName, string associationName, bool allowManual, bool startCreate, bool startUpdate)
        {
            var web = list.ParentWeb;
            var lcid = (int)web.Language;
            var defaultCulture = new CultureInfo(lcid);

            // Create the workflow association
            SPList taskList = EnsureListExist(web, workflowTaskListName);
            SPList historyList = EnsureListExist(web, workflowHistoryListName);

            var workflowAssociation =
                list.WorkflowAssociations.Cast<SPWorkflowAssociation>().FirstOrDefault(i => i.Name == associationName);
            if (workflowAssociation != null)
            {
                list.WorkflowAssociations.Remove(workflowAssociation);
                list.Update();
            }

            var template = web.WorkflowTemplates.GetTemplateByName(workflowTemplateName, defaultCulture);
            var association = SPWorkflowAssociation.CreateListAssociation(template, associationName, taskList, historyList);

            association.AllowManual = true;
            association.AutoStartChange = true;
            association.AutoStartCreate = true;

            list.WorkflowAssociations.Add(association);
            list.Update();

            association = list.WorkflowAssociations[association.Id];
            association.AllowManual = allowManual;
            association.AutoStartChange = startUpdate;
            association.AutoStartCreate = startCreate;
            association.AssociationData = "<Dummy></Dummy>";
            association.Enabled = true;
            list.WorkflowAssociations.Update(association);
            list.Update();

            Debug.WriteLine("Ensure.List.Workflow: " + associationName + " associated");

        }

        private static SPList EnsureListExist(SPWeb web, string listName)
        {
            SPList list = web.Lists.TryGetList(listName);
            if (list == null)
            {
                list = BLL.Workflows.CreateTaskList(web, listName);
            }
            return list;
        }

        public static SPList CreateTaskList(SPWeb web, string listName)
        {
            Guid listGuid = web.Lists.Add(listName, string.Empty, SPListTemplateType.Tasks);
            SPList list = web.Lists.GetList(listGuid, false);
            list.Hidden = false;
            list.Update();
            return list;
        }

        public static SPList CreateHistoryListy(SPWeb web, string listName)
        {
            Guid listGuid = web.Lists.Add(listName, string.Empty, SPListTemplateType.WorkflowHistory);
            SPList list = web.Lists.GetList(listGuid, false);
            list.Hidden = false;
            list.Update();
            return list;
        }

    }
}
