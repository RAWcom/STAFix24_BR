using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace STAFix24_Bcfuture.ImportFakturWF
{
    public sealed partial class ImportFakturWF : SequentialWorkflowActivity
    {
        public ImportFakturWF()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
        public SPListItem item;
        private string _tabFaktury = "tabFaktury";
        private string _libFaktury = "libFaktury";

        public String logLiczbaRekordow_HistoryOutcome = default(System.String);
        public String logMessage_HistoryOutcome = default(System.String);
        private StringBuilder sb;

        public String msgTo = default(System.String);
        public String msgSubject = default(System.String);
        public String msgBody = default(System.String);
        public System.Collections.Specialized.StringDictionary msgHeaders = new System.Collections.Specialized.StringDictionary();
        private Array aFaktury;
        private Array aObrazyFaktur;
        private IEnumerator myEnum;
        private SPListItem faktura;
        private string _tabKlienci = "Klienci";
        private Array aKlienci;
        private SPListItem foundKlient;
        private SPListItem foundDokument;
        private string _dicTerminyPlatnosci = "Terminy płatności";
        private DateTime terminPlatnosci;
        private string _STATUS_NOWY = "Nowy";
        private string _STATUS_WERYFIKACJA = "Weryfikacja";
        private string _STATUS_POWIAZANY = "Powiązany";
        private string _STATUS_WYSYLKA = "Wysyłka";
        private string _STATUS_ZAKONCZONY = "Zakończony";

        public String logErrorMessage_HistoryDescription = default(System.String);
        public String logErrorMessage_HistoryOutcome = default(System.String);
        private ArrayList distinctKlienci;
        private StringBuilder sbRpt; //przechowuje listę obsłużonych klientów
        private int currentKlientId;
        private ArrayList fakturyKlineta;
        private static string targetList = "Wiadomości";
        private string numerKontaBankowego;



        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            item = workflowProperties.Item;
            logMessage_HistoryOutcome = item.ContentType.Name;

            
        }

        private void isImportFaktur(object sender, ConditionalEventArgs e)
        {
            if (item.ContentType.Name.Equals("Import faktur")) e.Result = true;
        }

        private void isMode_Wysylka(object sender, ConditionalEventArgs e)
        {
            if (BLL.Tools.Get_Text(item, "enumMode").Equals("Wysyłka")) e.Result = true;
        }

        private void Preset_Data_ExecuteCode(object sender, EventArgs e)
        {
            aKlienci = GetKlienci(_tabKlienci);
            aFaktury = GetListItems(_tabFaktury);
            aObrazyFaktur = GetListItems(_libFaktury);

            logLiczbaRekordow_HistoryOutcome = string.Format("{0}/{1}", aFaktury.Length.ToString(), aObrazyFaktur.Length.ToString());
        }

        private Array GetKlienci(string _tabKlienci)
        {
            Array results = null;

            SPList list = item.Web.Lists.TryGetList(_tabKlienci);
            if (list != null)
            {
                results = list.Items.Cast<SPListItem>()
                    .Where(i => i.ContentType.Name.Equals("Klient"))
                    .ToArray();
            }

            return results;
        }

        private Array GetListItems(string listName)
        {
            Array results = null;

            SPList list = item.Web.Lists.TryGetList(listName);

            if (list != null)
            {
                results = list.Items.Cast<SPListItem>().ToArray();
            }

            return results;
        }

        private void Init_Report_ExecuteCode(object sender, EventArgs e)
        {
            sb = new StringBuilder();
        }

        private void Send_Report_MethodInvoking(object sender, EventArgs e)
        {
            msgTo = workflowProperties.OriginatorEmail;

            msgSubject = "Import faktur zakończony";

            //msgBody
            if (sb.Length > 0) msgBody = "<h3>Raport z importu</h3>" + string.Format("<ol>{0}</ol>", sb.ToString());
            else msgBody = string.Empty;

            if (sbRpt.Length > 0) msgBody = msgBody + "<h3>Lista klientów dla których przygotowano wiadomość</h3>" + string.Format("<ol>{0}</ol>", sbRpt.ToString());

            //msgHeaders
            //TODO:
        }

        private void Get_Enumerator_ExecuteCode(object sender, EventArgs e)
        {
            myEnum = aFaktury.GetEnumerator();
        }

        private void whileRecordExist(object sender, ConditionalEventArgs e)
        {
            if (myEnum.MoveNext() && myEnum != null) e.Result = true;
            else e.Result = false;
        }

        private void Set_Faktura_ExecuteCode(object sender, EventArgs e)
        {
            faktura = myEnum.Current as SPListItem;
            foundKlient = null;
            foundDokument = null;

            BLL.Tools.Set_Index(faktura, "selKlient", 0);
            //BLL.Tools.Set_Date(faktura, "colBR_TerminPlatnosci", new DateTime());
            //faktura["colBR_TerminPlatnosci"] = new DateTime();
            BLL.Tools.Set_Value(faktura, "_DokumentId", 0);
            BLL.Tools.Set_Text(faktura, "_PowiazanyDokument", string.Empty);
            BLL.Tools.Set_Text(faktura, "_Uwagi", string.Empty);
            BLL.Tools.Set_Text(faktura, "enumStatusImportu", _STATUS_NOWY);
        }



        private void Try_Klient_ExecuteCode(object sender, EventArgs e)
        {

            string kontrahent = BLL.Tools.Get_Text(faktura, "_Kontrahent");

            if (!string.IsNullOrEmpty(kontrahent))
            {
                string k = kontrahent.Trim().ToUpperInvariant();
                foreach (SPListItem klient in aKlienci)
                {
                    if (BLL.Tools.Get_Text(klient, "colNazwaFirmy").Trim().ToUpperInvariant().Contains(k))
                    {
                        foundKlient = klient;
                        break;
                    }
                }

                if (foundKlient == null)
                {
                    foreach (SPListItem klient in aKlienci)
                    {
                        if (BLL.Tools.Get_Text(klient, "colNazwaSkrocona").Trim().ToUpperInvariant().Contains(k))
                        {
                            foundKlient = klient;
                            break;
                        }
                    }
                }
            }
        }

        private void Try_Dokument_ExecuteCode(object sender, EventArgs e)
        {

            if (foundKlient != null)
            {
                string numerFaktury = BLL.Tools.Get_Text(faktura, "colBR_NumerFaktury");
                numerFaktury = Convert_NumerFaktury(numerFaktury);
                Debug.WriteLine("numerFaktury:" + numerFaktury);

                foreach (SPListItem dokument in aObrazyFaktur)
                {
                    if (BLL.Tools.Get_Text(dokument, "LinkFilename").Trim().ToUpperInvariant().StartsWith(numerFaktury))
                    {
                        foundDokument = dokument;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// zamienia / na _
        /// </summary>
        private string Convert_NumerFaktury(string s)
        {
            string pattern = "/";
            string replacement = "_";
            Regex rgx = new Regex(pattern);
            return rgx.Replace(s, replacement);
        }

        private void Try_TerminPłatności_ExecuteCode(object sender, EventArgs e)
        {

            DateTime dataWystawienia = BLL.Tools.Get_Date(faktura, "colBR_DataWystawieniaFaktury");
            terminPlatnosci = dataWystawienia != null ? dataWystawienia : DateTime.Today;

            if (foundKlient != null)
            {
                int terminPlatnosciId = BLL.Tools.Get_LookupId(foundKlient, "selTerminPlatnosci");
                if (terminPlatnosciId > 0 && dataWystawienia != null)
                {
                    int ld = int.Parse(Get_TerminPlatnosci_LiczbaDni(terminPlatnosciId).ToString());
                    terminPlatnosci = dataWystawienia.AddDays(ld);
                }
            }

        }

        private int Get_TerminPlatnosci_LiczbaDni(int terminPlatnosciId)
        {
            int result = 0;

            SPList list = item.Web.Lists.TryGetList(_dicTerminyPlatnosci);

            SPListItem listItem = list.GetItemById(terminPlatnosciId);

            if (listItem != null)
            {
                result = int.Parse(BLL.Tools.Get_Value(listItem, "colLiczbaDni").ToString());
            }

            return result;
        }

        private void Update_Faktura_ExecuteCode(object sender, EventArgs e)
        {
            StringBuilder sbErr = new StringBuilder();

            if (foundKlient == null) sbErr.AppendFormat("<li>{0}</li>", "Nie znaleziono powiązanej kartoteki klienta");
            else
            {
                BLL.Tools.Set_Index(faktura, "selKlient", foundKlient.ID);
            }
            if (foundDokument == null) sbErr.AppendFormat("<li>{0}</li>", "Brak załącznika");
            else
            {
                BLL.Tools.Set_Value(faktura, "_DokumentId", foundDokument.ID);
                BLL.Tools.Set_Text(faktura, "_PowiazanyDokument", foundDokument.Name);
            }
            if (terminPlatnosci == null)
                sbErr.AppendFormat("<li>{0}</li>", "Problem z wyliczeniem terminu płatności");
            else
            {
                if (terminPlatnosci <= DateTime.Today.AddMonths(-2))
                    sbErr.AppendFormat("<li>Termin płatności nie może być wcześniejszy niż {0}</li>", BLL.Tools.Format_Date(DateTime.Today.AddMonths(-2)));
                else
                {
                    BLL.Tools.Set_Date(faktura, "colBR_TerminPlatnosci", terminPlatnosci);
                }
            }

            if (foundKlient != null && foundDokument != null && terminPlatnosci != null)
            {
                BLL.Tools.Set_Text(faktura, "_Uwagi", string.Format("{0} OK", DateTime.Now.ToString()));

                BLL.Tools.Set_Text(faktura, "enumStatusImportu", _STATUS_POWIAZANY);
            }
            else
            {
                BLL.Tools.Set_Text(faktura, "_Uwagi", string.Format("{1}<ul>{0}</ul>", sbErr.ToString(), DateTime.Now.ToString()));
                BLL.Tools.Set_Text(faktura, "enumStatusImportu", _STATUS_WERYFIKACJA);
            }

            faktura.SystemUpdate();
        }



        private void ErrorHandler_ExecuteCode(object sender, EventArgs e)
        {
            FaultHandlerActivity fa = ((Activity)sender).Parent as FaultHandlerActivity;
            if (fa != null)
            {
                Debug.WriteLine(fa.Fault.Source);
                Debug.WriteLine(fa.Fault.Message);
                Debug.WriteLine(fa.Fault.StackTrace);

                logErrorMessage_HistoryDescription = string.Format("{0}::{1}",
                    fa.Fault.Message,
                    fa.Fault.StackTrace);


                ElasticEmail.EmailGenerator.ReportErrorFromWorkflow(workflowProperties, fa.Fault.Message, fa.Fault.StackTrace);

            }
        }

        private void Update_FakturaStatus_ExecuteCode(object sender, EventArgs e)
        {

            if (faktura != null)
            {
                BLL.Tools.Set_Text(faktura, "enumStatusImportu", _STATUS_NOWY);
                faktura.SystemUpdate();
            }

        }

        private void Select_ListaKlientow_ExecuteCode(object sender, EventArgs e)
        {
            numerKontaBankowego = BLL.admSetup.GetValue(item.Web, "BR_KONTO");
            
            sbRpt = new StringBuilder();

            distinctKlienci = new ArrayList();

            foreach (SPListItem faktura in aFaktury)
            {
                if (BLL.Tools.Get_Text(faktura, "enumStatusImportu").Equals(_STATUS_POWIAZANY))
                {
                    int klientId = BLL.Tools.Get_LookupId(faktura, "selKlient");

                    if (!distinctKlienci.Contains(klientId))
                    {
                        distinctKlienci.Add(klientId);
                        sbRpt.AppendFormat("<li>{0}</li>", BLL.Tools.Get_LookupValue(faktura, "selKlient"));
                    }
                }
            }
        }

        private void Get_KlientEnumerator_ExecuteCode(object sender, EventArgs e)
        {
            myEnum = distinctKlienci.GetEnumerator();
        }

        private void whileKlientExist(object sender, ConditionalEventArgs e)
        {
            if (myEnum.MoveNext() && myEnum != null) e.Result = true;
            else e.Result = false;
        }

        private void Select_FakturyKlienta_ExecuteCode(object sender, EventArgs e)
        {
            fakturyKlineta = new ArrayList();

            currentKlientId = (int)myEnum.Current;

            foreach (SPListItem faktura in aFaktury)
            {
                int klinetId = BLL.Tools.Get_LookupId(faktura, "selKlient");
                if (distinctKlienci.Contains(klinetId))
                {
                    if (klinetId.Equals(currentKlientId))
                    {
                        if (BLL.Tools.Get_Text(faktura, "enumStatusImportu").Equals(_STATUS_POWIAZANY))
                        {
                            fakturyKlineta.Add(faktura);
                        }
                    }
                }
            }
        }

        private void Create_Message_ExecuteCode(object sender, EventArgs e)
        {

            // przygotuj wiadomość
            string temat = string.Empty;
            string tresc = string.Empty;
            string trescHTML = string.Empty;

            //string nadawca = BLL.Tools.Get_CurrentUser(item); - wymusza przypisanie stopki operatora na podstawie aktualnego adresu nadawcy

            string nadawca = string.Empty; //wymusza aby testo czy trzeba dodać stopkę został wykonany w procedurze Get_TemplateByKod


            // przygotuj wiersze tabeli

            BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "INVOICE_LIST_TR_TEMPLATE", out temat, out trescHTML, nadawca);

            string rowTemplate = trescHTML;
            StringBuilder sb = new StringBuilder();

            foreach (SPListItem faktura in fakturyKlineta)
            {
                StringBuilder sbRow = new StringBuilder(rowTemplate);
                sbRow.Replace("[[BR_NumerFaktury]]", BLL.Tools.Get_Text(faktura, "colBR_NumerFaktury"));
                sbRow.Replace("[[colBR_DataWystawienia]]", BLL.Tools.Format_Date(BLL.Tools.Get_Date(faktura, "colBR_DataWystawieniaFaktury")));
                sbRow.Replace("[[colBR_WartoscDoZaplaty]]", BLL.Tools.Format_Currency(BLL.Tools.Get_Value(faktura, "colBR_WartoscDoZaplaty")));
                sbRow.Replace("[[colBR_Konto]]", BLL.Tools.Format_Konto(numerKontaBankowego));
                sbRow.Replace("[[colBR_TerminPlatnosci]]", BLL.Tools.Format_Date(BLL.Tools.Get_Date(faktura, "colBR_TerminPlatnosci")));

                sb.Append(sbRow.ToString());
            }

            //sprawdz czy nie nadpisać szablonu

            BLL.dicSzablonyKomunikacji.Get_TemplateByKod(item, "INVOICE_LIST_TEMPLATE.Include", out temat, out trescHTML, nadawca);

            StringBuilder sbBody = new StringBuilder(trescHTML);
            sbBody.Replace("[[TABLE_ROW]]", sb.ToString());

            sbBody.Replace("___FOOTER___", string.Empty);

            trescHTML = sbBody.ToString();


            string odbiorca = BLL.tabKlienci.Get_EmailById(item.Web, currentKlientId);
            string kopiaDla = BLL.Tools.Append_EmailCC(item.Web, currentKlientId, string.Empty);
            bool KopiaDoNadawcy = true;
            bool KopiaDoBiura = false;

            if (BLL.Tools.IsValidEmail(odbiorca))
            {
                SPListItem newItem = CreateMessageItem(item.Web, ref nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, new DateTime(), 0, currentKlientId, 0);

                ////obsługa wysyłki załączników jeżeli Item został przekazany w wywołaniu procedury
                //if (item != null)
                //{
                //    for (int attachmentIndex = 0; attachmentIndex < item.Attachments.Count; attachmentIndex++)
                //    {
                //        string url = item.Attachments.UrlPrefix + item.Attachments[attachmentIndex];
                //        SPFile file = item.ParentList.ParentWeb.GetFile(url);

                //        if (file.Exists)
                //        {

                //                    BLL.Tools.Copy_Attachement(newItem, file);
                //        }
                //    }
                //}

                newItem.SystemUpdate();

                //BLL.tabWiadomosci.AddNew(item.Web, item, nadawca, odbiorca, kopiaDla, KopiaDoNadawcy, KopiaDoBiura, temat, tresc, trescHTML, BLL.Tools.Get_Date(item, "colPlanowanaDataNadania"), item.ID, currentKlientId, 0, BLL.Models.Marker.WithAttachements);
            }

        }

        private static SPListItem CreateMessageItem(SPWeb web, ref string nadawca, string odbiorca, string kopiaDla, bool KopiaDoNadawcy, bool KopiaDoBiura, string temat, string tresc, string trescHTML, DateTime planowanaDataNadania, int zadanieId, int klientId, int kartaKontrolnaId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            SPListItem newItem = list.AddItem();
            newItem["Title"] = temat;
            if (string.IsNullOrEmpty(nadawca)) nadawca = BLL.admSetup.GetValue(web, "EMAIL_BIURA");

            newItem["colNadawca"] = nadawca;
            newItem["colOdbiorca"] = odbiorca;

            if (!string.IsNullOrEmpty(kopiaDla))
            {
                newItem["colKopiaDla"] = kopiaDla;
            }

            newItem["colTresc"] = tresc;
            newItem["colTrescHTML"] = trescHTML;
            if (!string.IsNullOrEmpty(planowanaDataNadania.ToString()) && planowanaDataNadania != new DateTime())
            {
                newItem["colPlanowanaDataNadania"] = planowanaDataNadania.ToString();
            }
            newItem["colKopiaDoNadawcy"] = KopiaDoNadawcy;
            newItem["colKopiaDoBiura"] = KopiaDoBiura;
            if (zadanieId > 0) newItem["_ZadanieId"] = zadanieId;

            if (klientId > 0) newItem["selKlient_NazwaSkrocona"] = klientId;

            if (kartaKontrolnaId > 0) newItem["_KartaKontrolnaId"] = kartaKontrolnaId;
            return newItem;
        }


    }
}
