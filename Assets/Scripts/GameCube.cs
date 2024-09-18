using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCube
{
    //Dictionary, where keys are Po2 value of cube and Value itself – color
    private readonly Dictionary<int, Color> colorBasedOnPo2Value = new()
    {
        { 1, new Color32(102, 204, 204, 255) },
        { 2, new Color32(255, 255, 178, 255) },
        { 3, new Color32(204, 153, 255, 255) },
        { 4, new Color32(255, 153, 153, 255) },
        { 5, new Color32(153, 255, 153, 255) },
        { 6, new Color32(153, 204, 255, 255) },
        { 7, new Color32(192, 192, 192, 255) },
        { 8, new Color32(204, 153, 204, 255) },
        { 9, new Color32(153, 255, 255, 255) },
        { 10, new Color32(255, 204, 153, 255) }
    };
    
    //Number of created cube
    public int ID { get; private set; }

    //Po2 value of cube. Real value of cube is 2^Po2Value
    public int Po2Value { get; private set; } = 1;

    //Real value. Number, written in cube sides
    public int Value { get; private set; } = 2;

    //Same value, but string type
    public string ValueString { get; private set; }

    //Color of cube
    public Color Color { get; private set; }

    //GameObject of cube
    public GameObject CubeGameObject { get; private set; }

    //Constructor
    public GameCube(int ID, GameObject cubeGameObject, int Po2Value = 1)
    {
        this.ID = ID;
        this.CubeGameObject = cubeGameObject;
        this.Po2Value = Po2Value;
        Value = (int)Mathf.Pow(2, Po2Value);
        ValueString = Value.ToString();
        Color = colorBasedOnPo2Value[Po2Value];

        InitGameObject();
    }

    //Function, that init parameters to cube GameObject
    private void InitGameObject()
    {
        CubeGameObject.name = ID.ToString();
        CubeGameObject.GetComponent<MeshRenderer>().material.color = Color;

        foreach (TMP_Text numberOnCubeTMP in CubeGameObject.GetComponentsInChildren<TMP_Text>())
        {
            numberOnCubeTMP.text = ValueString;
        }
    }
}
