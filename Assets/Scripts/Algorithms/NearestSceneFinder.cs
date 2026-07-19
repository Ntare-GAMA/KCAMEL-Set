using System.Collections.Generic;
using UnityEngine;

public static class NearestSceneFinder
{
    /// <summary>
    /// Returns the nearest unresolved FilmScene to the given position,
    /// or null if every scene has been resolved (completed or failed).
    /// </summary>
    public static FilmScene FindNearestIncomplete(Vector2 currentPosition, List<FilmScene> scenes)
    {
        FilmScene nearest = null;
        float nearestSqrDistance = float.MaxValue;

        foreach (FilmScene scene in scenes)
        {
            if (scene.IsResolved)
                continue; // skip scenes already completed OR failed — no retries

            float sqrDistance = (scene.Location - currentPosition).sqrMagnitude;

            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearest = scene;
            }
        }

        return nearest;
    }
}
