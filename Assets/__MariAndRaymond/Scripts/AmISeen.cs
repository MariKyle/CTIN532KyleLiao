using UnityEngine;


public class AmISeen : MonoBehaviour
{
    //public bool beingLookedAt = false;

    [System.Serializable]
    private enum LookState
    {
        Seen,
        FadingToSeen,
        FadingToUnseen,
        Unseen
    }

    // (JULIAN): Changing this is currently meaningless because of how BeingLookedAt functions
    // (Raymond): These two values now are used for fading in/out sfx for manifestations
    [SerializeField] float _fadingToSeenDurationSeconds = 1.0f;
    [SerializeField] float _fadingToUnseenDurationSeconds = 1.0f;
    [SerializeField] LookState _state = LookState.Unseen;
	public float _seenFraction = 0.0f;
    //public bool BeingLookedAt { get { return _seenFraction > Mathf.Epsilon; } }
    public bool BeingLookedAt { get { return _seenFraction > _fadingToSeenDurationSeconds * 0.9f; } }
    public void SetBeingLookedAt (bool val)
    {
        _state = val ? LookState.FadingToSeen : LookState.FadingToUnseen;
    }

    LookState ClampState (float currFrac, LookState currState)
    {
        if (currFrac <= 0f && currState == LookState.FadingToUnseen)
        {
            return LookState.Unseen;
        }
        if (currFrac >= 1f && currState == LookState.FadingToSeen)
        {
            return LookState.Seen;
        }

        return currState;
    }

    void Update ()
    {
        switch (_state)
        {
			case LookState.FadingToSeen:
            case LookState.Seen:
                _seenFraction = Mathf.Approximately(_fadingToSeenDurationSeconds, 0f) ? 1f :
                                 _seenFraction + Time.deltaTime * 1f / _fadingToSeenDurationSeconds;
                break;
			case LookState.FadingToUnseen:
			case LookState.Unseen:
			    _seenFraction = Mathf.Approximately (_fadingToUnseenDurationSeconds, 0f) ? 0f :
				_seenFraction - (Time.deltaTime * 1f / _fadingToUnseenDurationSeconds);
                break;
        }
        _state = ClampState(_seenFraction, _state);
        _seenFraction = Mathf.Clamp01(_seenFraction);
    }
}