using UnityEngine;

//Class, that represent merge 2 cubes event. Have all informaton about participants and their collision
public class CubeCollisionToMerge
{
    public int IDofFirstCube {get; private set;}
    public int IDofSecondCube {get; private set; }
    public int ValueOfFirstCube { get; private set; }
    public int ValueOfSecondCube { get; private set; }
    //Their collision
    public Collision Cube—ollision {get; private set; }
    //Score that player get for merge this cubes
    public int ScoreToGet { get; private set; }
    public string ScoreToGetString {get; private set; }

    //Constructor, that sets parameters
    public CubeCollisionToMerge(int IDofFirstCube, int IDofSecondCube, Collision collision, int ValueOfFirstCube, int ValueOfSecondCube)
    {
        this.IDofFirstCube = IDofFirstCube;
        this.IDofSecondCube = IDofSecondCube;
        this.Cube—ollision = collision;
        this.ValueOfFirstCube = ValueOfFirstCube;
        this.ValueOfSecondCube = ValueOfSecondCube;
        CalculateScore();
    }

    //Function for calculating score
    private void CalculateScore()
    {
        //Player goal. Earn scores. Each merge gives a reward Po2-value / 2,
        //so 1 score is the result of merging 2+2, 2 for 4+4, 4 for 8+8 etc.
        //c. Practical Assignment
        //
        //Maybe i dont understand something, but merge (for example) 4+4 result is 8, its 2^3,
        //so player should earn 3/2=1.5 points?. In this example formula is Po2Value-1, am I right?
        ScoreToGet = (int)Mathf.Log((float)ValueOfFirstCube * 2, 2f) - 1;
        ScoreToGetString = ScoreToGet.ToString();
    }
}
