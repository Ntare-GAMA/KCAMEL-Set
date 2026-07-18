// NearestSceneFinder.cs
// Location in Unity project: Assets/Scripts/Algorithms/NearestSceneFinder.cs
//
// ALGORITHM #1: Nearest-target search (linear search / greedy nearest-neighbor)
//
// Problem it solves: given the DP's current position and a list of scenes,
// which incomplete scene should the player head to next?
//
// Approach: linear scan comparing squared distance (avoids costly sqrt calls
// during comparison — sqrt is only applied once at the end if needed).
//
// Complexity: O(n) where n = number of scenes. Every scene must be checked
// once since there's no spatial partitioning (e.g. a grid or k-d tree) —
// acceptable here since a shoot day realistically has a small, fixed number
// of scenes (typically under 10), so O(n) never becomes a bottleneck.
// If this needed to scale to hundreds of scenes, a spatial data structure
// (quad-tree, grid buckets) would reduce average lookup cost.

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
