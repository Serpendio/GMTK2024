using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesFormation : MonoBehaviour
{
    private List<Transform> bones = new();
    private List<List<Transform>> neighbours = new();
    private List<List<float>> distances = new();
    private List<Vector3> positions = new();
    private List<Rigidbody2D> rigidbodies = new();
    private List<List<SpringJoint2D>> joints = new();
    private List<float> rotations = new();
    private List<float> angles_from_centre = new();
    [Min(0), SerializeField] private float separation = 50f, rotation_correction = 100f, correction_amount = 20, maxAngleOffset = 90, timePerCorrection = 5;
    [SerializeField] private Transform topBone, bottomBone;
    Vector3 centre;
    float timeSinceLastCorrection = 0;

    // Start is called before the first frame update
    void Start()
    {
        centre = Vector3.zero;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.StartsWith("bone"))
            {
                bones.Add(transform.GetChild(i));
                centre += bones[i].position;
            }
        }
        centre /= bones.Count;

        float current_rotation = Vector3.SignedAngle(Vector3.up, topBone.position - bottomBone.position, Vector3.forward);

        for (int i = 0; i < bones.Count; i++)
        {
            neighbours.Add(new List<Transform>());
            distances.Add(new List<float>());
            joints.Add(new List<SpringJoint2D>());
            positions.Add(bones[i].position - centre);
            angles_from_centre.Add(Vector3.SignedAngle(Vector3.up, bones[i].position - centre, Vector3.forward) - current_rotation);
            foreach (Rigidbody2D rb in bones[i].GetComponents<Rigidbody2D>())
            {
                rigidbodies.Add(rb);
                rotations.Add(rb.rotation);
            }
            foreach (SpringJoint2D joint in bones[i].GetComponents<SpringJoint2D>())
            {
                joints[i].Add(joint);
                neighbours[i].Add(joint.connectedBody.transform);
                distances[i].Add(joint.distance);
                //distances[i].Add(Vector2.Distance(bones[i].position, joint.connectedBody.transform.position));
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceLastCorrection += Time.fixedDeltaTime;
        if (timeSinceLastCorrection > timePerCorrection)
        {
            timeSinceLastCorrection = 0;
            ResetRotation(rotation_correction);
            ResetPositions();
        }

        centre = Vector3.zero;
        for (int i = 0; i < bones.Count; i++)
        {
            centre += bones[i].position;
            for (int j = 0; j < joints[i].Count; j++)
            {
                joints[i][j].distance = distances[i][j] * transform.localScale.x;
            }
        }
        centre /= bones.Count;
        for (int i = 0; i < bones.Count; i++)
        {
            for (int j = 0; j < neighbours[i].Count; j++)
            {
                Vector3 direction = neighbours[i][j].position - bones[i].position;
                float distance = direction.magnitude;
                float difference = distance - distances[i][j] * transform.localScale.x;
                rigidbodies[i].AddForce(difference * separation * direction.normalized);
            }
        }


        // it'd be nice if we could correct by a small amount each frame, but that doesn't seem to work
        float current_rotation = Vector3.SignedAngle(Vector3.up, topBone.position - bottomBone.position, Vector3.forward);
        if (Mathf.Abs(current_rotation) > maxAngleOffset)
        {
            ResetRotation(1);
            ResetPositions();
            timeSinceLastCorrection = 0;
        }

        current_rotation = Vector3.SignedAngle(Vector3.up, topBone.position - bottomBone.position, Vector3.forward);
        //Vector3 dist = transform.position - centre;
        //transform.Translate(-dist);
        for (int i = 0; i < bones.Count; i++)
        {
            //bones[i].Translate(dist);
            // rotate original position by the current rotation
            //rigidbodies[i].MovePosition(Vector2.Lerp(bones[i].position, centre + positions[i], Time.fixedDeltaTime * correction_amount));
            rigidbodies[i].MoveRotation(rotations[i] + current_rotation);
            //bones[i].Rotate(Vector3.back, rotations[i] + current_rotation);
        }

    }

    public void ResetPositions()
    {
        for (int i = 0; i < bones.Count; i++)
        {
            bones[i].position = centre + positions[i] * transform.localScale.x;
        }
    }

    private void ResetRotation(float percent)
    {
        float current_rotation = Vector3.SignedAngle(Vector3.up, topBone.position - bottomBone.position, Vector3.forward);

        transform.RotateAround(centre, Vector3.forward, -current_rotation * percent);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(centre, .1f);

        for (int i = 0; i < bones.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(positions[i] + centre, .1f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(centre, transform.position);
    }
}
