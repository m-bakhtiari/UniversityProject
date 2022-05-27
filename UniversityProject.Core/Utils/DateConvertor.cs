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

        public static string ToPersianMonth(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            var month = pc.GetMonth(value);
            if (month == 1)
            {
                return $"فروردین {pc.GetYear(value)}";
            }
            else if (month == 2)
            {
                return $"اردیبهشت {pc.GetYear(value)}";
            }
            else if (month == 3)
            {
                return $"خرداد {pc.GetYear(value)}";
            }
            else if (month == 4)
            {
                return $"تیر {pc.GetYear(value)}";
            }
            else if (month == 5)
            {
                return $"مرداد {pc.GetYear(value)}";
            }
            else if (month == 6)
            {
                return $"شهریور {pc.GetYear(value)}";
            }
            else if (month == 7)
            {
                return $"مهر {pc.GetYear(value)}";
            }
            else if (month == 8)
            {
                return $"آبان {pc.GetYear(value)}";
            }
            else if (month == 9)
            {
                return $"آذر {pc.GetYear(value)}";
            }
            else if (month == 10)
            {
                return $"دی {pc.GetYear(value)}";
            }
            else if (month == 11)
            {
                return $"بهمن {pc.GetYear(value)}";
            }
            else if (month == 12)
            {
                return $"اسفند {pc.GetYear(value)}";
            }
            return value.ToString();
        }
    }
}
