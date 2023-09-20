using UnityEngine;

public static class CollisionUtils
{
    /// <summary>
    /// Checks whether a layer mask contains any of the specified layer bits.
    /// </summary>
    /// <param name="layerMask">The layer mask to check against.</param>
    /// <param name="layer">The layer bits to check against.</param>
    /// <returns>True if the any of the layer mask and layer bits overlap.</returns>
    public static bool LayerMaskContainsAny(LayerMask layerMask, int layer)
    {
        int otherLayerMask = 1 << layer;
        return (otherLayerMask & layerMask) != 0;
    }
}
