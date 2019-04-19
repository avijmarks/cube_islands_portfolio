using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoundaryManager : MonoBehaviour {

    //boundary parent references for USE FOR position/rotations
    public GameObject boundaryI;
    public GameObject boundaryII;

    //boundary references for scale info etc -- not used yet (except halfboundarylength)
    public GameObject boundary1;
    public GameObject boundary2;

    //should be the same as y scale of actual straight boundary mesh
    float halfBoundaryLength = 2000f;

    public BoundaryDimensions dimensionsCurrent = new BoundaryDimensions();
    public BoundaryDimensions dimensionsNext = new BoundaryDimensions();

    //pool
    public Queue<GameObject> pool = new Queue<GameObject>(); 

    public struct BoundaryDimensions
    {
        public Vector3 Start;
        public Vector3 End;
        public GameObject Boundary;
    }


    void Start() {
        StartState();
    }


    void StartState() {
        //setting boundary II in pool
        pool.Enqueue(boundaryII);

        //setting first boundary position
        boundaryI.transform.position = new Vector3(0f, 0f, halfBoundaryLength);


        //getting start and end points relative to position/rotation of boundary
        //also done when setting new boundary without this in-depth annotation
        //because of scale of boundaries themselves, we need parent empties on boundaries -- and thus may need references to actual objects themselves
        Vector3 start = new Vector3(0f, 0f, -halfBoundaryLength);
        Vector3 end = new Vector3(0f, 0f, halfBoundaryLength);
        dimensionsCurrent.Start = boundaryI.transform.TransformPoint(start);
        dimensionsCurrent.End = boundaryI.transform.TransformPoint(end);
        dimensionsCurrent.Boundary = boundaryI;


        //activating and queueing boundaryI
        boundaryI.SetActive(true);
        pool.Enqueue(boundaryI);
    }


    public void NewBoundary() {
        //THIS IS FOR STRAIGHT BOUNDARIES ONLY, WILL NEED SECOND METHOD THAT IS CHOSEN IF NEW TILE IS A TURN TILE -- WILL ALSO NEED DECISION PROCESS FOR WHICH TYPE OF BOUNDARY
     
        Vector3 nextPosition;
        Quaternion nextRotation;
        nextPosition = dimensionsCurrent.Boundary.transform.TransformPoint(0, 0, (2 * halfBoundaryLength));
        nextRotation = dimensionsCurrent.Boundary.transform.rotation;


        GameObject nextBoundary = pool.Dequeue();
        nextBoundary.transform.position = nextPosition;
        nextBoundary.transform.rotation = nextRotation;
        pool.Enqueue(nextBoundary);

        Debug.Log(nextBoundary);
        Vector3 start = new Vector3(0f, 0f, -halfBoundaryLength);
        Vector3 end = new Vector3(0f, 0f, halfBoundaryLength);
        dimensionsNext.Boundary = nextBoundary;
        dimensionsNext.Start = nextBoundary.transform.TransformPoint(start);
        dimensionsNext.End = nextBoundary.transform.TransformPoint(end);
    }


    public void PlayerInNextBoundary() {
        dimensionsCurrent = dimensionsNext;
    }



}
