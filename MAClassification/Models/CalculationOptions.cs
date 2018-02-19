namespace MAClassification.Models
{
    public class CalculationOptions
    {
        public EuristicTypes EuristicType { get; set; }
        public PheromonesTypes PheromonesType { get; set; }
        public DivideTypes DivideType { get; set; }
        public bool IsPruned { get; set; }

        public int MaxAntsGenerationsNumber { get; set; }
        public int MaxNumberForConvergence { get; set; }
        public int MaxUncoveredCases { get; set; }
        public int MinCasesPerRule { get; set; }

        public double CrossValidationCoefficient { get; set; }
        public string DataPath { get; set; }

    }
}
