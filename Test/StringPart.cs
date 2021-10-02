using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public ref struct StringPartArray
    {
        Span<ReadOnlyMemory<char>> _baseArray;
        int _length;
        public int Length { get { return _length; } }
        public int Capacity { get { return _baseArray.Length; } }
        public StringPartArray(int Capacity)
        {
            if (Capacity < 0) throw new InvalidOperationException("StringPartArray length must be non-negatitive");
            _length = 0;
            _baseArray = new ReadOnlyMemory<char>[Capacity];
            for (int i = 0; i < Capacity; i++) _baseArray[i] = ReadOnlyMemory<char>.Empty;
        }

        public ReadOnlyMemory<char> this[int Index] {
            get { if (Index >= Length) throw new IndexOutOfRangeException(); return _baseArray[Index]; }
            set { if (Index >= Length) throw new IndexOutOfRangeException(); _baseArray[Index]=value;}
        }
        internal void SetLength(int NewLength)
        {
            if (NewLength < 0 || NewLength > Capacity)
                throw new InvalidOperationException("StringPartArray length cannot be negative or exceeds capacity");
            int to_clear = _length - NewLength;
            _length = NewLength;
            for (int i = 0; i < to_clear; i++) _baseArray[_length + i]=ReadOnlyMemory<char>.Empty;
        }

        public bool Add(in ReadOnlyMemory<char> Part)
        {
            if (_length >= Capacity)
            {
                return false;
            }
            _baseArray[_length] = Part;
            _length++;
            return true;
        }

    }

    public static class StringPartExtension
    {
        public static bool Split(this ReadOnlyMemory<char> Source, char Delimiter, ref StringPartArray ResultSpace)
        {
            ResultSpace.SetLength(0);
            int nonchecked_yet_pos = 0;
            bool no_place_for_result;
            do
            {
                int old_pos = nonchecked_yet_pos;
                nonchecked_yet_pos = Source.IndexOf(Delimiter, nonchecked_yet_pos) + 1;
                no_place_for_result = !ResultSpace.Add(Source.Slice(old_pos, nonchecked_yet_pos == 0 ? Source.Length- old_pos : nonchecked_yet_pos - 1 - old_pos));
            } while (nonchecked_yet_pos > 0 && !no_place_for_result);
            return !no_place_for_result;
        }

        public static bool Split(this string SourceString, char Delimiter, ref StringPartArray ResultSpace)
        {
            return SourceString.AsMemory().Split(Delimiter, ref ResultSpace);
        }

        public static int IndexOf(this ReadOnlyMemory<char> Source, char Value, int StartPos = 0)
        {
            int result;
            if (StartPos < 0 || StartPos > Source.Length) throw new ArgumentOutOfRangeException();
            if (!Source.IsEmpty)
            {
                result = Source.Span.Slice(StartPos).IndexOf(Value);
                if (result >= 0) result += StartPos;
            }
            else result = -1;
            return result;
        }


    }
}
