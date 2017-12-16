using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Interactable focus;

    public LayerMask movementMask;

    Camera cam;
	PlayerMotor motor;

    void Start()
    {
        cam = Camera.main;
		motor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        // If we release left mouse
        if(Input.GetMouseButtonUp(0)){
            
        }
        // If we press left mouse
        if (Input.GetMouseButton(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, movementMask))
            {
                motor.MoveToPoint(hit.point);
                RemoveFocus();
            }
        }
        // If we press right mouse
		if (Input.GetMouseButtonDown(1))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                var interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null){
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus(Interactable newFocus){
        if(newFocus != focus){
            if(focus != null)
                focus.OnDefocused();
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus(){
        if(focus != null)
            focus.OnDefocused();
        focus = null;
        motor.StopFollowingTarget();
    }
}
