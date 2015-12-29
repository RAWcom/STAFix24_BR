using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace BLL
{
    public class tabOkresy
    {
        const string targetList = "Okresy"; //"tabOkresy";



        private static void Get_ZUS_Terminy(bool isPracownicy, ref DateTime terminPlatnosci, ref DateTime terminPrzekazania, SPListItem item)
        {
            if (isPracownicy)
            {
                terminPlatnosci = DateTime.Parse(item["colZUS_TerminPlatnosciSkladek_ZP"].ToString());

            }
            else
            {
                terminPlatnosci = DateTime.Parse(item["colZUS_TerminPlatnosciSkladek_Be"].ToString());
            }

            int offset = int.Parse(item["ZUS_TerminPrzekazaniaWynikow_Ofs"].ToString());
            terminPrzekazania = terminPlatnosci.AddDays(offset);
        }

        public static DateTime Get_TerminPlatnosciByOkresId(SPWeb web, string nazwaKolumny, int okresId)
        {
            SPListItem item = Get_ItemById(web, okresId);
            return item[nazwaKolumny] != null ? DateTime.Parse(item[nazwaKolumny].ToString()) : new DateTime();
        }

        private static SPListItem Get_ItemById(SPWeb web, int okresId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            SPListItem item = list.GetItemById(okresId);
            return item;
        }

        internal static DateTime Get_TerminRealizacji(SPWeb web, int okresId, string key)
        {
            SPListItem item = Get_ItemById(web, okresId);
            DateTime startDate = Get_Date(item, "colDataZakonczenia").AddDays(1);
            DateTime targetDate = startDate;
            int offset = int.Parse(BLL.admSetup.GetValue(web, key));
            if (offset > 0)
            {
                targetDate = targetDate.AddDays(offset - 1);
            }
            while (targetDate.DayOfWeek == DayOfWeek.Saturday || targetDate.DayOfWeek == DayOfWeek.Sunday)
            {
                targetDate = targetDate.AddDays(1);
            }

            //set time of day

            TimeSpan ts = TimeSpan.Parse(BLL.admSetup.GetValue(web, "REMINDER_TIME").ToString());

            return targetDate.Add(ts);
        }

        private static DateTime Get_Date(SPListItem item, string col)
        {
            return item[col] != null ? DateTime.Parse(item[col].ToString()) : new DateTime();
        }

        public static string Get_PoprzedniMiesiacSlownieById(SPWeb web, int okresId, int offset)
        {
            SPListItem item = Get_ItemById(web, okresId);

            if (item != null)
            {
                DateTime start = BLL.Tools.Get_Date(item, "colDataRozpoczecia");
                DateTime targetDate = start.AddMonths(-1 * offset);
                switch (targetDate.Month)
                {
                    case 1: return "styczeń";
                    case 2: return "luty";
                    case 3: return "marzec";
                    case 4: return "kwiecień";
                    case 5: return "maj";
                    case 6: return "czerwiec";
                    case 7: return "lipiec";
                    case 8: return "sierpień";
                    case 9: return "wrzesień";
                    case 10: return "październik";
                    case 11: return "listopad";
                    case 12: return "grudzień";
                    default:
                        break;
                }

                return string.Empty;
            }

            return string.Empty;

        }

        internal static int Get_PoprzedniOkresIdById(SPWeb web, int okresId)
        {
            SPList list = web.Lists.TryGetList(targetList);
            SPListItem item = list.GetItemById(okresId);
            if (item!=null)
            {
                DateTime d = Get_Date(item, "colDataRozpoczecia");
                DateTime endDate = d.AddDays(-1);
                
                SPListItem foundItem = list.Items.Cast<SPListItem>()
                    .Where(i => i["colDataZakonczenia"]!=null)
                    .Where(i => (DateTime.Parse(i["colDataZakonczenia"].ToString())).Equals(endDate))
                    .FirstOrDefault();

                if (foundItem != null)
                {
                    return foundItem.ID;
                }
            }
            return 0;
        }

        internal static int Get_PoprzedniOkresKwartalnyIdById(SPWeb web, int okresId)
        {
            int mNumber = 0;
            int oId = okresId;
            do
            {
                oId = Get_PoprzedniOkresIdById(web, oId);
                try
                {
                    SPListItem item = Get_ItemById(web, oId);
                    mNumber = BLL.Tools.Get_Date(item, "colDataRozpoczecia").Month;
                }
                catch (Exception)
                {}


            } while (oId > 0 && (mNumber == 3 || mNumber == 6 || mNumber == 9 || mNumber == 12));

            return oId;
        }

        public static string Get_IdPlat_Miesiecznie(SPWeb web, int okresId)
        {
            SPListItem item = Get_ItemById(web, okresId);
            DateTime startDate = BLL.Tools.Get_Date(item, "colDataRozpoczecia");
            return string.Format("{0:yy}M{0:MM}", startDate);
        }

        public static string Get_IdPlat_Kwartalnie(SPWeb web, int okresId)
        {
            SPListItem item = Get_ItemById(web, okresId);
            DateTime startDate = BLL.Tools.Get_Date(item, "colDataRozpoczecia");

            string kw;
            if (startDate.Month ==1 | startDate.Month ==2 | startDate.Month ==3) kw = "01";
            else if (startDate.Month == 4 | startDate.Month == 5 | startDate.Month == 6) kw = "02";
            else if (startDate.Month == 7 | startDate.Month == 8 | startDate.Month == 9) kw = "03";
            else kw = "04";

            return string.Format("{0:yy}K{1}", startDate, kw);
        }

        public static string Get_IdPlatZUS(SPWeb web, int okresId)
        {
            SPListItem item = Get_ItemById(web, okresId);
            DateTime startDate = BLL.Tools.Get_Date(item, "colDataRozpoczecia");
            return string.Format("{0:yyyyMM}", startDate);
        }

        internal static int Get_ActiveOkresId(SPWeb web)
        {
            DateTime currentDate = DateTime.Today;
            DateTime targetDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endDate = targetDate.AddDays(-1);
            DateTime startDate = targetDate.AddMonths(-1); 

            SPListItem item = web.Lists.TryGetList(targetList).Items.Cast<SPListItem>()
                .Where(i => BLL.Tools.Get_Date(i, "colDataRozpoczecia").Equals(startDate)
                            && BLL.Tools.Get_Date(i, "colDataZakonczenia").Equals(endDate))
                .FirstOrDefault();

            if (item != null) return item.ID;
            else return 0;
            
        }
    }
}
