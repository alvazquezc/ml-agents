using UnityEngine;
using System.Collections.Generic;
using MLAgents.Sensor;
using System;

namespace MLAgents
{
    /// <summary>
    /// The Remote Policy only works when training.
    /// When training your Agents, the RemotePolicy will be controlled by Python.
    /// </summary>
    public class RemotePolicy : IPolicy
    {

        int m_AgentId;
        string m_FullyQualifiedBehaviorName;

        protected ICommunicator m_Communicator;

        /// <inheritdoc />
        public RemotePolicy(
            BrainParameters brainParameters,
            string fullyQualifiedBehaviorName)
        {
            m_FullyQualifiedBehaviorName = fullyQualifiedBehaviorName;
            m_Communicator = Academy.Instance.Communicator;
            m_Communicator.SubscribeBrain(m_FullyQualifiedBehaviorName, brainParameters);
        }

        /// <inheritdoc />
        public void RequestDecision(AgentInfo info, List<ISensor> sensors)
        {
            m_AgentId = info.episodeId;
            m_Communicator?.PutObservations(m_FullyQualifiedBehaviorName, info, sensors);
        }

        /// <inheritdoc />
        public float[] DecideAction()
        {
            m_Communicator?.DecideBatch();
            return m_Communicator?.GetActions(m_FullyQualifiedBehaviorName, m_AgentId);

        }

        public void Dispose()
        {
        }
    }
}
