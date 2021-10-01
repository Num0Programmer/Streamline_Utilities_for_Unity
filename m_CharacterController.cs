using UnityEngine;

/**
 * <summary>
 *      The m_CharacterController class is intended to be inhereted from. This class provides a few methods, which are
 *      overwritable, used to help streamline the process of setting up player movement in Unity.
 * </summary>
 *
 * <author>
 *      Num0Programmer
 * </author>
 */
[RequireComponent(typeof(Rigidbody))]
public class m_CharacterController : MonoBehaviour
{
    /**
     * <summary>
     *      Instance variable <c>moveForce</c> the force applied to the player object when directional input is provided.
     * </summary>
     */
    [SerializeField]
    protected float moveForce;

    /**
     * <summary>
     *      Instance variable <c>jumpForce</c> the force applied to the player object when jump input is provided.
     * </summary>
     */
    [SerializeField]
    protected float jumpForce;

    /**
    * <summary>
    *      Instance variable <c>showGroundChecking</c> used for <c>groundCheck</c> position and radius debugging.
    *      
    *      <example>
    *           If <c>showGroundChecking</c> is equal to true, then a wire sphere will be displayed in the Scene, with a
    *           specified radius.
    *           
    *           Otherwise, the wire sphere will NOT be displayed.
    *      </example>
    * </summary>
    */
    [SerializeField]
    private bool showGroundChecking;

    /**
    * <summary>
    *      Instance variable <c>rotationSmoother</c> controls how smooth the transition is from rotation n to
    *      rotation n + desire_rotation.
    * </summary>
    */
    [SerializeField]
    private float rotationSmoother = 1f;

    /**
    * <summary>
    *      Instance variable <c>rotationSmoothVelocity</c> represents the current rotation of the player object after
    *      smoothing is applied.
    * </summary>
    */
    private float rotationSmoothVelocity;

    /**
    * <summary>
    *      Instance variable <c>velocitySmoother</c> controls how smooth the transition is from velocity n to
    *      veloctiy n + wished_veloctiy.
    * </summary>
    */
    [SerializeField]
    private float velocitySmoother = 0.1f;

    /**
    * <summary>
    *      Instance variable <c>smoothVelocity</c> represents the current velocity of player object after smoothing is
    *      applied.
    * </summary>
    */
    private Vector3 smoothVelocity;

    /**
    * <summary>
    *      Instance variable <c>checkForGroundDist</c> the maximum vertical distance the player object can be from the
    *      ground and still be considered "grounded".
    * </summary>
    */
    [Range(0f, 1f)]
    [SerializeField]
    protected float checkForGroundDist;

    /**
    * <summary>
    *      Instance variable <c>body</c> is a reference to the Rigidbody component which is appears on the player object.
    * </summary>
    */
    protected Rigidbody body;

    /**
    * <summary>
    *      Instance variable <c>groundCheck</c> an arbitrary point in space, maintained by an empty game object which marks
    *      the center of a sphere with radius = <c>checkForGroundDist</c>.
    * </summary>
    */
    [SerializeField]
    protected Transform groundCheck;

    /**
    * <summary>
    *      Instance variable <c>groundMask</c> determines what layer is "ground".
    * </summary>
    */
    [SerializeField]
    protected LayerMask groundMask;

    /**
    * <summary>
    *      Instance variable <c>cam</c> which represents the position and rotation attributes of the camera accompanying
    *      the player object.
    * </summary>
    */
    protected Transform cam;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    /**
     * <summary>
     *      This method mutates the player object's velocity based on a desired direction.
     *      
     *      <param name="inWishedDir">
     *          <remarks>
     *              Vector3
     *          </remarks>
     *          
     *          A normalized vector representing the desired the player wishes to direct the player object.
     *      </param>
     *      
     *      <param name="jump">
     *          <remarks>
     *              Boolean
     *          </remarks>
     *          
     *          Identifies whether to add up force to the player object.
     *      </param>
     *      
     *      <param name="crouch">
     *          <remarks>
     *              Boolean
     *          </remarks>
     *          
     *          Identifies whether the player is crouching.
     *          
     *          <remarks>
     *              The player's velocity will be restricted to a depleated max velocity while <c>crouch</c> is
     *              enabled.
     *          </remarks>
     *      </param>
     * </summary>
     */
    public virtual void MoveInFirst(Vector3 inWishedDir, bool jump, bool crouch)
    {
        body.velocity = Vector3.SmoothDamp(body.velocity, inWishedDir * moveForce, ref smoothVelocity, velocitySmoother);

        if (jump && Grounded())
        {
            body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else if (!Grounded())
        {
            body.AddForce(transform.up * -1f, ForceMode.Impulse);
        }
    }

    /**
     * <summary>
     *      This method mutates the player object's velocity based on a desired direction.
     *      
     *      <remarks>
     *          The player object will be rotated in relation to the direction the camera is facing.
     *          
     *          <example>
     *              If the player object's forward vector is pointed to the left of the camera's view, and the player
     *              specifies a direction of forward (i.e. (0, 0, 1)), then the player object will be rotated to head in the
     *              direction the camera is pointing.
     *          </example>
     *      </remarks>
     *      
     *      <param name="inWishedDir">
     *          <remarks>
     *              Vector3
     *          </remarks>
     *          
     *          A normalized vector representing the desired the player wishes to direct the player object.
     *      </param>
     *      
     *      <param name="jump">
     *          <remarks>
     *              Boolean
     *          </remarks>
     *          
     *          Identifies whether to add up force to the player object.
     *      </param>
     *      
     *      <param name="crouch">
     *          <remarks>
     *              Boolean
     *          </remarks>
     *          
     *          Identifies whether the player is crouching.
     *          
     *          <remarks>
     *              The player's velocity will be restricted to a depleated max velocity while <c>crouch</c> is
     *              enabled.
     *          </remarks>
     *      </param>
     * </summary>
     */
    public virtual void MoveInThird(Vector3 inWishedDir, bool jump, bool crouch)
    {
        if (inWishedDir != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(inWishedDir.x, inWishedDir.z) * Mathf.Rad2Deg +
                                            cam.localEulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,
                                                                       ref rotationSmoothVelocity, rotationSmoother);
        }

        body.velocity = Vector3.SmoothDamp(body.velocity, transform.forward * inWishedDir.magnitude * moveForce,
                                           ref smoothVelocity, velocitySmoother);

        if (jump && Grounded())
        {
            body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else if (!Grounded())
        {
            body.AddForce(transform.up * -1f, ForceMode.Impulse);
        }
    }

    /**
     * <summary>
     *      This method checks for objects on <c>groundMask</c> within a specified radius.
     *      
     *      <returns>
     *          Boolean result of test for object within a sphere of radiuss <c>checkForGroundDist</c>.
     *      </returns>
     * </summary>
     */
    public bool Grounded()
    {
        return Physics.CheckSphere(groundCheck.position, checkForGroundDist, groundMask);
    }

    private void OnDrawGizmos()
    {
        if (showGroundChecking)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheck.position, checkForGroundDist);
        }
    }
}
