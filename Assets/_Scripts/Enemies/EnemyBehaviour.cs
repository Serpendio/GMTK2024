using Pathfinding;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Enemy),typeof(Seeker))]
public class EnemyBehaviour : MonoBehaviour
{
    Enemy enemy;
    [Header("Pathfinding")]
    Transform target;
    public float pathUpdateSeconds = 0.5f;
    public float nextWaypointDistance = 3f;

    [Header("Jump")]
    public float jumpThreshold = 1f;
    public float jumpOffsetX = 0.5f;
    public float jumpOffsetY = 0.1f;
    float offsetX;
    float offsetY;

    Path path;
    int currentWaypoint = 0;
    bool isGrounded;
    Seeker seeker;
    [SerializeField] Rigidbody2D body;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        seeker = GetComponent<Seeker>();

        offsetX = enemy.Data.Size + jumpOffsetX;
        offsetY = enemy.Data.Size + jumpOffsetY;

        InvokeRepeating(nameof(UpdatePath), 0f, this.pathUpdateSeconds);
    }
    bool TargetInRange()
    {
        target = Physics2D.OverlapCircleAll(body.position, this.enemy.Data.AwarenessRange)
            .FirstOrDefault(c => c.CompareTag("Player"))?.transform;
        if(target==null)
            return false;
        return body.position.Distance(target.transform.position.To2D()) < this.enemy.Data.AwarenessRange;
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
    }
    void UpdatePath()
    {
        if (seeker.IsDone() && TargetInRange())
        {
            seeker.StartPath(body.position, target.position, OnPathComplete);
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
            body.AddForce(this.enemy.Data.JumpForce * Time.deltaTime * new Vector2(direction.x/10,1).normalized, ForceMode2D.Impulse);
        }
        var force = this.enemy.Data.Speed * Time.deltaTime * direction;
        body.AddForce(force);

        var distance = body.position.Distance(path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }


    private void OnDrawGizmos()
    {
        if(enemy!= null)
        {
            Gizmos.DrawWireSphere(this.body.position, this.enemy.Data.AwarenessRange);
        }
        Gizmos.color= Color.red;
        Gizmos.DrawLine(this.body.position, body.position + Vector2.right * offsetX);
        Gizmos.DrawLine(this.body.position, body.position + Vector2.left * offsetX);
        Gizmos.DrawLine(this.body.position, body.position + Vector2.down * offsetY);
    }

}
