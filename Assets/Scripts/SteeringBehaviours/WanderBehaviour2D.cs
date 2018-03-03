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
/// Implementation of the Wander steering behaviour in 2D. This behaviour makes the object
/// to wander around in a random fashion but without abrupt changing its route. This is done
/// by simply calculating the desired velocity of the seek behaviour towards a point
/// (a "carrot", i.e. something that keeps attracking the moving object) randomly positioned
/// around the perimeter of a circle in front of the object direction.
/// </summary>
public class WanderBehaviour2D: SeekBehaviour2D
{
    /// <summary>
    /// Distance from the center of the object in which the center of the circle will be
    /// positioned.
    /// </summary>
    [SerializeField]
    private float circleDistance = 1f;

    /// <summary>
    /// Public property to control the access to the <c>circleDistance</c> class attribute.
    /// </summary>
    public float CircleDistance
    {
        get { return circleDistance; }
        set { circleDistance = Mathf.Max(value, 1f); }
    }

    /// <summary>
    /// Radius of the circle used to randomly position the carrot.
    /// </summary>
    [SerializeField]
    private float circleRadius = 0.5f;

    /// <summary>
    /// Public property to control the access to the <c>circleRadius</c> class attribute.
    /// </summary>
    public float CircleRadius
    {
        get { return circleRadius; }
        set { circleRadius = Mathf.Max(value, 0f); }
    }

    /// <summary>
    /// The current center of the circle used for the random movement.
    /// </summary>
    private Vector2 circleCenter = Vector2.zero;

    /// <summary>
    /// The current displacement on the circle used for the random movement.
    /// </summary>
    private Vector2 displacement = Vector2.zero;

    /// <summary>
    /// Angle used to wander in the last frame.
    /// </summary>
    private float wanderAngle = 0f;

    /// <summary>
    /// Initialization of the script.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        UpdateRandomPosition();
    }

    /// <summary>
    /// Updates the current circle center and displacement based on the
    /// current's object direction.
    /// </summary>
    private void UpdateRandomPosition()
    {
        circleCenter = (Vector2) transform.position + (manager.Direction * circleDistance);

        float angle = Random.Range(-360f, 360f);
        wanderAngle += angle * Time.deltaTime;

        displacement = new Vector2(circleRadius, 0f);
        displacement = displacement.Rotate(wanderAngle);
    }

    /// <summary>
    /// Calculates the desired velocity of this steering behaviour.
    /// </summary>
    /// <param name="targetPos">Vector2 with the target position to consider.</param>
    /// <returns>Vector2 with the desired velocity for the object according to this
    /// steering behaviour.</returns>
    protected override Vector2 CalculateDesiredVelocity(Vector2 targetPos)
    {
        Vector2 velocity = ((circleCenter + displacement) - (Vector2) transform.position).normalized * manager.MaxSpeed;
        UpdateRandomPosition();
        return velocity;
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
            Gizmos.DrawWireSphere(circleCenter, circleRadius);
            Gizmos.DrawSphere(circleCenter + displacement, 0.05f);
        }
    }
}
