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
        waiting,
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
                case EState.pickuped:
                    m_rigidBody.useGravity = false;
                    m_collider.enabled = false;
                    break;

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

    [SerializeField]
    protected FOF_FakeVRController _vrController;

    protected Rigidbody m_rigidBody;
    protected Collider m_collider;

    protected virtual void Awake()
    {
        if (_vrController == null)
        {
            GameObject vrControllerObj = GameObject.Find("FakeVRController");
            Debug.Assert(vrControllerObj != null);
            _vrController = vrControllerObj.GetComponent<FOF_FakeVRController>();
            Debug.Assert(_vrController != null);

            m_rigidBody = GetComponent<Rigidbody>();
            Debug.Assert(m_rigidBody != null);
            m_collider = GetComponent<Collider>();
            Debug.Assert(m_collider != null);

            _audioSrc = GetComponent<AudioSource>();
            Debug.Assert(_audioSrc != null);
            _audioSrc.playOnAwake = false;
            _audioSrc.loop = false;
        }
    }

    protected virtual void Update()
    {
        if (m_state == EState.waiting)
        {
            // Temperal
            if (Input.GetButtonDown("Fire1"))
            {
                _vrController.PickUp(this);
                State = EState.pickuped;
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (_dropSFX != null && !_audioSrc.isPlaying)
        {
            _audioSrc.clip = _dropSFX;
            _audioSrc.Play();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.LogError("OnTriggerEnter");
        // Check if touched by the VR controller.
        FOF_FakeVRController controller = other.GetComponent<FOF_FakeVRController>();
        if (controller != null)
        {
            if (m_state == EState.normal)
            {
                controller.State = FOF_FakeVRController.EState.highlighted;
                m_state = EState.waiting;
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // Check if touched by the VR controller.
        FOF_FakeVRController controller = other.GetComponent<FOF_FakeVRController>();
        if (controller != null)
        {
            //if (m_state == EState.normal)
            {
                controller.State = FOF_FakeVRController.EState.normal;
                m_state = EState.normal;
            }
        }
    }

    public virtual void BeDropped()
    {
        State = EState.normal;
    }

}