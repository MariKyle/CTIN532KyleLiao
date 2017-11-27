using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_WineGlassBehavior : FOF_PickupBehavior
{
	[SerializeField]
	private ParticleSystem _traumaActivatedParticle;
	[SerializeField]
	private AudioClip _traumaActivatedSFX;

	//[SerializeField]
	//private AudioSource lionsSFXAudioSource01;
	//[SerializeField]
	//private AudioSource lionsSFXAudioSource02;

	[SerializeField]
	private Transform _originalPos;
    // *TEMPERARY USE
    [SerializeField]
    private Transform _wearPos;
    private bool _wearing;

	private Animator _anim;

	private bool _isInteracting;

	// called by gameManager
	[SerializeField]
	private Light _spotLight;
	public void OnWineGlassTutorial()
	{
		_spotLight.enabled = true;
		OnTraumaActivated ();
	}

	public void OnTraumaActivated()
	{
		_traumaActivatedParticle.Play ();
		_audioSrc.clip = _traumaActivatedSFX;
		_audioSrc.Play ();
	}

	protected virtual void Awake()
	{
		base.Awake ();

		Debug.Assert (_traumaActivatedParticle != null);
		_traumaActivatedParticle.Stop ();
		Debug.Assert (_traumaActivatedSFX != null);

		Debug.Assert (_spotLight != null);
		_spotLight.enabled = false;

		//Debug.Assert (lionsSFXAudioSource01 != null);
		//lionsSFXAudioSource01.playOnAwake = false;
		//lionsSFXAudioSource01.loop = true;
		//lionsSFXAudioSource01.Stop ();
		//Debug.Assert (lionsSFXAudioSource02 != null);
		//lionsSFXAudioSource02.playOnAwake = false;
		//lionsSFXAudioSource02.loop = true;
		//lionsSFXAudioSource02.Stop ();

		Debug.Assert (_originalPos != null);

		_anim = GetComponent<Animator> ();
		Debug.Assert (_anim != null);
	}

	protected override void Update()
	{
		base.Update ();
		if (_isInteracting) {
			MetricManagerScript._metricsInstance.WineAccumulatedInteractionTime += Time.deltaTime;
		}

        // *TEMPERARY USE
        if (Input.GetKeyDown(KeyCode.W) && _wearPos != null)
        {
            transform.position = _wearPos.position;
            //transform.rotation = _wearPos.rotation;
            GetComponent<Rigidbody>().useGravity = false;
            _wearing = true;

            if (FOF_GameManager.Instance.Status == FOF_GameManager.EStatus.wineTutorial)
            {
                FOF_GameManager.Instance.EndWineGlassTutorial();
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && _wearPos != null)
        {
            transform.position = _originalPos.position;
            //transform.rotation = _originalPos.rotation;
            GetComponent<Rigidbody>().useGravity = true;
            _wearing = false;
        }

        if (_wearing)
        {
            transform.position = _wearPos.position;
        }
	}

    public override void BePickedUP()
    {
        base.BePickedUP();
        //MetricManagerScript._metricsInstance.LogTime("The Wine Glass being picked up");

		//lionsSFXAudioSource01.Play ();
		//lionsSFXAudioSource02.Play ();

		//_anim.SetTrigger ("Perspective On");
		_isInteracting = true;
    }

    public override void BeDropped()
    {
        base.BeDropped();
        //MetricManagerScript._metricsInstance.LogTime("The Wine Glass being put down");

		//lionsSFXAudioSource01.Stop ();
		//lionsSFXAudioSource02.Stop ();

		//_anim.SetTrigger ("Perspective Off");
		_isInteracting = false;
    }

	protected override void OnCollisionEnter(Collision other)
	{
		base.OnCollisionEnter (other);
		if (other.gameObject.name == "Floor") 
		{
			transform.position = _originalPos.position;
		}
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter (other);
		if (other.gameObject.name == "WineGlass_Reset_Trigger")
		{
			transform.position = _originalPos.position;
		}

		if (other.gameObject.name == "WineGlass Perspective Trigger") 
		{
			_anim.SetTrigger ("Perspective On");


			if (FOF_GameManager.Instance.Status == FOF_GameManager.EStatus.wineTutorial)
			{
				FOF_GameManager.Instance.EndWineGlassTutorial ();
			}
		}
	}

	protected override void OnTriggerStay(Collider other)
	{
		base.OnTriggerStay (other);
		if (other.gameObject.name == "WineGlass_Reset_Trigger")
		{
			transform.position = _originalPos.position;
		}
	}


	protected override void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "WineGlass Perspective Trigger") 
		{
			_anim.SetTrigger ("Perspective Off");
		}
	}
}
