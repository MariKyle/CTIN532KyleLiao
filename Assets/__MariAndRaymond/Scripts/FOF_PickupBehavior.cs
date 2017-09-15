using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_PickupBehavior : MonoBehaviour
{
    public enum EState
    {
        normal,
        waiting,
        pickuped,
    }
    private EState m_state;
    public EState State
    {
        get { return m_state; }
        set { m_state = value; }
    }

    [SerializeField]
    private FOF_FakeVRController _vrController;

    private void Awake()
    {
        if (_vrController == null)
        {
            GameObject vrControllerObj = GameObject.Find("FakeVRController");
            Debug.Assert(vrControllerObj != null);
            _vrController = vrControllerObj.GetComponent<FOF_FakeVRController>();
            Debug.Assert(_vrController != null);
        }
    }

    private void Update()
    {
        if (m_state == EState.waiting)
        {
            // Temperal
            if (Input.GetButtonDown("Fire1"))
            {
                _vrController.PickUp(this);
                m_state = EState.pickuped;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerExit(Collider other)
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

}