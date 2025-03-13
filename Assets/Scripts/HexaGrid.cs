using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaGrid : MonoBehaviour
{
    private HexaStack stack;

    public Vector3 GetPos { get => transform.position; }

    [Header("Elements")]
    [SerializeField] private new Renderer renderer;

    public bool IsOccupied
    {
        get => stack != null; //trả về cái hexagrid này có đang bị cái stack nào chiếm hay k 
        private set { }
    }


    public void AssignStack(HexaStack stack)
    {
        this.stack = stack;
    }
}
