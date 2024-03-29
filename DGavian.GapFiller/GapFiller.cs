﻿using DGavian.GapFiller.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DGavian.GapFiller
{
    public sealed class GapFiller<T> : IGapFiller<T> where T : IOffset, new()
    {
        private readonly decimal _expectedInterval;

        public GapFiller(decimal expectedInterval)
        {
            _expectedInterval = expectedInterval;
        }

        /// <summary>
        /// Fills gaps in any List&lt;T&gt; where T implements IOffset.
        /// </summary>
        /// <param name="items">A list of items with gaps to fill.</param>
        public void FillGaps(List<T> items)
        {
            FillGaps(items, GetDefaultRecord);
        }

        /// <summary>
        /// Fills gaps in any List&lt;T&gt; where T implements IOffset.
        /// </summary>
        /// <param name="items">A list of items with gaps to fill.</param>
        /// <param name="getDefaultRecord">A delegate wrapping a method to use to generate default records to fill gaps.</param>
        public void FillGaps(List<T> items, Func<decimal, T> getDefaultRecord)
        {
            var gapData = GetGapData(items);
            foreach (var gd in gapData)
            {
                items.InsertRange(gd.Index, GetGapFillData(gd, getDefaultRecord));
            }
        }

        private List<GapData> GetGapData(IEnumerable<T> items)
        {
            var result = new List<GapData>();

            var data = items.ToArray();
            // Set the first previous to the first element in the array.
            decimal previous = data[0].Offset;
            int count = 0;
            int runningCount = 0;

            // Start on the 2nd element.
            for (int i = 1; i < data.Length; i++)
            {
                decimal current = data[i].Offset;
                decimal gap = current - previous;
                if (gap > _expectedInterval)
                {
                    // Number of items to insert.
                    count = (int)((gap - _expectedInterval) / _expectedInterval);
                    // Increase the index by the running count to account for previously inserted item(s) shifting indexes. 
                    result.Add(new GapData { PreviousOffset = previous, Count = count, Index = i + runningCount });
                    runningCount += count;
                }
                previous = current;
            }

            return result;
        }

        private List<T> GetGapFillData(GapData gapData, Func<decimal, T> getDefaultRecord)
        {
            var result = new List<T>();
            for (int i = 1; i <= gapData.Count; i++)
            {
                decimal offset = gapData.PreviousOffset + (i * _expectedInterval);
                result.Add(getDefaultRecord(offset));
            }
            return result;
        }

        // Default implementation of default record just sets the offset and leaves any other fields as their default value.
        private T GetDefaultRecord(decimal offset)
        {
            return new T { Offset = offset };
        }

        private class GapData
        {
            public decimal PreviousOffset { get; set; }
            public int Index { get; set; }
            public int Count { get; set; }
        }
    }
}
