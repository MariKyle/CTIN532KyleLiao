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

	private bool votingTutorialStarted;
	private bool wineGlassTutorialStarted;

    public enum EStatus
    {
        votingTutorialA,
        votingTutorialB,
        votingTutorialC,
		wineTutorialA,
		wineTutorialB,
        proposing,
        voting,
    }
    private EStatus m_status;
    public EStatus Status { get { return m_status; } }

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

    }

	public void HoldUpRightHand()
	{
		if (m_status == EStatus.votingTutorialB) 
		{
			if (votingTutorialStarted == false) 
			{
				Champion.IntroduceVotingB ();
				votingTutorialStarted = true;

				for (int i = 0; i < Characters.Length; ++i)
				{
					if (Characters[i] != Champion)
						Characters[i].Vote(Random.Range(0, 2) > 0);
				}
			}
			
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

        FOF_GameManager.Instance.BGMManager.ChangeStatus(FoF_BGMManager.EStatus.round01);
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

        FOF_GameManager.Instance.BGMManager.ChangeStatus(FoF_BGMManager.EStatus.round02);

        NextCharacterToPropose();
    }

	// can be called by GameManager after finishing the wine glass tutorial to continue
	public void NextCharacterToPropose()
    {
        if (m_allRoundsEnd)
        {
            Debug.LogError("All rounds End!");
            return;
        }
        Debug.Log("Proposal Starts!");

		if (FOF_GameManager.Instance.Status != FOF_GameManager.EStatus.normal)
			return;

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

        // Other characters ramdonly vote
        for (int i = 0; i < Characters.Length; ++i)
        {
			switch (m_currentRound)
			{
			    case 1:
				    if (i != m_currentCharacterID)
				    {
                        bool otherVote = Random.Range(0, 2) > 0;

                        // ** [GoldMaster]
                        if (otherVote && !Round2Characters.Contains(Characters[m_currentCharacterID]))
                            Round2Characters.Add(Characters[m_currentCharacterID]);

                        Characters[i].Vote(otherVote);
				    }
				    break;

			    case 2:
				    if (Characters [i] != Round2Characters [m_currentCharacterID] as FOF_Character) 
				    {
                        bool otherVote = Random.Range(0, 2) > 0;

                        Characters[i].Vote(otherVote);
				    }
				    break;
			}

        }

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
                    // ** [GoldMaster]
                    if (!Round2Characters.Contains(Characters[m_currentCharacterID]))
                    {
                        Round2Characters.Add(Characters[m_currentCharacterID]);
                    }
                        
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

                    // ** [GoldMaster] Players don't want the repeating round! **
                    //if (Round2Characters.Count < 2)
                    //{
                    //    Champion.ReLeadRound1(Round2Characters.Count > 0);
                    //}
                    //else
                    //{
                    //    Champion.LeadTheProposalRound2();
                    //}

                    Champion.LeadTheProposalRound2();
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