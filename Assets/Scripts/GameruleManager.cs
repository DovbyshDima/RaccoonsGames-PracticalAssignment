using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameruleManager : MonoBehaviour, ICubeMergeObserver
{
    //Set in inspector
    [SerializeField] private GameObject CubeSample;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CubeFactory cubeFactory;
    [SerializeField] private MergeCubesEventManager mergeCubesEventManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject cubeStorage;

    

    //Object of current cube (cube that we drag and (in the future throw). After throw replaced with a new one)
    public GameCube CurrentCube { get; private set; }

    //Base vector parameters
    private Vector3 forceToCube = new(0, 0, 1200);

    //Bool for block controling of already used cube
    private bool isSent;

    //List of all created cubes
    public List<GameCube> AllCreatedCubes { get; private set; } = new();

    // Start is called before the first frame update
    public void Start()
    {
        //To synchronize cube id and it id in list. To avoid counting from zero
        AllCreatedCubes.Add(null);
        //Add this script as observer of cube merge event
        SubscribeToCubesMerge();
        //Create a new cube ready to drag
        CreateNewCube();
    }

    //Subscribe to event cubes merge
    public void SubscribeToCubesMerge()
    {
        mergeCubesEventManager.AddObserver(this);
    }

    //Function, that create a new cube, ready to drag and throw
    private GameCube CreateNewCube()
    {
        GameCube newCube = cubeFactory.CreateNewCube();

        AllCreatedCubes.Add(newCube);

        CurrentCube = newCube;

        return newCube;
    }

    //Overloaded metod to create cube not to drag, in exact position and rotation
    private GameCube CreateNewCube(Vector3 position, Quaternion quaternion, int po2Value)
    {
        GameCube newCube = cubeFactory.CreateNewCube(position, quaternion, po2Value);

        AllCreatedCubes.Add(newCube);

        return newCube;
    }

    //Function, that control new cube dragging
    //Version for UnityEditor and Android
#if UNITY_EDITOR
    public void Drug()
    {
        if(!isSent)
        {
            Vector3 currentPos = CurrentCube.CubeGameObject.transform.position;
            Vector3 newPos = new(Input.mousePosition.x, 0, Camera.main.nearClipPlane);
            CurrentCube.CubeGameObject.transform.position = new(Camera.main.ScreenToWorldPoint(newPos).x, currentPos.y, currentPos.z);
        }
    }

#elif PLATFORM_ANDROID
    public void Drug()
    {
        if (!isSent)
        {
            Vector3 currentPos = CurrentCube.CubeGameObject.transform.position;
            Vector3 newPos = new(Input.touches[0].position.x, 0, Camera.main.nearClipPlane);
            CurrentCube.CubeGameObject.transform.position = new(Camera.main.ScreenToWorldPoint(newPos).x, currentPos.y, currentPos.z);
        }
    }
#endif

    //Function, that throw cube after stop dragging
    public void PointerUp()
    {
        if(!isSent)
        {
            isSent = true;

            Rigidbody rigidBody = CurrentCube.CubeGameObject.GetComponent<Rigidbody>();

            //Unlock thrown cube move and add force to it
            EnablePhysicForCreatedCube(CurrentCube.CubeGameObject);

            rigidBody.AddForce(forceToCube);

            CurrentCube = null;

            StartCoroutine(nameof(WaitForOldCube));
        }
    }
    
    //Timer to stop control of cube, that already thrown and prevent new cube to spawn to early
    private IEnumerator WaitForOldCube()
    {
        yield return new WaitForSeconds(1);
        CreateNewCube();
        isSent = false;
    }

    //Invoke when 2 cubes merge, part of Interface ICubeMergeObserver
    public void OnCubeMerge(CubeCollisionToMerge cubeCollision)
    {
        Merge2Cubes(cubeCollision.IDofFirstCube, cubeCollision.IDofSecondCube, cubeCollision.Cube—ollision.impulse);
    }

    //Merge 2 cubes together
    private void Merge2Cubes(int IDofFirstCube, int IDofSecondCube, Vector3 impulse)
    {
        impulse.Set(impulse.x, impulse.y, Mathf.Abs(impulse.z));
        GameObject firstCube = AllCreatedCubes[IDofFirstCube].CubeGameObject;
        GameObject secondCube = AllCreatedCubes[IDofSecondCube].CubeGameObject;

        Vector3 newPosition = Vector3.Slerp(firstCube.transform.position, secondCube.transform.position, 0.5f);
        Quaternion newQuaternion = Quaternion.Slerp(firstCube.transform.rotation, secondCube.transform.rotation, 0.5f);

        Destroy(AllCreatedCubes[IDofFirstCube].CubeGameObject);
        Destroy(AllCreatedCubes[IDofSecondCube].CubeGameObject);

        GameObject mergedCube = CreateNewCube(newPosition, newQuaternion, AllCreatedCubes[IDofFirstCube].Po2Value + 1).CubeGameObject;

        EnablePhysicForCreatedCube(mergedCube);

        mergedCube.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);
    }

    //Unlock cube physic
    private void EnablePhysicForCreatedCube(GameObject cube)
    {
        Rigidbody rigidbody = cube.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    //Bool, that prevent "death" event invoking twice
    private bool isAlreadyOver = false;

    //Function for end game, Start ui animation
    public void EndGame()
    {
        if(!isAlreadyOver)
        {
            isAlreadyOver = true;
            mergeCubesEventManager.RemoveObserver(this);
            uiManager.EndGame();
        }
    }

    //Restart all processes in game to start another one
    public void RestartGame()
    {
        AllCreatedCubes.Clear();
        cubeFactory.ClearFactory();
        foreach (Rigidbody cube in cubeStorage.GetComponentsInChildren<Rigidbody>())
        {
            Destroy(cube.gameObject);
        }
        Start();
        isAlreadyOver = false;
    }
}
