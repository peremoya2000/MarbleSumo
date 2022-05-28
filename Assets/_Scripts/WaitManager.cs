using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitManager{
    private static readonly Dictionary<float, WaitForSeconds> waits = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds Wait(float time) {
        if (waits.TryGetValue(time, out WaitForSeconds wait)) return wait;

        waits[time]= new WaitForSeconds(time);
        return waits[time];
    }

}
