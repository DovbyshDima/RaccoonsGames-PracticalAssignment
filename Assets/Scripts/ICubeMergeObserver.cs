//Every script, which want to observe merge of 2 cubes,
//should realise this interface and subscribe to event
public interface ICubeMergeObserver
{
    //Function to subscribe
    void SubscribeToCubesMerge();
    //Function, that launched by merge event
    void OnCubeMerge(CubeCollisionToMerge cubeMerge);
}
