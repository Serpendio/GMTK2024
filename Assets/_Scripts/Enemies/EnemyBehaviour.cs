using Pathfinding;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Enemy))]
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
    Rigidbody2D body;
    Collider2D collider2;
    [SerializeField] float flipThreshold = 0.05f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        seeker = GetComponent<Seeker>();
        body = GetComponent<Rigidbody2D>();
        collider2 = GetComponent<Collider2D>();
        offsetX = collider2.bounds.extents.x + this.jumpOffsetX;
        offsetY = collider2.bounds.extents.y + this.jumpOffsetY;

        InvokeRepeating(nameof(UpdatePath), 0f, this.pathUpdateSeconds);
    }
    bool TargetInRange()
    {
        target = Physics2D.OverlapCircleAll((Vector2)transform.position, this.enemy.Data.AwarenessRange)
            .FirstOrDefault(c => c.CompareTag("Player"))?.transform;
        if(target==null)
            return false;
        return transform.Distance(target.transform) < this.enemy.Data.AwarenessRange;
    }

    bool IsGrounded()
        => Physics2D.OverlapCircleAll((Vector2)transform.position, offsetY)
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

        Vector2 direction = transform.position.DirectionTo((Vector2)path.vectorPath[currentWaypoint]);
        
        bool isBLocked = transform.position.IsBlocked(new Vector3(direction.x,-0.3f*Mathf.Abs(direction.x),0).normalized,this.offsetX);
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

        //flip graphics
        if(body.velocity.x > this.flipThreshold)
        {
            transform.localScale = transform.localScale.With(x: -1f * Mathf.Abs(transform.localScale.x));
        }
        else if(body.velocity.x < -this.flipThreshold)
        {
            transform.localScale = transform.localScale.With(x: Mathf.Abs(transform.localScale.x));
        }
    }


    private void OnDrawGizmos()
    {
        if(enemy!= null)
        {
            Gizmos.DrawWireSphere(this.transform.position, this.enemy.Data.AwarenessRange);
        }
        Gizmos.color= Color.red;
        Gizmos.DrawLine(this.transform.position, transform.position + Vector3.right * offsetX);
        Gizmos.DrawLine(this.transform.position, transform.position + Vector3.left * offsetX);
        Gizmos.DrawLine(this.transform.position, transform.position + Vector3.down * offsetY);
    }

}
