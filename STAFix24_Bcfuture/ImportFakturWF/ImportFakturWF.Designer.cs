using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace STAFix24_Bcfuture.ImportFakturWF
{
    public sealed partial class ImportFakturWF
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind9 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.CodeCondition codecondition4 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind11 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind10 = new System.Workflow.ComponentModel.ActivityBind();
            this.ErrorHandler2 = new System.Workflow.Activities.CodeActivity();
            this.faultHandlerActivity3 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.faultHandlersActivity3 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.Remove_ZaimportowaneFaktury = new System.Workflow.Activities.CodeActivity();
            this.Add_FakturyDoRejestru = new System.Workflow.Activities.CodeActivity();
            this.Create_Message = new System.Workflow.Activities.CodeActivity();
            this.Select_FaturyKlienta = new System.Workflow.Activities.CodeActivity();
            this.SetStatus_Nowy = new System.Workflow.Activities.CodeActivity();
            this.ErrorHandler1 = new System.Workflow.Activities.CodeActivity();
            this.PrzygotujWiadomosc = new System.Workflow.Activities.SequenceActivity();
            this.faultHandlerActivity2 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.Weryfikcja = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.whileActivity2 = new System.Workflow.Activities.WhileActivity();
            this.Get_KlientEnumerator = new System.Workflow.Activities.CodeActivity();
            this.Select_Rozliczenia = new System.Workflow.Activities.CodeActivity();
            this.Select_ListaKlientow = new System.Workflow.Activities.CodeActivity();
            this.Wysyłka = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.faultHandlersActivity2 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.Update_Faktura = new System.Workflow.Activities.CodeActivity();
            this.Try_TerminPłatności = new System.Workflow.Activities.CodeActivity();
            this.Try_Dokument = new System.Workflow.Activities.CodeActivity();
            this.Try_Okres = new System.Workflow.Activities.CodeActivity();
            this.Try_Klient = new System.Workflow.Activities.CodeActivity();
            this.Set_Faktura = new System.Workflow.Activities.CodeActivity();
            this.elseMode_Weryfikacja = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifMode_Wysylka = new System.Workflow.Activities.IfElseBranchActivity();
            this.ObsługaFaktury = new System.Workflow.Activities.SequenceActivity();
            this.Test_MODE = new System.Workflow.Activities.IfElseActivity();
            this.whileActivity1 = new System.Workflow.Activities.WhileActivity();
            this.Get_Enumerator = new System.Workflow.Activities.CodeActivity();
            this.Validate_Records = new System.Workflow.Activities.SequenceActivity();
            this.logLiczbaRekordow = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Preset_Data = new System.Workflow.Activities.CodeActivity();
            this.logErrorMessage = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ErrorHandler = new System.Workflow.Activities.CodeActivity();
            this.LogErrCT = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.SetStatus_Zakonczone = new System.Workflow.Activities.CodeActivity();
            this.Send_Report = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.ManageImportFaktur = new System.Workflow.Activities.SequenceActivity();
            this.Init_Report = new System.Workflow.Activities.CodeActivity();
            this.logCT = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
            this.else1 = new System.Workflow.Activities.IfElseBranchActivity();
            this.ImportFaktur = new System.Workflow.Activities.IfElseBranchActivity();
            this.cancellationHandlerActivity1 = new System.Workflow.ComponentModel.CancellationHandlerActivity();
            this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
            this.Test_CT = new System.Workflow.Activities.IfElseActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // ErrorHandler2
            // 
            this.ErrorHandler2.Name = "ErrorHandler2";
            this.ErrorHandler2.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // faultHandlerActivity3
            // 
            this.faultHandlerActivity3.Activities.Add(this.ErrorHandler2);
            this.faultHandlerActivity3.FaultType = typeof(System.Exception);
            this.faultHandlerActivity3.Name = "faultHandlerActivity3";
            // 
            // faultHandlersActivity3
            // 
            this.faultHandlersActivity3.Activities.Add(this.faultHandlerActivity3);
            this.faultHandlersActivity3.Name = "faultHandlersActivity3";
            // 
            // Remove_ZaimportowaneFaktury
            // 
            this.Remove_ZaimportowaneFaktury.Name = "Remove_ZaimportowaneFaktury";
            this.Remove_ZaimportowaneFaktury.ExecuteCode += new System.EventHandler(this.Remove_ZaimportowaneFaktury_ExecuteCode);
            // 
            // Add_FakturyDoRejestru
            // 
            this.Add_FakturyDoRejestru.Name = "Add_FakturyDoRejestru";
            this.Add_FakturyDoRejestru.ExecuteCode += new System.EventHandler(this.Add_FakturyDoRejestru_ExecuteCode);
            // 
            // Create_Message
            // 
            this.Create_Message.Name = "Create_Message";
            this.Create_Message.ExecuteCode += new System.EventHandler(this.Create_Message_ExecuteCode);
            // 
            // Select_FaturyKlienta
            // 
            this.Select_FaturyKlienta.Name = "Select_FaturyKlienta";
            this.Select_FaturyKlienta.ExecuteCode += new System.EventHandler(this.Select_FakturyKlienta_ExecuteCode);
            // 
            // SetStatus_Nowy
            // 
            this.SetStatus_Nowy.Name = "SetStatus_Nowy";
            this.SetStatus_Nowy.ExecuteCode += new System.EventHandler(this.Update_FakturaStatus_ExecuteCode);
            // 
            // ErrorHandler1
            // 
            this.ErrorHandler1.Name = "ErrorHandler1";
            this.ErrorHandler1.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // PrzygotujWiadomosc
            // 
            this.PrzygotujWiadomosc.Activities.Add(this.Select_FaturyKlienta);
            this.PrzygotujWiadomosc.Activities.Add(this.Create_Message);
            this.PrzygotujWiadomosc.Activities.Add(this.Add_FakturyDoRejestru);
            this.PrzygotujWiadomosc.Activities.Add(this.Remove_ZaimportowaneFaktury);
            this.PrzygotujWiadomosc.Activities.Add(this.faultHandlersActivity3);
            this.PrzygotujWiadomosc.Name = "PrzygotujWiadomosc";
            // 
            // faultHandlerActivity2
            // 
            this.faultHandlerActivity2.Activities.Add(this.ErrorHandler1);
            this.faultHandlerActivity2.Activities.Add(this.SetStatus_Nowy);
            this.faultHandlerActivity2.FaultType = typeof(System.Exception);
            this.faultHandlerActivity2.Name = "faultHandlerActivity2";
            // 
            // Weryfikcja
            // 
            this.Weryfikcja.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.Weryfikcja.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.Weryfikcja.HistoryDescription = "MODE";
            this.Weryfikcja.HistoryOutcome = "Weryfikacja";
            this.Weryfikcja.Name = "Weryfikcja";
            this.Weryfikcja.OtherData = "";
            this.Weryfikcja.UserId = -1;
            // 
            // whileActivity2
            // 
            this.whileActivity2.Activities.Add(this.PrzygotujWiadomosc);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileKlientExist);
            this.whileActivity2.Condition = codecondition1;
            this.whileActivity2.Name = "whileActivity2";
            // 
            // Get_KlientEnumerator
            // 
            this.Get_KlientEnumerator.Name = "Get_KlientEnumerator";
            this.Get_KlientEnumerator.ExecuteCode += new System.EventHandler(this.Get_KlientEnumerator_ExecuteCode);
            // 
            // Select_Rozliczenia
            // 
            this.Select_Rozliczenia.Name = "Select_Rozliczenia";
            this.Select_Rozliczenia.ExecuteCode += new System.EventHandler(this.Select_Rozliczenia_ExecuteCode);
            // 
            // Select_ListaKlientow
            // 
            this.Select_ListaKlientow.Name = "Select_ListaKlientow";
            this.Select_ListaKlientow.ExecuteCode += new System.EventHandler(this.Select_ListaKlientow_ExecuteCode);
            // 
            // Wysyłka
            // 
            this.Wysyłka.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.Wysyłka.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.Wysyłka.HistoryDescription = "MODE";
            this.Wysyłka.HistoryOutcome = "Wysyłka";
            this.Wysyłka.Name = "Wysyłka";
            this.Wysyłka.OtherData = "";
            this.Wysyłka.UserId = -1;
            // 
            // faultHandlersActivity2
            // 
            this.faultHandlersActivity2.Activities.Add(this.faultHandlerActivity2);
            this.faultHandlersActivity2.Name = "faultHandlersActivity2";
            // 
            // Update_Faktura
            // 
            this.Update_Faktura.Name = "Update_Faktura";
            this.Update_Faktura.ExecuteCode += new System.EventHandler(this.Update_Faktura_ExecuteCode);
            // 
            // Try_TerminPłatności
            // 
            this.Try_TerminPłatności.Name = "Try_TerminPłatności";
            this.Try_TerminPłatności.ExecuteCode += new System.EventHandler(this.Try_TerminPłatności_ExecuteCode);
            // 
            // Try_Dokument
            // 
            this.Try_Dokument.Name = "Try_Dokument";
            this.Try_Dokument.ExecuteCode += new System.EventHandler(this.Try_Dokument_ExecuteCode);
            // 
            // Try_Okres
            // 
            this.Try_Okres.Name = "Try_Okres";
            this.Try_Okres.ExecuteCode += new System.EventHandler(this.Try_Okres_ExecuteCode);
            // 
            // Try_Klient
            // 
            this.Try_Klient.Name = "Try_Klient";
            this.Try_Klient.ExecuteCode += new System.EventHandler(this.Try_Klient_ExecuteCode);
            // 
            // Set_Faktura
            // 
            this.Set_Faktura.Name = "Set_Faktura";
            this.Set_Faktura.ExecuteCode += new System.EventHandler(this.Set_Faktura_ExecuteCode);
            // 
            // elseMode_Weryfikacja
            // 
            this.elseMode_Weryfikacja.Activities.Add(this.Weryfikcja);
            this.elseMode_Weryfikacja.Name = "elseMode_Weryfikacja";
            // 
            // ifMode_Wysylka
            // 
            this.ifMode_Wysylka.Activities.Add(this.Wysyłka);
            this.ifMode_Wysylka.Activities.Add(this.Select_ListaKlientow);
            this.ifMode_Wysylka.Activities.Add(this.Select_Rozliczenia);
            this.ifMode_Wysylka.Activities.Add(this.Get_KlientEnumerator);
            this.ifMode_Wysylka.Activities.Add(this.whileActivity2);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isMode_Wysylka);
            this.ifMode_Wysylka.Condition = codecondition2;
            this.ifMode_Wysylka.Name = "ifMode_Wysylka";
            // 
            // ObsługaFaktury
            // 
            this.ObsługaFaktury.Activities.Add(this.Set_Faktura);
            this.ObsługaFaktury.Activities.Add(this.Try_Klient);
            this.ObsługaFaktury.Activities.Add(this.Try_Okres);
            this.ObsługaFaktury.Activities.Add(this.Try_Dokument);
            this.ObsługaFaktury.Activities.Add(this.Try_TerminPłatności);
            this.ObsługaFaktury.Activities.Add(this.Update_Faktura);
            this.ObsługaFaktury.Activities.Add(this.faultHandlersActivity2);
            this.ObsługaFaktury.Name = "ObsługaFaktury";
            // 
            // Test_MODE
            // 
            this.Test_MODE.Activities.Add(this.ifMode_Wysylka);
            this.Test_MODE.Activities.Add(this.elseMode_Weryfikacja);
            this.Test_MODE.Name = "Test_MODE";
            // 
            // whileActivity1
            // 
            this.whileActivity1.Activities.Add(this.ObsługaFaktury);
            codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.whileRecordExist);
            this.whileActivity1.Condition = codecondition3;
            this.whileActivity1.Name = "whileActivity1";
            // 
            // Get_Enumerator
            // 
            this.Get_Enumerator.Name = "Get_Enumerator";
            this.Get_Enumerator.ExecuteCode += new System.EventHandler(this.Get_Enumerator_ExecuteCode);
            // 
            // Validate_Records
            // 
            this.Validate_Records.Activities.Add(this.Get_Enumerator);
            this.Validate_Records.Activities.Add(this.whileActivity1);
            this.Validate_Records.Activities.Add(this.Test_MODE);
            this.Validate_Records.Name = "Validate_Records";
            // 
            // logLiczbaRekordow
            // 
            this.logLiczbaRekordow.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logLiczbaRekordow.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logLiczbaRekordow.HistoryDescription = "Liczba rekordów";
            activitybind1.Name = "ImportFakturWF";
            activitybind1.Path = "logLiczbaRekordow_HistoryOutcome";
            this.logLiczbaRekordow.Name = "logLiczbaRekordow";
            this.logLiczbaRekordow.OtherData = "";
            this.logLiczbaRekordow.UserId = -1;
            this.logLiczbaRekordow.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // Preset_Data
            // 
            this.Preset_Data.Name = "Preset_Data";
            this.Preset_Data.ExecuteCode += new System.EventHandler(this.Preset_Data_ExecuteCode);
            // 
            // logErrorMessage
            // 
            this.logErrorMessage.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logErrorMessage.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            activitybind2.Name = "ImportFakturWF";
            activitybind2.Path = "logErrorMessage_HistoryDescription";
            activitybind3.Name = "ImportFakturWF";
            activitybind3.Path = "logErrorMessage_HistoryOutcome";
            this.logErrorMessage.Name = "logErrorMessage";
            this.logErrorMessage.OtherData = "";
            this.logErrorMessage.UserId = -1;
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryDescriptionProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.logErrorMessage.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            // 
            // ErrorHandler
            // 
            this.ErrorHandler.Name = "ErrorHandler";
            this.ErrorHandler.ExecuteCode += new System.EventHandler(this.ErrorHandler_ExecuteCode);
            // 
            // LogErrCT
            // 
            this.LogErrCT.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.LogErrCT.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.LogErrCT.HistoryDescription = "Nieobsługiwany CT";
            activitybind4.Name = "ImportFakturWF";
            activitybind4.Path = "logMessage_HistoryOutcome";
            this.LogErrCT.Name = "LogErrCT";
            this.LogErrCT.OtherData = "";
            this.LogErrCT.UserId = -1;
            this.LogErrCT.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // SetStatus_Zakonczone
            // 
            this.SetStatus_Zakonczone.Name = "SetStatus_Zakonczone";
            this.SetStatus_Zakonczone.ExecuteCode += new System.EventHandler(this.SetStatus_Zakonczone_ExecuteCode);
            // 
            // Send_Report
            // 
            this.Send_Report.BCC = null;
            activitybind5.Name = "ImportFakturWF";
            activitybind5.Path = "msgBody";
            this.Send_Report.CC = null;
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "ImportFakturWF";
            this.Send_Report.CorrelationToken = correlationtoken1;
            this.Send_Report.From = null;
            activitybind6.Name = "ImportFakturWF";
            activitybind6.Path = "msgHeaders";
            this.Send_Report.IncludeStatus = false;
            this.Send_Report.Name = "Send_Report";
            activitybind7.Name = "ImportFakturWF";
            activitybind7.Path = "msgSubject";
            activitybind8.Name = "ImportFakturWF";
            activitybind8.Path = "msgTo";
            this.Send_Report.MethodInvoking += new System.EventHandler(this.Send_Report_MethodInvoking);
            this.Send_Report.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.ToProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            this.Send_Report.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.SubjectProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            this.Send_Report.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.BodyProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            this.Send_Report.SetBinding(Microsoft.SharePoint.WorkflowActions.SendEmail.HeadersProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            // 
            // ManageImportFaktur
            // 
            this.ManageImportFaktur.Activities.Add(this.Preset_Data);
            this.ManageImportFaktur.Activities.Add(this.logLiczbaRekordow);
            this.ManageImportFaktur.Activities.Add(this.Validate_Records);
            this.ManageImportFaktur.Name = "ManageImportFaktur";
            // 
            // Init_Report
            // 
            this.Init_Report.Name = "Init_Report";
            this.Init_Report.ExecuteCode += new System.EventHandler(this.Init_Report_ExecuteCode);
            // 
            // logCT
            // 
            this.logCT.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logCT.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logCT.HistoryDescription = "CT";
            activitybind9.Name = "ImportFakturWF";
            activitybind9.Path = "logMessage_HistoryOutcome";
            this.logCT.Name = "logCT";
            this.logCT.OtherData = "";
            this.logCT.UserId = -1;
            this.logCT.SetBinding(Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity.HistoryOutcomeProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind9)));
            // 
            // faultHandlerActivity1
            // 
            this.faultHandlerActivity1.Activities.Add(this.ErrorHandler);
            this.faultHandlerActivity1.Activities.Add(this.logErrorMessage);
            this.faultHandlerActivity1.FaultType = typeof(System.Exception);
            this.faultHandlerActivity1.Name = "faultHandlerActivity1";
            // 
            // else1
            // 
            this.else1.Activities.Add(this.LogErrCT);
            this.else1.Name = "else1";
            // 
            // ImportFaktur
            // 
            this.ImportFaktur.Activities.Add(this.logCT);
            this.ImportFaktur.Activities.Add(this.Init_Report);
            this.ImportFaktur.Activities.Add(this.ManageImportFaktur);
            this.ImportFaktur.Activities.Add(this.Send_Report);
            this.ImportFaktur.Activities.Add(this.SetStatus_Zakonczone);
            codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.isImportFaktur);
            this.ImportFaktur.Condition = codecondition4;
            this.ImportFaktur.Name = "ImportFaktur";
            // 
            // cancellationHandlerActivity1
            // 
            this.cancellationHandlerActivity1.Name = "cancellationHandlerActivity1";
            // 
            // faultHandlersActivity1
            // 
            this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            // 
            // Test_CT
            // 
            this.Test_CT.Activities.Add(this.ImportFaktur);
            this.Test_CT.Activities.Add(this.else1);
            this.Test_CT.Name = "Test_CT";
            activitybind11.Name = "ImportFakturWF";
            activitybind11.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind10.Name = "ImportFakturWF";
            activitybind10.Path = "workflowProperties";
            this.onWorkflowActivated1.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.onWorkflowActivated1_Invoked);
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind11)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind10)));
            // 
            // ImportFakturWF
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.Test_CT);
            this.Activities.Add(this.faultHandlersActivity1);
            this.Activities.Add(this.cancellationHandlerActivity1);
            this.Name = "ImportFakturWF";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity Try_Okres;

        private CodeActivity SetStatus_Zakonczone;

        private CodeActivity Select_Rozliczenia;

        private CodeActivity ErrorHandler2;

        private FaultHandlerActivity faultHandlerActivity3;

        private FaultHandlersActivity faultHandlersActivity3;

        private CancellationHandlerActivity cancellationHandlerActivity1;

        private CodeActivity Get_KlientEnumerator;

        private CodeActivity SetStatus_Nowy;

        private CodeActivity ErrorHandler1;

        private FaultHandlerActivity faultHandlerActivity2;

        private FaultHandlersActivity faultHandlersActivity2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logErrorMessage;

        private CodeActivity ErrorHandler;

        private FaultHandlerActivity faultHandlerActivity1;

        private FaultHandlersActivity faultHandlersActivity1;

        private CodeActivity Update_Faktura;

        private CodeActivity Set_Faktura;

        private CodeActivity Get_Enumerator;

        private Microsoft.SharePoint.WorkflowActions.SendEmail Send_Report;

        private CodeActivity Remove_ZaimportowaneFaktury;

        private CodeActivity Add_FakturyDoRejestru;

        private CodeActivity Create_Message;

        private CodeActivity Select_FaturyKlienta;

        private SequenceActivity PrzygotujWiadomosc;

        private WhileActivity whileActivity2;

        private CodeActivity Select_ListaKlientow;

        private CodeActivity Try_TerminPłatności;

        private CodeActivity Try_Dokument;

        private CodeActivity Try_Klient;

        private SequenceActivity ObsługaFaktury;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity LogErrCT;

        private CodeActivity Init_Report;

        private IfElseBranchActivity else1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logCT;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logLiczbaRekordow;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity Weryfikcja;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity Wysyłka;

        private IfElseBranchActivity elseMode_Weryfikacja;

        private IfElseBranchActivity ifMode_Wysylka;

        private WhileActivity whileActivity1;

        private IfElseActivity Test_MODE;

        private SequenceActivity Validate_Records;

        private CodeActivity Preset_Data;

        private SequenceActivity ManageImportFaktur;

        private IfElseBranchActivity ImportFaktur;

        private IfElseActivity Test_CT;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;
















































    }
}
