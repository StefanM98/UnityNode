using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMove : MonoBehaviour {

    public SocketIOComponent socket;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMove(Vector3 position)
    {
        // Send position to node
        Debug.Log("sending position to node" + TestSocketIO.VectorToJson(position));
        socket.Emit("move", new JSONObject(TestSocketIO.VectorToJson(position)));
    }
}
