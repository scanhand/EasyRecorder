﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aga.Diagrams.FlowChart
{
	public static class CollectionHelper
	{
		public static void RemoveRange<T>(this ICollection<T> source, Func<T, bool> predicate)
		{
			var arr = source.Where(p => predicate(p)).ToArray();
			foreach (var t in arr)
				source.Remove(t);
		}
	}
}
