using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitModeState
{
    public void EnterMode();
    public void DoModeAction();
    public void ExitMode();
}
