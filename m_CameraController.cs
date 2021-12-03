using UnityEngine;

/// <summary>
///     The m_CameraController provides a basic framework for camera control;
///     this is meant to be changed at the whim of the user to achieve
///     ideal camera control for the player
///
///     Note: This class is intended to be inhereted from
///
///     Author: Num0Programmer
/// </summary>
[RequireComponent( typeof( Camera ) ) ]
public class m_CameraController : MonoBehaviour
{
    /// <summary>
    ///     Represents the rotation of the camera around the y-axis
    /// </summary>
    protected float yaw = 0f;

    /// <summary>
    ///     Represents the rotation of the camera around the x-axis
    /// </summary>
    protected float pitch = 0f;

    /// <summary>
    ///     Used to define the smallest degree from 0 the camera can rotate
    ///     around the x-axis
    ///
    ///     Note: This must be a positive number; a fix is coming
    /// </summary>
    [SerializeField]
    protected float minPitch;

    /// <summary>
    ///     Used to define the greatest degree from 0 the camera can rotate
    ///     around the x-axis
    ///
    ///     Note: This must be a negative number; a fix is coming
    /// </summary>
    [SerializeField]
    protected float maxPitch;

    /// <summary>
    ///     Controls the rotation speed of the camera around the y-axis
    /// </summary>
    [SerializeField]
    protected float sensitivityH = 10f;

    /// <summary>
    ///     Controls the rotation speed of the camera around the x-axis
    /// </summary>
    [SerializeField]
    protected float sensitivityV = 10f;

    /// <summary>
    ///     Determines how the camera will move about the player
    ///
    ///     Note: Off - rotate around player's z- and x-axies - First Person
    ///           On - rotate around the player object - Third Person
    /// </summary>
    [SerializeField]
    private bool thirdPersonCam;

    /// <summary>
    ///     Determines the distance the camera will keep from the player object
    /// </summary>
    [SerializeField]
    protected float distFromTarget = 4f;

    /// <summary>
    ///     Used to control how smooth the transition is from rotation n to
    ///     rotation n + mouse_input
    /// </summary>
    [SerializeField]
    private float rotationSmoother = 0.1f;

    /// <summary>
    ///     Represents the current rotation of the camera
    /// </summary>
    private Vector3 currentRotation = Vector3.zero;

    /// <summary>
    ///     Used during rotation calculation to help smooth from one rotation to
    ///     another
    /// </summary>
    private Vector3 rotationSmoothVelocity;

    /// <summary>
    ///     Reference to an arbitrary point in space, maintained by an empty
    ///     game object, which marks the center of the object the camera will
    ///     rotate about
    /// </summary>
    protected Transform target;

    /// <summary>
    ///     Uses the current pitch and yaw of the camera to rotate around the
    ///     target object
    /// </summary>
    protected virtual void RotateInThird()
    {
        currentRotation = Vector3.SmoothDamp( currentRotation, new Vector3( pitch, yaw ), ref rotationSmoothVelocity,
                                              rotationSmoother );

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distFromTarget;
    }

    /// <summary>
    ///     Uses the current pitch and yaw of the camera to rotate about the
    ///     local y-axis of the target object
    /// </summary>
    protected virtual void RotateInFirst()
    {
        transform.eulerAngles = new Vector3(pitch,
                                            transform.eulerAngles.y,
                                            transform.eulerAngles.z);

        target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x,
                                                   yaw,
                                                   target.transform.eulerAngles.z);
    }

    /// <summary>
    ///     Mutates the pitch and yaw of the camera based on mouse input
    ///
    ///     Note: This method is intended to be called in the FixedUpdate() in a
    ///           camera controller script
    /// </summary>
    protected virtual void RotatingInput()
    {
        if ( thirdPersonCam )
        {
            yaw += Input.GetAxisRaw( "Mouse X" ) * sensitivityH;
            pitch += Input.GetAxisRaw( "Mouse Y" ) * sensitivityV;
            pitch = Mathf.Clamp( pitch, minPitch, maxPitch );
        }
        else
        {
            yaw += Input.GetAxisRaw( "Mouse X" ) * sensitivityH;
            pitch -= Input.GetAxisRaw( "Mouse Y" ) * sensitivityV;
            pitch = Mathf.Clamp( pitch, -minPitch, -maxPitch );
        }
    }

    /// <summary>
    ///     This method will lock and hide the player's mouse while the game is running
    /// </summary>
    protected virtual void LockNHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}