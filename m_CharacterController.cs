using UnityEngine;

/// <summary>
///     The m_CharacterController provides a basic framework for player
///     movement; this is meant to be changed at the whim of the user to achieve
///     an ideal player movement experience
///
///     Note: This class is intended to be inhereted from
///
///     Author: Num0Programmer
/// </summary>
[ RequireComponent( typeof( Rigidbody ) ) ]
public class m_CharacterController : MonoBehaviour
{
    /// <summary>
    ///      Used to define how fast the player could possible travel in a given
    ///      direction; used to determine the maximum velocity
    /// </summary>
    [SerializeField]
    protected float maximumSpeed = 20f;

    /// <summary>
    ///     Used to define how fast the player reaches maximum velocity
    /// </summary>
    [SerializeField]
    protected float acceleration = 50f;

    /// <summary>
    ///     Used to apply force to the player when they are jumping
    /// </summary>
    [SerializeField]
    protected float jumpForce;

    /// <summary>
    ///     A flag which identifies if the player is standing on an object that
    ///     is on the ground layer
    ///
    ///     Note: The name given to the "ground layer" is user defined
    ///
    ///     Note: This flag is intended to be updated with a call to the
    ///     Grounded() method every fixed update; this is simply done in the
    ///     interest of performance
    /// </summary>
    protected bool grounded;

    /// <summary>
    ///     Used for groundCheck position and radius debugging
    /// </summary>
    [SerializeField]
    private bool showGroundChecking;

    /// <summary>
    ///     Used to define the maximum vertical distance the player object can
    ///     be above the ground and still be considered on the ground
    /// </summary>
    [Range(0f, 1f)]
    [SerializeField]
    protected float checkForGroundDist;

    /// <summary>
    ///     Reference to the Rigidbody component which is appears on the player
    ///     object
    /// </summary>
    protected Rigidbody body;

    /// <summary>
    ///     Reference to an arbitrary point in space, maintained by an empty
    ///     game object which marks the center of a sphere with a radius equal
    ///     to checkForGroundDist
    /// </summary>
    [SerializeField]
    protected Transform groundCheck;

    /// <summary>
    ///     Determines what layer is "ground"
    /// </summary>
    [SerializeField]
    protected LayerMask groundMask;

    /// <summary>
    ///     Reference to an arbitrary point in space, maintainded by a Camera
    ///     object, which represents the position and rotation attributes of the
    ///     camera accompanying the player object
    /// </summary>
    [SerializeField]
    protected Transform cam;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    /// <summary>
    ///     This method applies an acceleration force to the player object
    ///     toward the defined maximum velocity
    /// </summary>
    /// 
    /// <param name="inDesiredDirection">
    ///     A normalized Vector3 which represents the direction the player wants
    ///     to move in
    /// </param>
    /// 
    /// <param name="jump">
    ///     A boolean representing if the player pressed the designated jump
    ///     button
    /// </param>
    ///
    /// <param name="crouch">
    ///     A boolean representing if the player pressed the designated crouch
    ///     button
    /// </param>
    protected virtual void Move( Vector3 inDesiredDirection, bool jump,
                                 bool crouch )
    {
        // define a Vector3 to hold the calculated velocity left to gain before
        // reaching maximum velocity
        Vector3 velocityToGain;

        // define a Vector3 to hold the found acceleration
        Vector3 neededAcceleration;

        // find velocity left to gain before reaching maximum velocity
        velocityToGain = ( inDesiredDirection * maximumSpeed ) - body.velocity;

        // find the acceleration, for this time update, to reach maximum
        // velocity
        neededAcceleration = velocityToGain / Time.fixedDeltaTime;

        neededAcceleration =
            Vector3.MoveTowards( body.velocity,
                                 neededAcceleration,
                                 acceleration / Time.fixedDeltaTime );

        // apply a force with needed acceleration in relation to this object's
        // mass
        body.AddForce( new Vector3( neededAcceleration.x,
                                    body.velocity.y,
                                    neededAcceleration.z ) * body.mass );

        // Check if player is jumping and is grounded
        if ( jump && grounded ) Jump();
    }

    /// <summary>
    ///     Simply adds an upward acceleration force to the player object
    /// </summary>
    protected virtual void Jump()
    {
        body.AddForce( transform.up * jumpForce * body.mass );
    }

    /// <summary>
    ///     Checks for objects on groundMask within a specified radius
    /// </summary>
    /// 
    /// <returns>
    ///     Boolean identifying if the player is standing on top of an object
    ///     that is on the "ground mask"
    /// </returns>
    protected bool Grounded()
    {
        return Physics.CheckSphere( groundCheck.position, checkForGroundDist,
                                    groundMask );
    }

    /// <summary>
    ///     OnDrawGizmos is called regardless; this method renders a wire
    ///     sphere, with a maximum radius of 1, to the game scene
    ///
    ///     Note: User must set showGroundChecking to true in the inspector
    ///           before the wire sphere will appear
    /// </summary>
    private void OnDrawGizmos()
    {
        try
        {
            if ( showGroundChecking )
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere( groundCheck.position,
                                       checkForGroundDist );
            }
        }
        catch( UnassignedReferenceException )
        {
            Debug.LogWarning( "Missing reference to a ground check position" );
        }
    }
}