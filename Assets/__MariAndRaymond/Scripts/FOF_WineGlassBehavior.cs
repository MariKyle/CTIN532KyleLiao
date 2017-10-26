using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_WineGlassBehavior : FOF_PickupBehavior
{
    public override void BePickedUP()
    {
        base.BePickedUP();
        MetricManagerScript._metricsInstance.LogTime("The Wine Glass being picked up");
    }

    public override void BeDropped()
    {
        base.BeDropped();
        MetricManagerScript._metricsInstance.LogTime("The Wine Glass being put down");
    }
}
