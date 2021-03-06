using System;
using System.Linq;
using System.Collections.Generic;

public class IDBasedContainer<T>
    where T:IIDBasedElememt
{
    private Dictionary<int, T> data = new Dictionary<int, T>();
    private Stack<int> unusedID = new Stack<int>();
    public int MaxID {
        get; private set;
    }
    
    public int Count {
        get {
            return this.data.Count;
        }
    }
    
    public IDBasedContainer() {
        this.MaxID    = -1;
    }
    
    public int Add(T element) {
        int id = 0;
        if (this.unusedID.Count == 0) {
            id = this.MaxID + 1;
        }
        else {
            id = this.unusedID.Pop();
        }
        this.AddWithID(id, element);
        return id;
    }
    
    public T Get(int id) {
        return this.data[id];
    }
    
    public bool Contains(int id) {
        return this.data.ContainsKey(id);
    }
    
    public void Remove(T element) {
        this.Remove(element.ID);
    }
    
    public void Remove(int id) {
        this.data.Remove(id);
        this.unusedID.Push(id);
    }
    
    public IEnumerable<int> IDs() {
        foreach (int id in this.data.Keys) {
            yield return id;
        }
    }
    
    public void Clear() {
        this.MaxID = -1;
        this.unusedID.Clear();
        this.data.Clear();
    }
    
    public int LowestID() {
        int result = -1;
        foreach (int id in this.IDs()) {
            if (result < 0 || id < result) {
                result = id;
            }
        }
        return result;
    }
    
    public void AddWithID(int id, T element) {
        if (id < 0) {
            this.Add(element);
        }
        else {
            this.data[id] = element;
            element.ID = id;
            this.MaxID = Math.Max(id, this.MaxID);
        }
    }
    
    public void BuildUnusedID() {
        this.unusedID.Clear();
        for (int i = 0; i <= this.MaxID; ++i) {
            if (!this.data.ContainsKey(i)) {
                this.unusedID.Push(i);
            }
        }
    }
}
