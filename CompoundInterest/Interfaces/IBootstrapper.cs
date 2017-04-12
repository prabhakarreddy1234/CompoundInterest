using CompoundInterest.Entities;

namespace CompoundInterest.Interfaces
{
    public interface IBootstrapper
    {
        Output CalculateInterest(string[] args);
    }
}