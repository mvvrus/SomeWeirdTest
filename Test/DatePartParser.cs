using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringPart = System.String;

namespace Test
{
    class DatePartParser : TwoDelimParser
    {
        public const char DELIM = '.';
        static readonly PartListParserSpecifier[] _partParsers = new PartListParserSpecifier[]
        {
            new PartListParserSpecifier(PartConsts.DAYS, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH),
            new PartListParserSpecifier(PartConsts.MONTHS, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH),
            new PartListParserSpecifier(PartConsts.YEARS, PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR)
        };

        public DatePartParser() : base(DELIM, _partParsers.Length, _partParsers) { }

        public override bool Parse(StringPart Part, ref bool[][] AllowedLists)
        {
            if (!base.Parse(Part, ref AllowedLists)) return false;
            //Check if any valid combination of day/month (and maybe year) exists
            if (AllowedLists[PartConsts.DAYS] == null || AllowedLists[PartConsts.MONTHS] == null)
                return true; //all days and/or months are allowed
            if (AllowedLists[PartConsts.DAYS][PartConsts.LAST_DAY_IN_MONTH- PartConsts.FIRST_DAY_IN_MONTH])
                return true; //The last day in month is implicitly allowed
            int min_day_allowed = -1;
            for (int day = PartConsts.FIRST_DAY_IN_MONTH; day <PartConsts.LAST_DAY_IN_MONTH ; day++) //LAST_DAY_IN_MONTH is not a real day
                //Search for minimal day allowed (and return true if it's allowed in any month)
                if (AllowedLists[PartConsts.DAYS][day - PartConsts.FIRST_DAY_IN_MONTH]) {
                    if(day<=PartConsts.MIN_DAYS_IN_MONTH) return true; //Allowed day exists in any month
                    else {
                        //Allowed day doesn't exist in all months, need to check monts allowed
                        min_day_allowed = day;
                        break;
                    }
                }
            if (min_day_allowed < 0) throw new Exception("No allowed days in the month - it's impossible but happens");
            //Check months if any allowed one include the minimal allowed day
            for (int month = PartConsts.FIRST_MONTH; month <= PartConsts.LAST_MONTH; month++)
                if (AllowedLists[PartConsts.MONTHS][month - PartConsts.FIRST_MONTH] &&
                    PartConsts.DAYS_IN_MONTHS[month - PartConsts.FIRST_MONTH] >= min_day_allowed)
                    return true; //The allowed day exists in some allowed month
            if(min_day_allowed==PartConsts.MIN_DAYS_IN_MONTH+1)
            {
                //The minimal allowed day is 29th. We could come here only if February is the only allowed month
                //Should check for any leap year is allowed
                if (AllowedLists[PartConsts.YEARS] == null) return true;
                for (int year = PartConsts.FIRST_YEAR; year <= PartConsts.LAST_YEAR; year++)
                    if (AllowedLists[PartConsts.YEARS][year - PartConsts.FIRST_YEAR] && PartConsts.IsLeapYear(year))
                        return true; //Allowed leap year found, so "29th February only" is a valid schedule
            }
            return false; //No possible combination of allowed day/month/year exists. The schedule is considered invalid
        }


    }
}
