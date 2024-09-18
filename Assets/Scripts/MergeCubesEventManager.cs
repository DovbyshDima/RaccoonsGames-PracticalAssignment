using System.Collections.Generic;
using UnityEngine;

//Merge 2 cubes event manager class
public class MergeCubesEventManager : MonoBehaviour
{
    //List of all observers to merge event
    private List<ICubeMergeObserver> observersOfMerge = new();

    //Add a new observer, it must realise ICubeMergeObresver interface
    public void AddObserver(ICubeMergeObserver newObserver)
    {
        observersOfMerge.Add(newObserver);
    }

    //Remove observer
    public void RemoveObserver(ICubeMergeObserver oldObserver)
    {
        observersOfMerge.Remove(oldObserver);
    }

    //Event of merge. When it happen, this must be called
    public void NotifyObservesOfMerge(CubeCollisionToMerge cubeCollisionToMerge)
    {
        foreach (ICubeMergeObserver observer in observersOfMerge)
        {
            observer.OnCubeMerge(cubeCollisionToMerge);
        }
    }

    //Clear list of observers
    public void ClearListOfObservers()
    {
        observersOfMerge.Clear();
    }
}
