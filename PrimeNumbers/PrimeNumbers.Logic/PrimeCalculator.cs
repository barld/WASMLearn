namespace PrimeNumbers.Logic
{
    public class PrimeCalculator
    {
        public static IEnumerable<int> PrimesBelow(int n)
        {
            var primes = new List<int> { 2, 3 };
            for(int i = 5; i < n; i += 2)
            {
                var sqrtI = Math.Sqrt(i);
                var isPrime = true;
                for (int j = 3; j < sqrtI; j+= 2)
                {
                    if (i%j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    primes.Add(i);
                }
            }

            return primes;
        }
    }
}
