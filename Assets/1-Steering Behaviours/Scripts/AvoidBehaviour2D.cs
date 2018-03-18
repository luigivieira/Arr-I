/*
 * This file is part of the Arr-I project. The complete source code is
 * available at https://github.com/luigivieira/Arr-I.
 *
 * Copyright (c) 2018, Luiz Carlos Vieira (http://www.luiz.vieira.nom.br)
 *
 * MIT License
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using UnityEngine;

/// <summary>
/// Implementation of the Avoid steering behaviour in 2D. This behaviour makes the object
/// to chase the target where it is moving to, by simply calculating the desired velocity of
/// the seek behaviour towards a future position predicted based on the target's estimated
/// velocity.
/// </summary>
public class AvoidBehaviour2D: BaseBehaviour2D
{
    /// <summary>
    /// Mask of the layers where obstacles should be considered.
    /// </summary>
    public LayerMask obstacleLayerMask = Physics2D.DefaultRaycastLayers;

    /// <summary>
    /// Distance for the object to look ahead in its direction and check for collisions.
    /// </summary>
    [SerializeField]
    private float seeAheadDistance = 2f;

    /// <summary>
    /// Public property to control the access to the <c>seeAheadLength</c> class attribute.
    /// </summary>
    public float SeeAheadDistance
    {
        get { return seeAheadDistance; }
        set { seeAheadDistance = Mathf.Max(value, 1f); }
    }

    /// <summary>
    /// Radius of the circle cast issued for seeing ahead of the game object and detecting
    /// collisions. The value of 0 means a simple ray cast (i.e. no circle cast).
    /// </summary>
    [SerializeField]
    private float raycastRadius = 0f;

    /// <summary>
    /// Public property to control the access to the <c>raycastRadius</c> class attribute.
    /// </summary>
    public float RaycastRadius
    {
        get { return raycastRadius; }
        set { raycastRadius = Mathf.Max(value, 0f); }
    }

    /// <summary>
    /// Encapsulates the calls to Unity's raycast functions to use the appropriate
    /// version based on the configuration.
    /// </summary>
    /// <param name="origin">Vector with the origin of the raycast.</param>
    /// <param name="direction">Vector with the direction of the raycast.</param>
    /// <param name="length">Float with the length of the raycast.</param>
    /// <returns></returns>
    private RaycastHit2D RayCast(Vector2 origin, Vector2 direction, float length)
    {
        if(raycastRadius == 0f)
            return Physics2D.Raycast(origin, direction, length, obstacleLayerMask);
        else
            return Physics2D.CircleCast(origin, raycastRadius, direction, length, obstacleLayerMask);
    }

    /// <summary>
    /// Calculates the desired velocity of this steering behaviour.
    /// </summary>
    /// <param name="targetPos">Vector2 with the target position to consider.</param>
    /// <returns>Vector2 with the desired velocity for the object according to this
    /// steering behaviour.</returns>
    protected override Vector2 CalculateDesiredVelocity(Vector2 targetPos)
    {
        RaycastHit2D hit = RayCast(transform.position, manager.Direction, seeAheadDistance);
        if(hit.collider != null)
        {
            Vector2 steer = (hit.point - (Vector2) hit.collider.bounds.center).normalized * manager.MaxSpeed;
            return (steer + manager.Velocity).normalized * manager.MaxSpeed;
        }
        else
            return Vector2.zero;
    }

    /// <summary>
    /// Draws debug information using gizmos.
    /// </summary>
    protected override void OnDrawGizmos()
    {
        if(Active && debug && Application.isPlaying)
        {
            Gizmos.color = Color.magenta;

            // Draw the see ahead vector
            Vector2 seeAhead = (Vector2) transform.position + manager.Direction * seeAheadDistance;
            GizmosExt.DrawArrow(transform.position, seeAhead);

            // Get the closest obstacle hit
            RaycastHit2D hit = RayCast(transform.position, manager.Direction, seeAheadDistance);
            if(hit.collider != null)
            {
                // Draw the point of collision with the obstacle
                Gizmos.DrawSphere(hit.point, 0.15f);

                // Draw the desired velocity
                Vector2 velocity = CalculateDesiredVelocity(Vector2.zero);

                Gizmos.color = Color.white;
                GizmosExt.DrawArrow(transform.position, (Vector2) transform.position + velocity);
            }
        }
    }
}
