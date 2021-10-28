using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{

    Node rootTile;
    public PlayerController player;
    bool generated = false;
    [SerializeField] private static int OFFSET = 20;
    

    //Initialization constructor
    public TileGenerator()
    {
        //instantiate root tile to null
        rootTile = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        //instantiate new node for root tile
        startingTiles();
        Instantiate(rootTile.tile, new Vector3(rootTile.xPos, 0, rootTile.zPos), Quaternion.identity);
        Instantiate(rootTile.right.tile, new Vector3(rootTile.right.xPos, 0, rootTile.right.zPos), Quaternion.identity);
        Instantiate(rootTile.left.tile, new Vector3(rootTile.left.xPos, 0, rootTile.left.zPos), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //check if root tile points to tile that player is on
        //utilizes a function within player contoller that does the raycasting
        if (player.notOnRootTile())                          //this was just to test to see if i can make a new one on the right side         
        {
            rootTile.left = rootTile;
            rootTile = rootTile.right;
            rootTile.right = new Node(rootTile.xPos + OFFSET, 0, "Tile");

            Instantiate(rootTile.right.tile, new Vector3(rootTile.right.xPos, 0, rootTile.right.zPos), Quaternion.identity);
            setNewRootRef(); 
        }
        
    }

    public Node getRootRef()
    {
        //return if this ref is equal to root tile

        return rootTile;
        //return null;
    }

    public void setNewRootRef()
    {
        //check for if root ref moved
        if (getRootRef().tile.tag == "Tile")
        {
            //set new ref's tag back to root
            getRootRef().tile.tag = "root";

            rootTile.left.tile.tag = "Tile";
            rootTile.right.tile.tag = "Tile";
        }
    }
    
    public void startingTiles()
    {
        rootTile = new Node(0, 0, "root");

        rootTile.right = new Node(OFFSET, 0, "Tile");
        rootTile.left = new Node(-OFFSET, 0, "Tile");
    }
    
}
