using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool onground;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Slider speedSlider;
    private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Inicializa los controles
        controls = new PlayerControls();

        // Se suscribe a las acciones
        controls.Basic.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Basic.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Basic.Jump.performed += ctx => Jump();
    }

    // Habilita los controles cuando se active
    private void OnEnable()
    {
        controls.Basic.Enable();
    }

    // Deshabilita los controles cuando se desactive
    private void OnDisable()
    {
        controls.Basic.Disable();
    }

    // Setea la velocidad del jugador con la del slider del menu de pausa
    private void Update() {
        moveSpeed = speedSlider.value;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Funcion de salto
    private void Jump()
    {
        if (onground)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Comprueba que el cubo este tocando el suelo y si lo esta, setea la valirable "onground" a true
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onground = true;
        }
    }

    // Comprueba que el cubo este tocando el suelo y si no lo esta, setea la valirable "onground" a false
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onground = false;
        }
    }

    // Funcion que reinicia la posicion del jugador a la original, esta funcion se llama desde el menu de pausa tocando el portal
    public void ResetPosition()
    {
        rb.position = new Vector3(0, 1, 0);
    }
}