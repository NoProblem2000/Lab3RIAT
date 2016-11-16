using System.Linq;

namespace Lab3RIAT
{
    public class Input
    {
        public int K { get; set; }
        public decimal[] Sums { get; set; }
        public int[] Muls { get; set; }

        public Output DoOutPut()
        {
            var output = new Output
            {
                SumResult = Sums.Sum() * K,
                MulResult = Muls.Aggregate((item, current) => item * current),
                SortedInputs = Sums
                    .Concat(Muls.Select(x => (decimal)x))
                    .OrderBy(x => x)
                    .ToArray()
            };
            return output;
        }
    }
}