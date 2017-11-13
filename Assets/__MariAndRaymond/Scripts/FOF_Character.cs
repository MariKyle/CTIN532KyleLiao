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

    protected void Awake()
    {
        Debug.Assert(_traumaVersion != null);

        _audioSrc = GetComponent<AudioSource>();
        _audioSrc.playOnAwake = false;
        _audioSrc.loop = false;

        _animator = GetComponent<Animator>();
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
        }
        else
        {
            _audioSrc.clip = _proposalRejectedSFX;
            _traumaVersion.SetActive(true);
            _animator.SetTrigger("Proposal Rejected");

			FOF_GameManager.Instance.OnTraumaActivated ();
        }

        _audioSrc.Play();
    }
}