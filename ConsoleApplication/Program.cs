using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{

    static List<int> GetPrimesSync(int start, int end)
    {
        var primes = new List<int>();
        for (int i = start; i <= end; i++)
        {
            if (IsPrime(i))
            {
                primes.Add(i);
            }
        }
        return primes;
    }

    static async Task<List<int>> GetPrimesAsync(int start, int end)
    {
        var primes = new List<int>();
        var tasks = new List<Task>();

        for (int i = start; i <= end; i++)
        {
            int n = i;
            tasks.Add(Task.Run(() =>
            {
                if (IsPrime(n))
                {
                    lock (primes)
                    {
                        primes.Add(n);
                    }
                }
            }));
        }

        await Task.WhenAll(tasks);
        return primes;
    }

    static List<int> GetPrimesParallel(int start, int end)
    {
        var primes = new List<int>();
        Parallel.For(start, end + 1, i =>
        {
            if (IsPrime(i))
            {
                lock (primes)
                {
                    primes.Add(i);
                }
            }
        });
        return primes;
    }

    static bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }

        for (var i = 2; i * i <= number; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    static async Task Main(string[] args)
    {
        try
        {
            Console.Write("Enter the start of the range: ");
            int start = int.Parse(Console.ReadLine());
            Console.Write("Enter the end of the range: ");
            int end = int.Parse(Console.ReadLine());
            Console.WriteLine("-------");

            var stopwatch = Stopwatch.StartNew();
            var primesSync = GetPrimesSync(start, end);
            stopwatch.Stop();
            Console.WriteLine($"Synchronous execution time: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Number of primes found (sync): {primesSync.Count}");
            Console.WriteLine("-------");

            stopwatch.Restart();
            var primesAsync = GetPrimesAsync(start, end);
            stopwatch.Stop();
            Console.WriteLine($"Asynchronous execution time: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Number of primes found (async): {primesAsync.Result.Count}");
            Console.WriteLine("-------");

            stopwatch.Restart();
            var primesParallel = GetPrimesParallel(start, end);
            stopwatch.Stop();
            Console.WriteLine($"Asynchronous execution time: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Number of primes found (async): {primesParallel.Count}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }
}
