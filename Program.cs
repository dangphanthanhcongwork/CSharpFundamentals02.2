using System.Diagnostics;

namespace CSharpFundamentals02;

class Program
{
    public static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2 || number == 3) return true;

        var boundary = (int)Math.Floor(Math.Sqrt(number));
        for (int i = 2; i <= boundary; i++)
            if (number % i == 0)
                return false;

        return true;
    }
    public static List<int> FindPrimeInRangeSync(int start, int end)
    {
        List<int> primes = [];
        foreach (var i in Enumerable.Range(start, end - 1))
        {
            if (IsPrime(i))
            {
                primes.Add(i);
            }
        }
        return primes;
    }
    public static async Task<List<int>> FindPrimeInRangeAsync(int start, int end)
    {
        List<int> primes = [];
        List<Task> tasks = new(end - start + 1);
        foreach (var i in Enumerable.Range(start, end - 1))
        {
            tasks.Add(Task.Run(() =>
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }));
        }
        await Task.WhenAll(tasks);
        return primes;
    }
    static void Main(string[] args)
    {
        int start = 2;
        int end = 20000;

        var sw = new Stopwatch();
        sw.Start();
        var primesSync = FindPrimeInRangeSync(start, end);
        sw.Stop();
        foreach (var prime in primesSync)
        {
            Console.Write(prime + " ");
        }
        Console.WriteLine();
        Console.WriteLine("Using Synchronous");
        Console.WriteLine("Total prime numbers: " + primesSync.Count);
        Console.WriteLine("Process time: " + sw.ElapsedMilliseconds + " milliseconds");
        Console.WriteLine("-------");

        sw = new Stopwatch();
        sw.Start();
        var primesAsync = FindPrimeInRangeAsync(start, end);
        sw.Stop();
        foreach (var prime in primesAsync.Result)
        {
            Console.Write(prime + " ");
        }
        Console.WriteLine();
        Console.WriteLine("Using Asynchronous");
        Console.WriteLine("Total prime numbers: " + primesAsync.Result.Count);
        Console.WriteLine("Process time: " + sw.ElapsedMilliseconds + " milliseconds");
    }
}
