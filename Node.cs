using UnityEngine;

public class Node : MonoBehaviour
{
    //refs to adjacent nodes
    public Node top;
    public Node bottom;
    public Node right;
    public Node left;
    public GameObject tile;

    public float xPos, zPos;

    //Initialization constructor
    public Node(float xPos, float zPos, string tag)
    {
        //instantiate new tile with given x and z pos
        this.xPos = xPos;
        this.zPos = zPos;

        //check for if this current GameObject tile wants to be root
        if(tag == "root")
        {
            tile = GameObject.FindWithTag("root");
        }

        //otherwise, assume tile is not root
        else
        {
            tile = GameObject.FindWithTag("Tile");
        }
    }

    void Update()
    {
        
    }

    public Node genNewTile(float xPos, float zPos)
    {
        /*
        //check for this is equal to root
        if(this == TileGenerator.getRootRef())
        {
            top = genNewTile(xPos, zPos + transform.localScale.x * 2);

            bottom = genNewTile(xPos, zPos - transform.localScale.x * 2);

            left = genNewTile(xPos - transform.localScale.x * 2, zPos);

            right = genNewTile(xPos + transform.localScale.x * 2, zPos);
        }
        

        */
        return this;
    }
    
}
