using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_Champion : FOF_Character
{
    [SerializeField]
    private AudioClip tuto_WeWillStart;
    public float Tuto_WeWillStartLength;
    [SerializeField]
    private AudioClip tuto_ThereWill;
    public float Tuto_ThereWillLength;
    [SerializeField]
    private AudioClip tuto_ForTheFirst;
    public float Tuto_ForTheFirstLength;
    [SerializeField]
    private AudioClip tuto_LetsPractice;
    public float Tuto_LetsPracticeLength;
    [SerializeField]
    private AudioClip tuto_GoodNow;
    public float Tuto_GoodNowLength;
    [SerializeField]
    private AudioClip tuto_LetsBegin;
    public float Tuto_LetsBeginLength;

    [SerializeField]
    private AudioClip round1_NoVote;
    public float Round1_NoVoteLength;
    [SerializeField]
    private AudioClip round1_WeNeed;
    public float Round1_WeNeedLength;
    [SerializeField]
    private AudioClip round1_ThatsTheEnd;
    public float Round1_ThatsTheEndLength;

    [SerializeField]
    private AudioClip round2_NowWeBeginRound2;
    public float Round2_NowWeBeginRound2Length;

    [SerializeField]
    private AudioClip end_ThatsTheEnd;
    public float End_ThatsTheEndsLength;

    public void IntroduceVotingA()
    {
        StartCoroutine(IntroduceVotingACo());
    }
    private IEnumerator IntroduceVotingACo()
    {
        yield return new WaitForSeconds(5.0f);

        _animator.SetTrigger("Seat Talking");
        _audioSrc.Stop();
        _audioSrc.clip = tuto_WeWillStart;
        _audioSrc.Play();
        yield return new WaitForSeconds(Tuto_WeWillStartLength);
        _audioSrc.Stop();
        _audioSrc.clip = tuto_ThereWill;
        _audioSrc.Play();
        yield return new WaitForSeconds(Tuto_ThereWillLength);
        _audioSrc.Stop();
        _audioSrc.clip = tuto_ForTheFirst;
        _audioSrc.Play();
        yield return new WaitForSeconds(Tuto_ForTheFirstLength);
        _audioSrc.Stop();
        _audioSrc.clip = tuto_LetsPractice;
        _audioSrc.Play();
        yield return new WaitForSeconds(Tuto_LetsPracticeLength);
        _animator.SetTrigger("Seat Idle");

        FOF_GameManager.Instance.VotingManager.VotingTutorialWait();
    }
    public void IntroduceVotingB()
    {
        StartCoroutine(IntroduceVotingBCo());
    }
    private IEnumerator IntroduceVotingBCo()
    {
        _animator.SetTrigger("Seat Talking");
        _audioSrc.Stop();
        _audioSrc.clip = tuto_GoodNow;
        _audioSrc.Play();
        yield return new WaitForSeconds(Tuto_GoodNowLength);

        FOF_GameManager.Instance.VotingManager.VotingTutorialContinue();

        _audioSrc.Stop();
        _audioSrc.clip = tuto_LetsBegin;
        _audioSrc.Play();
        yield return new WaitForSeconds(Tuto_LetsBeginLength);
        _animator.SetTrigger("Seat Idle");

        FOF_GameManager.Instance.VotingManager.VotingTutorialEnd();
    }

    public void ReLeadRound1(bool hasAnyVote)
    {
        StartCoroutine(ReLeadRound1Co(hasAnyVote));
    }
    private IEnumerator ReLeadRound1Co(bool hasAnyVote)
    {
        _animator.SetTrigger("Seat Talking");
        _audioSrc.Stop();
        if (hasAnyVote)
        {
            _audioSrc.clip = round1_WeNeed;
            _audioSrc.Play();
            yield return new WaitForSeconds(Round1_WeNeedLength);
        }
        else
        {
            _audioSrc.clip = round1_NoVote;
            _audioSrc.Play();
            yield return new WaitForSeconds(Round1_NoVoteLength);
        }
        _animator.SetTrigger("Seat Idle");

        FOF_GameManager.Instance.VotingManager.ReStartRoundOne();
    }

    public void LeadTheProposalRound2()
    {
        StartCoroutine(LeadTheProposalRound2Co());
    }
    private IEnumerator LeadTheProposalRound2Co()
    {
        _animator.SetTrigger("Seat Talking");
        _audioSrc.Stop();
        _audioSrc.clip = round1_ThatsTheEnd;
        _audioSrc.Play();
        yield return new WaitForSeconds(Round1_ThatsTheEndLength);
        _audioSrc.Stop();
        _audioSrc.clip = round2_NowWeBeginRound2;
        _audioSrc.Play();
        yield return new WaitForSeconds(Round2_NowWeBeginRound2Length);
        _animator.SetTrigger("Seat Idle");

        FOF_GameManager.Instance.VotingManager.BeginRoundTwo();
    }
    public void EndTheConference()
    {
        _animator.SetTrigger("Seat Talking");
        _audioSrc.Stop();
        _audioSrc.clip = end_ThatsTheEnd;
        _audioSrc.Play();
        _animator.SetTrigger("Seat Idle");
    }

}
