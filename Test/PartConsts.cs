namespace Test
{
    static internal class PartConsts{
        internal const int FIRST_MSEC = 0;
        internal const int LAST_MSEC = 999;
        internal const int FIRST_SEC = 0;
        internal const int LAST_SEC = 59;
        internal const int FIRST_MIN = 0;
        internal const int LAST_MIN = 59;
        internal const int FIRST_HOUR = 0;
        internal const int LAST_HOUR = 23;
        internal const int MONTHS_IN_YEAR = 12;
        internal const int FIRST_DAY_IN_MONTH = 1;
        internal const int LAST_DAY_IN_MONTH = 32;
        internal const int MIN_DAYS_IN_MONTH = 28;
        internal const int FIRST_MONTH = 1;
        internal const int LAST_MONTH = 12;
        internal const int FEBRUARY_MONTH = 2;
        internal static readonly int[] DAYS_IN_MONTHS = new int[PartConsts.MONTHS_IN_YEAR] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        internal const int FIRST_YEAR = 2000;
        internal const int LAST_YEAR = 2100;
        internal const int DAYS_IN_NONLEAP_YEAR = 365;
        internal const int FIRST_DOW = 0;
        internal const int LAST_DOW = 6;
        internal const int DAYS_IN_WEEK = 7;
        internal const int MSECS = 0, SECS = 1, MINUTES = 2, HOURS = 3, DAYS = 4, MONTHS = 5, YEARS = 6, DOW = 7;
        internal const int NUM_PARTS = 8;
    }
}
