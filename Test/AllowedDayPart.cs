using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class AllowedDayPart : AllowedDateTimePart
    {

        AllowedDayPart(bool[] AllowedList, int PartNumber) : base(PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH, AllowedList, PartNumber)
        {
            ;
        }

        public static AllowedDateTimePart CreateDateTimePart(bool[] AllowedList)
        {
            return new AllowedDayPart(AllowedList, PartConsts.DAYS);
        }

        int GetMaxAllowed(int CurrentMonth,bool LeapYear) 
        {
            return PartConsts.DAYS_IN_MONTHS[CurrentMonth - PartConsts.FIRST_MONTH] + (LeapYear && CurrentMonth == PartConsts.FEBRUARY_MONTH ? 1 : 0);
        }


        public override bool ValueIsAllowed(int[] ValueParts)
        {
            int Value = ValueParts[PartNumber];
            int current_month = ValueParts[PartConsts.MONTHS];
            bool leap_year = PartConsts.IsLeapYear(ValueParts[PartConsts.YEARS]);
            return base.ValueIsAllowed(Value,ValueParts) && Value<=GetMaxAllowed(current_month,leap_year) || 
                Value== this.GetMaxAllowed(current_month, leap_year) && base.ValueIsAllowed(PartConsts.LAST_DAY_IN_MONTH, ValueParts);
        }
        
        public override bool StepValue(bool ToNext, int[] ValueParts)
        {
            int Value = ValueParts[PartNumber];
            int day_from = Value; //Save value from which we start for check if the last day in month is allowed
            bool NoWrap = base.StepValue(ToNext, ValueParts);
            Value = ValueParts[PartNumber];
            int current_month = ValueParts[PartConsts.MONTHS];
            bool leap_year = PartConsts.IsLeapYear(ValueParts[PartConsts.YEARS]);
            int last_day_in_this_month = GetMaxAllowed(current_month,leap_year);
            if (Value> last_day_in_this_month) {
                if (NoWrap ) //We would find a good next day in the current month if it were longer
                {
                    if (ToNext) ///While steping forward? Two cases are possible
                    {
                        if (Value == PartConsts.LAST_DAY_IN_MONTH && day_from < last_day_in_this_month)
                            //Special case: The last day in the month is allowed implicitly (as day=32), but not explicitly
                            ValueParts[PartNumber] = last_day_in_this_month;
                        else
                        {
                            //General case: this month is two short or, may be, the last day was already allowed explicitly 
                            // So wrap around the range to the first valid value and force making step for a subsequent(month) part
                            ValueParts[PartNumber] = FirstAllowedValue;
                            NoWrap = false;
                        }
                    }
                    else //While stepping backward? This should never happen 
                        throw new Exception("Wrap forward while stepping backward");
                }
            }
            return NoWrap;
        }

        public override bool Wrap(bool ToNext, int[] ValueParts)
        {
            bool NoWrapMore = base.Wrap(ToNext, ValueParts);
            //The allowed day in this month may not exist, if all allowed days fall beyond the end of the month
            int current_month = ValueParts[PartConsts.MONTHS];
            bool leap_year = PartConsts.IsLeapYear(ValueParts[PartConsts.YEARS]);
            int last_day_in_this_month = GetMaxAllowed(current_month,leap_year);
            if (ValueParts[PartNumber]> last_day_in_this_month)
            {
                //The allowed day found is beyond the end of the month
                if (ValueParts[PartNumber] == PartConsts.LAST_DAY_IN_MONTH)  //The last day in month is implicitly allowed and is wrapped to.
                    ValueParts[PartNumber] = last_day_in_this_month;  // Return it
                else
                    //The day found is not the implicit "last day of the month"
                    if (ToNext)
                        //Wrapping forward beyond the number of days in this month occured
                        NoWrapMore = false; //No days in this month are allowed. Try next/previos month
                    else {
                        //Wrapping backward occured. Try to find the last allowed day in this month (if any)
                        int? wrap_to = FindNextPrevValue(last_day_in_this_month + 1, PartConsts.FIRST_DAY_IN_MONTH - 1);
                        NoWrapMore = wrap_to.HasValue; 
                        if (wrap_to.HasValue) ValueParts[PartNumber] = wrap_to.Value; //That day is found. Return it.
                    }
            }
            return NoWrapMore;
        }
    }
}
