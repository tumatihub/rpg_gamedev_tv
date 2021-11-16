using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mover))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    Mover mover;

    private void Awake()
    {
        mover = GetComponent<Mover>();    
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
    }

    void MoveToCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            mover.MoveTo(hit.point);
        }
    }
}
