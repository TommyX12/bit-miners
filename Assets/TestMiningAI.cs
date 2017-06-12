using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMiningAI : MyMono {

    public MiningComponent miner;
    public MiningLogicComponent miningControl;
    public MoveComponent mover;
    public GameObject miningTarget = null;

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        
        if (miningTarget == null) {
            miningTarget = miningControl.GetNearestResource("iron");
            Debug.Log("FindTarget");
        }

        if (miningTarget != null)
        {
            if (miner.stored < miner.MaxCapacity)
            {
                mover.SetVectorTarget(miningTarget.transform.position);
                miner.startMining();
            }
            else
            {
                miningControl.GoToNearestSilo("iron");
                miner.TurnIn();
            }
        }
    }
}
