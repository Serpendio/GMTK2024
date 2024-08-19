using Pathfinding;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] EnemyData Data;
    [Header("Pathfinding")]
    Transform target;
    Vector3 idleTarget;
    public float pathUpdateSeconds = 0.5f;
    public float nextWaypointDistance = 3f;

    [Header("Jump")]
    public float groundCheckOffsetX = 1f;
    public float groundCheckOffsetY = 0.5f;
    float offsetX;
    float offsetY;

    Path path;
    int currentWaypoint = 0;
    bool isGrounded;
    Seeker seeker;
    [SerializeField] Rigidbody2D body;
    bool isIdle = true;
    int currentIdleTime = 0;


    private void Awake()
    {
        seeker = GetComponent<Seeker>();

        offsetX = Data.Size * transform.localScale.x  + groundCheckOffsetX;
        offsetY = Data.Size * transform.localScale.x + groundCheckOffsetY;

        InvokeRepeating(nameof(UpdatePath), 0f, this.pathUpdateSeconds);
        InvokeRepeating(nameof(UpdateIdleTime), 0f, 1);
    }
    void UpdateIdleTime()
    {
        if (currentIdleTime >= Data.IdleSeconds)
            return;

        currentIdleTime++;
        if(currentIdleTime >= Data.IdleSeconds)
        {
            isIdle= true;
        }
    }
    bool TargetInRange()
    {
        target = Physics2D.OverlapCircleAll(body.position, Data.AwarenessRange * transform.localScale.x)
            .FirstOrDefault(c => c.CompareTag("Player"))?.transform;

        if(target==null)
            return false;

        var check = body.position.Distance(target.transform.position.To2D()) < Data.AwarenessRange * transform.localScale.x;
        if (check)
        {
            isIdle = false;
            currentIdleTime = 0;
        }

        return check;
    }

    bool IsGrounded()
        => Physics2D.OverlapCircleAll(body.position, offsetY)
            .FirstOrDefault(c => c.CompareTag("Ground")) != null;

    private void FixedUpdate()
    {
        PathFollow();
    }
    void UpdatePath()
    {
        if (!seeker.IsDone())
            return;

        if (TargetInRange())
        {
            seeker.StartPath(body.position, target.position, OnPathComplete);
        }
        else if(IdleTarget())
        {
            seeker.StartPath(body.position, idleTarget, OnPathComplete);
        }
    }

    private bool IdleTarget()
    {
        if (!this.isIdle)
            return false;

        var idleRange = Data.IdleRange * transform.localScale.x;
        this.idleTarget = body.position + new Vector2(Random.Range(-idleRange, idleRange),0);
        isIdle= false;
        currentIdleTime = 0;
        return true;
    }

    void OnPathComplete(Path path)
    {
        if (path.error)
            return;

        this.path = path;
        currentWaypoint = 0;
    }
    void PathFollow()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
            return;

        this.isGrounded = IsGrounded();
        if (!this.isGrounded)
            return;

        Vector2 direction = body.position.DirectionTo((Vector2)path.vectorPath[currentWaypoint]);
        
        bool isBLocked = body.position.IsBlocked(new Vector2(direction.x,-0.3f*Mathf.Abs(direction.x)).normalized,this.offsetX);
        if(this.isGrounded && isBLocked)
        {
            body.AddForce(Data.JumpForce * Time.deltaTime * new Vector2(direction.x/10,1).normalized, ForceMode2D.Impulse);
        }
        var force = Data.Speed * Time.deltaTime * direction;
        body.AddForce(force);

        var distance = body.position.Distance(path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.body.position, Data.AwarenessRange * transform.localScale.x);
        Gizmos.DrawWireSphere(this.body.position, Data.IdleRange * transform.localScale.x);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.body.position, Data.Size * transform.localScale.x);

        Gizmos.color= Color.red;
        Gizmos.DrawLine(this.body.position, body.position + Vector2.right * offsetX);
        Gizmos.DrawLine(this.body.position, body.position + Vector2.left * offsetX);
        Gizmos.DrawLine(this.body.position, body.position + Vector2.down * offsetY);
    }

}
