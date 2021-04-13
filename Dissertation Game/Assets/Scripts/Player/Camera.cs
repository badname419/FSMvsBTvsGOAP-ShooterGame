using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private float smooth = 0.3f;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float zOffset = 28f;
    [SerializeField] private float yOffset = -48f;
    private Vector3 velocity = Vector3.zero;

    public Transform player;

    private void Update()
    {
        Vector3 pos = new Vector3();
        pos.x = player.position.x - xOffset;
        pos.z = player.position.z - zOffset;
        pos.y = player.position.y - yOffset;
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smooth);
    }

}
