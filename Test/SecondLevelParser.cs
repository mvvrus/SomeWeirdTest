using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StringPart = System.String;


namespace Test
{
	public abstract class SecondLevelParser
	{
		public abstract bool Recognize(in ReadOnlyMemory<char> Part);
		public abstract bool Parse(in ReadOnlyMemory<char> Part, ref bool[][] AllowedLists);
	}

}
