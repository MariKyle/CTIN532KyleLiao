using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FOF_PickupBehavior : MonoBehaviour
{
    public enum EState
    {
        normal,
        pickuped,
    }
    protected EState m_state;
    public EState State
    {
        get { return m_state; }
        set
        {
            switch (value)
            {
                //case EState.pickuped:
                //    m_rigidBody.useGravity = false;
                //    m_collider.enabled = false;
                //    break;

                case EState.normal:
                    m_rigidBody.useGravity = true;
                    m_collider.enabled = true;
                    break;
            }

            m_state = value;
        }
    }

    [SerializeField]
    protected AudioClip _dropSFX;
    protected AudioSource _audioSrc;

    protected Rigidbody m_rigidBody;
    protected Collider m_collider;

    protected virtual void Awake()
    {

		m_rigidBody = GetComponent<Rigidbody>();
		Debug.Assert(m_rigidBody != null);
		m_collider = GetComponent<Collider>();
		Debug.Assert(m_collider != null);

		_audioSrc = GetComponent<AudioSource>();
		Debug.Assert(_audioSrc != null);
 		_audioSrc.playOnAwake = false;
		_audioSrc.loop = false;
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (_dropSFX != null && !_audioSrc.isPlaying)
        {
            _audioSrc.clip = _dropSFX;
            _audioSrc.Play();
        }
    }

	public virtual void BePickedUP()
	{
		State = EState.pickuped;
	}

    public virtual void BeDropped()
    {
        State = EState.normal;
    }

}