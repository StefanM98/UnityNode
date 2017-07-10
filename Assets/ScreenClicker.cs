using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenClicker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire2"))
            Clicked();
	}

    private void Clicked()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            var clickMove = hit.collider.gameObject.GetComponent<ClickMove>();
            clickMove.OnClick(hit.point);
        }
    }
}
