using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FOF_GameManager : MonoBehaviour
{
    public enum ELevel
    {
        titleSequence,
        tableScene,
        endingCredit,
    }
    [SerializeField]
    private ELevel m_currentLevel;
    public ELevel CurrentLevel
    { get { return m_currentLevel; } }

	public enum EStatus
	{
		normal,
		wineTutorialA,
        wineTutorialB,
	}
	private EStatus m_status;
	public EStatus Status
	{ get { return m_status; } }

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
    [SerializeField]
    private FoF_BGMManager _bgmManager;
    public FoF_BGMManager BGMManager
    {
        get
        {
            if (_bgmManager == null)
            {
                Debug.LogError("[FOF_BGMManager] NO BGM Manager");
            }
            return _bgmManager;
        }
    }

    private FOF_WineGlassBehavior _wineGlass;
	private bool _wineGlassTutorialFinished;
	private AudioSource _audioSrc;

    [SerializeField]
    private AudioClip _wineGlassTutorialSFX01Initial;
    public float wineGlassTutorialSFX01Initial_Length;
    [SerializeField]
	private AudioClip _wineGlassTutorialSFX01;

	[SerializeField]
	private AudioClip _wineGlassTutorialSFX02;

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
			Debug.Assert (_votingManager != null);
            GameObject bgmManagerObj = GameObject.Find("BGM Manager");
            Debug.Assert(bgmManagerObj != null);
            _bgmManager = bgmManagerObj.GetComponent<FoF_BGMManager>();
            Debug.Assert(_bgmManager != null);
			GameObject wineGlassObj = GameObject.FindWithTag ("Wine Glass");
			Debug.Assert (wineGlassObj != null);
			_wineGlass = wineGlassObj.GetComponent<FOF_WineGlassBehavior> ();
			Debug.Assert (_wineGlass != null);

			_audioSrc = GetComponent<AudioSource> ();
			Debug.Assert (_audioSrc != null);
			_audioSrc.playOnAwake = false;
			_audioSrc.loop = false;
			_audioSrc.Stop ();
			Debug.Assert (_wineGlassTutorialSFX01 != null);
			Debug.Assert (_wineGlassTutorialSFX02 != null);
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

	public void OnTraumaActivated()
	{
		if (!_wineGlassTutorialFinished) 
		{
			StartWineGlassTutorial ();
		}
		else
		{
			_wineGlass.OnTraumaActivated ();
		}
	}

	private void StartWineGlassTutorial()
	{
		m_status = EStatus.wineTutorialA;
		StartCoroutine (StartWineGlassTutorialCo ());

	}

	private IEnumerator StartWineGlassTutorialCo()
	{
		yield return new WaitForSeconds (3.0f);

		_audioSrc.clip = _wineGlassTutorialSFX01Initial;
		_audioSrc.Play ();

		yield return new WaitForSeconds (wineGlassTutorialSFX01Initial_Length);

        m_status = EStatus.wineTutorialB;

        if (!_wineGlassTutorialFinished)
        {
            _audioSrc.Stop();
            _audioSrc.clip = _wineGlassTutorialSFX01;
            _audioSrc.loop = true;
            _audioSrc.Play();

            _wineGlass.OnWineGlassTutorial();
        }

	}

	public void EndWineGlassTutorial()
	{
        StopCoroutine(StartWineGlassTutorialCo());
		StartCoroutine (EndWineGlassTutorialCo ());
	}
	private IEnumerator EndWineGlassTutorialCo()
	{
        _wineGlassTutorialFinished = true;
        m_status = EStatus.normal;

        _audioSrc.clip = _wineGlassTutorialSFX02;
		_audioSrc.loop = false;
		_audioSrc.Play ();

		yield return new WaitForSeconds (4.0f);

        _audioSrc.Stop();
		_votingManager.NextCharacterToPropose ();
	}

}