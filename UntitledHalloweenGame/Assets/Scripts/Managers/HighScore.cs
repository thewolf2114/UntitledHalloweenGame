using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighScore
{
    public static SortedDictionary<float, string> Scores { get; private set; }

    public static void Init()
    {
        Scores = new SortedDictionary<float, string>();
    }

    public static void AddPlayerScore(string name, float score)
    {
        if (!Scores.ContainsKey(score)) 
            Scores.Add(score, name);
    }
}
