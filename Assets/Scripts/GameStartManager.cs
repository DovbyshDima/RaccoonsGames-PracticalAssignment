using System.Collections;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    //Set in inspector
    [SerializeField] private GameruleManager gameruleManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Canvas canvas;

    //Start end end point for objects to move
    [SerializeField] private Transform startComposition;
    [SerializeField] private Transform StartCompositionRemovePoint;

    [SerializeField] private Transform tripodForCamera;
    [SerializeField] private Transform cameraGamePositionTransform;

    //Base parameters. canvasPlaneDistance>cameraNearClipPlane
    [SerializeField] private float cameraVelocity = 0.5f;
    [SerializeField] private float cameraNearClipPlane = 8;
    [SerializeField] private float canvasPlaneDistance = 8.5f;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    //Event Handler for click on start button
    public void ClickOnStartButton()
    {
        gameruleManager.enabled = true;
        uiManager.enabled = true;
        StartCoroutine(nameof(CameraFly));
    }

    //Animation of camera fly and startcomposition of cubes remove after click at start button
    private IEnumerator CameraFly()
    {
        while(tripodForCamera.position != cameraGamePositionTransform.position || tripodForCamera.rotation != cameraGamePositionTransform.rotation)
        {
            tripodForCamera.SetPositionAndRotation(
                Vector3.MoveTowards(tripodForCamera.position, cameraGamePositionTransform.position, Time.deltaTime * cameraVelocity), 
                Quaternion.RotateTowards(tripodForCamera.rotation, cameraGamePositionTransform.rotation, Time.deltaTime * cameraVelocity * 5
                ));
            startComposition.position = Vector3.MoveTowards(startComposition.position, StartCompositionRemovePoint.position, Time.deltaTime * cameraVelocity);
            yield return new WaitForEndOfFrame();
        }
        //Destroy Start composition
        Destroy(startComposition.gameObject);

        //Set parameters of plane for camera and canvas
        canvas.planeDistance = canvasPlaneDistance;
        tripodForCamera.GetComponentInChildren<Camera>().nearClipPlane = cameraNearClipPlane;
    }
}
