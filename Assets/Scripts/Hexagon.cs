using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private new Renderer renderer;
    public HexaStack HexStack { get; private set; }
    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value; 
    }

    //Set hexa hiện tại cho 1 cái stack chứa nó
    public void Configure(HexaStack hexStack)
    {
        HexStack = hexStack;
    }
}
