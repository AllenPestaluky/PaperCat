using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateRun : MoveStateBase
{
    public new void Start()
    {
        base.Start();
    }

    public override CatMovement.EMoveState GetStateEnum()
    { 
        return CatMovement.EMoveState.Run;
    }

}
