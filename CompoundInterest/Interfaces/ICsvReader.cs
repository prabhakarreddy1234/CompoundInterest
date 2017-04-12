using System.Collections.Generic;

namespace CompoundInterest.Interfaces
{
    public interface ICsvReader
    {
        IList<Amount> Read();
    }
}