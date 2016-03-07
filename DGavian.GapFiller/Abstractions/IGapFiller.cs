using System;
using System.Collections.Generic;

namespace DGavian.GapFiller.Abstractions
{
    public interface IGapFiller<T>
     where T : IOffset, new()
    {
        void FillGaps(List<T> items);
        void FillGaps(List<T> items, Func<decimal, T> getDefaultRecord);
    }
}
