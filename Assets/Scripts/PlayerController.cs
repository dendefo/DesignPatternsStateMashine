using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MovementState movementState;
    private void Awake()
    {
        movementState = new WalkingState();
    }
    // Update is called once per frame
    void Update()
    {
        movementState.SwitchState(this);
        movementState.ReadInput(this);
    }
    public void Move(Vector2 direction)
    {
        transform.position += new Vector3(direction.x, direction.y, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && movementState is JumpingState)
        {
            movementState = new WalkingState();
        }

    }
}
abstract public class MovementState
{
    abstract public void ReadInput(PlayerController player);
    abstract public void SwitchState(PlayerController player);
}
public class WalkingState : MovementState
{
    const float SPEED = 5;
    public override void ReadInput(PlayerController player)
    {
        var mov = Input.GetAxis("Horizontal");
        player.Move(new Vector2(mov * SPEED * Time.deltaTime, 0));
    }
    public override void SwitchState(PlayerController player)
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            player.movementState = new CrouchState();
        if (Input.GetKeyDown(KeyCode.Space))
            player.movementState = new JumpingState();
    }
}
public class CrouchState : MovementState
{
    const float SPEED = 2.5f;

    public override void ReadInput(PlayerController player)
    {
        var mov = Input.GetAxis("Horizontal");
        player.Move(new Vector2(mov * SPEED * Time.deltaTime, 0));
    }
    public override void SwitchState(PlayerController player)
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
            player.movementState = new WalkingState();
    }
}
public class JumpingState : MovementState
{
    const float SPEED = 5;
    const float JUMP_TIME = 0.5f;
    float timer = 0;
    public override void ReadInput(PlayerController player)
    {
        timer += Time.deltaTime;
        if (timer > JUMP_TIME) return;

        var mov = Input.GetAxis("Horizontal");
        player.Move(new Vector2(mov * SPEED * Time.deltaTime, 5 * Time.deltaTime));
    }
    public override void SwitchState(PlayerController player)
    {
        if (Input.GetKeyUp(KeyCode.Space))
            player.movementState = new WalkingState();
    }
}