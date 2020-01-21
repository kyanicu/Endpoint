using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyTrigger : AbsTrigger
{
    private void OnDestroy()
    {
        handleResult();
    }
}
