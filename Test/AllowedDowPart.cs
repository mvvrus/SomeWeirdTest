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
        AllowedDowPart(bool[] AllowedList):base(PartConsts.FIRST_DOW, PartConsts.LAST_DOW,AllowedList)
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
            return new AllowedDowPart(AllowedList);
        }

        int NumFebs29InRange(int[] DateContext, bool CountBack)
        {
            int first_year, last_year; //Range of years to search for leap ones

            if (CountBack)
            {
                first_year = DateContext[PartConsts.YEARS];
                if (DateContext[PartConsts.MONTHS] > PartConsts.FEBRUARY_MONTH) first_year++;
                last_year = _startYear;
                if (_startMonth <= PartConsts.FEBRUARY_MONTH) last_year--;
            }
            else
            {
                first_year = _startYear;
                if (_startYear > PartConsts.FEBRUARY_MONTH) first_year++;
                last_year = DateContext[PartConsts.YEARS];
                if (DateContext[PartConsts.MONTHS] <= PartConsts.FEBRUARY_MONTH) last_year--;
            }
            int result = 0;
            for(int year=first_year;year<last_year;year++)
                if(year % 4 == 0 && year % 400 != 0) result++;
            return result;
        }

        public override bool ValueIsAllowed(int _1, int[] DateContext)
        {
            //Has side effect! Computes and sets DateContext[DOW] value.
            bool count_back = _startYear > DateContext[PartConsts.YEARS] ||
                (_startYear == DateContext[PartConsts.YEARS] && (_startMonth > DateContext[PartConsts.MONTHS])
                || _startMonth == DateContext[PartConsts.MONTHS] && _startDay > DateContext[PartConsts.DAYS]);
            int feb29_count = NumFebs29InRange (DateContext, count_back);

            int days_passed;
            days_passed = (DateContext[PartConsts.DAYS] - _startDay)+ DaysInMonthsPassedNonLeap(_startMonth, DateContext[PartConsts.MONTHS])
                +(DateContext[PartConsts.YEARS]-_startYear)* PartConsts.DAYS_IN_NONLEAP_YEAR +(count_back?-feb29_count:feb29_count);
            int dow = (_startDow + days_passed) % PartConsts.DAYS_IN_WEEK;
            if (dow < 0) dow = (PartConsts.DAYS_IN_WEEK + _startDow) % PartConsts.DAYS_IN_WEEK;
            DateContext[PartConsts.DOW] = dow;
            return base.ValueIsAllowed(dow, DateContext);
        }

        private int DaysInMonthsPassedNonLeap(int CurMonth, int MonthToCome)
        {
            int result = 0;
            int months_to_pass = (MonthToCome + PartConsts.MONTHS_IN_YEAR - CurMonth) % PartConsts.MONTHS_IN_YEAR;
            for (int i = 0; i<months_to_pass;i++) 
                result+= PartConsts.DAYS_IN_MONTHS[(CurMonth+i- PartConsts.FIRST_MONTH) % PartConsts.MONTHS_IN_YEAR];
            return result;
        }

        public override int StepValue(int Value, bool ToNext, out bool NoWrap, int[] DateContext)
        {
            throw new NotImplementedException(); //Should not ever be called
        }

        public override int Wrap(bool ToNext, out bool NoWrapMore, int[] DateContext)
        {
            throw new NotImplementedException(); //Should not ever be called
        }

        public override int MinimalDependentPart { get { return PartConsts.DAYS; } }

    }
}
