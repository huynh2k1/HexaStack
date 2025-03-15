using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private LayerMask hexagonLayerMask; //Layer của các hexagon
    [SerializeField] private LayerMask gridHexagonLayerMask; //Layer của grid hexagon
    [SerializeField] private LayerMask groundLayerMask; //Layer của ground grid hexagon

    [SerializeField] private HexaStack currentStack;
    private Vector3 currentStackInitPos;

    [Header("Somethings")]
    [SerializeField] HexaGrid targetGridCell;


    public static Action<HexaGrid> OnStackPlace;

    private void Update()
    {
        ControlInput();
    }

    void ControlInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ControlMouseDown();
        }
        else if (Input.GetMouseButton(0) && currentStack != null)
        {
            ControlMouseDrag();
        }

        else if (Input.GetMouseButtonUp(0) && currentStack != null)
        {
            ControlMouseUp();
        }
    }

    void ControlMouseDown()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, hexagonLayerMask);

        if(hit.collider == null)
        {
            return;
        }

        currentStack = hit.collider.GetComponent<Hexagon>().HexStack;
        currentStackInitPos = currentStack.GetPos;
    }

    void ControlMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayerMask);

        if(hit.collider == null)
        {
            DraggingAboveGround();
        }else
        {
            DraggingAboveGrid(hit);
        }
    }


    void DraggingAboveGround()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, groundLayerMask);

        if(hit.collider == null)
        {
            Debug.LogError("No ground detected, this is unusual");
            return;
        }

        Vector3 currentStackTargetPos = hit.point.With(y: 2);
        currentStack.transform.position = Vector3.MoveTowards(
            currentStack.transform.position,
            currentStackTargetPos,
            Time.deltaTime * 30);

        targetGridCell = null;
    }

    void DraggingAboveGrid(RaycastHit hit)
    {
        HexaGrid hexaGrid = hit.collider.GetComponent<HexaGrid>();

        //Nếu chưa bị chiếm đóng
        if(hexaGrid.IsOccupied)
        {
            DraggingAboveGround();
        }
        else
        {
            DraggingAboveNoneOccupiedGridCell(hexaGrid);
        }
    }

    void DraggingAboveNoneOccupiedGridCell(HexaGrid newHexaGrid)
    {
        Vector3 currentStackTargetPos = newHexaGrid.transform.position.With(y: 2);
        currentStack.transform.position = Vector3.MoveTowards(
            currentStack.transform.position,
            currentStackTargetPos,
            Time.deltaTime * 30);

        targetGridCell = newHexaGrid;
    }
    void ControlMouseUp()
    {
        //Nếu k đặt được stack thì sẽ back về vị trí ban đầu
        if(targetGridCell == null)
        {
            currentStack.transform.position = currentStackInitPos;
            currentStack = null;
            return;
        }

        //ngược lại thì đặt stack lên grid
        currentStack.transform.position = targetGridCell.GetPos.With(y : 0.1f);
        currentStack.transform.SetParent(targetGridCell.transform);
        currentStack.Place();

        targetGridCell.AssignStack(currentStack);

        OnStackPlace?.Invoke(targetGridCell);

        currentStack = null;
        targetGridCell = null;
    }

    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
