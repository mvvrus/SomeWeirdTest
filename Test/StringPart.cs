using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class StringPart
    {
        internal string _baseString;
        internal int _start, _end;
        public StringPart(String BaseString)
        {
            _baseString = BaseString;
            _start = 0;
            if (BaseString != null) _end = BaseString.Length;
            else _end = 0;
        }

        internal void Clear()
        {
            _baseString = null;
            _start = 0;
            _end = 0;
        }

        public int Length { get { return _end - _start; } }
        public String BaseString { get { return _baseString; } }
        public int Start { get { return _start; } }
        public int End { get { return _end; } }
        public Char this[int Index] 
        { get{ 
                if (_baseString != null && Index>=0 && Index<Length) return _baseString[Index+_start]; 
                throw new IndexOutOfRangeException(); 
             } }
        public bool Truncate(int NewLength)
        {
            if (NewLength < 0 || NewLength > Length) return false;
            _end = _start + NewLength;
            return true;
        }

        public StringPartArray Split(char Delimiter, StringPartArray ResultSpace)
        {
            ResultSpace.SetLength(0);
            if (_baseString == null)
            {
                ResultSpace.Add(null, 0, 0);
                return ResultSpace;
            }
            int nonchecked_yet_pos = _start;
            bool no_place_for_result;
            do
            {
                int old_pos = nonchecked_yet_pos;
                nonchecked_yet_pos = _baseString.IndexOf(Delimiter, nonchecked_yet_pos, _end-nonchecked_yet_pos) + 1; 
                no_place_for_result = !ResultSpace.Add(_baseString, old_pos, nonchecked_yet_pos == 0 ? _end: nonchecked_yet_pos-1); 
            } while (nonchecked_yet_pos>0 && !no_place_for_result);
            return no_place_for_result ? null : ResultSpace;
        }

        public int IndexOf(char Value, int StartPos=0)
        {
            int result;
            if (StartPos < 0 || StartPos > Length) throw new ArgumentOutOfRangeException();
            if (_baseString != null)
            {
                result = _baseString.IndexOf(Value, StartPos + _start, Length - StartPos);
                if (result >= 0) result -= _start;
            }
            else result = -1;
            return result;
        }

        public StringPart SubPart(int RelativeStart, int RelativeEnd, StringPart WorkSpace)
        {
            WorkSpace._baseString = _baseString;
            if (RelativeStart < 0 || RelativeEnd < 0 || RelativeStart > Length || RelativeEnd > Length || RelativeEnd < RelativeStart)
                throw new ArgumentOutOfRangeException();
            WorkSpace._start = RelativeStart + _start;
            WorkSpace._end = RelativeEnd + _start;
            return WorkSpace;
        }

        public override string ToString()
        {
            return _baseString==null?String.Empty:_baseString.Substring(Start,Length);
        }


    }

    public class StringPartArray
    {
        readonly StringPart[] _baseArray;
        int _length;
        readonly int _capacity;
        public int Length { get { return _length; } }
        public int Capacity { get { return _capacity; } }
        public StringPartArray(int Capacity)
        {
            if (Capacity < 0) throw new InvalidOperationException("StringPartArray length cannot be negative");
            _length = 0;
            _capacity = Capacity;
            _baseArray = new StringPart[Capacity];
            for (int i = 0; i < _capacity; i++) _baseArray[i] = new StringPart(null);
        }

        public StringPart this[int Index] {
            get { if (Index >= _length) throw new IndexOutOfRangeException(); return _baseArray[Index]; }
        }
        internal void SetLength(int NewLength)
        {
            if (NewLength < 0 || NewLength > _capacity)
                throw new InvalidOperationException("StringPartArray length cannot be negative or exceeds capacity");
            int to_clear = _length - NewLength;
            _length = NewLength;
            for (int i = 0; i < to_clear; i++) _baseArray[_length + i].Clear();
        }

        public bool Add(String BaseString, int Start, int End)
        {
            if (_length >= _capacity) return false;
            _baseArray[_length]._baseString = BaseString;
            _baseArray[_length]._start = Start;
            _baseArray[_length]._end = End;
            _length++;
            return true;
        }
    }

    public static class StringPartExtension
    {
        public static StringPartArray Split(this string SourceString, char Delimiter, StringPartArray ResultSpace)
        {
            return new StringPart(SourceString).Split(Delimiter, ResultSpace);
        }


    }
}
