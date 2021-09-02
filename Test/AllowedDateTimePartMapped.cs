using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class AllowedDateTimePartMapped: AllowedDateTimePart
    {
        //This implementation uses multi-level summary maps to speed up seacrhing for the next/previous valid value
        //Each summary map element contains true if any of the lower-level map element contains true, false othervise
        //The lowest level map is a map of the ancestor class uses and have formal index of -1
        //All other maps a stored in the _maps array (from lower to upper level)
        //A number of the lower-level map elements summarized at each level called the scale of the level 
        // are passed as an int[]  parameter of the constructor, the length of the array being the number of the additional maps used
        bool[][] _maps; //Maps of summary for blocks of lower-level map values (one map for each level)
        int[] _scales; //Array of scales for each map level
        int _baseValue;//To remeber minimal value for this part
        int _baseMapLength; //To remeber size of the full map
        protected AllowedDateTimePartMapped(int MinAllowed, int MaxAllowed, bool[] AllowedList, int PartNumber,int [] Scales) :
            base(MinAllowed, MaxAllowed, AllowedList,PartNumber)
        {
            if (AllowedList != null)
            {
                _scales = Scales;
                _maps = new bool[_scales.Length][];
                _baseValue = MinAllowed;
                _baseMapLength = MaxAllowed - MinAllowed + 1;
                bool[] prev_map = AllowedList;
                for (int i = 0; i < _scales.Length; i++)
                {
                    _maps[i] = new bool[(prev_map.Length + _scales[i] - 1) / _scales[i]];
                    for (int j = 0; j < _maps[i].Length; j++)
                    {
                        _maps[i][j] = false;
                        for (int k = 0; k < Math.Min(_scales[i], prev_map.Length - j * _scales[i]) && _maps[i][j]; k++)
                            if (prev_map[j * _scales[i] + k]) _maps[i][j] = true;
                    }
                }
            }
        }

        public override bool StepValue(bool ToNext, ref Span<int> ValueParts)
        {
            //The implementation of this method (searcing for next/previsous valid value)
            //is optimized the following way. The method does not perform a linear scan of the whole map
            //but tries to find the valid value within a block of values, presented by one upper-level map
            //If such a value is not found within a block the method tries to find the thirst block in the specified direction 
            // that contains valid value, the process being recursive in the case of the block is not found within a block of upper-level map
            //Then the block of valid values is found, a process (also recursive) is performed to find the valid value number, that is returned
            int value = ValueParts[PartNumber]; //Value to start from, starting from _baseValue (AKA MinValue in the possible range of values)
            int cur_value = value-_baseValue; //The start value of search (zero based) at the level of block we are to searc
                                              //will be scaled by block map of the level of the map we search 
            int map_limit; //Limit of value to so search (block noundary) according to the level of a map we are searching in
            int map_no=-1; //Level of a map we are searching in (-1 - full map of the ancestor class, otherwise - index of block map to search in)
            int? value_tocome; //Result of search performed
            do  { //Perform search on all required levels
                //Determine the block boundary in the search direction (becoming the whole map boundary if we are search in to-level map)
                if (map_no + 1 < _scales.Length) {
                    map_limit = (cur_value / _scales[map_no + 1]) * _scales[map_no + 1] + (ToNext ? _scales[map_no+1] : -1);
                    //Correct block limit to avoid reading beyond the map
                    map_limit = Math.Min(map_limit, map_no<0? _baseMapLength : _maps[map_no].Length); 
                }
                else map_limit = ToNext ? _maps[map_no].Length : -1;
                //Search for the next/prev valid value/block at this level
                value_tocome = map_no < 0 ?
                    FindNextPrevValue(cur_value + _baseValue, map_limit + _baseValue) : //Search in the full map via ancestor class method
                    FindNextPrevValue(cur_value, map_limit, _maps[map_no], 0); //Search in the block map
                if (value_tocome.HasValue) break; //Valid value/block found in the required direction at this level. No need to search upper level map
                //Prepare to search ove level upper (if it exists)
                if(map_no < _maps.Length) cur_value = cur_value / _scales[map_no + 1]; //Scale the 
                map_no++;
            } while (map_no<_maps.Length);
            //See if valid value/block with valid value found on some level 
            if(!value_tocome.HasValue)  {
                //No more blocks with valid values found on the top level in the specified direction
                //So perform a wrap
                ValueParts[PartNumber] = ToNext ? FirstAllowedValue : LastAllowedValue;
                return false;
            }
            //Come here if valid value/block with valid value was found on some level 
            while (map_no >= 0) { //We have found not value itself but a block of the upper level, containing it
                //Search at the lower level to find valid value or block of the lower level, containing it
                //in the case of block, not the value, decrease level an continue search on the level below
                map_limit = cur_value * _scales[map_no]+(ToNext ? -1 : _scales[map_no]); //Compute the block boundary for the lower level map
                cur_value = cur_value * _scales[map_no]+(ToNext?0:_scales[map_no]-1); //Compute a start value to search on the lower level
                map_no--; //Come one level lower to perform search on it
                //Correct block limit to avoid possible reading beyond the map
                map_limit = Math.Min(map_limit, map_no<0?_baseMapLength:_maps[map_no].Length); 
                //Search for the next/prev valid value/block at this level
                value_tocome = map_no < 0 ? 
                    FindNextPrevValue(cur_value + _baseValue, map_limit + _baseValue) : //Search in the full map via ancestor class method
                    FindNextPrevValue(cur_value, map_limit, _maps[map_no], 0); //Search in the block map
                if (!value_tocome.HasValue) throw new Exception("No valid value found in the block marked as containing valid values");
                cur_value = value_tocome.Value;
            }
            //So we determined next/prev valid value, return it
            ValueParts[PartNumber] = cur_value;
            return true;
        }

    }
}
