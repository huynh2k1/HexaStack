using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexaStack : MonoBehaviour
{
    public List<Hexagon> Hexagons { get; private set; }
    public Vector3 GetPos {  get => transform.position;}
    public void Add(Hexagon hexa)
    {
        if(Hexagons == null)
            Hexagons = new List<Hexagon>();
        Hexagons.Add(hexa);
        hexa.SetParent(transform);
    }

    public void Place()
    {
        foreach (Hexagon hexagon in Hexagons)
            hexagon.DisableCollider();
    }

    public Color GetTopHexagonColor()
    {
        return Hexagons[Hexagons.Count - 1].Color; 
        //return Hexagons.Last().Color; //Linq
        //return Hexagons[^1].Color; 
    }

    public bool Contains(Hexagon hexagon)
    {
        return Hexagons.Contains(hexagon);
    }

    public void Remove(Hexagon hexagon)
    {
        Hexagons.Remove(hexagon);
        if(Hexagons.Count <= 0)
            DestroyImmediate(gameObject);
    }
}
