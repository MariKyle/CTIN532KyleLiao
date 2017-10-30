using UnityEngine;

public class FOF_PlayerLookingAt : MonoBehaviour {
    [SerializeField] Transform _reticleTransform;
    public bool ReticleFixedDistance;
	private float _lastHitDistance;
    [SerializeField] float _reticleDistance = 4f;
    [Tooltip("Smaller radii work best (sphere is hollow)")]
    [SerializeField] float _radius = 0.3f;

    Vector3 _rayDirection;
    [SerializeField] float _lookRange = 7f;
    [SerializeField] LayerMask _rayhitMask;

    Transform _cameraTransform = null;
    FOF_AmISeen _lastHit;
    RaycastHit _hit;
    RaycastHit _rayhit;
    Vector3 _rayStart;
    const float _rayLength = 30f;

	//Teleport Code Added
	[SerializeField] bool _enableTeleport = false;

    void Awake () {
		_cameraTransform = Camera.main.transform;
        ReticleFixedDistance = true;
    }

    void Update () {
        _rayDirection = _cameraTransform.forward;
        _rayStart = _cameraTransform.position;
        Debug.DrawRay(_rayStart, _rayDirection * _rayLength, Color.green);

        if (_lastHit != null) {
            _lastHit.SetBeingLookedAt(false);
            _lastHit = null;
        }

        if (Physics.SphereCast(_rayStart, _radius, _rayDirection * _rayLength, out _hit, _lookRange)) {

			FOF_AmISeen amISeen = _hit.collider.GetComponent<FOF_AmISeen>();


            if (amISeen) {
                _lastHit = amISeen;
                amISeen.SetBeingLookedAt(true);
				//Debug.Log ("Looked at: " +amISeen.BeingLookedAt);
            }
            _reticleTransform.position = _rayStart +
                (_cameraTransform.TransformDirection(Vector3.forward) * (_hit.distance));
			_lastHitDistance = _hit.distance;
        } else {
			if (ReticleFixedDistance) {
				_reticleTransform.position = _rayStart +
				(_cameraTransform.TransformDirection (Vector3.forward) * _reticleDistance);
			} else {
				_reticleTransform.position = _rayStart +
				(_cameraTransform.TransformDirection(Vector3.forward) * _lastHitDistance);
			}
        }
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.white;
        for (float i = 1f; i < _rayLength; i += 2f) {
            Gizmos.DrawWireSphere(_rayStart + (_rayDirection * i), _radius);
        }
    }
}