using Pathfinding;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Seeker))]
public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] EnemyData Data;
    [Header("Pathfinding")]
    Transform target;
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

    private void Awake()
    {
        seeker = GetComponent<Seeker>();

        offsetX = Data.Size * transform.localScale.x  + groundCheckOffsetX;
        offsetY = Data.Size * transform.localScale.x + groundCheckOffsetY;

        InvokeRepeating(nameof(UpdatePath), 0f, this.pathUpdateSeconds);
    }
    bool TargetInRange()
    {
        target = Physics2D.OverlapCircleAll(body.position, Data.AwarenessRange)
            .FirstOrDefault(c => c.CompareTag("Player"))?.transform;
        if(target==null)
            return false;
        return body.position.Distance(target.transform.position.To2D()) < Data.AwarenessRange;
    }

    bool IsGrounded()
        => Physics2D.OverlapCircleAll(body.position, offsetY)
            .FirstOrDefault(c => c.CompareTag("Ground")) != null;

    private void FixedUpdate()
    {
        if (TargetInRange())
        {
            PathFollow();
        }
        else
        {
            PathIdle();
        }

    }
    void UpdatePath()
    {
        if (!seeker.IsDone())
            return;

        if (TargetInRange())
        {
            seeker.StartPath(body.position, target.position, OnPathComplete);
        }
        else
        {
            seeker.StartPath(body.position, body.position + Vector2Ext.RandomVector(1, 0));
        }
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
    void PathIdle()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.body.position, Data.AwarenessRange);
        Gizmos.DrawWireSphere(this.body.position, Data.IdleRange);
        
        Gizmos.color= Color.red;
        Gizmos.DrawLine(this.body.position, body.position + Vector2.right * offsetX);
        Gizmos.DrawLine(this.body.position, body.position + Vector2.left * offsetX);
        Gizmos.DrawLine(this.body.position, body.position + Vector2.down * offsetY);
    }

}
