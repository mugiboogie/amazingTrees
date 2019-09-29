using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;
    private Vector3 lookAtPosition;
    public Transform camTransform;
    public float sensitivityX;
    public float sensitivityY;

    public Transform target;
    //public Vector3 targetOffset = new Vector3(0f,1f,0f);
    //public float turnSpeed = 5f;
    public float followSpeed = 5f;
    public bool follow;
    public Transform anchor;
    public float desiredFOV;
    private float currentFOV;
    public float distance = 7.0f;

    private Vector3 setPosition;
    private Vector3 targetPosition;

    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    private Camera cam;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float inputY;

    public LayerMask levelLayers;
    private PlayerTargetting playerTargetting;

    private float trackTime;
    public CombatZoneController combatZone;


    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerTargetting = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTargetting>();
    }

    void Update()
    {


        OldStyle();

    }

    private void NewStyle()
    {
       //Set currentX
        if (playerTargetting.lockedOn == false)
        {
            currentX += Mathf.Clamp(Input.GetAxis("Mouse X"), -5f, 5f) * sensitivityX;
        }
        else
        {
            Vector3 PlayerPosition = target.transform.position;
            PlayerPosition.y = 0f;
            Vector3 EnemyPosition = playerTargetting.enemyTarget.transform.position;
            EnemyPosition.y = 0f;
            Vector3 TargetDirection = EnemyPosition - PlayerPosition;
            float lockOnAngle = -Vector3.SignedAngle(TargetDirection, Vector3.forward, Vector3.up);
            currentX = Mathf.Lerp(currentX, lockOnAngle, Mathf.Min(10f * Time.deltaTime, .0325f));
        }

        if (currentX > 360f) { currentX = 0f; }

        float desiredDistance = 5f;
        Vector3 origin = lookAt.position + Vector3.up * 2f;

        Vector3 backDir = origin + (Quaternion.Euler(0, currentX, 0) * new Vector3(0, 0, 1f) * desiredDistance);
        Vector3 upDir = origin + (Vector3.up * desiredDistance);


        //Get maxYAngle
        int iterations = 36;
        float radius = .3f;
        float maxYAngle = Y_ANGLE_MAX;
        
        for(int i=0; i<iterations; i++)
        {
            float testAngle = -(Mathf.Abs(Y_ANGLE_MIN) / Mathf.Round(iterations)) * Mathf.Round(i);
            Vector3 destination = origin + (Quaternion.Euler(testAngle, currentX, 0) * new Vector3(0, 0, 1f) * desiredDistance);
            Vector3 targetDir = destination - origin;
            RaycastHit hit;

            Debug.DrawLine(origin, destination);

            if (Physics.SphereCast(origin, radius, targetDir.normalized, out hit, desiredDistance, levelLayers))
            {
                maxYAngle = testAngle;
                Debug.DrawLine(origin, destination, Color.red);
            }

        }
        Debug.Log(maxYAngle);

        inputY += Input.GetAxis("Mouse Y") * sensitivityY;
        inputY = Mathf.Clamp(inputY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        currentY = Mathf.Clamp(inputY, Y_ANGLE_MIN, maxYAngle);

        Vector3 desiredDestination = origin + (Quaternion.Euler(currentY, currentX, 0) * new Vector3(0,0,1f) * (desiredDistance-radius));
        Debug.DrawLine(origin, desiredDestination, Color.green);

        setPosition = Vector3.Lerp(setPosition, desiredDestination, 5f * Time.unscaledDeltaTime);

        transform.position = setPosition;
        camTransform.LookAt(origin);


    }

    private void OldStyle()
    {
        if (currentX > 360f) { currentX = 0f; }

        distance = CalculateDistance();

        if (combatZone == null)
        {
            //distance = Mathf.Lerp(distance, 7f,5f*Time.deltaTime);
            //trackTime = Time.time + 5f;
        }
        else
        {
            //distance = Mathf.Lerp(distance, 14f, 5f * Time.deltaTime);

        }
        //follow = (anchor==null);

        //if(follow)
        {
            lookAt = target;
            if (playerTargetting.lockedOn == false)
            {
                currentX += Mathf.Clamp(Input.GetAxis("Mouse X"), -5f, 5f) * sensitivityX;
            }

            else
            {
                Vector3 PlayerPosition = target.transform.position;
                PlayerPosition.y = 0f;
                Vector3 EnemyPosition = playerTargetting.enemyTarget.transform.position;
                EnemyPosition.y = 0f;
                Vector3 TargetDirection = EnemyPosition - PlayerPosition;
                float lockOnAngle = -Vector3.SignedAngle(TargetDirection, Vector3.forward, Vector3.up);
                currentX = Mathf.Lerp(currentX, lockOnAngle, Mathf.Min(10f * Time.deltaTime, .0325f));

                //currentX += 180f * Time.deltaTime * (lockOnAngle<currentX?-1f:1f);
            }

            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            setPosition = lookAt.position + (Vector3.up * 2f) + rotation * dir;
            lookAtPosition = lookAt.position + (Vector3.up * 1f);
            //camTransform.LookAt(lookAt.position);

        }
        /*else
        {
            //setPosition = anchor.position;
            currentY = transform.eulerAngles.x;
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
            currentX = transform.eulerAngles.y;

            if (Time.time > trackTime)
            {
                float distance = Vector3.Distance(new Vector3(target.position.x, 0f, target.position.z), new Vector3(transform.position.x, 0f, transform.position.z));
                if (distance > 10f)
                {
                    setPosition += (target.position - transform.position) * Time.deltaTime;

                }

                setPosition.y = (target.position.y + 1f) - transform.forward.y * 6f;

            }
            else if(combatZone!=null)
            {
                setPosition = Vector3.Lerp(transform.position,combatZone.transform.position,5f*Time.deltaTime);
                setPosition.y = (target.position.y + 1f) - transform.forward.y * 6f;
            }


            lookAt = target;

            
                lookAtPosition = lookAt.position;
            
            //camTransform.LookAt(lookAt.position);
        }*/

        transform.position = Vector3.Lerp(transform.position, setPosition, 5f * Time.unscaledDeltaTime);

        targetPosition = Vector3.Lerp(targetPosition, lookAtPosition, 4f * Time.unscaledDeltaTime);
        camTransform.LookAt(targetPosition);

        currentFOV = Mathf.Lerp(currentFOV, desiredFOV, 4f * Time.unscaledDeltaTime);
        cam.fieldOfView = currentFOV;
    }


    private float CalculateDistance()
    {
        float defaultDistance = combatZone==null?4f:8f;

        
        
        float setDistance = defaultDistance;

        Vector3 origin = lookAt.position + Vector3.up * 2f;
        RaycastHit hit;

        float radius = .3f;
        Vector3 targetDir = -camTransform.forward;

        Debug.DrawLine(origin, origin + targetDir);

        if (Physics.SphereCast(origin, radius, targetDir.normalized, out hit, defaultDistance, levelLayers))
        {
            setDistance = hit.distance;
        }


        if (setDistance < defaultDistance)
        {
            //verticalOffset = Mathf.Lerp(verticalOffset, setVerticalOffset, 10f * Time.unscaledDeltaTime);
            return Mathf.Lerp(distance, setDistance, 10f * Time.unscaledDeltaTime);
        }
        else
        {
            //verticalOffset = Mathf.Lerp(verticalOffset, setVerticalOffset, 10f * Time.unscaledDeltaTime);
            return Mathf.Lerp(distance, setDistance, 10f * Time.unscaledDeltaTime);
        }
        
        //return setDistance;
    }

}
