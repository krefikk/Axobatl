using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float followSpeed = 0.5f;
    public float offset = -10;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = PlayerController.player.transform.position + new Vector3(0, 0, offset);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, PlayerController.player.transform.position + new Vector3(0, 0, offset), followSpeed);
    }
}
