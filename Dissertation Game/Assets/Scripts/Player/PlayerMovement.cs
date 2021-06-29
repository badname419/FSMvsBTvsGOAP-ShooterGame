using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string leftKey = "a";
    [SerializeField] private string rightKey = "d";
    [SerializeField] private string backKey = "s";
    [SerializeField] private string forwardKey = "w";

    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rigidbody;

    public GameObject camera;
    Vector3 distantForward;
    Vector3 distantRight;
    Vector3 distantLeft;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;

        if(playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(leftKey))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0, Space.World);
        }
        if (Input.GetKey(rightKey))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
        }
        if (Input.GetKey(forwardKey))
        {
            transform.Translate(0, 0, moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(backKey))
        {
            transform.Translate(0, 0, -moveSpeed * Time.deltaTime, Space.World);
        }

        //Debug
        Vector3 forward = transform.forward;
        forward.y = 0;
        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;

        float radius = 10f;
        distantForward = transform.position + forward * radius;

        Vector3 rightVector = Quaternion.Euler(0, 120, 0) * forward;
        distantRight = transform.position + rightVector * radius;

        Vector3 leftVector = Quaternion.Euler(0, 240, 0) * forward;
        distantLeft = transform.position + leftVector * radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, distantForward);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, distantRight);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, distantLeft);
    }
}
