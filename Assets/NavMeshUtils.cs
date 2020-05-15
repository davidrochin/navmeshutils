using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

namespace DavidRochin {

    public static class NavMeshUtils {
        
        public static Vector3 SlidePoint(Vector3 position, Vector3 delta, out Vector3[] path, int quality = 5) {
            //Debug.Log($"Initial magnitude = {delta.magnitude}");

            List<Vector3> points = new List<Vector3>();
            points.Add(position);

            for (int i = 0; i < quality; i++) {

                NavMeshHit hit;
                if (NavMesh.Raycast(position, position + delta, out hit, NavMesh.AllAreas)) {
                    Vector3 safeHitPosition = hit.position - delta.normalized * 0.01f;
                    points.Add(safeHitPosition);

                    //Debug.Log($"[{i}] Hit Distance = {hit.distance}");
                    Vector3 remainingDelta = delta - delta.normalized * (hit.distance - 0.01f);

                    position = safeHitPosition;

                    //Debug.Log($"[{i}] Remaining magnitude = {remainingDelta.magnitude}");
                    delta = CalculateSlide(remainingDelta, hit.normal);

                    //Debug.Log($"[{i}] delta.magnitude = {delta.magnitude}");

                } else {
                    points.Add(position + delta);
                    break;
                }

            }

            path = points.ToArray();
            return points[points.Count - 1];
        }

        [System.Obsolete]
        public static Vector3[] MovePosition(Vector3 position, Vector3 delta, int quality = 5) {

            //Debug.Log($"Initial magnitude = {delta.magnitude}");

            position = Place(position);

            List<Vector3> points = new List<Vector3>();
            points.Add(position);

            for (int i = 0; i < quality; i++) {

                NavMeshHit hit;
                if (NavMesh.Raycast(position, position + delta, out hit, NavMesh.AllAreas)) {
                    Vector3 safeHitPosition = hit.position - delta.normalized * 0.01f;
                    points.Add(safeHitPosition);

                    //Debug.Log($"[{i}] Hit Distance = {hit.distance}");
                    Vector3 remainingDelta = delta - delta.normalized * (hit.distance - 0.01f);

                    position = safeHitPosition;

                    //Debug.Log($"[{i}] Remaining magnitude = {remainingDelta.magnitude}");
                    delta = CalculateSlide(remainingDelta, hit.normal);

                    //Debug.Log($"[{i}] delta.magnitude = {delta.magnitude}");
                    
                } else {
                    points.Add(position + delta);
                    break;
                }

            }
            
            return points.ToArray();
        }

        public static Vector3? Raycast(Vector3 position, Vector3 delta) {
            position = Place(position);
            NavMeshHit hit;
            if (NavMesh.Raycast(position, position + delta, out hit, NavMesh.AllAreas)) {
                return hit.position;
            } else {
                return null;
            }
        }

        private static Vector3 CalculateSlide(Vector3 remainingDelta, Vector3 normal) {
            Vector3 a = Vector3.Cross(normal, remainingDelta);
            Vector3 b = Vector3.Cross(a, normal);
            Vector3 d = b.normalized;
            float dist = Vector3.Dot(d, remainingDelta);
            return d * dist;
        }

        public static Vector3 Place(Vector3 point, float maxDistance = 2f, int areaMask = NavMesh.AllAreas) {
            NavMeshHit hit;
            bool valid = NavMesh.SamplePosition(point, out hit, maxDistance, NavMesh.AllAreas);
            if (valid) {
                return hit.position;
            } else {
                throw new System.Exception("Could not place point on Nav Mesh");
            }
            
        }

    }

}