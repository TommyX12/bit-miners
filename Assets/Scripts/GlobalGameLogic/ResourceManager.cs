using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MyMono {

    private static int MaxCapacity;
    private static int AmtStored;

    private static List<SiloComponent> silos = new List<SiloComponent>();

    public static void AddCapacity(int amt) {
        MaxCapacity += amt;
    }

    public static void RemoveCapacity(int amt) {
        MaxCapacity -= amt;

        if (MaxCapacity <= AmtStored) {
            AmtStored = MaxCapacity;
        }
    }

    public static int GetMaxCapacity() {
        return MaxCapacity;
    }

    public static int GetAmtStored() {
        return AmtStored;
    }

    public static void Add(int amt) {
        AmtStored += amt;
        if (AmtStored >= MaxCapacity) {
            AmtStored = MaxCapacity;
        }
    }

    public static void Remove(int amt) {
        if (amt > AmtStored)
        {
            AmtStored = 0;
        }
        else {
            AmtStored -= amt;
        }
    }

    public static bool HasEnough(int amt) {
        if (AmtStored < amt)
        {
            return false;
        }
        else {
            return true;
        }
    }

    public static void AddSilo(SiloComponent silo) {
        if (silos.Contains(silo))
        {
            return;
        }
        else {
            silos.Add(silo);
            Refresh();
        }
    }

    public static void RemoveSilo(SiloComponent silo) {
        silos.Remove(silo);
        Refresh();
    }

    public static void Refresh() {
        MaxCapacity = 0;

        foreach (SiloComponent s in silos) {
            MaxCapacity += s.MaxCapacity;
        }

        if (AmtStored >= MaxCapacity) {
            AmtStored = MaxCapacity;
        }
    }

    public static GameObject GetNearestSilo(Vector2 position) {
        GameObject winner = null;
        foreach (SiloComponent silo in silos) {
            if (winner == null) {
                winner = silo.gameObject;
            }
            if (((Vector2)(position - (Vector2)winner.transform.position)).magnitude >
                ((Vector2)(position - (Vector2)silo.gameObject.transform.position)).magnitude)
            {
                winner = silo.gameObject;
            }

        }
        return winner;
    }
}