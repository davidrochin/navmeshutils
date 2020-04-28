using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DavidRochin;

public class MovePointTest : MonoBehaviour {

    public float Distance = 10f;
    public int Quality = 5;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(NavMeshUtils.Place(transform.position), NavMeshUtils.Place(transform.position) + transform.forward * Distance);

        Gizmos.color = Color.blue;
        Vector3[] points = NavMeshUtils.MovePosition(transform.position, transform.forward * Distance, Quality);
        if(points.Length >= 2) {
            for (int i = 1; i < points.Length; i++) {
                Gizmos.DrawSphere(points[i], 0.05f);
                Gizmos.DrawLine(points[i - 1], points[i]);
            }
        }

    }
}
