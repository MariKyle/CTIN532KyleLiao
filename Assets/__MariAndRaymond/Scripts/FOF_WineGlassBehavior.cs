using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_WineGlassBehavior : FOF_PickupBehavior
{
	[SerializeField]
	private AudioSource lionsSFXAudioSource01;
	[SerializeField]
	private AudioSource lionsSFXAudioSource02;

	protected virtual void Awake()
	{
		base.Awake ();

		Debug.Assert (lionsSFXAudioSource01 != null);
		lionsSFXAudioSource01.playOnAwake = false;
		lionsSFXAudioSource01.loop = true;
		lionsSFXAudioSource01.Stop ();
		Debug.Assert (lionsSFXAudioSource02 != null);
		lionsSFXAudioSource02.playOnAwake = false;
		lionsSFXAudioSource02.loop = true;
		lionsSFXAudioSource02.Stop ();
	}

    public override void BePickedUP()
    {
        base.BePickedUP();
        MetricManagerScript._metricsInstance.LogTime("The Wine Glass being picked up");

		lionsSFXAudioSource01.Play ();
		lionsSFXAudioSource02.Play ();
    }

    public override void BeDropped()
    {
        base.BeDropped();
        MetricManagerScript._metricsInstance.LogTime("The Wine Glass being put down");

		lionsSFXAudioSource01.Stop ();
		lionsSFXAudioSource02.Stop ();
    }
}
