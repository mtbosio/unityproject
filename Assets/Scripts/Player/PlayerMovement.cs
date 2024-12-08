/** 
 * Place holder script for player movement
 */

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Movement speed
    public float lookSpeed = 2f;  // Mouse sensitivity
    public float boostMultiplier = 3f; // Speed multiplier when holding Shift

    private float yaw = 0f; // Horizontal rotation
    private float pitch = 0f; // Vertical rotation

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        yaw += mouseX * lookSpeed;
        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Prevent flipping upside-down
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // Handle movement
        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * boostMultiplier : moveSpeed;
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow
        float moveY = 0f;

        // Fly up/down with Q/E keys
        if (Input.GetKey(KeyCode.Q)) moveY = -1f;
        if (Input.GetKey(KeyCode.E)) moveY = 1f;

        Vector3 move = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;
        transform.position += move * speed * Time.deltaTime;
    }
}
