using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FOF_GameManager : MonoBehaviour
{
    public enum ELevel
    {
		menu,
        titleSequence,
        tableScene,
        endingCredit,
    }
    [SerializeField]
    private ELevel m_currentLevel;
    public ELevel CurrentLevel
    { get { return m_currentLevel; } }

    private static FOF_GameManager _instance;
    public static FOF_GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (FOF_GameManager)FindObjectOfType(typeof(FOF_GameManager));
            }

            return _instance;
        }
    }

    [SerializeField]
    private FOF_VotingManager _votingManager;
    public FOF_VotingManager VotingManager
    { 
		get
		{
			if (_votingManager == null) 
			{
				Debug.LogError ("[FOF_GameManager] No Voting Manager");
			}
			return _votingManager; 
		} 
	}

	void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        
		m_currentLevel = (ELevel)SceneManager.GetActiveScene ().buildIndex;
		switch (CurrentLevel) 
		{
		case ELevel.titleSequence:
			break;

		case ELevel.tableScene:
			GameObject votingManagerObj = GameObject.Find ("Voting Manager");
			Debug.Assert (votingManagerObj != null);
			_votingManager = votingManagerObj.GetComponent<FOF_VotingManager> ();
			Debug.Assert(_votingManager != null);
			break;

		case ELevel.endingCredit:
			break;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

	}

	public void LoadEndingCreditsScene()
	{
		Application.LoadLevel ("FeastOfFools_EndingCredits");
	}
    

}