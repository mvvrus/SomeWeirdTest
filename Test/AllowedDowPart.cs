using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class AllowedDowPart: AllowedDateTimePart
    {

        readonly int _startDay, _startMonth, _startYear;
        readonly int _startDow;
        AllowedDowPart(bool[] AllowedList, int PartNumber):base(PartConsts.FIRST_DOW, PartConsts.LAST_DOW,AllowedList, PartNumber)
        {
            DateTime today = DateTime.Today;
            _startDay = today.Day;
            _startMonth = today.Month;
            _startYear = today.Year;
            _startDow = (int)today.DayOfWeek;
        }

        public override bool IsCheckOnly { get { return true; } }

        public static AllowedDateTimePart CreateDateTimePart(bool[] AllowedList)
        {
            return new AllowedDowPart(AllowedList, PartConsts.DOW);
        }

        int NumFebs29InRange(int[] ValueParts, bool CountBack)
        {
            int first_year, last_year; //Range of years to search for leap ones

            if (CountBack)
            {
                first_year = ValueParts[PartConsts.YEARS];
                if (ValueParts[PartConsts.MONTHS] > PartConsts.FEBRUARY_MONTH) first_year++;
                last_year = _startYear;
                if (_startMonth <= PartConsts.FEBRUARY_MONTH) last_year--;
            }
            else
            {
                first_year = _startYear;
                if (_startYear > PartConsts.FEBRUARY_MONTH) first_year++;
                last_year = ValueParts[PartConsts.YEARS];
                if (ValueParts[PartConsts.MONTHS] <= PartConsts.FEBRUARY_MONTH) last_year--;
            }
            int result = 0;
            for(int year=first_year;year<last_year;year++)
                if(PartConsts.IsLeapYear(year)) result++;
            return result;
        }

        public override bool ValueIsAllowed(int[] ValueParts)
        {
            //Has side effect! Computes and sets ValueParts[DOW] value.
            bool count_back = _startYear > ValueParts[PartConsts.YEARS] ||
                (_startYear == ValueParts[PartConsts.YEARS] && (_startMonth > ValueParts[PartConsts.MONTHS])
                || _startMonth == ValueParts[PartConsts.MONTHS] && _startDay > ValueParts[PartConsts.DAYS]);
            int feb29_count = NumFebs29InRange (ValueParts, count_back);

            int days_passed;
            days_passed = (ValueParts[PartConsts.DAYS] - _startDay)+ DaysInMonthsPassedNonLeap(_startMonth, ValueParts[PartConsts.MONTHS])
                +(ValueParts[PartConsts.YEARS]-_startYear)* PartConsts.DAYS_IN_NONLEAP_YEAR +(count_back?-feb29_count:feb29_count);
            int dow = (_startDow + days_passed) % PartConsts.DAYS_IN_WEEK;
            if (dow < 0) dow = (PartConsts.DAYS_IN_WEEK + _startDow) % PartConsts.DAYS_IN_WEEK;
            ValueParts[PartNumber] = dow;
            return base.ValueIsAllowed(dow, ValueParts);
        }

        private int DaysInMonthsPassedNonLeap(int CurMonth, int MonthToCome)
        {
            int result = 0;
            int months_to_pass = (MonthToCome + PartConsts.MONTHS_IN_YEAR - CurMonth) % PartConsts.MONTHS_IN_YEAR;
            for (int i = 0; i<months_to_pass;i++) 
                result+= PartConsts.DAYS_IN_MONTHS[(CurMonth+i- PartConsts.FIRST_MONTH) % PartConsts.MONTHS_IN_YEAR];
            return result;
        }

        public override bool StepValue(bool ToNext, int[] ValueParts)
        {
            throw new NotImplementedException(); //Should not ever be called
        }

        public override bool Wrap(bool ToNext, int[] ValueParts)
        {
            throw new NotImplementedException(); //Should not ever be called
        }

        public override int MinimalDependentPart { get { return PartConsts.DAYS; } }

    }
}
