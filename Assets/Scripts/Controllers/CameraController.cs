using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

    public static CameraController GetInstance()
    {
        return instance;
    }

    public TimerManager timermanager;
    public Transform target;
    public GameObject Map;

    private Rigidbody myRigidbody;
    private Vector3 actualTarget;
    private Vector3 offset;
    private Vector3 mouseOrigin;
    private Vector3 lastPanPosition;
    private Vector3 OriginalPosition;

    public float distance = 70.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = 24f;
    public float yMaxLimit = 50f;

    public float distanceMin = 10f;
    public float distanceMax = 40f;

    public float fPanSpeed = 0.02f;

    private float t = 0.0f;
    float x = 0.0f;
    float y = 0.0f;

    bool bIsRotating = false;
    bool bIsPanning = false;

    Quaternion rotation;

    float startTime;
    float speed = 1.0f;
    float journeyLength;

    // Use this for initialization
    void Start()
    {
        instance = this;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        actualTarget = new Vector3(0f, 0.0f, 0f);

        myRigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (myRigidbody != null)
        {
            myRigidbody.freezeRotation = true;
        }


        rotation = Quaternion.Euler(y, x, 0);

        OriginalPosition = transform.position;
    }

    void Update()
    {
        lastPanPosition = Input.mousePosition;

        if (!timermanager.bTutorial)
        {
            if (Input.GetMouseButtonDown(1))
            {
                bIsRotating = true;
            }
        }
        

        if (distance < 30.0f)
        {
            bIsPanning = true;
        }
        else
        {
            bIsPanning = false;
        }

    }

        void LateUpdate()
    {


        // if it is rotating then update the x and y
        if (bIsRotating)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 10, distanceMin, distanceMax);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + actualTarget + offset;
               
        //vec3 movetowrads
        if (distance == 42.0f)
        {
            position.x = Mathf.Lerp(transform.position.x, 0.0f, 5.0f * Time.deltaTime);
            position.z = Mathf.Lerp(transform.position.z, (rotation * negDistance).z, 10.0f * Time.deltaTime);
            offset = Vector3.zero;
        }

        transform.rotation = rotation;
        transform.position = position;
                
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");

        //// Perform the movement
        Vector3 RIGHT = transform.TransformDirection(Vector3.right);
        Vector3 FORWARD = transform.TransformDirection(new Vector3(0,0,1));
        RIGHT.y = 0;
        FORWARD.y = 0;
        
        if (bIsPanning)
        {
            offset += (RIGHT * xAxisValue) * fPanSpeed + (FORWARD * zAxisValue) * fPanSpeed;
        }
                
        if (!Map.GetComponent<Renderer>().isVisible)
        {
            t += 0.2f * Time.deltaTime;
            // lerp back to original
            position.x = Mathf.Lerp(transform.position.x, OriginalPosition.x, t);
            position.z = Mathf.Lerp(transform.position.y, OriginalPosition.y, t);
            position.z = Mathf.Lerp(transform.position.z, OriginalPosition.z, t);
            offset = Vector3.zero;
            t = 0.0f;
            transform.rotation = rotation;
            transform.position = position;
        }


        if (!Input.GetMouseButton(1)) bIsRotating = false;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public bool GetIsRotating()
    {
        return bIsRotating;
    }
}
