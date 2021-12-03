using UnityEngine;

/// <summary>
///     Provides a very basic interactable implementation; it is up to the user
///     to add functionality based on their needs
///
///     Authors: Num0Programmer
/// </summary>
public class InteractionManager : MonoBehaviour
{
    /// <summary>
    ///     Used to define the maximum distance the player can be away from an
    ///     object and still interact with it
    /// </summary>
    [SerializeField]
    private float reach;

    /// <summary>
    ///     Used to identify when the user wants to interact with an object
    /// </summary>
    private bool interact;

    /// <summary>
    ///     Reference to an arbitrary point in space, maintained by the Camera
    ///     which accompanies the player
    /// </summary>
    [SerializeField]
    private Transform eyesPosition;

    /// <summary>
    ///     Used to define the "interactable layer"
    ///
    ///     Note: Interactable layer is defined by the user in the Unity Editor
    /// </summary>
    [SerializeField]
    private LayerMask interactableMask;

    private void Update()
    {
        GetInput();

        if ( interact ) Interact();
    }

    /// <summary>
    ///     Checks for any incoming input from the user
    /// </summary>
    private void GetInput()
    {
        interact = Input.GetKeyDown( KeyCode.E );
    }

    /// <summary>
    ///     Tests if anything that the player is looking at is an interactable
    ///     object; proceeds to carry out initial handshake between player and
    ///     the found object
    /// </summary>
    public void Interact()
    {
        RaycastHit outHit;

        Physics.Raycast( eyesPosition.position, eyesPosition.forward,
                         out outHit, reach, interactableMask );

        try
        {
            Debug.Log("Interacting with " + outHit.transform.name);
        }
        catch ( System.NullReferenceException )
        {
            // Left blank on purpose so console does not get over loaded with logs

            // If not getting a reading from the object attempting to interact
            // with, it is prudent to ensure you have your objects on the
            // interactable layer and that interactableMask is set to the
            // interactable layer, then it would be time to explore other
            // options
        }
    }
}