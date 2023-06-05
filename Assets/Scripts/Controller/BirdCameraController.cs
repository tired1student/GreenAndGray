using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdCameraController : MonoBehaviour
{
    public Transform Bird;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Bird.position.x, 0, -10f);
    }

}