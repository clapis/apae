using System;
using System.Globalization;
using NodaTime;

namespace Apae.Extensions
{
    public static class DateTimeExtensions
    {
        private static CultureInfo BrazilCulture = CultureInfo.CreateSpecificCulture("pt-BR");
        private static DateTimeZone BrazilTimeZone = DateTimeZoneProviders.Tzdb["America/Sao_Paulo"];

        public static DateTime FromUtcToBrazil(this DateTime date)
        {
            return Instant.FromDateTimeUtc(DateTime.SpecifyKind(date, DateTimeKind.Utc))
                .InZone(BrazilTimeZone).ToDateTimeUnspecified();
        }

        public static DateTime? FromUtcToBrazil(this DateTime? date)
        {
            return date.HasValue ? date.Value.FromUtcToBrazil() : (DateTime?)null;
        }

        public static DateTime FromBrazilToUtc(this DateTime date)
        {
            return LocalDateTime.FromDateTime(date).InZoneLeniently(BrazilTimeZone).ToDateTimeUtc();
        }

        public static DateTime? FromBrazilToUtc(this DateTime? date)
        {
            return date.HasValue ? date.Value.FromBrazilToUtc() : (DateTime?)null;
        }


        public static string Humanize(this DateTime date)
        {
            return Humanizer.DateHumanizeExtensions.Humanize(date);
        }

        public static string ToMonthName(this int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month).Capitalize();
        }

        public static string ToBirthDate(this DateTime value)
        {
            return $"{value:d} ({value.Age()} anos) {(value.IsBirthday() ? "\uD83C\uDF81 !" : string.Empty)}";
        }

        public static int Age(this DateTime value)
        {
            var today = DateTime.UtcNow.FromUtcToBrazil();

            var age = today.Year - value.Year;
            var cake = CakeCompare(today, value);

            if (cake < 0) age--;

            return age;
        }

        public static bool IsBirthday(this DateTime value)
            => CakeCompare(DateTime.UtcNow.FromUtcToBrazil(), value) == 0;

        private static int CakeCompare(this DateTime a, DateTime b)
            => a.Month.CompareTo(b.Month) == 0 ? a.Day.CompareTo(b.Day) : a.Month.CompareTo(b.Month);

    }
}
