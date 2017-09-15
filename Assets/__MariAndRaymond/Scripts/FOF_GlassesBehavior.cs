using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_GlassesBehavior : FOF_PickupBehavior
{
    [SerializeField]
    private Transform _originPlace;
    [SerializeField]
    private Transform _wearPlace;
    [SerializeField]
    private float _minDistanceToWear = 0.5f;
    private Transform _cameraPlace;

    [SerializeField]
    private bool m_wornOnHead;

    [SerializeField]
    protected AudioClip _wearSFX;


    protected override void Awake()
    {
        base.Awake();

        Debug.Assert(_originPlace != null);
        Debug.Assert(_wearPlace != null);
        _cameraPlace = Camera.main.transform;

        Debug.Assert(_wearSFX != null);
    }

    protected override void Update()
    {
        base.Update();

        switch (m_state)
        {
            case EState.normal:
            case EState.waiting:
                if (m_wornOnHead)
                {
                    transform.position = _wearPlace.position;
                    transform.rotation = _wearPlace.rotation;
                }
                break;

            default:
                break;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        FOF_FloorBehavior floor = other.GetComponent<FOF_FloorBehavior>();
        if (floor != null)
        {
            // put the glasses at the origin place.
            transform.position = _originPlace.position;
            transform.rotation = _originPlace.rotation;
        }
    }

    public override void BeDropped()
    {
        base.BeDropped();

        float dist = Vector3.Distance(transform.position, _cameraPlace.position);
        if (dist <= _minDistanceToWear)
        {
            //m_collider.enabled = false;
            m_rigidBody.useGravity = false;
            m_wornOnHead = true;

            _audioSrc.clip = _wearSFX;
            _audioSrc.Play();
        }
        else
        {
            m_rigidBody.useGravity = true;
            m_wornOnHead = false;
        }
    }


}
