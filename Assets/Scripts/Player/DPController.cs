// DPController.cs
// Location in Unity project: Assets/Scripts/Player/DPController.cs
//
// MonoBehaviour — needs Update()/FixedUpdate() and lives on the DP GameObject.

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DPController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Read input every frame; apply it in FixedUpdate for physics consistency
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Exposes current position for algorithms like NearestSceneFinder
    /// without exposing the Rigidbody2D itself (encapsulation).
    /// </summary>
    public Vector2 GetCurrentPosition() => rb.position;
}
