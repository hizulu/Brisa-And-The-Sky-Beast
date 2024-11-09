using UnityEngine;

public class PlayerMovementBasic : MonoBehaviour
{
    private Rigidbody rbPlayer;
    [SerializeField] private float velMov = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        Walk();
    }

    public void Walk()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(moveX, 0, moveZ);

        if (direction.magnitude > 1f)
            direction.Normalize();

        rbPlayer.MovePosition(transform.position + direction * velMov * Time.deltaTime);
    }
}
