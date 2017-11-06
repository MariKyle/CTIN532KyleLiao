using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_PlayerRightHandBehavior : MonoBehaviour
{
    // Simulation
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FOF_GameManager.Instance.VotingManager.HoldUpRightHand();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
		if (other.name == "Voting_Hand_Trigger")
		{
			//Debug.Log ("Yeah1!");
			FOF_GameManager.Instance.VotingManager.HoldUpRightHand();
		}
    }
}
