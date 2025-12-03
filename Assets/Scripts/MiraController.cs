using UnityEngine;

public class MiraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Crafting System")]
    public GameObject craftingPanel;

    [Header("UI References")]
    public GameObject messagePanel;

    private Vector2 movement;
    private bool canMove = true;
    private bool isCraftingOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCraftingPanel();
        }

        if (!canMove) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        if (animator != null)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (movement != Vector2.zero)
            {
                animator.SetFloat("LastHorizontal", movement.x);
                animator.SetFloat("LastVertical", movement.y);
            }
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void ToggleCraftingPanel()
    {
        isCraftingOpen = !isCraftingOpen;

        if (craftingPanel != null)
        {
            craftingPanel.SetActive(isCraftingOpen);
        }

        if (messagePanel != null && isCraftingOpen)
        {
            messagePanel.SetActive(false);
        }

        SetMovement(!isCraftingOpen);

        Time.timeScale = isCraftingOpen ? 0f : 1f;
    }

    public void SetMovement(bool enabled)
    {
        canMove = enabled;
        if (!enabled)
        {
            movement = Vector2.zero;
            if (animator != null)
                animator.SetFloat("Speed", 0f);
        }
    }
}