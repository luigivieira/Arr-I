﻿/*
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
/// Implementation of the Evade steering behaviour in 2D. This behaviour makes the object
/// to flee from target where it is moving to, by simply calculating the desired velocity of
/// the flee behaviour towards a future position predicted based on the target's estimated
/// velocity. As with flee regarding seek, evade is also the opposed vector of the pursuit
/// behaviour.
/// </summary>
public class EvadeBehaviour2D: PursuitBehaviour2D
{
    /// <summary>
    /// Calculates the desired velocity of this steering behaviour.
    /// </summary>
    /// <param name="targetPos">Vector2 with the target position to consider.</param>
    /// <returns>Vector2 with the desired velocity for the object according to this
    /// steering behaviour.</returns>
    protected override Vector2 CalculateDesiredVelocity(Vector2 targetPos)
    {
        return base.CalculateDesiredVelocity(targetPos) * -1;
    }
}
