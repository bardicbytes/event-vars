//alex@bardicbytes.com

using BardicBytes.EventVars;
using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField]
    private Vector3EventVar positionEvent = default;

    public float turnSpeed = 90f;
    public FloatEventVar.Field speedField = default;
    public float changeDirectionTime = 2f;
    private Rigidbody rb;
    private Vector3 movementDirection;
    private float timer;

    private bool isPlayerInput = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ChangeDirection();
        positionEvent.AddListener(DirectTo);
    }

    private void DirectTo(Vector3 arg0)
    {
        if (timer < .15f) return;
        var h = arg0 - rb.position;
        h.y = 0;
        movementDirection = h.normalized;
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection();
        }
    }

    void FixedUpdate()
    {
        MoveCapsule();
    }

    void ChangeDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        movementDirection = new Vector3(randomX, 0, randomZ).normalized;
        timer = 0;
    }

    void MoveCapsule()
    {
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(movementDirection, Vector3.up), turnSpeed * Time.deltaTime));
        // speedField, a FloatEventVar.Field has implicit type conversion so no type casting is necessary
        rb.MovePosition(rb.position + transform.forward * speedField * Time.fixedDeltaTime * (isPlayerInput ? 1.5f : 1f));
    }
}
