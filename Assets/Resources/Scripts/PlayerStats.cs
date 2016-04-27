using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats
{

    private static Dictionary<string, PlayerStatStruct> stats;

    public static PlayerStatStruct getStats(string pid)
    {
        if (stats == null) stats = new Dictionary<string, PlayerStatStruct>();
        if (!stats.ContainsKey(pid)) stats.Add(pid, new PlayerStatStruct());

        return stats[pid];
    }

    public static void clearStats(string pid)
    {
        if (stats.ContainsKey(pid)) stats[pid] = new PlayerStatStruct();
    }

}

public class PlayerStatStruct
{
    public int kills = 0;
    public int deaths = 0;
    public int coinsLost = 0;
    public int coinsGained = 0;
    public int attacksDone = 0;
    public int itemsUsed = 0;
    public int damageDone = 0;
    public int damageReceived = 0;

    public PlayerStatStruct() { }

}