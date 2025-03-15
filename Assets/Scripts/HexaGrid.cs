using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaGrid : MonoBehaviour
{
    public HexaStack Stack { get; private set; }
    public int PosX { get; set; }
    public int PosY { get; set; }

    public Vector3 GetPos => transform.position;

    [Header("Elements")]
    [SerializeField] private new Renderer renderer;

    public bool IsOccupied
    {
        get => Stack != null; //trả về cái hexagrid này có đang bị cái stack nào chiếm hay k 
        private set { }
    }


    public void AssignStack(HexaStack stack)
    {
        Stack = stack;
    }
}
