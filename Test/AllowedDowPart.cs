using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class AllowedDowPart: AllowedDateTimePart
    {

        int _curDay, _curMonth, _curYear;
        int _curDow;
        AllowedDowPart(bool[] AllowedList):base(PartConsts.FIRST_DOW, PartConsts.LAST_DOW,AllowedList)
        {
            DateTime today = DateTime.Today;
            _curDay = today.Day;
            _curMonth = today.Month;
            _curYear = today.Year;
            _curDow = (int)today.DayOfWeek;
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
                last_year = _curYear;
                if (_curMonth <= PartConsts.FEBRUARY_MONTH) last_year--;
            }
            else
            {
                first_year = _curYear;
                if (_curYear > PartConsts.FEBRUARY_MONTH) first_year++;
                last_year = DateContext[PartConsts.YEARS];
                if (DateContext[PartConsts.MONTHS] <= PartConsts.FEBRUARY_MONTH) last_year--;
            }
            int result = 0;
            for(int year=first_year;year<last_year;year++)
                if(year % 4 == 0 && year % 400 != 0) result++;
            return result;
        }

        public override void SetContext(int[] DateContext)
        {
            //Has side effect! Computes and sets DateContext[DOW] value.
            bool count_back = _curYear > DateContext[PartConsts.YEARS] ||
                (_curYear == DateContext[PartConsts.YEARS] && (_curMonth > DateContext[PartConsts.MONTHS])
                || _curMonth == DateContext[PartConsts.MONTHS] && _curDay > DateContext[PartConsts.DAYS]);
            int feb29_count = NumFebs29InRange (DateContext, count_back);

            int days_passed;
            days_passed = (DateContext[PartConsts.DAYS] - _curDay)+ DaysInMonthsPassedNonLeap(_curMonth, DateContext[PartConsts.MONTHS])
                +(DateContext[PartConsts.YEARS]-_curYear)* PartConsts.DAYS_IN_NONLEAP_YEAR +(count_back?-feb29_count:feb29_count);
            _curDow = (_curDow + days_passed) % PartConsts.DAYS_IN_WEEK;
            if (_curDow < 0) _curDow = (PartConsts.DAYS_IN_WEEK + _curDow) % PartConsts.DAYS_IN_WEEK;
            _curDay = DateContext[PartConsts.DAYS];
            _curMonth = DateContext[PartConsts.MONTHS];
            _curYear = DateContext[PartConsts.YEARS];
            DateContext[PartConsts.DOW] = _curDow;
        }

        private int DaysInMonthsPassedNonLeap(int CurMonth, int MonthToCome)
        {
            int result = 0;
            int months_to_pass = (MonthToCome + PartConsts.MONTHS_IN_YEAR - CurMonth) % PartConsts.MONTHS_IN_YEAR;
            for (int i = 0; i<months_to_pass;i++) 
                result+= PartConsts.DAYS_IN_MONTHS[(CurMonth+i- PartConsts.FIRST_MONTH) % PartConsts.MONTHS_IN_YEAR];
            return result;
        }

        public override int StepValue(int Value, bool ToNext, out bool NoWrap, ref bool NeedAdjustment)
        {
            throw new NotImplementedException(); //Should not ever be called
        }

        public override int Wrap(bool ToNext, out bool NoWrapMore)
        {
            throw new NotImplementedException(); //Should not ever be called
        }

        public override int MinimalDependentPart { get { return PartConsts.DAYS; } }

    }
}
