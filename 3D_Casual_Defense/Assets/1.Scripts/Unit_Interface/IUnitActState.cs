using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitActState
{
    public void Enter();
    public void DoAction();
    public void Exit();
}
