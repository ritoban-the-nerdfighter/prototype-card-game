using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class Util
    {
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }

        public static void SetSortingLayerRecursively(this GameObject obj, string layer)
        {
            if (obj.GetComponent<Renderer>() != null)
                obj.GetComponent<Renderer>().sortingLayerName = layer;
            if (obj.GetComponent<Canvas>() != null)
                obj.GetComponent<Canvas>().sortingLayerName = layer;
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetSortingLayerRecursively(layer);
            }
        }

        public static float ManhattanDistance(Vector3 a, Vector3 b)
        {

            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);

        }
    }
}
