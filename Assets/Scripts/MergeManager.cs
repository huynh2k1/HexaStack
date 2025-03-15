using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{

    [SerializeField] GridGenerate gridCtrl;
    List<HexaGrid> updateCells = new List<HexaGrid>();

    private void Awake()
    {
        InputController.OnStackPlace += StackPlacedCallBack;
    }

    private void OnDestroy()
    {
        InputController.OnStackPlace -= StackPlacedCallBack;
    }

    void StackPlacedCallBack(HexaGrid cellPlaced)
    {
        StartCoroutine(StackPlacedCoroutine(cellPlaced));
    }

    IEnumerator StackPlacedCoroutine(HexaGrid cellPlaced)
    {
        updateCells.Add(cellPlaced);
        while(updateCells.Count > 0)
            yield return CheckForMerge(updateCells[0]);

    }

    IEnumerator CheckForMerge(HexaGrid cellPlaced)
    {
        updateCells.Remove(cellPlaced);

        if (!cellPlaced.IsOccupied)
            yield break;

        //Kiểm tra các stack lân cận ?
        List<HexaGrid> neightbors = gridCtrl.GetListHexaGridNeightbor(cellPlaced);
        if (neightbors.Count <= 0)
            yield break;

        //Lấy ra màu của hexa top
        Color topColor = cellPlaced.Stack.GetTopHexagonColor();

        //Kiểm tra phần tử trên cùng của các stack lân cận có cùng màu ?
        List<HexaGrid> similarNeightborTopColor = GetSimilarNeightborHexagrid(topColor, neightbors);
        if (similarNeightborTopColor.Count <= 0)
            yield break;

        updateCells.AddRange(similarNeightborTopColor);

        //Lấy danh sách các hexagon có thể add
        List<Hexagon> hexagonsToAdd = GetHexagonsToAdd(topColor, similarNeightborTopColor);

        //Remove the hexagons from their stacks
        RemoveHexagonsFromTheirStacks(similarNeightborTopColor, hexagonsToAdd.ToArray());

        //Move Hexagons To Cur Stack
        MoveHexagons(cellPlaced, hexagonsToAdd, 0.1f);
        yield return new WaitForSeconds(0.3f + hexagonsToAdd.Count * 0.01f);
        //Merge
        yield return CheckForCompleteStack(cellPlaced, topColor);

    }

    List<HexaGrid> GetSimilarNeightborHexagrid(Color topColor, List<HexaGrid> neightbors)
    {
        List<HexaGrid> similarNeightborTopColor = new List<HexaGrid>();

        foreach (HexaGrid hexa in neightbors)
        {
            Color neightborHexaGridTopHexagonColor = hexa.Stack.GetTopHexagonColor();

            if (neightborHexaGridTopHexagonColor == topColor)
                similarNeightborTopColor.Add(hexa);
        }
        return similarNeightborTopColor;
    }

    List<Hexagon> GetHexagonsToAdd(Color topColor, List<HexaGrid> similarNeightborTopColor)
    {
        List<Hexagon> hexagonsToAdd = new List<Hexagon>();
        foreach (HexaGrid neightbor in similarNeightborTopColor)
        {
            HexaStack hexaStack = neightbor.Stack;
            for (int i = hexaStack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = hexaStack.Hexagons[i];
                if (hexagon.Color != topColor)
                    break;

                hexagonsToAdd.Add(hexagon);
                hexagon.SetParent(null);
            }
        }
        return hexagonsToAdd;
    }

    void RemoveHexagonsFromTheirStacks(List<HexaGrid> similarNeightborTopColor, Hexagon[] hexagonsToAdd)
    {
        foreach (HexaGrid neightbor in similarNeightborTopColor)
        {
            HexaStack stack = neightbor.Stack;

            foreach (Hexagon hexagon in hexagonsToAdd)
            {
                if (stack.Contains(hexagon))
                {
                    stack.Remove(hexagon);
                }
            }
        }
    }

    void MoveHexagons(HexaGrid cellPlaced, List<Hexagon> hexagonsToAdd, float heightHexa = 0.1f)
    {
        //Tính vị trí y của stack placed
        //0.2f: là chiều cao của 1 hexagon
        float initY = cellPlaced.Stack.Hexagons.Count * heightHexa;

        for (int i = 0; i < hexagonsToAdd.Count; i++)
        {
            Hexagon hexagon = hexagonsToAdd[i];

            float targetY = initY + i * heightHexa;
            Vector3 targetLocalPos = Vector3.up * targetY;

            cellPlaced.Stack.Add(hexagon);
            hexagon.MoveToLocal(targetLocalPos);
        }
    }

    IEnumerator CheckForCompleteStack(HexaGrid cellPlaced, Color topColor)
    {
        if (cellPlaced.Stack.Hexagons.Count < 10)
            yield break;

        List<Hexagon> similarHexagons = new List<Hexagon>();

        for (int i = cellPlaced.Stack.Hexagons.Count - 1; i >= 0; i--)
        {
            Hexagon hexagon = cellPlaced.Stack.Hexagons[i];

            if (hexagon.Color != topColor)
                break;

            similarHexagons.Add(hexagon);
        }
        int similarHexagonCount = similarHexagons.Count;
        if (similarHexagons.Count < 10)
            yield break;

        float delay = 0;

        while(similarHexagons.Count > 0)
        {
            similarHexagons[0].SetParent(null);
            similarHexagons[0].Vanish(delay);

            delay += 0.01f;

            //DestroyImmediate(similarHexagons[0].gameObject);
            cellPlaced.Stack.Remove(similarHexagons[0]);
            similarHexagons.RemoveAt(0);
        }

        updateCells.Add(cellPlaced);

        yield return new WaitForSeconds(0.3f + similarHexagonCount * 0.01f);
    }
}
