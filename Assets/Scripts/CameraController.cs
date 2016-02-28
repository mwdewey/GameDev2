using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject cameraObject;
    public float x_min;
    public float x_max;
    public float y_min;
    public float y_max;

    private Camera camera;
    private Vector3 pos;

	// Use this for initialization
	void Start () {

        camera = cameraObject.GetComponent<Camera>();
        //camera.transform.SetParent(transform);
        pos = camera.transform.position;

	}
	
	// Update is called once per frame
	void Update () {

        pos = camera.transform.position;

        pos.x = transform.position.x;
        pos.y = transform.position.y;

        if (pos.x < x_min) pos.x = x_min;
        if (pos.x > x_max) pos.x = x_max;
        if (pos.y < y_min) pos.y = y_min;
        if (pos.y > y_max) pos.y = y_max;

        camera.transform.position = pos;
	
	}
}
