using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackableController : QTEManager
{
    /// <summary>
    /// Function that gets called after player successfully completes QTE
    /// Upon completion, switches Player and Enemy bodies
    /// </summary>
    public override void successfulHack()
    {
        if (listIndex == buttonStack.Count || InstantHack)
        {
            StopCoroutine(Listener());
            listening = false;
            PlayerController.instance.Switch();
        }
    }
}
