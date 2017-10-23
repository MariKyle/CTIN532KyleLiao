using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOF_PlayerRightHandBehavior : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
		if (other.name == "Voting_Hand_Trigger")
		{
			//Debug.Log ("Yeah1!");
			FOF_GameManager.Instance.VotingManager.HoldUpRightHand();
		}
    }
}
