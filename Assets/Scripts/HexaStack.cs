using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexaStack : MonoBehaviour
{
    public List<Hexagon> Hexagons { get; private set; }
    public Vector3 GetPos {  get => transform.position;}
    public void Add(Hexagon hexa)
    {
        if(Hexagons == null)
            Hexagons = new List<Hexagon>();
        Hexagons.Add(hexa);
    }
}
