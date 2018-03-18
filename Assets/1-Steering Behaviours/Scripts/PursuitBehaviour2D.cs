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

using System.Collections;
using UnityEngine;

/// <summary>
/// Implementation of the Pursuit steering behaviour in 2D. This behaviour makes the object
/// to chase the target where it is moving to, by simply calculating the desired velocity of
/// the seek behaviour towards a future position predicted based on the target's estimated
/// velocity.
/// </summary>
public class PursuitBehaviour2D: SeekBehaviour2D
{
    /// <summary>
    /// Max number of future steps for the prediction of the target position.
    /// </summary>
    [SerializeField]
    private int maxFutureSteps = 5;

    /// <summary>
    /// Public property to control the access to the <c>maxFutureSteps</c> class attribute.
    /// </summary>
    public int MaxFutureSteps
    {
        get { return maxFutureSteps; }
        set { maxFutureSteps = Mathf.Max(value, 0); }
    }

    /// <summary>
    /// Coroutine used for the estimation of the target velocity.
    /// </summary>
    private Coroutine estimationCoroutine;

    /// <summary>
    /// The estimated target velocity.
    /// </summary>
    protected Vector2 targetVelocity = Vector2.zero;

    /// <summary>
    /// Overload the property to restart/pause the coroutine as the behaviour
    /// is activated/deactivated.
    /// </summary>
    override public bool Active
    {
        get { return base.Active; }
        set
        {
            bool diff = base.Active != value;
            base.Active = value;
            if(diff & Application.isPlaying)
            {
                if(value)
                    estimationCoroutine = StartCoroutine(EstimateTargetVelocity());
                else
                {
                    StopCoroutine(estimationCoroutine);
                    estimationCoroutine = null;
                }
            }
        }
    }

    /// <summary>
    /// Initialization of the script.
    /// </summary>
    protected void Start()
    {
        if(Active)
            estimationCoroutine = StartCoroutine(EstimateTargetVelocity());
        else
            estimationCoroutine = null;
    }

    /// <summary>
    /// Coroutine used to estimate the target's velocity based on its displacement
    /// over time.
    /// </summary>
    /// <returns>Enumerator to be used by the coroutine.</returns>
    private IEnumerator EstimateTargetVelocity()
    {
        targetVelocity = Vector2.zero;
        Vector2 lastTargetPos = target.position;

        while(Active)
        {
            // Wait for one second to allow for the target to move
            yield return new WaitForSeconds(1f);

            // Estimate the velocity of the target based on its basic formula: v = s/t
            // (where t is fixed as t = 1)
            targetVelocity = (Vector2) target.position - lastTargetPos;

            // Update the last known target position
            lastTargetPos = target.position;
        }

        targetVelocity = Vector2.zero;
    }

    /// <summary>
    /// Public read-only property to obtain the desired velocity of this steering behaviour.
    /// </summary>
    public override Vector2 DesiredVelocity
    {
        get
        {
            // Calculate the number of steps into the future to try to predict
            // the position of the target
            float distance = Vector2.Distance(transform.position, target.position);

            // The number of steps in the future for the prediction is proportional
            // to the distance from the target, so the object does not waste time
            // trying to reach a very wrong prediction when it gets closer to the
            // target (instead, going straight to it!)
            int futureSteps = Mathf.RoundToInt(distance / manager.MaxSpeed);

            // The number of future steps is also limited to a configured maximum, so
            // it does not wander too much if the target is very quick
            futureSteps = Mathf.Min(maxFutureSteps, futureSteps);

            // Predict the position of the target in the future based on its
            // current estimated velocity - the desired velocity is the same as
            // in the seek behaviour (parent of this class) but towards the predicted
            // target position
            Vector2 predTarget = (Vector2) target.position + targetVelocity * futureSteps;
            return CalculateDesiredVelocity(predTarget) * Weight;
        }
    }
}
