using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesFormation : MonoBehaviour
{
    readonly private List<Transform> bones = new();
    readonly private List<List<Transform>> neighbours = new();
    readonly private List<List<float>> distances = new();
    readonly private List<Rigidbody2D> rigidbodies = new();
    readonly private List<float> rotations = new();
    [Min(0), SerializeField] private float separation = 50f, rotation_strength = 10f;
    [SerializeField] private Transform topBone, bottomBone;

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
        Vector3 centre = Vector3.zero;
        for (int i = 0; i < bones.Count; i++)
        {
            centre += bones[i].position;
            for (int j = 0; j < neighbours[i].Count; j++)
            {
                Vector3 direction = neighbours[i][j].position - bones[i].position;
                float distance = direction.magnitude;
                float difference = distance - distances[i][j];
                rigidbodies[i].AddForce(direction.normalized * difference * separation);
            }
        }
        centre /= bones.Count;

        // get rotation by calculating the angle between the top and bottom bones
        float current_rotation = Vector3.SignedAngle(Vector3.up, topBone.position - bottomBone.position, Vector3.forward);
        //print(current_rotation);
        for (int i = 0; i < bones.Count; i++)
        {
            //bones[i].RotateAround(centre, Vector3.forward, -current_rotation * rotation_strength * Time.deltaTime);
            rigidbodies[i].MoveRotation(rotations[i] + current_rotation);
        }
        //transform.Rotate(0, 0, (current_rotation < 0 ? -1 : 1) * rotation_strength * Time.deltaTime);
    }
}
