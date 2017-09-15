using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_FakeVRController : MonoBehaviour
{
    public enum EState
    {
        normal,
        highlighted,
        holding,
    }
    private EState m_state;
    public EState State
    {
        set
        {
            Material currentMat = (value == EState.highlighted) ? _highlightedMat : _originMat;
            foreach (MeshRenderer renderer in _renderers)
            {
                renderer.material = currentMat;
            }

            m_state = value;
        }
    }

    [SerializeField]
    private Transform _holdingPlace;    // for pickup items
    [SerializeField]
    private FOF_PickupBehavior _holdingItem = null;

    public float SensityZ = 1.0f;

    [SerializeField]
    private float _distance = 0.5f;
    [SerializeField]
    private float distanceMax = 1.0f;
    [SerializeField]
    private float distanceMin = 0.25f;

    [SerializeField]
    private float _castRadius = 0.5f;

    [SerializeField]
    private Material _highlightedMat;
    private Material _originMat;
    private MeshRenderer[] _renderers;
    private bool m_highlighted;

    private Ray _ray;
    private RaycastHit _hit;

    void Start ()
    {
        Debug.Assert(_highlightedMat != null);
        _renderers = GetComponentsInChildren<MeshRenderer>();
        Debug.Assert(_renderers.Length > 0);
        _originMat = _renderers[0].material;

        Debug.Assert(_holdingPlace != null);
    }
	
	void Update ()
    {
        if (Input.GetMouseButton(1))  // Right mouse button down
        {
            _distance += Input.GetAxis("Mouse Y") * SensityZ;
            _distance = Mathf.Clamp(_distance, distanceMin, distanceMax);
        }
        else
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        Vector3 origin = _ray.origin;
        Vector3 endPos = _ray.direction * _distance;
        Debug.DrawRay(origin, endPos, Color.red);


        switch (m_state)
        {
            case EState.holding:
                if (Input.GetMouseButtonUp(0))
                {
                    break;
                }

                Debug.Assert(_holdingItem != null);
                Rigidbody itemRBody = _holdingItem.GetComponent<Rigidbody>();
                Debug.Assert(itemRBody != null);
                itemRBody.useGravity = false;
                Collider itemCollider = _holdingItem.GetComponent<Collider>();
                Debug.Assert(itemCollider != null);
                itemCollider.enabled = false;
                _holdingItem.transform.position = _holdingPlace.position;
                _holdingItem.transform.rotation = _holdingPlace.rotation;
                break;

            case EState.normal:

                break;

            default:
                break;
        }

        //if (Physics.SphereCast(origin, _castRadius, _ray.direction, out _hit, _distance))
        if (Physics.Raycast(origin, _ray.direction, out _hit, _distance))
        {
            //Debug.Log(_hit.distance);
            _distance = Mathf.Min(_distance, _hit.distance);
        }

        transform.position = origin + endPos;
        transform.rotation = Quaternion.LookRotation(_ray.direction);
    }

    public void PickUp(FOF_PickupBehavior item)
    {
        _holdingItem = item;
        State = EState.holding;
    }

}
