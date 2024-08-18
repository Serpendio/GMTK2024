using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesFormation : MonoBehaviour
{
    readonly private List<Transform> bones = new();
    readonly private List<List<Transform>> neighbours = new();
    readonly private List<List<float>> distances = new();
    readonly private List<Vector3> positions = new();
    readonly private List<Rigidbody2D> rigidbodies = new();
    readonly private List<List<SpringJoint2D>> joints = new();
    readonly private List<float> rotations = new();
    [Min(0), SerializeField] private float separation = 50f, dist_before_panic = 1f;
    [SerializeField] private Transform topBone, bottomBone;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 centre = Vector3.zero;
        for (int i = 0; i < bones.Count; i++)
        {
            centre += bones[i].position;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            bones.Add(transform.GetChild(i));
            neighbours.Add(new List<Transform>());
            distances.Add(new List<float>());
            joints.Add(new List<SpringJoint2D>());
            positions.Add(bones[i].position - centre);
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

        Vector3 centre = Vector3.zero;
        for (int i = 0; i < bones.Count; i++)
        {
            centre += bones[i].localPosition;
            for (int j = 0; j < joints[i].Count; j++)
            {
                joints[i][j].distance = distances[i][j] * transform.localScale.x;
            }
        }
        centre /= bones.Count;
        for (int i = 0; i < bones.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                bones[i].localPosition = centre + positions[i] + Vector3.left * 4;
            }

            for (int j = 0; j < neighbours[i].Count; j++)
            {
                Vector3 direction = neighbours[i][j].position - bones[i].position;
                float distance = direction.magnitude;
                float difference = distance - distances[i][j];
                rigidbodies[i].AddForce(difference * separation * direction.normalized);
            }
        }


        // get rotation by calculating the angle between the top and bottom bones
        float current_rotation = Vector3.SignedAngle(Vector3.up, topBone.position - bottomBone.position, Vector3.forward);
        for (int i = 0; i < bones.Count; i++)
        {
            rigidbodies[i].MoveRotation(rotations[i] + current_rotation);
            //bones[i].Rotate(Vector3.back, rotations[i] + current_rotation);
        }
    }
}
