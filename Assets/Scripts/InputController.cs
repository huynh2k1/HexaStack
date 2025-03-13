using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private LayerMask hexagonLayerMask; //Layer của các hexagon
    [SerializeField] private LayerMask gridHexagonLayerMask; //Layer của grid hexagon

    [SerializeField] private HexaStack currentStack;
    private Vector3 currentStackInitPos;
    private void Start()
    {
        
    }

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

        else if (Input.GetMouseButtonUp(0))
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
            Debug.Log("We have not detected any hexagon");
            return;
        }

        currentStack = hit.collider.GetComponent<Hexagon>().HexStack;
        currentStackInitPos = currentStack.GetPos;
    }

    void ControlMouseDrag()
    {
        RaycastHit hit;
        Physics.Raycast(GetClickedRay(), out hit, 500, gridHexagonLayerMask);

    }

    void ControlMouseUp()
    {

    }

    private Ray GetClickedRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
}
