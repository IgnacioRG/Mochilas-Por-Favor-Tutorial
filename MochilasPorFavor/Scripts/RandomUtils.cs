using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomUtils
{
    /// <summary>
    /// Agrega <c>n</c> enteros en el intervalo [0,<c>max</c>] escogidos al azar y sin repetición a <c>list</c> 
    /// </summary>

    public static void FillWithRandomInts(List<int> list, int n, int max)
    {
        if (n > max)
            throw new ArgumentException("n debe ser menor o igual a max");

        List<int> integers = Enumerable.Range(0, max).ToList();
        RandomUtils.FillWithRandomFrom(list, n, integers);
    }

    /// <summary>
    /// Agrega <c>n</c> elementos de <c>from</c> escogidos al azar y sin repetición a <c>list</c>
    /// </summary>
    public static List<T> FillWithRandomFrom<T>(List<T> list, int n, List<T> from)
    {
        if (n > from.Count)
            throw new ArgumentException("n debe ser menor o igual al número de elementos en from");

        List<T> fill = new List<T>();
        List<int> indexes = Enumerable.Range(0, from.Count).ToList();

        for (int j=0; j < n; j++)
        {
            int i = UnityEngine.Random.Range(0, indexes.Count);
            list.Add(from[indexes[i]]);
            fill.Add(from[indexes[i]]);
            indexes.RemoveAt(i);
        }
        return fill;
    }

    /// <summary>
    /// Agrega <c>n</c> enteros en el intervalo [0,<c>max</c>] y que no estén en <c>exclude</c> 
    /// escogidos al azar y sin repetición a <c>list</c>
    /// </summary>
    public static List<int> FillWithRandomExclude(List<int> list, int n, List<int> exclude, int max)
    {
        if (n > max)
            throw new ArgumentException("n debe ser menor o igual a max");

        var indexes = Enumerable.Range(0, max);
        List<int> diff = indexes.Except(exclude).ToList();
        return FillWithRandomFrom(list, n, diff);
    }

    /// <summary>
    /// Revuelve una lista con el shuffle de Fisher–Yates
    /// </summary>
    public static void Shuffle<T>(List<T> list)
    {
        int rand;

        for (int i=(list.Count-1); i>1; i--)
        {
            rand = UnityEngine.Random.Range(0, i);
            T temp = list[rand];
            list[rand] = list[i];
            list[i] = temp;
        }
    }
}