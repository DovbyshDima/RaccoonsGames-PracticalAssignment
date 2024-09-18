using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFactory : MonoBehaviour
{
    //Set in inspector
    [SerializeField] private GameObject CubeSample;
    [SerializeField] private Transform StorageForNewCubes;

    //Number of created cube
    private int IDofCurrentCube = 0;

    //Start position for new cube to Dragand Throw
    private Vector3 cubeStartPosition = new(0, 0.5f, -5);

    //Create a new cube to drag
    public GameCube CreateNewCube()
    {
        IDofCurrentCube++;

        GameCube newCube;
        newCube = new(IDofCurrentCube, Instantiate(CubeSample, cubeStartPosition, Quaternion.identity, StorageForNewCubes), UnityEngine.Random.Range(0, 3) == 0 ? 2 : 1);

        //Enable script on cube to prevent it start on sample object
        newCube.CubeGameObject.GetComponent<CubeCollisionDetector>().enabled = true;

        return newCube;
    }

    //Create new cube with certain position, rotation and value
    public GameCube CreateNewCube(Vector3 position, Quaternion quaternion, int po2Value)
    {
        IDofCurrentCube++;

        GameCube newCube;
        newCube = new(IDofCurrentCube, Instantiate(CubeSample, position, quaternion, StorageForNewCubes), po2Value);

        //Enable script on cube to prevent it start on sample object
        newCube.CubeGameObject.GetComponent<CubeCollisionDetector>().enabled = true;

        return newCube;
    }

    public void ClearFactory()
    {
        IDofCurrentCube = 0;
    }
}
