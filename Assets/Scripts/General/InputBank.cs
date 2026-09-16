using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBank : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;

    public InputAction aimCut;
    public Vector2 aimCutVector2 { get { return aimCut.ReadValue<Vector2>(); } }
    public InputAction cut;
    public InputAction cancelCut;
    public InputAction parry;
    public InputAction move;
    public Vector2 moveVector2 { get { return move.ReadValue<Vector2>(); } }

    private void Awake()
    {
        aimCut = inputActions.FindAction("Aim Cut");
        cut = inputActions.FindAction("Cut");
        cancelCut = inputActions.FindAction("Cancel Cut");
        parry = inputActions.FindAction("Parry");
        move = inputActions.FindAction("Move");
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
