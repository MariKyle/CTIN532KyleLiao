using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_GameManager : MonoBehaviour
{
    public enum ELevel
    {
        titleSequence,
        tableScene,
        credit,
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
    { get { return _votingManager; } }

	void Awake ()
    {
        Screen.SetResolution(1920, 1080, true);
        
        if (CurrentLevel == ELevel.tableScene)
            Debug.Assert(_votingManager != null);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

	}
    

}