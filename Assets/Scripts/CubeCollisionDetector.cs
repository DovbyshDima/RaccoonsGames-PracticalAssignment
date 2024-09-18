using UnityEngine;

public class CubeCollisionDetector : MonoBehaviour
{
    //Set in inspector on CubeSample
    [SerializeField] private GameruleManager gameruleManager;
    [SerializeField] private MergeCubesEventManager mergeCubesEventManager;

    //Const impulse, which need to merge 2 cubes
    private const float impulseToMerge = 0.5f;
    
    //Event handler of collision 2 cubes
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Cube"))
        {
            int IDofFirstCube = int.Parse(name);
            int IDofSecondCube = int.Parse(collision.transform.name);
            //Collision of cubes has 2 partisipant, each try to invoke this function,
            //so i avoid it with this condition, because only 1 of them has bigger id
            if (IDofFirstCube > IDofSecondCube)
            {
                //This condition used for detect player loss. If this is true, game ends and death screen invoke
                if (gameruleManager.AllCreatedCubes[IDofFirstCube].Equals(gameruleManager.CurrentCube) ||
                   gameruleManager.AllCreatedCubes[IDofSecondCube].Equals(gameruleManager.CurrentCube))
                {
                    gameruleManager.EndGame();
                }
                //Cubes allow to merge only if they hame same value
                else if (gameruleManager.AllCreatedCubes[IDofFirstCube].Po2Value == gameruleManager.AllCreatedCubes[IDofSecondCube].Po2Value)
                {
                    //Cubes allow to merge only if impulse is enough, this value is const on top of script
                    if (collision.impulse.magnitude >= impulseToMerge)
                    {
                        mergeCubesEventManager.NotifyObservesOfMerge(new CubeCollisionToMerge(IDofFirstCube, IDofSecondCube, collision, gameruleManager.AllCreatedCubes[IDofFirstCube].Value, gameruleManager.AllCreatedCubes[IDofSecondCube].Value));
                    }
                }
            }
        }
    }
}
