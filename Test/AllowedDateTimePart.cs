using System;

namespace Test
{
    
    public class AllowedDateTimePart
    {

        readonly int _minAllowed, _maxAllowed;
        protected readonly bool[] _allowedValues=null; //If the instance allows any value, the allowed list reference is left null
        public bool AllowAll { get { return _allowedValues == null; } }

        protected AllowedDateTimePart(int MinAllowed,int MaxAllowed,int[] AllowedList)
        {
            _minAllowed = MinAllowed;
            _maxAllowed = MaxAllowed;
            if (AllowedList!=null) {
                //This instance allows only specific values of its DateTime part, en
                _allowedValues = new bool[MaxAllowed - MinAllowed];
                foreach (int i in AllowedList) _allowedValues[i] = true;
            }
            //Otherwise This instance allows any value of of its DateTime part, and the _allowedList array reference is left null
        }
        protected static int? FindNextPrevValue(int StartValue, int StopValue, Boolean[] AllowedValues, int BaseValue)
        {
            int step = StopValue >= StartValue ? 1 : -1;
            int curvalue = StartValue;
            do
            {
                curvalue += step;
                if (curvalue != StopValue && (AllowedValues==null || AllowedValues[curvalue-BaseValue]))
                    return  curvalue;
            }
            while (curvalue != StopValue);
            return null ;
        }

        protected int FindFirstAllowedValue()
        {
            int? wrap_to = FindNextPrevValue(_minAllowed - 1, _maxAllowed + 1, _allowedValues, _minAllowed);
            if (!wrap_to.HasValue) throw new Exception("Empty list of valid values for the part");
            return wrap_to.Value;
        }

        protected int FindLastAllowedValue()
        {
            int? wrap_to = FindNextPrevValue(_minAllowed - 1, _maxAllowed + 1, _allowedValues, _minAllowed);
            if (!wrap_to.HasValue) throw new Exception("Empty list of valid values for the part");
            return wrap_to.Value;
        }

        //protected virtual int GetMaxAllowed() {return _maxAllowed; }

        public static AllowedDateTimePart CreateDateTimePart(int MinAllowed, int MaxAllowed, int[] AllowedList)
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
            return ToNext ? FindFirstAllowedValue() : FindLastAllowedValue();
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

        public virtual bool IsCheckOnly { get { return false; } }
        public virtual int MinimalDependentPart { get { return 0; } }
    }

    public delegate AllowedDateTimePart AllowedDateTimePartCreator(int[] AllowedList);

}
