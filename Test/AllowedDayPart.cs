using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class AllowedDayPart : AllowedDateTimePart
    {

        private int _current_month=1;
        private bool _leap_year;
        private int _day_after_step = 0;
        AllowedDayPart(int[] AllowedList) : base(PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH, AllowedList)
        {
            ;
        }

        public static AllowedDateTimePart CreateDateTimePart(int[] AllowedList)
        {
            return new AllowedDayPart(AllowedList);
        }

        int GetMaxAllowed() 
        {
            return PartConsts.DAYS_IN_MONTHS[_current_month - PartConsts.FIRST_MONTH] + (_leap_year && _current_month == PartConsts.FEBRUARY_MONTH ? 1 : 0);
        }

        public override bool ValueIsAllowed(int Value)
        {
            return base.ValueIsAllowed(Value) && Value<=GetMaxAllowed() || 
                Value== this.GetMaxAllowed() && base.ValueIsAllowed(PartConsts.LAST_DAY_IN_MONTH);
        }
        
        public override void SetContext(int[] DateContext) {
            _current_month = DateContext[PartConsts.MONTHS];
            _leap_year = DateContext[PartConsts.YEARS] % 4 == 0 && DateContext[PartConsts.MONTHS] % 400 != 0;
        }

        public override int StepValue(int Value, bool ToNext, out bool NoWrap, ref bool NeedAdjustment)
        {
            _day_after_step = 0;
            int day_from = Value; //Save value from which we start for check if the last day in month is allowed
            int result = base.StepValue(Value, ToNext, out NoWrap, ref NeedAdjustment);
            if (result > PartConsts.MIN_DAYS_IN_MONTH)  //Adjustment after change of month/year and reset due to DayOfWeek failed check may be required
                _day_after_step = result;
            int last_day_in_this_month = GetMaxAllowed();
            if (result> last_day_in_this_month) {
                if (NoWrap ) //We would find a good next day in the current month if it were longer
                {
                    if (ToNext) ///While steping forward? Two cases are possible
                    {
                        if (result == PartConsts.LAST_DAY_IN_MONTH && day_from < last_day_in_this_month)
                            //Special case: The last day in the month is allowed implicitly (as day=32), but not explicitly
                            result = last_day_in_this_month;
                            //Do not set NeedAdjustment here due to the month and year are cosidered final 
                            // (or their change will be followed  by reset)
                        else
                        {
                            //General case: this month is two short or, may be, the last day was already allowed explicitly 
                            // So wrap around the range to the first valid value and force making step for a subsequent(month) part
                            result = FindFirstAllowedValue();
                            NoWrap = false;
                        }
                    }
                    else //While stepping backward? This should never happen 
                        throw new Exception("Wrap forward while stepping backward");
                }
                else //Wrap backward occured, adjustment is required to set correct part value
                    NeedAdjustment = true; 
            }
            return result;
        }

        public override int Adjust(int Value, bool ToNext, out bool adjusted)
        {
            adjusted = true;
            int last_day_in_this_month = GetMaxAllowed();
            if (Value<= last_day_in_this_month)
                //No adjustment needed
                return Value;
            if (Value == PartConsts.LAST_DAY_IN_MONTH)
                //Adjust the allowed "last day in the month" to its real value
                return last_day_in_this_month;
            if (ToNext)  {
                //Adjust after stepping forward: any value except LAST_DAY_IN_MONTH (the case above) beyond the end of the month 
                // alwais means wrap to occur, so adjustment is unsuccessfull
                adjusted = false;
                return PartConsts.LAST_DAY_IN_MONTH; //To wrap forward in the step to follow
            }
            else
            {
                //Adjust after stepping backward: try to find the last allowed day in the month
                int? adjustment_result = FindNextPrevValue(last_day_in_this_month+1, PartConsts.FIRST_DAY_IN_MONTH - 1, _allowedValues, PartConsts.FIRST_DAY_IN_MONTH);
                if (adjustment_result.HasValue) return adjustment_result.Value;
                //Come here if no day in the current month is allowed e.g. in June if only 31-st days allowed
                adjusted = false;
                return PartConsts.FIRST_DAY_IN_MONTH; //To wrap backward in the step to follow
            }
        }

        public override bool ShouldReadjust(ref int Value, bool ToNext)
        {
            if (_day_after_step > 0) Value= _day_after_step;
            return _day_after_step > 0;
        }

    }
}
