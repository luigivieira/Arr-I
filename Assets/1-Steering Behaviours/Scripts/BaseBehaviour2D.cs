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
/// Base class for all Reynolds's Steering Behaviours implemented in 2D.
/// </summary>
[RequireComponent(typeof(SteeringManager2D))]
public abstract class BaseBehaviour2D: MonoBehaviour
{
    /// <summary>
    /// Indicates if the debug of visual information should be activated.
    /// </summary>
    public bool debug = true;

    /// <summary>
    /// Identifies if this steering behaviour is active or not.
    /// </summary>
    [SerializeField]
    private bool active = true;

    /// <summary>
    /// Public property to control the access to the <c>active</c> class attribute.
    /// </summary>
    public virtual bool Active
    {
        get { return active; }
        set
        {
            bool triggerChanges = false;
            if(active != value)
                triggerChanges = true;

            active = value;

            if(triggerChanges)
                GetComponent<SteeringManager2D>().RefreshBehaviours();
        }
    }

    /// <summary>
    /// Reference to the target for the steering behaviour.
    /// </summary>
    public Transform target;

    /// <summary>
    /// The weight of the behaviour response.
    /// </summary>
    [SerializeField]
    private float weight = 1f;

    /// <summary>
    /// Public property to control the access to the <c>weight</c> class attribute.
    /// </summary>
    public float Weight
    {
        get { return weight; }
        set { weight = Mathf.Max(0f, Mathf.Min(value, 1f)); }
    }

    /// <summary>
    /// Reference to the steering manager in this object.
    /// </summary>
    protected SteeringManager2D manager;

    /// <summary>
    /// Initialization of the script.
    /// </summary>
    protected virtual void Awake()
    {
        manager = GetComponent<SteeringManager2D>();
    }

    /// <summary>
    /// Abstract public read-only property used to allow the steering manager to obtain
    /// the desired velocity of a given steering behaviour.
    /// This property may be overwritten by the subclasses if custom internal actions are needed,
    /// but the base implementation simply calls the protected method <c>CalculateDesiredVelocity.</c>
    /// But if this property is overhidden, DO NOT FORGET to multiply the result by the configured
    /// weight!
    /// </summary>
    /// <param name="mass">Float with the mass of the object.</param>
    public virtual Vector2 DesiredVelocity
    {
        get
        {
            Vector2 targetPos = target != null ? (Vector2) target.position : Vector2.zero;
            return CalculateDesiredVelocity(targetPos) * Weight;
        }
    }

    /// <summary>
    /// Abstract and protected helper method used to calculate the desired velocity based
    /// on a given target position. The intention of having this method separated from the
    /// get method interface above is to allow its reuse among children classes in the
    /// behaviour inheritance hierarchy (using the predicted position of the target instead
    /// of its current one, for example).
    /// This method MUST be implemented by all the subclasses, since it is the core
    /// functionality for all different behaviours.
    /// </summary>
    /// <param name="targetPos">Vector2 with the target position to consider.</param>
    /// <returns>Vector2 with the desired velocity for the object according to this
    /// steering behaviour.</returns>
    protected abstract Vector2 CalculateDesiredVelocity(Vector2 targetPos);

    /// <summary>
    /// Draws debug information using gizmos.
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        if(active && debug && Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            GizmosExt.DrawArrow(transform.position, (Vector2) transform.position + DesiredVelocity);
        }
    }
}
