using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MyMono {

    private static int MaxCapacity;
    private static int AmtStored;

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

    public static int GEtAmtStored() {
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

}
