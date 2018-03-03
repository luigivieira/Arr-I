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
/// Implementation of the Magnet steering behaviour in 2D. This behaviour makes the object
/// to be attracted by the target as it gets closer to it. It is implemented
/// by using the seek behaviour and applying an acceleration factor inverselly proportional
/// to the distance from the target (in a configured accelerating radius centered on the
/// the game object).
/// </summary>
public class MagnetBehaviour2D: SeekBehaviour2D
{
    /// <summary>
    /// Radius of the circular area around the target in which the game object
    /// will start to feel the attraction.
    /// </summary>
    [SerializeField]
    private float attractionRadius = 2f;

    /// <summary>
    /// Public property to control the access to the <c>attractionRadius</c> class attribute.
    /// </summary>
    public float AttractionRadius
    {
        get { return attractionRadius; }
        set { attractionRadius = Mathf.Max(value, 1f); }
    }

    /// <summary>
    /// Calculates the desired velocity of this steering behaviour.
    /// </summary>
    /// <param name="targetPos">Vector2 with the target position to consider.</param>
    /// <returns>Vector2 with the desired velocity for the object according to this
    /// steering behaviour.</returns>
    protected override Vector2 CalculateDesiredVelocity(Vector2 targetPos)
    {
        float distance = Vector2.Distance(transform.position, targetPos);
        float acceleration;

        if(distance > attractionRadius)
            acceleration = 0f;
        else
            acceleration = 1f - (distance / attractionRadius);

        return base.CalculateDesiredVelocity(targetPos) * acceleration;
    }

    /// <summary>
    /// Draws debug information using gizmos.
    /// </summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(Active && Application.isPlaying)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, attractionRadius);
        }
    }
}
