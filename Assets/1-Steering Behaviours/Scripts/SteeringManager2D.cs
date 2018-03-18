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

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies a set of Reynolds's Steering Behaviours implemented in 2D to the game object
/// this script is attached to.
/// At each frame, the movement result is the average of all intended steering from all
/// the individual steering behaviours attached to the game object.
/// </summary>
public class SteeringManager2D: MonoBehaviour
{
    /// <summary>
    /// List of update modes available for the steering behaviours.
    /// </summary>
    public enum UpdateModes
    {
        /// <summary>
        /// Indicates that the steering behaviour updates shall happen in the regular
        /// update calls.
        /// </summary>
        Update = 0,

        /// <summary>
        /// Indicates that the steering behaviour updates shall happen in the late
        /// update calls.
        /// </summary>
        LateUpdate,

        /// <summary>
        /// Indicates that the steering behaviour updates shall happen in the fixed
        /// update calls.
        /// </summary>
        FixedUpdate
    }

    /// <summary>
    /// The update mode used by the steering behaviours attached to this component.
    /// </summary>
    public UpdateModes updateMode = UpdateModes.Update;

    /// <summary>
    /// Identifies if the steering behaviours are active or not.
    /// </summary>
    [SerializeField]
    private bool active = true;

    /// <summary>
    /// Public property to control the access to the <c>active</c> class attribute.
    /// </summary>
    public bool Active
    {
        get { return active; }
        set
        {
            if(active != value)
                velocity = Vector2.zero;
            active = value;
        }
    }

    /// <summary>
    /// Defines the maximum speed (in unities per second) of the object using this steering behaviour.
    /// </summary>
    [SerializeField]
    private float maxSpeed = 5f;

    /// <summary>
    /// Public property to control the access to the <c>maxSpeed</c> class attribute.
    /// </summary>
    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = Mathf.Max(0f, value); }
    }

    /// <summary>
    /// Defines the mass (in kilograms) of the object using the steering behaviours.
    /// </summary>
    [SerializeField]
    private float mass = 1f;

    /// <summary>
    /// Public property to control the access to the <c>mass</c> class attribute.
    /// </summary>
    public float Mass
    {
        get { return mass; }
        set { mass = Mathf.Max(1f, value); }
    }

    /// <summary>
    /// Indicates if the object shall always rotates towards its direction of movement
    /// as it responds to the steering behaviours.
    /// </summary>
    public bool rotate = true;

    /// <summary>
    /// Indicates if the animation of the behaviours should be independent of the time scale.
    /// </summary>
    public bool timescaleIndependent = false;

    /// <summary>
    /// Indicates if the debug of visual information should be activated.
    /// </summary>
    public bool debug = true;

    /// <summary>
    /// Current velocity of the object.
    /// </summary>
    private Vector2 velocity = Vector2.zero;

    /// <summary>
    /// Ready-only public property to access the <c>velocity</c> class attribute.
    /// </summary>
    public Vector2 Velocity
    {
        get { return velocity; }
    }

    /// <summary>
    /// Vector with the (normalized) direction of the game object.
    /// </summary>
    private Vector2 direction = Vector2.right;

    /// <summary>
    /// Public read-only property to control the access to the <c>direction</c> class attribute.
    /// </summary>
    public Vector2 Direction
    {
        get { return direction; }
    }

    /// <summary>
    /// List of steering behaviours attached to this game object.
    /// </summary>
    private List<BaseBehaviour2D> behaviours = new List<BaseBehaviour2D>();

    /// <summary>
    /// Initialization of the script.
    /// </summary>
    private void Awake()
    {
        RefreshBehaviours();
    }

    /// <summary>
    /// Captures the event indicating updates of frames.
    /// </summary>
    private void Update()
    {
        if(updateMode == UpdateModes.Update)
            DoUpdate();
    }

    /// <summary>
    /// Captures the event indicating late updates of frames.
    /// </summary>
    private void LateUpdate()
    {
        if(updateMode == UpdateModes.LateUpdate)
            DoUpdate();
    }

    /// <summary>
    /// Captures the event indicating fixed updates of frames.
    /// </summary>
    private void FixedUpdate()
    {
        if(updateMode == UpdateModes.FixedUpdate)
            DoUpdate();
    }

    /// <summary>
    /// Updates the movement based on the attached steering behaviours.
    /// </summary>
    private void DoUpdate()
    {
        // Do nothing if the steering behaviours are not active
        if(!active)
            return;

        // Call all active steering behaviours to calculate the average of their desired velocities
        Vector2 desiredVelocity = Vector2.zero;
        BaseBehaviour2D behaviour;
        int count = 0;
        for(int i = 0; i < behaviours.Count; i++)
        {
            behaviour = behaviours[i];
            if(behaviour.Active)
            {
                desiredVelocity += behaviour.DesiredVelocity;
                count++;
            }
        }
        if(count > 0)
            desiredVelocity /= count;

        // Calculate the steering force
        Vector2 steeringForce = desiredVelocity - velocity;

        // Calculate the acceleration and update the velocity accordingly
        // (using Newton's second law: force = mass x acceleration)
        Vector2 acceleration = steeringForce / mass;
        velocity += acceleration;

        // Move the object in space
        float dt = timescaleIndependent ? Time.unscaledDeltaTime : Time.deltaTime;
        transform.position += (Vector3) velocity * dt;

        // Update the direction based on the velocity vector (if not null)
        // Also, rotate the object if this is configured
        if(velocity.x != 0 || velocity.y != 0)
        {
            direction = velocity.normalized;
            if(rotate)
                LookTowards2D(direction);
        }
    }

    /// <summary>
    /// Rebuild the list of all steering behaviours attached to this game object.
    /// </summary>
    public void RefreshBehaviours()
    {
        behaviours.Clear();
        BaseBehaviour2D[] items = GetComponents<BaseBehaviour2D>();
        for(int i = 0; i < items.Length; i++)
            behaviours.Add(items[i]);
    }

    /// <summary>
    /// Implements a bidimensional LookAt function, which rotates the object around
    /// the z axis only.
    /// </summary>
    /// <param name="dir">Vector2 with the direction to which the object should look to.</param>
    private void LookTowards2D(Vector2 dir)
    {
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    /// <summary>
    /// Draws debug information using gizmos.
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        if(active && debug && Application.isPlaying)
        {
            Gizmos.color = Color.red;
            GizmosExt.DrawArrow(transform.position, (Vector2) transform.position + velocity);
        }
    }
}
