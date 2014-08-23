namespace SixConcepts.BeachVine.Extensions
{
  using System.Collections.Generic;

  public static class EnumerableExtensions
  {
    public static void Shuffle<T>(this IList<T> list)
    {
      int n = list.Count;
      while (n > 1)
      {
        n--;
        int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }
  }
}
