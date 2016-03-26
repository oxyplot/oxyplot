using System.Collections.Generic;

namespace OxyPlot.Series
{
	public class DataPointHelper
	{
		/// <summary>
		/// Find index of max(x) &lt;= target x in a list of data points
		/// </summary>
		/// <param name='items'>
		/// vector of data points
		/// </param>
		/// <param name='targetX'>
		/// target x.
		/// </param>
		/// <param name='guess'>
		/// initial guess index.
		/// </param>
		/// <returns>
		/// index of x with max(x) &lt;= target x or -1 if cannot find
		/// </returns>
		public static int FindIndex(List<DataPoint> items, double targetX, int guess)
		{
			int lastguess = 0;
			int start = 0;
			int end = items.Count - 1;

			while (start <= end)
			{
				if (guess < start)
				{
					return lastguess;
				}
				else if (guess > end)
				{
					return end;
				}

				var guessX = items[guess].X;
				if (guessX.Equals(targetX))
				{
					return guess;
				}
				else if (guessX > targetX)
				{
					end = guess - 1;
					if (end < start)
					{
						return lastguess;
					}
					else if (end == start)
					{
						return end;
					}
				}
				else
				{
					start = guess + 1;
					lastguess = guess;
				}

				if (start >= end)
				{
					return lastguess;
				}

				var endX = items[end].X;
				var startX = items[start].X;

				var m = (end - start + 1) / (endX - startX);
				guess = start + (int)((targetX - startX) * m);
			}

			return lastguess;
		}
	}
}