using System.Collections.Generic;
using UnityEngine;

public static class Stats
{

    //Stats 
    public static string COUNT_SCORE = "Score", COUNT_ENNEMIES = "CountEnnemies", COUNT_BOSS = "CountBosses",
                         COUNT_CLICK = "CountClick", COUNT_BULLETS = "CountBullets", COUNT_JUMP = "CountJump",
                         COUNT_DOUBLEJUMP = "CountDoubleJump", COUNT_TIME = "TimePlayed", COUNT_DISTANCE = "Distance";

    static Dictionary<string, float> counters = new Dictionary<string, float>();

    static float time0 = Time.time;

    public static void AddStatCounter(string counterName, float value = 1)
    {
        if (!counters.ContainsKey(counterName))
        {
            counters.Add(counterName, value);
        }
        else
        {
            counters[counterName] += value;
        }
    }

    public static string GetStatCounter(string counterName)
    {

        if (counterName == COUNT_TIME)
        {
            return getTimedCounter();
        }
        else
        {
            if (counters.ContainsKey(counterName))
            {
                return Mathf.Round(counters[counterName]).ToString();
            }
            else
            {
                Debug.Log(counterName);
            }
        }

        return "";
    }

    public static float GetStatCounterFloat(string counterName)
    {
        return Mathf.Round(counters[counterName]);
    }

    private static string getTimedCounter()
    {
        float time = Time.time - time0;
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
