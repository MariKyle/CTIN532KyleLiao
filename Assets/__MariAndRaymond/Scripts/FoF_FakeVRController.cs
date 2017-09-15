using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoF_FakeVRController : MonoBehaviour
{
    public float SensityZ = 1.0f;

    [SerializeField]
    private float _distance = 0.5f;
    [SerializeField]
    private float distanceMax = 1.0f;
    [SerializeField]
    private float distanceMin = 0.25f;

    [SerializeField]
    float _castRadius = 0.5f;

    private Ray _ray;
    private RaycastHit _hit;

    void Start ()
    {

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

        //if (Physics.SphereCast(origin, _castRadius, _ray.direction, out _hit, _distance))
        if (Physics.Raycast(origin, _ray.direction, out _hit, _distance))
        {
            Debug.Log(_hit.distance);
            _distance = Mathf.Min(_distance, _hit.distance);
        }

        transform.position = origin + endPos;
        transform.rotation = Quaternion.LookRotation(_ray.direction);
    }
}
