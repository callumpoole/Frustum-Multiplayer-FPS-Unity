
using UnityEngine;

public class Util : MonoBehaviour {
    public static void recursivelyApplyLayer(GameObject go, int layer) {
        if (go == null)
            return;
        go.layer = layer;
        foreach (Transform t in go.transform) {
            if (t == null)
                continue;
            recursivelyApplyLayer(t.gameObject, layer);
        }
    }
}
