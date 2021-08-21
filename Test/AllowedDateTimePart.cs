using System;

namespace Test
{

    public class AllowedDateTimePart
    {

        readonly int _minAllowed, _maxAllowed;
        readonly bool[] _allowedValues = null; //If the instance allows any value, the allowed list reference is left null
        readonly int _partNumber;
        protected int FirstAllowedValue { get; private set; }
        protected int LastAllowedValue { get; private set; }
        public bool AllAllowed { get { return _allowedValues == null; } }
        public int PartNumber { get { return _partNumber; } }

        static int? FindNextPrevValue(int StartValue, int StopValue, Boolean[] AllowedValues, int BaseValue)
        {
            int step = StopValue >= StartValue ? 1 : -1;
            int curvalue = StartValue;
            do
            {
                curvalue += step;
                if (curvalue != StopValue && (AllowedValues == null || AllowedValues[curvalue - BaseValue]))
                    return curvalue;
            }
            while (curvalue != StopValue);
            return null;
        }

        protected AllowedDateTimePart(int MinAllowed, int MaxAllowed, bool[] AllowedList, int PartNumber)
        {
            _minAllowed = MinAllowed;
            _maxAllowed = MaxAllowed;
            _allowedValues = AllowedList;
            _partNumber = PartNumber;
            if (_allowedValues != null)
            {
                int? temp;
                temp = FindNextPrevValue(_minAllowed - 1, _maxAllowed + 1, _allowedValues, _minAllowed);
                if (!temp.HasValue) throw new Exception("Empty list of valid values for the part");
                FirstAllowedValue=temp.Value;
                temp = FindNextPrevValue(_maxAllowed + 1, _minAllowed - 1, _allowedValues, _minAllowed);
                if (!temp.HasValue) throw new Exception("Empty list of valid values for the part");
                LastAllowedValue=temp.Value;
            }
            else
            {
                FirstAllowedValue = _minAllowed;
                LastAllowedValue = _maxAllowed;
            }
        }
        protected int? FindNextPrevValue(int StartValue, int StopValue)
        {
            return FindNextPrevValue(StartValue, StopValue, _allowedValues, _minAllowed);
        }

        public static AllowedDateTimePart CreateDateTimePart(int MinAllowed, int MaxAllowed, bool[] AllowedList, int PartNumber)
        {
            return new AllowedDateTimePart(MinAllowed, MaxAllowed, AllowedList, PartNumber);
        }
        public virtual bool ValueIsAllowed(int Value, int[] ValueParts) {
            if (AllAllowed) return true;
            return _allowedValues[Value-_minAllowed]; 
        }


        public virtual bool StepValue(bool ToNext, int[] ValueParts)
        {
            int Value=ValueParts[PartNumber];
            //Search for the next allowed value forward/back
            int? result = FindNextPrevValue(Value, ToNext?_maxAllowed + 1: _minAllowed - 1, _allowedValues, _minAllowed);
            if (result.HasValue) ValueParts[PartNumber] = result.Value;  //Allowed value found in the rest of the range.
            else
                //No allowed value found in the rest of the range. Wrap shoud occur
                //Search the first(or last) allowed value in the whole range of values
                ValueParts[PartNumber] = ToNext ? FirstAllowedValue :LastAllowedValue;
            return result.HasValue;
        }

        public virtual bool Wrap(bool ToNext, int[] ValueParts)
        {
            ValueParts[PartNumber] = ToNext ? FirstAllowedValue: LastAllowedValue;
            return true;//The allowed value always exists (it was checked during schedule string parsing)
        }

        public virtual bool IsCheckOnly { get { return false; } }
        public virtual int MinimalDependentPart { get { return 0; } }
    }

    public delegate AllowedDateTimePart AllowedDateTimePartCreator(bool[] AllowedList);

}
