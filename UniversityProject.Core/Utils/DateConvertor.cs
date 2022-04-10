using System;
using System.Globalization;

namespace UniversityProject.Core.Utils
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }

        public static string ToEnglishNumbers(this string date)
        {
            if (date == null)
            {
                return date;
            }
            if (date.Contains("۱"))
            {
                date = date.Replace("۱", "1");
            }
            if (date.Contains("۲"))
            {
                date = date.Replace("۲", "2");
            }
            if (date.Contains("۳"))
            {
                date = date.Replace("۳", "3");
            }
            if (date.Contains("۴"))
            {
                date = date.Replace("۴", "4");
            }
            if (date.Contains("۵"))
            {
                date = date.Replace("۵", "5");
            }
            if (date.Contains("۶"))
            {
                date = date.Replace("۶", "6");
            }
            if (date.Contains("۷"))
            {
                date = date.Replace("۷", "7");
            }
            if (date.Contains("۸"))
            {
                date = date.Replace("۸", "8");
            }
            if (date.Contains("۹"))
            {
                date = date.Replace("۹", "9");
            }
            if (date.Contains("۰"))
            {
                date = date.Replace("۰", "0");
            }

            return date;
        }
    }
}
