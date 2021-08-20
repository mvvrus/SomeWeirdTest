using System;
using System.Collections.Generic;


namespace Test
{
	public class NoMoreEventsException: Exception
    {
		public NoMoreEventsException():base("No more events") { }
	}

	public class Schedule
	{
		static readonly AllowedDateTimePartCreator[] SchedulePartCreators = new AllowedDateTimePartCreator[PartConsts.NUM_PARTS] {
			AllowedList=> AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC, AllowedList),
			AllowedList=> AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_SEC, PartConsts.LAST_SEC, AllowedList),
			AllowedList=> AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MIN, PartConsts.LAST_MIN, AllowedList),
			AllowedList=> AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_HOUR, PartConsts.LAST_HOUR, AllowedList),
			AllowedDayPart.CreateDateTimePart,
			AllowedList=> AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH, AllowedList),
			AllowedList=> AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR, AllowedList),
			AllowedDowPart.CreateDateTimePart
		};

		static readonly SecondLevelParser[][] SecondLevelParsers = new SecondLevelParser[][] {
			new SecondLevelParser[] {new DatePartParser(), new DayOfWeekPartParser(), new TimePartParser() }, //yyyy.MM.dd w HH:mm:ss.fff, yyyy.MM.dd w HH:mm:ss
			new SecondLevelParser[] {new DatePartParser(), new TimePartParser() },                            //yyyy.MM.dd HH:mm:ss.fff, yyyy.MM.dd HH:mm:ss 
			new SecondLevelParser[] {new DatePartParser() },                                                  //HH:mm:ss.fff., HH:mm:ss
		};

		const int MAX_SCHEDULE_PARTS = 3;
		static readonly StringPartArray _mainPartsSpace = new StringPartArray(MAX_SCHEDULE_PARTS);
		static StringPartArray SplitInitialString (String scheduleString)
        {
			return scheduleString.Split(' ', _mainPartsSpace);
        }
		static void MainParser(String scheduleString, ref bool[][] AllowedLists)
        {
			lock(_mainPartsSpace)
            {
				StringPartArray scheduleParts = SplitInitialString(scheduleString);
				bool parse_failed = scheduleParts==null;
				if (!parse_failed)
				{
					bool recognized = false;
					SecondLevelParser[] parser_list = null;
					foreach (SecondLevelParser[] second_level_parser_list in SecondLevelParsers)
					{
						recognized = false;
						if (second_level_parser_list.Length == scheduleParts.Length)
						{
							for (int i = 0; i < scheduleParts.Length; i++)
								recognized = recognized || second_level_parser_list[i].Recognize(scheduleParts[i]);
						}
						if (recognized)
						{
							parser_list = second_level_parser_list;
							break;
						}
					}
					if (recognized)
					{
						for (int i = 0; i < parser_list.Length; i++)
							parse_failed = parse_failed || !parser_list[i].Parse(scheduleParts[i], ref AllowedLists);
					}
					else parse_failed = true;
				}
				if (parse_failed) throw new InvalidOperationException("Parsing the scheduling string'" + scheduleString + "' failed");
			}
		}

		readonly AllowedDateTimePart[] ScheduleParts = new AllowedDateTimePart[PartConsts.NUM_PARTS];
		public Schedule() : this(null)
		{
		}

		public Schedule(string scheduleString)
		{
			bool[][] AllowedLists = new bool[PartConsts.NUM_PARTS][];
			if (scheduleString != null)
				MainParser(scheduleString, ref AllowedLists);
			for (int i = 0; i < PartConsts.NUM_PARTS; i++)
				ScheduleParts[i] = SchedulePartCreators[i](AllowedLists[i]);
		}

		void SplitDateTimeToParts(DateTime Value, int[] ValueParts)
		{
			ValueParts[PartConsts.MSECS] = Value.Millisecond;
			ValueParts[PartConsts.SECS] = Value.Second;
			ValueParts[PartConsts.MINUTES] = Value.Minute;
			ValueParts[PartConsts.HOURS] = Value.Hour;
			ValueParts[PartConsts.DOW] = (int)Value.DayOfWeek;
			ValueParts[PartConsts.DAYS] = Value.Day;
			ValueParts[PartConsts.MONTHS] = Value.Month;
			ValueParts[PartConsts.YEARS] = Value.Year;
		}

		DateTime JoinDateTimeFromParts(int[] ValueParts) {
			return new DateTime(ValueParts[PartConsts.YEARS],
				ValueParts[PartConsts.MONTHS],
				ValueParts[PartConsts.DAYS],
				ValueParts[PartConsts.HOURS],
				ValueParts[PartConsts.MINUTES],
				ValueParts[PartConsts.SECS],
				ValueParts[PartConsts.MSECS]);
		}
		
		//Next function pair acquires/releases memory for int arrays of size NUM_PARTS 
		// according to the memory allocation strategy.
		// For now we use the simpliest strategy - heap allocation/garbage collection but the strategy may be changed for a purpose of optimization

		int[] AcquirePartsArray() //Acquire memory for  storing parts of the DateTme under processing in operations with the scedule 
		{
			return new int[PartConsts.NUM_PARTS];
		}
		void ReleasePartsArray(int[] _1) //Release memory for stack(implemented as array) of parts that need to be adjusted
		{
			//Do nothing
		}

		Boolean CheckCurrentEvent(int[] ValueParts)
		{
			for (int i = 0; i < PartConsts.NUM_PARTS; i++)
			{
				
				if (!ScheduleParts[i].ValueIsAllowed(ValueParts[i],ValueParts)) return false;
			}
			return true;
		}

		bool StepToNearestEvent(bool ToNext, ref int[] ValueParts)
        {
			bool step_made;
			int part_number = PartConsts.NUM_PARTS - 1;
			bool still_valid = true;
			bool no_wrap = true;
			do
			{ 
				do {
					//Process validation (initial) and making step to the next/prev event
					if (!ScheduleParts[part_number].IsCheckOnly) //Check-only steps are always skipped here
					{
						if (still_valid || !no_wrap) //Need make validation and/or step
						{
							if (still_valid) 
								//Should continue validation from upper parts 
								//if it's not the first part (for which a step is required)
								still_valid = part_number>0 || ScheduleParts[part_number].ValueIsAllowed(ValueParts[part_number], ValueParts);
							if (!still_valid) 
								//Validation of this or some upper parts (including check-only ones on previous run of the outer do-while cycle) failed 
								//or it's a first_step
								ValueParts[part_number] =
									ScheduleParts[part_number].StepValue(ValueParts[part_number], ToNext, out no_wrap, ValueParts);
						}
						else //A step to next/prev value somewhere in the upper part processing occured.
							ValueParts[part_number] = ScheduleParts[part_number].Wrap(ToNext, out no_wrap, ValueParts);
					}
					if (no_wrap) part_number--; else part_number++; //Change part_number accordingly of wheather we need perform step on the upper part
				} while (part_number >= 0 && part_number < PartConsts.NUM_PARTS); 
				//Come here if we complete making a step or determined that we could not complete it (no more events)
				step_made = part_number < PartConsts.NUM_PARTS;
				if (step_made)//Step is completed and all parts where the step is possible (i.e. non-checkonly) are valid here 
					//Validate check-only parts now
					for (part_number = 0; part_number < PartConsts.NUM_PARTS; part_number++) {
						if(ScheduleParts[part_number].IsCheckOnly)
                        {
							if (!ScheduleParts[part_number].ValueIsAllowed(ValueParts[part_number], ValueParts))
                            {
								//Validity of this part was violated (it's possible for composite part, like DayOfWeek, checks).
								//Restart making the step from the first part, from which this composite part is dependent
								part_number = ScheduleParts[part_number].MinimalDependentPart; //Reset the current part number
								step_made = false;
							}
						}
					}
			} while (!step_made);
			return step_made;
        }

		public DateTime NearestEvent(DateTime t1)
		{
			int[] daytime_parts = AcquirePartsArray();
			DateTime result;
            try
            {
				SplitDateTimeToParts(t1, daytime_parts);
				if (CheckCurrentEvent(daytime_parts)) result=t1;
				else
                {
					if(!StepToNearestEvent(true, ref daytime_parts)) throw new NoMoreEventsException();
					result = JoinDateTimeFromParts(daytime_parts);
                }
			}
			finally
            {
				ReleasePartsArray(daytime_parts);
            }
			return result;
		}

		public DateTime NearestPrevEvent(DateTime t1)
		{
			int[] daytime_parts = AcquirePartsArray();
			DateTime result;
			try
			{
				SplitDateTimeToParts(t1, daytime_parts);
				if (CheckCurrentEvent(daytime_parts)) result = t1;
				else
				{
					if (!StepToNearestEvent(false, ref daytime_parts)) throw new NoMoreEventsException();
					result = JoinDateTimeFromParts(daytime_parts);
				}
			}
			finally
			{
				ReleasePartsArray(daytime_parts);
			}
			return result;
		}

		public DateTime NextEvent(DateTime t1)
		{
			int[] daytime_parts = AcquirePartsArray();
			DateTime result;
			try
			{
				SplitDateTimeToParts(t1, daytime_parts);
				if (!StepToNearestEvent(true, ref daytime_parts)) throw new NoMoreEventsException();
				result = JoinDateTimeFromParts(daytime_parts);
			}
			finally
			{
				ReleasePartsArray(daytime_parts);
			}
			return result;
		}

		public DateTime PrevEvent(DateTime t1)
		{
			int[] daytime_parts = AcquirePartsArray();
			DateTime result;
			try
			{
				SplitDateTimeToParts(t1, daytime_parts);
				if (!StepToNearestEvent(false, ref daytime_parts)) throw new NoMoreEventsException();
				result = JoinDateTimeFromParts(daytime_parts);
			}
			finally
			{
				ReleasePartsArray(daytime_parts);
			}
			return result;
		}
	}
}
