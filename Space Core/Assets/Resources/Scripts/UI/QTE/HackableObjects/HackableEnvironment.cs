using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackableEnvironment : QTEManager
{
    public HackableEnvFunctionality envFunc;

    /// <summary>
    /// Function that gets called after player successfully completes QTE
    /// Upon completion, runs attached hackable environment functionality
    /// </summary>
    public override void successfulHack()
    {
        if (listIndex == buttonStack.Count || InstantHack)
        {
            StopCoroutine(Listener());
            listening = false;
            envFunc.Run();
        }
    }
}
