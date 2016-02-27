using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject cameraObject;

    private Camera camera;

	// Use this for initialization
	void Start () {

        camera = cameraObject.GetComponent<Camera>();

        camera.transform.SetParent(transform);

	}
	
	// Update is called once per frame
	void Update () {


	
	}
}
