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
/// Implements the sensors for the agent used in the Finite State Machine demo.
/// </summary>
public class AgentSensors: MonoBehaviour
{
    /// <summary>
    /// Radius of the sensoring ahead of the agent.
    /// </summary>
    public float sensorRadius = 5f;

    /// <summary>
    /// Angle of the sensoring ahead of the agent.
    /// </summary>
    public float sensorAngle = 45f;

    /// <summary>
    /// Check if the the given game object is seen by the agent, according
    /// to its sensor configuration.
    /// </summary>
    /// <param name="avatar">Reference to the agent's game object.</param>
    /// <returns>True if the player is seen, false otherwise.</returns>
    public bool IsObjectSeen(GameObject avatar)
    {
        Vector2 dir = avatar.transform.position - transform.position;
        float distance = dir.magnitude;
        Vector2 forward = transform.right; // Because of the way the sprite is created, in 2D
        float angle = Mathf.Abs(Vector2.SignedAngle(dir, forward));
        return (angle < sensorAngle && distance < sensorRadius);
    }

    /// <summary>
    /// Draws debug information using gizmos.
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Vector2 ray = transform.right * sensorRadius;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sensorRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, Vector2Extension.Rotate(ray, -sensorAngle));
        Gizmos.DrawRay(transform.position, Vector2Extension.Rotate(ray, sensorAngle));
    }
}
