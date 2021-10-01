using UnityEngine;

/**
 * <summary>
 *      The m_CameraController class is intended to be inhereted from. This class provides a few methods, which are
 *      overwritable, used to help streamline the process of setting up player cameras in Unity.
 * </summary>
 */
[RequireComponent(typeof(Camera))]
public class m_CameraController : MonoBehaviour
{
    /**
     * <summary>
     *      Instance variable <c>yaw</c> represents the rotation of the camera around the y-axis.
     * </summary>
     */
    protected float yaw = 0f;

    /**
     * <summary>
     *      Instance variable <c>pitch</c> represents the rotation of the camera around the x-axis.
     * </summary>
     */
    protected float pitch = 0f;

    /**
     * <summary>
     *      Instance variable <c>minPitch</c> controls the smallest degree from 0 the camera can rotate around the
     *      x-axis.
     * </summary>
     */
    [SerializeField]
    protected float minPitch;

    /**
    * <summary>
    *       Instance variable <c>maxPitch</c> controls the greatest degree from 0 the camera can rotate around the
    *       x-axis.
    * </summary>
    */
    [SerializeField]
    protected float maxPitch;

    /**
     * <summary>
     *      Instance variable <c>sensitivityH</c> controls the rotation speed of the camera around the y-axis.
     * </summary>
     */
    [SerializeField]
    protected float sensitivityH = 10f;

    /**
     * <summary>
     *      Instance variable <c>sensitivityV</c> controls the rotation speed of the camera around the x-axis.
     * </summary>
     */
    [SerializeField]
    protected float sensitivityV = 10f;

    /**
     * <summary>
     *      Instance variable <c>thirdPersonCam</c> used to control how to interpret mouse input.
     *      
     *      <example>
     *          If <c>thidPersonCam</c> is true, then mouse input will be interpreted assuming third person.
     *          
     *          Otherwise, mouse input will be interpreted assuming first person.
     *      </example>
     * </summary>
     */
    [SerializeField]
    private bool thirdPersonCam;

    /**
     * <summary>
     *      Instance variable <c>distFromTarget</c> controls how far away from the player the camera will be.
     * </summary>
     */
    [SerializeField]
    protected float distFromTarget = 4f;

    /**
     * <summary>
     *      Instance variable <c>rotationSmoother</c> controls how smooth the transition is from rotation n to
     *      rotation n + mouse_input.
     * </summary>
     */
    [SerializeField]
    private float rotationSmoother = 0.1f;

    /**
     * <summary>
     *      Instance variable <c>currentRotation</c> represents the current rotation of the camera
     * </summary>
     */
    private Vector3 currentRotation = Vector3.zero;

    /**
     * <summary>
     *      Instance variable <c>rotationSmoothVelocity</c> used during rotation calculation to help smooth from one
     *      rotation to another.
     * </summary>
     */
    private Vector3 rotationSmoothVelocity;

    /**
     * <summary>
     *      Instance variable <c>target</c> represents the object the camera will focus on and move/rotate based on.
     *      
     *      <example>
     *          Generally, the <c>target</c> will be the player object.
     *      </example>
     * </summary>
     */
    protected Transform target;

    /**
     * <summary>
     *      This method uses the current <c>pitch</c> and <c>yaw</c> of the camera to rotate around the <c>target</c>
     *      object.
     *      
     *      <remarks>This method should be called in LateUpdate()</remarks>
     * </summary>
     */
    protected virtual void RotateInThird()
    {
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity,
                                             rotationSmoother);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distFromTarget;
    }

    /**
     * <summary>
     *      This method uses the current <c>pitch</c> and <c>yaw</c> of the camera to rotate about the local y-axis of the
     *      <c>target</c> object.
     * </summary>
     */
    protected virtual void RotateInFirst()
    {
        transform.eulerAngles = new Vector3(pitch,
                                            transform.eulerAngles.y,
                                            transform.eulerAngles.z);

        target.transform.eulerAngles = new Vector3(target.transform.eulerAngles.x,
                                                   yaw,
                                                   target.transform.eulerAngles.z);
    }

    /**
     * <summary>
     *      This method mutates <c>pitch</c> and <c>yaw</c> based on mouse input.
     *      
     *      Based on the state of <c>thirdPersonCam</c>, this method will assume and mutate <c>pitch</c> and <c>yaw</c>
     *      accordingly.
     * </summary>
     */
    protected virtual void RotatingInput()
    {
        if (thirdPersonCam)
        {
            yaw += Input.GetAxisRaw("Mouse X") * sensitivityH;
            pitch += Input.GetAxisRaw("Mouse Y") * sensitivityV;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
        else
        {
            yaw += Input.GetAxisRaw("Mouse X") * sensitivityH;
            pitch -= Input.GetAxisRaw("Mouse Y") * sensitivityV;
            pitch = Mathf.Clamp(pitch, -minPitch, -maxPitch);
        }
    }

    /**
     * <summary>
     *      This method will lock and hide the player's mouse while the game is running.
     * </summary>
     */
    protected virtual void LockNHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}