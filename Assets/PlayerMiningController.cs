using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiningController : MyMono {
    public MiningComponent miner;

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        if (Input.GetKey(KeyCode.E)) {
            miner.startMining();
        }
    }


}
