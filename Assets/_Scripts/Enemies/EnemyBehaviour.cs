using Pathfinding;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Resources))]
[RequireComponent(typeof(EnemyWeakpoint))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] EnemyData Data;
    [Header("Pathfinding")]
    Transform target;
    Vector3 idleTarget;
    public float pathUpdateSeconds = 0.5f;
    public float nextWaypointDistance = 1f;

    [Header("Jump")]
    [SerializeField] bool isFlying = false;
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
    Resources resources;
    EnemyWeakpoint weakPoint;


    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        resources = GetComponent<Resources>();
        weakPoint = GetComponent<EnemyWeakpoint>();

        offsetX = resources.CurrentSize + groundCheckOffsetX;
        offsetY = resources.CurrentSize + groundCheckOffsetY;

        InvokeRepeating(nameof(UpdatePath), 0f, this.pathUpdateSeconds);
        InvokeRepeating(nameof(UpdateIdleTime), 0f, 1);
    }
    private void OnValidate()
    {
        resources ??= GetComponent<Resources>();
    }
    void UpdateIdleTime()
    {
        if (currentIdleTime >= Data.IdleSeconds)
            return;

        currentIdleTime++;
        if (currentIdleTime >= Data.IdleSeconds)
        {
            isIdle = true;
        }
    }
    bool TargetInRange()
    {
        target = body.position.Physics()
            .OverlapCircleAll(Data.AwarenessRange * transform.localScale.x)
            .FirstOrDefault(c => c.CompareTag("Player"))?
            .transform;

        if (target == null)
            return false;

        var check = body.position.Distance(target.transform.position.To2D()) < Data.AwarenessRange * transform.localScale.x;
        if (check)
        {
            isIdle = false;
            currentIdleTime = 0;
        }

        return check;
    }
    bool TargetIsWeaker()
    {
        var targetResources = target.GetComponentInParent<Resources>();
        if (weakPoint.playerShouldBeBigger)
        {
            if (resources.CurrentSize < targetResources.CurrentSize)
            {
                return false;
            }
        }
        else
        {
            if (resources.CurrentSize > targetResources.CurrentSize)
            {
                return false;
            }
        }

        return true;
    }
    bool IsGrounded()
        => body.position.Physics()
            .OverlapCircleAll(offsetY)
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
            if (TargetIsWeaker())
                seeker.StartPath(body.position, target.position, OnPathComplete);
            else
                seeker.StartPath(body.position, body.position + (target.position.DirectionTo(body.position).To2D() * transform.localScale.x), OnPathComplete);
        }
        else if (IdleTarget())
        {
            seeker.StartPath(body.position, idleTarget, OnPathComplete);
        }
    }

    private bool IdleTarget()
    {
        if (!this.isIdle)
            return false;

        var idleDistance = Random.Range(resources.BaseSize + Data.IdleRange / 3, Data.IdleRange) * transform.localScale.x;
        this.idleTarget = body.position + new Vector2(idleDistance * RandomExt.Sign(), 0);
        isIdle = false;
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

        if (!isFlying)
        {
            this.isGrounded = IsGrounded();
            if (!this.isGrounded)
                return;
        }

        Vector2 direction = body.position.DirectionTo((Vector2)path.vectorPath[currentWaypoint]);

        if (!isFlying)
        {
            bool isBLocked = body.position.IsBlocked(new Vector2(direction.x, -0.3f * Mathf.Abs(direction.x)).normalized, this.offsetX);
            if (this.isGrounded && isBLocked)
            {
                body.AddForce(Data.JumpForce * Time.deltaTime * new Vector2(direction.x / 10, 1).normalized, ForceMode2D.Impulse);
            }
        }
        var force = Data.Speed * Time.deltaTime * direction;
        if (target == null)
        {
            force *= Data.IdleSpeedFactor;
        }
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
        Gizmos.DrawWireSphere(this.body.position, resources.CurrentSize);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.body.position, body.position + Vector2.right * offsetX);
        Gizmos.DrawLine(this.body.position, body.position + Vector2.left * offsetX);
        Gizmos.DrawLine(this.body.position, body.position + Vector2.down * offsetY);
    }

}
