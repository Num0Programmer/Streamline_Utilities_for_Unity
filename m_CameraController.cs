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
[RequireComponent( typeof( Camera ) )]
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
    ///     Reference to an arbitrary point in space, maintained by an empty
    ///     game object, which marks the center of the object the camera will
    ///     rotate about
    /// </summary>
    protected Transform target;

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
