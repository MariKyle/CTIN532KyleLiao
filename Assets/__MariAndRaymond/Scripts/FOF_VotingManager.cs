using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_VotingManager : MonoBehaviour
{
    [SerializeField]
    private FOF_Character[] Characters;
    [SerializeField]
    private FOF_Champion Champion;


    private ArrayList Round2Characters;
    [SerializeField]
    private int m_currentCharacterID = 0;
    [SerializeField]
    private int m_currentRound = 1;
    public int CuurentRound
    { get { return m_currentRound; } }

    [SerializeField]
    private bool m_allRoundsEnd;

    public enum EStatus
    {
        votingTutorialA,
        votingTutorialB,
        votingTutorialC,
        proposing,
        voting,
    }
    private EStatus m_status;

    [SerializeField]
    private float _votingInterval = 5.0f;
    [SerializeField]
    private float _characterInterval = 2.0f;

    protected void Start()
    {
        Round2Characters = new ArrayList();

        Debug.Assert(Champion != null);
        Champion.IntroduceVotingA();
        //NextCharacterToPropose();
    }

    protected void Update()
    {
        if (m_status == EStatus.votingTutorialB)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Champion.IntroduceVotingB();
            }
        }
        if (m_status == EStatus.voting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerVote(true);
            }
        }
    }

	public void HoldUpRightHand()
	{
		if (m_status == EStatus.votingTutorialB) 
		{
			Champion.IntroduceVotingB();
		}
		if (m_status == EStatus.voting)
		{
			PlayerVote(true);
		}
	}

    // Called by FOF_Champion
    public void VotingTutorialWait()
    {
        m_status = EStatus.votingTutorialB;
    }
    public void VotingTutorialContinue()
    {
        m_status = EStatus.votingTutorialC;
    }
    public void VotingTutorialEnd()
    {
        NextCharacterToPropose();
    }
    public void ReStartRoundOne()
    {
        m_currentRound = 1;
        m_currentCharacterID = 0;
        Round2Characters.Clear();

        NextCharacterToPropose();
    }
    public void BeginRoundTwo()
    {
        m_currentRound = 2;
        m_currentCharacterID = 0;

        NextCharacterToPropose();
    }

    protected void NextCharacterToPropose()
    {
        if (m_allRoundsEnd)
        {
            Debug.LogError("All rounds End!");
            return;
        }
        Debug.Log("Proposal Starts!");

        switch (m_currentRound)
        {
            case 1:
                Characters[m_currentCharacterID].StandUpForProposal();
                break;

            case 2:
                if (Round2Characters.Count > 0)
                {
                    FOF_Character current = Round2Characters[m_currentCharacterID] as FOF_Character;
                    Debug.Assert(current != null);
                    current.StandUpForProposal();
                }
                break;
        }
        m_status = EStatus.proposing;
    }

    // Called by ProposalEnd in FOF_Character
    public void ProposalEnd()
    {
        StartCoroutine(ProposalEndCo());
    }
    private IEnumerator ProposalEndCo()
    {
        m_status = EStatus.voting;
        yield return new WaitForSeconds(_votingInterval);
        if (m_status == EStatus.voting)
        {
            PlayerVote(false);
        }
    }

    // Called by ProposalEndCo() and ?
    public void PlayerVote(bool accept)
    {
        m_status = EStatus.proposing;   // avoid repeated voting
        StartCoroutine(PlayerVoteCo(accept));
    }
    private IEnumerator PlayerVoteCo(bool accept)
    {
        switch (m_currentRound)
        {
            case 1:
                Characters[m_currentCharacterID].SetVotingResult(accept);
                if (accept)
                {
                    Round2Characters.Add(Characters[m_currentCharacterID]);
                    MetricManagerScript._metricsInstance.LogTime(
                        "Vote for " + Characters[m_currentCharacterID].MyName +
                        " in Round 1");
                }
                break;

            case 2:
                if (Round2Characters.Count > 0)
                {
                    FOF_Character current = Round2Characters[m_currentCharacterID] as FOF_Character;
                    Debug.Assert(current != null);
                    current.SetVotingResult(accept);

                    if (accept)
                    {
                        MetricManagerScript._metricsInstance.LogTime(
                            "Vote for " + current.MyName +
                            " in Round 2");
                    }
                }
                break;
        }

        yield return new WaitForSeconds(_characterInterval);

        m_currentCharacterID++;
        switch (m_currentRound)
        {
            case 1:
                if (m_currentCharacterID >= Characters.Length)
                {
                    //m_currentRound = 2;
                    //m_currentCharacterID = 0;
                    if (Round2Characters.Count < 2)
                    {
                        Champion.ReLeadRound1(Round2Characters.Count > 0);
                    }
                    else
                    {
                        Champion.LeadTheProposalRound2();
                    }

                }
                else
                {
                    NextCharacterToPropose();
                }
                break;

            case 2:
                if (m_currentCharacterID >= Round2Characters.Count)
                {
                    m_allRoundsEnd = true;
                    Champion.EndTheConference();
                }
                else
                {
                    NextCharacterToPropose();
                }
                break;
        }

        //NextCharacterToPropose();
    }


}