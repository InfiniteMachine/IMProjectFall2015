﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float distance;
//    public Canvas DayNightCanvas;
//	public float close, far;
//	public Vector3 cameraClose, cameraMid, cameraFar;
//
//	int groundLayer = 8;
//	int layerMask;
//
//	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player(Clone)");
//		playerCamera = Camera.main.gameObject;
//		layerMask = 1 << groundLayer;
        //DayNightCanvas = GameObject.FindGameObjectWithTag("DayNightCanvas").GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, distance);

//		RaycastHit2D hit = (Physics2D.Raycast(transform.position, Vector2.up, far, layerMask));
//		if (hit.collider != null) {
//			if (hit.distance < close) {
//				RaycastHit2D hitDown = (Physics2D.Raycast(transform.position, Vector2.down, close, layerMask));
//				if (hitDown.collider !=null) {
//
//				}
//				playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, cameraClose, 10f * Time.fixedDeltaTime);
//			} else if (hit.distance >= close && hit.distance <= far) {
//				playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, cameraMid, 10f * Time.fixedDeltaTime);
//			} 
//		} else {
//			playerCamera.transform.localPosition = Vector3.MoveTowards(playerCamera.transform.localPosition, cameraFar, 10f * Time.fixedDeltaTime);
//		}
	}

//    void Update()
//    {
//        DayNightCanvas.planeDistance = -playerCamera.transform.localPosition.z + 0.1f;
//    }
}
