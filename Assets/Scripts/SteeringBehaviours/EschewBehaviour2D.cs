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
/// Implementation of the Eschew steering behaviour in 2D. This behaviour makes the object
/// to flee from the target only until it feels safe, hence eschewing the target as much
/// as possible. It is implemented in a similar fashion of the Arrival behaviour, but
/// using a safe radius to descelerate as it moves away from the target.
/// </summary>
public class EschewBehaviour2D: FleeBehaviour2D
{
    /// <summary>
    /// Radius of the circular area around the target in which the game object
    /// will try to stay outside.
    /// </summary>
    [SerializeField]
    private float safeRadius = 2f;

    /// <summary>
    /// Public property to control the access to the <c>safeRadius</c> class attribute.
    /// </summary>
    public float SafeRadius
    {
        get { return safeRadius; }
        set { safeRadius = Mathf.Max(value, 1f); }
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
        float deceleration = Mathf.Max(0f, 1 - (distance / safeRadius));
        return base.CalculateDesiredVelocity(targetPos) * deceleration;
    }

    /// <summary>
    /// Draws debug information using gizmos.
    /// </summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(Active && debug && Application.isPlaying)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(target.transform.position, safeRadius);
        }
    }
}
