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
        public static void StartWorkflow(SPListItem listItem, string workflowName)
        {
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
                            var result = manager.StartWorkflow(listItem, objWorkflowAssociation, objWorkflowAssociation.AssociationData, SPWorkflowRunOptions.SynchronousAllowPostpone);

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
        }

        public static void StartSiteWorkflow(SPSite site, string workflowName)
        {
            StartSiteWorkflow(site, workflowName, string.Empty);
        }

        public static void StartSiteWorkflow(SPSite site, string workflowName, string initParameters)
        {
            using (SPWeb web = site.OpenWeb()) // get the web
            {
                //find workflow to start
                var assoc = web.WorkflowAssociations.GetAssociationByName(workflowName, CultureInfo.InvariantCulture);

                //this is the call to start the workflow
                var result = site.WorkflowManager.StartWorkflow(null, assoc, initParameters , SPWorkflowRunOptions.Synchronous);

            }
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
    }
}
