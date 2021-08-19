using System;

namespace Test
{
    
    public class AllowedDateTimePart
    {

        readonly int _minAllowed, _maxAllowed;
        protected readonly bool[] _allowedValues=null; //If the instance allows any value, the allowed list reference is left null
        protected bool JustWrapped { get; private set; }
        protected int FirstAllowedValue { get; private set; }
        protected int LastAllowedValue { get; private set; }
        public bool AllowAll { get { return _allowedValues == null; } }

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

        protected AllowedDateTimePart(int MinAllowed,int MaxAllowed,bool[] AllowedList)
        {
            _minAllowed = MinAllowed;
            _maxAllowed = MaxAllowed;
            _allowedValues = AllowedList;
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

        public static AllowedDateTimePart CreateDateTimePart(int MinAllowed, int MaxAllowed, bool[] AllowedList)
        {
            return new AllowedDateTimePart(MinAllowed, MaxAllowed, AllowedList);
        }
        public virtual bool ValueIsAllowed(int Value) {
            if (AllowAll) return true;
            return _allowedValues[Value-_minAllowed]; 
        }

        public virtual void SetContext(int[] DateContext) { }

        public virtual int StepValue(int Value, bool ToNext, out bool NoWrap, ref bool NeedAdjustment)
        {
            //Search for the next allowed value forward/back
            int? result = FindNextPrevValue(Value, ToNext?_maxAllowed + 1: _minAllowed - 1, _allowedValues, _minAllowed);
            NoWrap = result.HasValue;
            if (result.HasValue) return result.Value; //Allowed value found in the rest of the range.
            //No allowed value found in the rest of the range. Wrap shoud occur
            //Search the first(or last) allowed value in the whole range of values
            return ToNext ? FirstAllowedValue :LastAllowedValue;
        }

        public virtual int Adjust(int Value, bool ToNext, out bool adjusted)
        {
            adjusted = true;
            return Value;
        }

        public virtual bool ShouldReadjust(ref int Value, bool ToNext)
        {
            return false;
        }

        public virtual int Wrap(bool ToNext, out bool NoWrapMore)
        {
            NoWrapMore = true; //The allowed value always exists (it was checked during schedule string parsing)
            return ToNext ? FirstAllowedValue: LastAllowedValue;
        }

        public virtual bool IsCheckOnly { get { return false; } }
        public virtual int MinimalDependentPart { get { return 0; } }
    }

    public delegate AllowedDateTimePart AllowedDateTimePartCreator(bool[] AllowedList);

}
