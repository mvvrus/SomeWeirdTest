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
		static StringPartArray _mainPartsSpace = new StringPartArray(new StringPart[MAX_SCHEDULE_PARTS]);
		static StringPartArray SplitInitialString (String scheduleString)
        {
			return scheduleString.Split(' ', ref _mainPartsSpace);
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
		
		//Next two function pairs acquires/releases memory for int arrays of size NUM_PARTS (2 purposes, see below) 
		// according to the memory allocation strategy.
		// For now we use the simpliest strategy - heap allocation/garbage collection but the strategy may be changed for a purpose of optimization
		int[] AcquireAdjustmentStack() //Acquire memory for stack(implemented as array) of parts that need to be adjusted
		{
			return new int[PartConsts.NUM_PARTS];
        }

		void ReleaseAdjustmentStack(int[] _1) //Release memory for stack(implemented as array) of parts that need to be adjusted
        {
			//Do nothing
        }

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
				if (!ScheduleParts[i].ValueIsAllowed(ValueParts[i])) return false;
			return true;
		}

		bool StepToNearestEvent(bool ToNext, ref int[] ValueParts)
        {
			int part_number;
			bool step_made = false;
			int[] parts_to_adjust = AcquireAdjustmentStack();
			try {
				part_number = PartConsts.NUM_PARTS - 1;
				bool need_adjustment = false;
				bool still_valid = true;
				bool no_wrap = true;
				do
				{ 
					do {
						//Process intiallly invalid values
						ScheduleParts[part_number].SetContext(ValueParts);
						if (!ScheduleParts[part_number].IsCheckOnly) //Check-only steps are always skipped here
						{
							if (still_valid || !no_wrap) //Need make validation and/or step
							{
								if (still_valid) //Should continue validation from upper parts if it's not the lowest part (for which a step is required)
									still_valid = part_number>0 || ScheduleParts[part_number].ValueIsAllowed(ValueParts[part_number]);
								if (!still_valid) //Validation of this or upper parts failed;
									ValueParts[part_number] =
										ScheduleParts[part_number].StepValue(ValueParts[part_number], ToNext, out no_wrap, ref need_adjustment);
							}
							else //A step to next/prev value somewhere in the upper part processing occured.
								ValueParts[part_number] = ScheduleParts[part_number].Wrap(ToNext, out no_wrap);
						}
						if (no_wrap) part_number--; else part_number++;
					} while (part_number >= 0 && part_number < PartConsts.NUM_PARTS);
					step_made = true;
					if (part_number<0)//All parts where the step is possible (i.e. non-checkonly) are valid here after the step made
						for (part_number = 0; part_number < PartConsts.NUM_PARTS; part_number++) {//Validate check-only parts
						   if(ScheduleParts[part_number].IsCheckOnly)
                           {
								if (!ScheduleParts[part_number].ValueIsAllowed(ValueParts[part_number]))
                                {
									//Validity of this part was violated (it's possible for composite part, like DayOfWeek, checks).
									//Restart making the step from the first part, from wich this composite part is dependent
									part_number = ScheduleParts[part_number].MinimalDependentPart; //Reset the current part number
									step_made = false;
								}
							}
						}
				} while (!step_made);

				do
				{
					need_adjustment=false;
					int adjust_from = -1;
					part_number = 0;
					do
					{
						ScheduleParts[part_number].SetContext(ValueParts);
						if (step_made)
						{ //Successfully made the step in one of previous parts.
						  //No step on this and subsequent parts is needed, 
						  // but we must check for validity of subsequent parts, because it may changed for composite parts, like DayOfWeek
							if (!ScheduleParts[part_number].ValueIsAllowed(ValueParts[part_number]))
							{
								//Validity of this part was violated (it's possible for composite part, like DayOfWeek, checks).
								//Restart making the step from the first part, from wich this composite part is dependent
								part_number = ScheduleParts[part_number].MinimalDependentPart; //Reset the current part number
								//Restore "adjustment needed" status for parts, for which the step won't be restarted, but those can be affected
								adjust_from = -1;
								for (int prev_part_number=0;prev_part_number<part_number;prev_part_number++)
									if (ScheduleParts[prev_part_number].ShouldReadjust(ref ValueParts[prev_part_number],ToNext))
										parts_to_adjust[++adjust_from] = part_number;
								step_made = false;
								break;
							}
						}
						else {
							//Must make the step here for some reason 
							// for the first time or after wraping around on previous step or due to restart on unsuccessful chechk or adjustment
							if (!ScheduleParts[part_number].IsCheckOnly) { //... but only if part allows making step
								//Make a step 
								ValueParts[part_number]=
									ScheduleParts[part_number].StepValue(ValueParts[part_number],ToNext,out step_made,ref need_adjustment);
								if (step_made) {
									//Try to adjust previous parts that may be affected (e.g. day of the month part due to nuber of days in the month changed
									while (adjust_from >= 0) {
										//Try to adjust this part
										int part_to_adjust = adjust_from--; //Push the part index of the adjustment from stack
										  //We can made it here, because even if the step is restarted after this part, 
										  //(in the case of unsuccessful check) the ajustment status of this part will be set accordingly
										bool adjusted;
										ScheduleParts[part_to_adjust].SetContext(ValueParts);
										ValueParts[part_to_adjust] = ScheduleParts[part_to_adjust].Adjust(ValueParts[part_to_adjust], ToNext, out adjusted);
										if(!adjusted) {
											//The adjustment attempt of a previous part is unsuccessful (will end up causing wrap).
											//Restart making step from this part
											part_number = part_to_adjust;
											step_made = false;
											break;
										}
									}
								}
								if (need_adjustment) parts_to_adjust[++adjust_from] = part_number;
							}
						}
						part_number++;
					} while (part_number< PartConsts.NUM_PARTS); 
					if(!step_made && part_number>= PartConsts.NUM_PARTS) return false; //No more events
				} while (!step_made); //Repeat in the case of restart
			}
			finally { 
				ReleaseAdjustmentStack(parts_to_adjust); 
			}
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
