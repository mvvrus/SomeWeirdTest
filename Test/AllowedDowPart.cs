using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class AllowedDowPart: AllowedDateTimePart
    {

        AllowedDowPart(bool[] AllowedList): this(AllowedList, DateTime.Today)
        {
        }

        internal AllowedDowPart(bool[] AllowedList, DateTime _1) : base(PartConsts.FIRST_DOW, PartConsts.LAST_DOW, AllowedList, PartConsts.DOW)
        {
        }


        public override bool IsDependent { get { return true; } }

        public static AllowedDateTimePart CreateDateTimePart(bool[] AllowedList)
        {
            return new AllowedDowPart(AllowedList);
        }

        static public int DayOfWeek(int Year, int Month, int Day)
        {
            DateTime t = new DateTime(Year, Month, Day);
            return (int)t.DayOfWeek;
        }

        public override bool ValueIsAllowed(ref Span<int> ValueParts)
        {
            //Has side effect! Computes and sets ValueParts[DOW] value.
            int dow = DayOfWeek(Year: ValueParts[PartConsts.YEARS], Month: ValueParts[PartConsts.MONTHS], Day: ValueParts[PartConsts.DAYS]);
            ValueParts[PartNumber] = dow;
            return base.ValueIsAllowed(dow, ref ValueParts);
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
