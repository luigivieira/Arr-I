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
/// Implements the behaviour of the Chasing state.
/// </summary>
public class ChasingStateBehaviour: StateMachineBehaviour
{
    /// <summary>
    /// Reference to the player's avatar, cached to improve performance.
    /// </summary>
    private GameObject avatar = null;

    /// <summary>
    /// Reference to the agent's sensors, cached to improve performance.
    /// </summary>
    private AgentSensors sensors = null;

    /// <summary>
    /// Captures the event indicating that the state machine just entered this state.
    /// </summary>
    /// <param name="agent">Instance of the agent animator.</param>
    /// <param name="stateInfo">Data of the state machine behaviour for this state.</param>
    /// <param name="layerIndex">Layer of the state machine in which this state is on.</param>
    override public void OnStateEnter(Animator agent, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.GetComponent<SteeringManager2D>().MaxSpeed = 2f;
        agent.GetComponent<SeekBehaviour2D>().Active = true;
    }

    /// <summary>
    /// Captures the event indicating that the state machine just exited this state.
    /// </summary>
    /// <param name="agent">Instance of the agent animator.</param>
    /// <param name="stateInfo">Data of the state machine behaviour for this state.</param>
    /// <param name="layerIndex">Layer of the state machine in which this state is on.</param>
    override public void OnStateExit(Animator agent, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.GetComponent<SeekBehaviour2D>().Active = false;
    }

    /// <summary>
    /// Update event called while the state machine is in this state.
    /// In this state, the agent is seeking the player's avatar. If the player's avatar is no 
    /// longer "seen", moves the FSM back to the <Patrolling> state.
    /// </summary>
    /// <param name="agent">Instance of the agent animator.</param>
    /// <param name="stateInfo">Data of the state machine behaviour for this state.</param>
    /// <param name="layerIndex">Layer of the state machine in which this state is on.</param>
    override public void OnStateUpdate(Animator agent, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!avatar)
            avatar = GameObject.Find("Ship Avatar");
        if(!sensors)
            sensors = agent.GetComponent<AgentSensors>();

        if(!sensors.IsObjectSeen(avatar))
            agent.SetTrigger("Patrolling");
    }
}
