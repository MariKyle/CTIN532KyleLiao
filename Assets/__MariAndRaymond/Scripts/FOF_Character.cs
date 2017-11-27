using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class FOF_Character : MonoBehaviour
{
    public enum EName
    {
        Champion,
        Red,
        Bowtie,
        Candy,
        Curls,
    }
    [SerializeField]
    private EName m_myName;
    public EName MyName
    { get { return m_myName; } }

    [SerializeField]
    protected GameObject _traumaVersion;

    public float proposalRound1Time = 0.0f;
    public float proposalRound2Time = 0.0f;

    protected AudioSource _audioSrc;
    [SerializeField]
    protected AudioClip _proposalRound1_SFX;
    [SerializeField]
    protected AudioClip _proposalRound2_SFX;
    [SerializeField]
    protected AudioClip _proposalAcceptedSFX;
    [SerializeField]
    protected AudioClip _proposalRejectedSFX;
	[SerializeField]
	protected AudioClip _proposalWaitingSFX;
    [SerializeField]
    protected AudioClip _proposalAgreeSFX;
    [SerializeField]
    protected AudioClip _proposalDisagreeSFX;

    protected Animator _animator;


    // Reticle for the Manifestation Mechanic
    public enum EMentalState
    {
        normal,
        trauma,
    }
    private EMentalState m_mentalState;
    public enum ETraumaSeenState
    {
        seen,
        unseen,
    }
    private ETraumaSeenState m_traumaSeenState;
    private float m_traumaSeenFraction;
    public float TraumaSeenSFXMultiplier = 1.0f;
    public float TraumaSeenStaringMultiplier = 2.0f;
    public float traumaSeenDelay = 1.0f;

    [SerializeField]
    private AmISeen _amISceen;
    [SerializeField]
    private AudioSource _traumaAudioSrc;
    private float _traumaAudioSrcMaxVolume;
    [SerializeField]
    private Transform _traumaStaringTarget;

    //public AnimationCurve m_traumaAudioMultiplier;
    //public AnimationCurve m_traumaLookingAtMultiplier;

    protected void Awake()
    {
        Debug.Assert(_traumaVersion != null);

        _audioSrc = GetComponent<AudioSource>();
        _audioSrc.playOnAwake = false;
        _audioSrc.loop = false;

        _animator = GetComponent<Animator>();

        if (_amISceen == null)
        {
            _amISceen = GetComponentInChildren<AmISeen>();
        }
        Debug.Assert(_amISceen != null);
        Debug.Assert(_traumaAudioSrc != null);
        _traumaAudioSrcMaxVolume = _traumaAudioSrc.volume;
        _traumaStaringTarget = Camera.main.transform;
        Debug.Assert(_traumaStaringTarget != null);
    }

    protected void Update()
    {
        if (m_mentalState == EMentalState.trauma)
        {
            _traumaAudioSrc.volume = Mathf.Clamp( m_traumaSeenFraction * TraumaSeenSFXMultiplier, 
                0.0f, _traumaAudioSrcMaxVolume);

            m_traumaSeenState = _amISceen.BeingLookedAt ? 
                ETraumaSeenState.seen : ETraumaSeenState.unseen;

            //----------------------------------------------
            if (m_traumaSeenState == ETraumaSeenState.seen)
            {
                m_traumaSeenFraction = m_traumaSeenFraction + Time.deltaTime;
            }
            else
            {
                m_traumaSeenFraction = m_traumaSeenFraction - Time.deltaTime;
            }
            m_traumaSeenFraction = Mathf.Clamp01(m_traumaSeenFraction);
        }
        

        // For Debugging
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    m_mentalState = (m_mentalState == EMentalState.normal) ? 
        //        EMentalState.trauma : EMentalState.normal;
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    m_traumaSeenState = (m_traumaSeenState == ETraumaSeenState.unseen) ? 
        //        ETraumaSeenState.seen : ETraumaSeenState.unseen;
        //}
    }

    public virtual void StandUpForProposal()
    {
        _animator.SetTrigger("Stand Up");
    }

    // Called at the beginning of the animation StandArguing
    public virtual void MakeProposal()
    {
        _audioSrc.Stop();

        switch (FOF_GameManager.Instance.VotingManager.CuurentRound)
        {
            case 1:
                _audioSrc.clip = _proposalRound1_SFX;
                break;
            case 2:
                _audioSrc.clip = _proposalRound2_SFX;
                break;

        }

        _audioSrc.Play();
        StartCoroutine(MakeProposalCo());
    }

    protected virtual IEnumerator MakeProposalCo()
    {
        float timeToWait = 0.0f;

        switch (FOF_GameManager.Instance.VotingManager.CuurentRound)
        {
            case 1:
                timeToWait = proposalRound1Time;
                break;
            case 2:
                timeToWait = proposalRound2Time;
                break;

        }

        yield return new WaitForSeconds(timeToWait);

        _animator.SetTrigger("Sit Down");
    }

    // Called at the end of the animation SitDown
    public void ProposalEnd()
    {
        FOF_GameManager.Instance.VotingManager.ProposalEnd();
		_audioSrc.Stop ();
		_audioSrc.clip = _proposalWaitingSFX;
		_audioSrc.loop = true;
		_audioSrc.Play ();
    }

    public void Vote(bool agree)
    {
        StartCoroutine(VoteCo(agree));
    }
    protected IEnumerator VoteCo(bool agree)
    {
        float waitTime = Random.Range(0.1f, 1.5f);
        yield return new WaitForSeconds(waitTime);

        if (agree)
        {
            _animator.SetTrigger("Proposal Raise Hand");

            _audioSrc.Stop();
            _audioSrc.clip = _proposalAgreeSFX;
			_audioSrc.loop = false;
            _audioSrc.Play();
        }
        else
        {
            _audioSrc.Stop();
            _audioSrc.clip = _proposalDisagreeSFX;
			_audioSrc.loop = false;
            _audioSrc.Play();
        }
    }

    // Called by PlayerVote in VotingManager
    public virtual void SetVotingResult(bool accepted)
    {
        _audioSrc.Stop();
		_audioSrc.loop = false;

        if (accepted)
        {
            _audioSrc.clip = _proposalAcceptedSFX;
            _traumaVersion.SetActive(false);
            _animator.SetTrigger("Proposal Accepted");

            m_mentalState = EMentalState.normal;
        }
        else
        {
            _audioSrc.clip = _proposalRejectedSFX;
            _traumaVersion.SetActive(true);
            _animator.SetTrigger("Proposal Rejected");

			FOF_GameManager.Instance.OnTraumaActivated ();

            m_mentalState = EMentalState.trauma;
        }

        _audioSrc.Play();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _animator.SetLookAtPosition(_traumaStaringTarget.position);
        float lookAtWeight = (m_mentalState == EMentalState.trauma) ? Mathf.Clamp01( m_traumaSeenFraction * TraumaSeenStaringMultiplier) : 0.0f;
        _animator.SetLookAtWeight(lookAtWeight, 0.1f, 1.0f, 1.0f, 0.5f);
    }

}