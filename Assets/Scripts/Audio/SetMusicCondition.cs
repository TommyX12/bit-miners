using UnityEngine;

public class SetMusicCondition : MyMono {
    
    public string Condition;
    public bool Value = true;
    
    void Start() {
        MusicManager.Current.SetCondition(this.Condition, this.Value);
    }
    
}
