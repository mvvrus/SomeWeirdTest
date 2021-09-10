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
        AllowedDowPart(bool[] AllowedList): this(AllowedList, DateTime.Today)
        {
        }

        internal AllowedDowPart(bool[] AllowedList, DateTime BaseDate) : base(PartConsts.FIRST_DOW, PartConsts.LAST_DOW, AllowedList, PartConsts.DOW)
        {
            _startDay = BaseDate.Day;
            _startMonth = BaseDate.Month;
            _startYear = BaseDate.Year;
            _startDow = (int)BaseDate.DayOfWeek;
        }


        public override bool IsDependent { get { return true; } }

        public static AllowedDateTimePart CreateDateTimePart(bool[] AllowedList)
        {
            return new AllowedDowPart(AllowedList);
        }

        int NumFebs29InRange(in Span<int> ValueParts, bool CountBack)
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
                if (_startMonth > PartConsts.FEBRUARY_MONTH) first_year++;
                last_year = ValueParts[PartConsts.YEARS];
                if (ValueParts[PartConsts.MONTHS] <= PartConsts.FEBRUARY_MONTH) last_year--;
            }
            int result = 0;
            for(int year=first_year;year<=last_year;year++)
                if(PartConsts.IsLeapYear(year)) result++;
            return result;
        }

        public override bool ValueIsAllowed(ref Span<int> ValueParts)
        {
            //Has side effect! Computes and sets ValueParts[DOW] value.
            bool count_back = _startYear > ValueParts[PartConsts.YEARS] ||
                (_startYear == ValueParts[PartConsts.YEARS] && ((_startMonth > ValueParts[PartConsts.MONTHS])
                || _startMonth == ValueParts[PartConsts.MONTHS] && _startDay > ValueParts[PartConsts.DAYS]));
            int feb29_count = NumFebs29InRange (in ValueParts, count_back);

            int days_passed,month_days_passed;
            int month_int_start=_startMonth, month_int_end=ValueParts[PartConsts.MONTHS];
            if(count_back)
            {
                month_int_start = ValueParts[PartConsts.MONTHS];
                month_int_end = _startMonth; 
            }
            days_passed = (ValueParts[PartConsts.DAYS] - _startDay);
            month_days_passed = DaysInMonthsPassedNonLeap(month_int_start, month_int_end);
            if (count_back) month_days_passed = -month_days_passed;
            days_passed += month_days_passed;
            int full_years_passed = ValueParts[PartConsts.YEARS] - _startYear;
            if (month_int_start > month_int_end) 
                if (count_back) full_years_passed++; 
                else  full_years_passed--;
            days_passed += full_years_passed * PartConsts.DAYS_IN_NONLEAP_YEAR +(count_back?-feb29_count:feb29_count);
            int dow = (_startDow + days_passed) % PartConsts.DAYS_IN_WEEK;
            if (dow < 0) dow = PartConsts.DAYS_IN_WEEK + dow;
            ValueParts[PartNumber] = dow;
            return base.ValueIsAllowed(dow, ref ValueParts);
        }

        private int DaysInMonthsPassedNonLeap(int MonthIntervalStart, int MonthIntervalEnd)
        {
            int result = 0;
            int months_to_pass = (MonthIntervalEnd + PartConsts.MONTHS_IN_YEAR - MonthIntervalStart) % PartConsts.MONTHS_IN_YEAR;
            for (int i = 0; i<months_to_pass;i++) 
                result+= PartConsts.DAYS_IN_MONTHS[(MonthIntervalStart+i- PartConsts.FIRST_MONTH) % PartConsts.MONTHS_IN_YEAR];
            return result;
        }

        public override bool StepValue(bool ToNext, ref Span<int> ValueParts)
        {
            throw new NotImplementedException(); //Should not ever be called
        }

        public override bool Wrap(bool ToNext, ref Span<int> ValueParts)
        {
            throw new NotImplementedException(); //Should not ever be called
        }

        public override int MinimalDependentPart { get { return PartConsts.DAYS; } }

    }
}
