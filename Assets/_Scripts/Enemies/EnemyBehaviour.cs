using Pathfinding;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyBehaviour : MonoBehaviour
{
    Enemy enemy;
    [Header("Pathfinding")]
    Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public float nextWaypointDistance = 3f;

    [Header("Jump")]
    public float jumpThreshold = 1f;
    public float jumpForce = 1f;
    public float jumpOffset = 0.1f;
    float offset;

    Path path;
    int currentWaypoint = 0;
    bool isGrounded;
    Seeker seeker;
    Rigidbody2D body;
    [SerializeField] float flipThreshold = 0.05f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        seeker = gameObject.GetComponent<Seeker>();
        body = gameObject.GetComponent<Rigidbody2D>();
        offset = GetComponent<Collider2D>().bounds.extents.y + this.jumpOffset;

        InvokeRepeating(nameof(UpdatePath), 0f, this.pathUpdateSeconds);
    }
    bool TargetInRange()
    {
        target = Physics2D.OverlapCircleAll((Vector2)transform.position, activateDistance)
            .FirstOrDefault(c => c.CompareTag("Player"))?.transform;
        if(target==null)
            return false;
        return transform.Distance(target.transform) < this.activateDistance;
    }

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

        this.isGrounded = transform.position.IsGrounded(this.offset);
        if(!this.isGrounded)
        {
            Debug.Log($"isGrounded {isGrounded}", this);
        }
        Vector2 direction = body.position.DirectionTo((Vector2)path.vectorPath[currentWaypoint]);

        if(this.isGrounded)
        {
            if (direction.y > jumpThreshold)
            {
                Debug.Log($"jump", this);

                body.AddForce(jumpForce * this.enemy.Data.Speed * Vector2.up, ForceMode2D.Impulse);
            }
        }
        var force = this.enemy.Data.Speed * Time.deltaTime * direction;
        Debug.Log($"add force{force}", this);
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
        Gizmos.DrawWireSphere(this.transform.position, this.activateDistance);
        Gizmos.color= Color.red;
        Gizmos.DrawLine(this.transform.position, transform.position + Vector3.down * offset);
    }

}
