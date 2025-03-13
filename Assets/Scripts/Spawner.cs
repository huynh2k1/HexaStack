using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("ELEMENTS")]
    public Transform stackPositionsParent;
    public Hexagon hexaPrefab;
    public HexaStack hexaStackPrefab;

    [Header("SETTINGS")]
    [SerializeField] private Color[] colors;
    [SerializeField] private int stackCounter;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        InputController.OnStackPlace += StackPlaceCallback;
    }

    private void OnDestroy()
    {
        InputController.OnStackPlace -= StackPlaceCallback;
    }

    private void StackPlaceCallback(HexaGrid gridCell)
    {
        stackCounter++;
        if(stackCounter >= 3)
        {
            stackCounter = 0;
            GenerateStacks();
        }
    }

    private void Start()
    {
        GenerateStacks();
    }

    void GenerateStacks()
    {
        for(int i = 0; i < stackPositionsParent.childCount; i++)
        {
            GenerateStack(stackPositionsParent.GetChild(i));
        }
    }

    void GenerateStack(Transform parent)
    {
        HexaStack hexaStack = Instantiate(hexaStackPrefab, parent.position, Quaternion.identity, parent);
        hexaStack.name = $"Stack {parent.GetSiblingIndex() }";


        int amount = Random.Range(2, 8); //return 2 -> 7
        int firstColorHexagonCount = Random.Range(0, amount); //return 0, 6
        Color[] colorArray = GetRandomColors();
        for(int i = 0; i < amount; i++)
        {
            Vector3 hexaLocalPos = Vector3.up * i * 0.15f;
            Vector3 spawnPos = hexaStack.transform.TransformPoint(hexaLocalPos);
            Hexagon hexa = Instantiate(hexaPrefab, spawnPos, Quaternion.identity, hexaStack.transform);
            hexa.Color = (i < firstColorHexagonCount) ? colorArray[0] : colorArray[1];
            hexaStack.Add(hexa);
            hexa.Configure(hexaStack);
        }
    }

    Color[] GetRandomColors()
    {
        List<Color> colorList = new List<Color>();
        colorList.AddRange(colors);

        if(colorList.Count <= 0)
        {
            Debug.LogError("No color found");
            return null;
        }

        Color firstColor = colorList.OrderBy(x => Random.value).First();
        colorList.Remove(firstColor);

        if(colorList.Count <= 0)
        {
            Debug.LogError("Only one color was found");
            return null;
        }

        Color secondColor = colorList.OrderBy(x => Random.value).First();

        return new Color[] { firstColor, secondColor }; 
    }
}
