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
        //Each summary map element contains summary of the block of the lower-level map elements - result of OR operation of values of the elments.
        //The lowest level map is a map of the ancestor class uses and have formal index of -1
        //All other maps a stored in the Map array in the LevelInfo for the level(from lower to upper level)
        //A number of the lower-level map elements summarized at each level called the scale of the level 
        // are passed as an int[]  parameter of the constructor, the length of the array being the number of the additional maps used
        struct LevelInfo
        {
            internal bool[] Map; //Maps of summary for blocks of lower-level map values (one map for each level)
            internal int MinAllowed; //Block containing FirstAllowedValue (all blocks before this surely doesn't contain any value)
            internal int MaxAllowed; //Block containing LastAllowedValue (all blocks after this surely doesn't contain any value)
        }
        LevelInfo[] _levelInfos;
        int[] _scales; //Array of scales for each map level
        int _baseValue;//To remeber minimal value for this part
        int _baseMapLength; //To remeber size of the full map
        protected AllowedDateTimePartMapped(int MinAllowed, int MaxAllowed, bool[] AllowedList, int PartNumber,int [] Scales) :
            base(MinAllowed, MaxAllowed, AllowedList,PartNumber)
        {
            if (AllowedList != null)
            {
                _scales = Scales;
                _levelInfos = new LevelInfo[_scales.Length];
                _baseValue = MinAllowed;
                _baseMapLength = MaxAllowed - MinAllowed + 1;
                bool[] prev_map = AllowedList;
                for (int i = 0; i < _scales.Length; i++)
                {
                    _levelInfos[i].Map = new bool[(prev_map.Length + _scales[i] - 1) / _scales[i]];
                    for (int j = 0; j < _levelInfos[i].Map.Length; j++)
                    {
                        _levelInfos[i].Map[j] = false;
                        for (int k = 0; k < Math.Min(_scales[i], prev_map.Length - j * _scales[i]) && !_levelInfos[i].Map[j]; k++)
                            if (prev_map[j * _scales[i] + k]) _levelInfos[i].Map[j] = true;
                    }
                    prev_map = _levelInfos[i].Map;
                    _levelInfos[i].MinAllowed = (i==0? FirstAllowedValue-_baseValue:_levelInfos[i-1].MinAllowed) / _scales[i];
                    _levelInfos[i].MaxAllowed = (i == 0 ? LastAllowedValue-_baseValue : _levelInfos[i - 1].MaxAllowed) / _scales[i];
                }
            }
        }

        public static AllowedDateTimePart CreateDateTimePart(int MinAllowed, int MaxAllowed, bool[] AllowedList, int PartNumber, int[] Scales)
        {
            return new AllowedDateTimePartMapped(MinAllowed, MaxAllowed, AllowedList, PartNumber, Scales);
        }
        public override bool StepValue(bool ToNext, int[] ValueParts)
        {
            if (_scales == null) return base.StepValue(ToNext, ValueParts); //If any value allowed, the base implementation is good enough
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
                int block_size = 0;
                //Determine the block boundary in the search direction (becoming the whole map boundary if we are to search in the top-level map)
                if (map_no + 1 < _scales.Length) {
                    block_size = _scales[map_no + 1]; //Size of block of map to search
                    map_limit = (cur_value / block_size) * block_size + (ToNext ? block_size : -1); //Set appropriate (upper/lower) limit of search
                    //Correct search limit to avoid useless processing beyond the range of allowed values
                    map_limit = ToNext?
                        Math.Min(map_limit, map_no<0? LastAllowedValue-_baseValue+1 : _levelInfos[map_no].MaxAllowed+1) 
                        : Math.Max(map_limit,map_no < 0?FirstAllowedValue-_baseValue-1: _levelInfos[map_no].MinAllowed - 1); 
                }
                else map_limit = ToNext ? _levelInfos[map_no].Map.Length : -1;
                //Search for the next/prev valid value/block at this level
                value_tocome = map_no < 0 ?
                    FindNextPrevValue(cur_value + _baseValue, map_limit + _baseValue) : //Search in the full map via ancestor class method
                    FindNextPrevValue(cur_value, map_limit, _levelInfos[map_no].Map, 0); //Search in the block map
                if (value_tocome.HasValue) break; //Valid value/block found in the required direction at this level. No need to search upper level map
                //Prepare to search ove level upper (if it exists)
                if(map_no+1 < _levelInfos.Length) cur_value = cur_value / block_size; //Scale the cur_value if at least one serch to be performed
                map_no++; //Increment map number
            } while (map_no<_levelInfos.Length);
            //See if valid value/block with valid value found on some level 
            if(!value_tocome.HasValue)  {
                //No more blocks with valid values found on the top level in the specified direction
                //So perform a wrap
                ValueParts[PartNumber] = ToNext ? FirstAllowedValue : LastAllowedValue;
                return false;
            }
            //Come here if valid value/block with valid value was found on some level 
            cur_value = value_tocome.Value;
            while (map_no >= 0) { //We have found not value itself but a block of the upper level, containing it
                //Search at the lower level to find valid value or block of the lower level, containing it
                //in the case of block, not the value, decrease level an continue search on the level below
                map_limit = cur_value * _scales[map_no]+(ToNext ?  _scales[map_no] : -1); //Compute the block boundary for the lower level map
                cur_value = cur_value * _scales[map_no]+(ToNext?-1:_scales[map_no]); //Compute a start value to search on the lower level
                map_no--; //Come one level lower to perform search on it
                //Correct block limit to avoid possible reading beyond the map
                map_limit = Math.Min(map_limit, map_no<0?_baseMapLength:_levelInfos[map_no].Map.Length); 
                //Search for the next/prev valid value/block at this level
                value_tocome = map_no < 0 ? 
                    FindNextPrevValue(cur_value + _baseValue, map_limit + _baseValue) : //Search in the full map via ancestor class method
                    FindNextPrevValue(cur_value, map_limit, _levelInfos[map_no].Map, 0); //Search in the block map
                if (!value_tocome.HasValue) throw new Exception("No valid value found in the block marked as containing valid values");
                cur_value = value_tocome.Value;
            }
            //So we determined next/prev valid value, return it
            ValueParts[PartNumber] = cur_value;
            return true;
        }

    }
}
