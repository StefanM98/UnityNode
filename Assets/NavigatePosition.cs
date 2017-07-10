using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatePosition : MonoBehaviour {

    UnityEngine.AI.NavMeshAgent agent;

	// Use this for initialization
	void Awake () {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	public void NavigateTo(Vector3 position) {
        agent.SetDestination(position);
	}

    private void Update()
    {
        GetComponent<Animator>().SetFloat("Distance", agent.remainingDistance);
    }
}

    