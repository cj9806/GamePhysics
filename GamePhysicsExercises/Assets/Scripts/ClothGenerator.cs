using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothGenerator : MonoBehaviour
{
    public float clothY;
    [SerializeField] int clothHeigth;
    [SerializeField] int clothWidth;

    private GameObject[,] clothGrid;
    // Start is called before the first frame update
    void Start()
    {
        clothGrid = new GameObject[clothWidth, clothHeigth];
        //generate nodes;
        for (int i = 0; i < clothWidth; i++)
        {
            for (int j = 0; j < clothHeigth; j++)
            {
                //generate a new gamobject
                GameObject node = new GameObject();
                //name it to make it easier to keep track of
                node.name = $"Node:{i + 1},{j + 1}";
                //set the transform of the node according to adress in array
                node.transform.position = new Vector3(i, clothY, j);
                //create the collider
                SphereCollider collider = node.AddComponent<SphereCollider>();
                collider.radius = .05f;
                //gice it a rigid body
                Rigidbody rigidbody = node.AddComponent<Rigidbody>();
                //put the node onto the cloth grid
                clothGrid[i, j] = node;
                //if(clothGrid[i-1] < 0)
            }
        }
        //generate springs
        for (int i = 0; i < clothWidth; i++)
        {
            for (int j = 0; j < clothHeigth; j++)
            {
                //create spring
                //look right
                if (i+1 < clothWidth)
                {
                    SpringJoint springOne = clothGrid[i, j].AddComponent<SpringJoint>();
                    springOne.connectedBody = clothGrid[i + 1, j].GetComponent<Rigidbody>();
                }
                //look down
                if (j + 1 < clothHeigth)
                {
                    SpringJoint springTwo = clothGrid[i, j].AddComponent<SpringJoint>();
                    springTwo.connectedBody = clothGrid[i, j + 1].GetComponent<Rigidbody>();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        
    }
    bool isInBounds(int desiredAdress, GameObject[] Array)
    {
        if (desiredAdress < 0 || desiredAdress > Array.Length) return false;
        else return true;
    }
}
/*square grid
 * 0-0-0->x
 * | | |
 * 0-0-0
 * | | |
 * 0-0-0
*/
