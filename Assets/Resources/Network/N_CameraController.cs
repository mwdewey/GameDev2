using UnityEngine;
using System.Collections;

public class N_CameraController : MonoBehaviour {
    private GameObject cameraObject;
    private Camera camera;
    private Vector3 pos;

    // Use this for initialization
    void Start()
    {
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        camera = cameraObject.GetComponent<Camera>();
        pos = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        pos = camera.transform.position;

        pos.x = transform.position.x;
        pos.y = transform.position.y;

        camera.transform.position = pos;

    }
}
