using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesFormation : MonoBehaviour
{
    private List<Transform> bones = new();
    private List<List<Transform>> neighbours = new();
    private List<List<float>> distances = new();
    private List<Rigidbody2D> rigidbodies = new();
    private List<float> rotations = new();
    public float separation = 0.1f;
    public Transform topBone, bottomBone;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bones.Add(transform.GetChild(i));
            neighbours.Add(new List<Transform>());
            distances.Add(new List<float>());
            foreach (Rigidbody2D rb in bones[i].GetComponents<Rigidbody2D>())
            {
                rigidbodies.Add(rb);
                rotations.Add(rb.rotation);
            }
            foreach (SpringJoint2D joint in bones[i].GetComponents<SpringJoint2D>())
            {

                neighbours[i].Add(joint.connectedBody.transform);
                distances[i].Add(joint.distance);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < bones.Count; i++)
        {
            for (int j = 0; j < neighbours[i].Count; j++)
            {
                Vector3 direction = neighbours[i][j].position - bones[i].position;
                float distance = direction.magnitude;
                float difference = distance - distances[i][j];
                rigidbodies[i].AddForce(direction.normalized * difference * separation);
            }
        }

        // get rotation by calculating the angle between the top and bottom bones
        float current_rotation = Vector3.SignedAngle(Vector3.up, topBone.position - bottomBone.position, Vector3.forward);
        for (int i = 0; i < bones.Count; i++)
        {
            rigidbodies[i].MoveRotation(rotations[i] + current_rotation);
        }
    }
}
