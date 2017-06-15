using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewResourceManager
{

    private static int MaxCapacity;
    private static int AmtStored;

    public static Dictionary<string, int> Capacity = new Dictionary<string, int>();
    public static Dictionary<string, int> Stored = new Dictionary<string, int>();

    private static List<SiloComponent> silos = new List<SiloComponent>();

    public static void AddCapacity(string type, int amt)
    {
        Capacity[type] += amt;
    }

    public static void RemoveCapacity(string type, int amt)
    {
        Capacity[type] -= amt;

        if (Capacity[type] <= Stored[type])
        {
            Capacity[type] = Stored[type];
        }
    }

    public static int GetMaxCapacity(string type)
    {
        return Capacity[type];
    }

    public static int GetAmtStored(string type)
    {
        return Stored[type];
    }

    public static void Add(string type, int amt)
    {
        Stored[type] += amt;

        if (Stored[type] >= Capacity[type])
        {
            Stored[type] = Capacity[type];
        }
    }

    public static void Add(Dictionary<string, int> values) {
        foreach (string key in values.Keys) {
            if (Stored.ContainsKey(key)) {
                Stored[key] += values[key];
                if (Stored[key] > Capacity[key]) {
                    Stored[key] = Capacity[key];
                }
            }
        }
    }

    public static void Remove(string type, int amt)
    {
        if (amt > Stored[type])
        {
            Stored[type] = 0;
        }
        else
        {
            Stored[type] -= amt;
        }
    }

    public static void Remove(Dictionary<string, int> values) {
        foreach (string key in values.Keys) {
            if (Stored.ContainsKey(key)) {
                if (Stored[key] >= values[key])
                {
                    Stored[key] -= values[key];
                }
                else {
                    Stored[key] = 0;
                }
            }
        }
    }

    public static bool HasEnough(string type, int amt)
    {
        if (Stored[type] < amt)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static bool HasEnough(Dictionary<string, int> costs) {
        foreach (string key in costs.Keys) {
            if (Stored.ContainsKey(key) && Stored[key] >= costs[key])
            {

            }
            else {
                return false;
            }
        }
        return true;
    }

    public static void AddSilo(SiloComponent silo)
    {
        if (silos.Contains(silo))
        {
            return;
        }
        else
        {
            silos.Add(silo);
            if (!Capacity.ContainsKey(silo.type))
            {

                Stored.Add(silo.type, 0);
                Capacity.Add(silo.type, 0);
            }
                Refresh();
        }
    }

    public static void RemoveSilo(SiloComponent silo)
    {
        silos.Remove(silo);
        Refresh();
    }

    public static void Refresh()
    {
        foreach (string key in (new List<string>(Capacity.Keys))) {
            Capacity[key] = 0;
        }

        foreach (SiloComponent s in silos)
        {
            Capacity[s.type] += s.MaxCapacity;
        }

        foreach (string key in Capacity.Keys) {
            if (Capacity[key] < Stored[key]) {
                Stored[key] = Capacity[key];
            }
        }
    }

    public static GameObject GetNearestSilo(Vector2 position, string type)
    {
        GameObject winner = null;
        foreach (SiloComponent silo in silos)
        {
            if (winner == null && silo.type == type)
            {
                winner = silo.gameObject;
            } else if (winner != null && ((Vector2)(position - (Vector2)winner.transform.position)).magnitude >
                ((Vector2)(position - (Vector2)silo.gameObject.transform.position)).magnitude && silo.type == type)
            {
                winner = silo.gameObject;
            }

        }
        return winner;
    }
}