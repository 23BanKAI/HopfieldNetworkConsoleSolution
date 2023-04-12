using System;

public class HopfieldTSP
{
    private int n; // number of cities
    private int[,] dist; // distance matrix
    private int[][] s; // array of state vectors
    private int[] bestTour; // best tour found so far
    private int bestCost; // cost of best tour

    public HopfieldTSP(int n, int[,] dist)
    {
        this.n = n;
        this.dist = dist;
        s = new int[n][];
        for (int i = 0; i < n; i++)
        {
            s[i] = new int[n];
        }
        bestTour = null;
        bestCost = int.MaxValue;
    }

    public void Train(int maxSteps)
    {
        Random rnd = new Random();

        // initialize state vectors randomly
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                s[i][j] = s[j][i] = rnd.Next(2);
            }
        }

        // update state vectors using Hopfield rule
        for (int k = 0; k < maxSteps; k++)
        {
            int i = rnd.Next(n);
            int j = rnd.Next(n);
            if (i != j)
            {
                int sum = 0;
                for (int l = 0; l < n; l++)
                {
                    if (l != i && l != j)
                    {
                        sum += dist[i, l] * (s[l][j] - s[l][i]);
                        sum += dist[j, l] * (s[i][l] - s[j][l]);
                    }
                }
                s[i][j] = s[j][i] = (sum < 0 ? 1 : 0);
            }
        }

        // find best tour using state vectors
        for (int i = 0; i < n; i++)
        {
            int[] tour = new int[n];
            int cost = 0;
            bool[] visited = new bool[n];
            visited[i] = true;
            tour[0] = i;
            for (int j = 1; j < n; j++)
            {
                int k = -1;
                int minDist = int.MaxValue;
                for (int l = 0; l < n; l++)
                {
                    if (!visited[l] && dist[tour[j - 1], l] < minDist)
                    {
                        k = l;
                        minDist = dist[tour[j - 1], l];
                    }
                }
                visited[k] = true;
                tour[j] = k;
                cost += dist[tour[j - 1], k];
            }
            cost += dist[tour[n - 1], i];
            if (cost < bestCost)
            {
                bestTour = tour;
                bestCost = cost;
            }
        }
    }

    public int[] GetBestTour()
    {
        return bestTour;
    }

    public int GetBestCost()
    {
        return bestCost;
    }
}

class Program
{
    static void Main(string[] args)
    {
        int n = 6;
        int[,] dist = {{0, 5, 2, 4, 9, 1},
{5, 0, 3, 9, 3, 7},
{2, 3, 0, 5, 6, 8},
{4, 9, 5, 0, 2, 7},
{9, 3, 6, 2, 0, 1},
{1, 7, 8, 7, 1, 0}};

        HopfieldTSP tsp = new HopfieldTSP(n, dist);
        tsp.Train(10000);

        int[] tour = tsp.GetBestTour();
        int cost = tsp.GetBestCost();

        Console.WriteLine("Best tour found:");
        for (int i = 0; i < n; i++)
        {
            Console.Write(tour[i] + " ");
        }
        Console.WriteLine("\nCost = " + cost);
    }
}