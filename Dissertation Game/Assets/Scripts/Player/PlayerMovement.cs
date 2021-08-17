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
    [SerializeField] private float rotationRate = 15f;
    [SerializeField] private float shootingWaitTime = 1f;
    [SerializeField] private int shootingDamage = 5;
    
    private Rigidbody rigidbody;
    private Vector3 moveInput;
    private Shooting shooting;
    
    private bool isMoving;

    public GameObject camera;
    Vector3 distantForward;
    Vector3 distantRight;
    Vector3 distantLeft;
    public bool activeMovement;
    



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        shooting = GetComponent<Shooting>();
        isMoving = false;
        activeMovement = true;
    }

    // Update is called once per frame
    void Update()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;

        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

        //Shooting
        if (Input.GetMouseButtonDown(0))
        {
            shooting.Shoot(shootingWaitTime, shootingDamage, transform);
        }
    }

    private void FixedUpdate()
    {
        if (activeMovement)
        {
            rigidbody.velocity = moveInput * moveSpeed; //multiply the direction by an appropriate speed, Vector3(0,1,0) is straight up.
        }

        isMoving = moveInput != Vector3.zero;
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

    public bool IsMoving()
    {
        return isMoving;
    }

}
