using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlantCaretakerAgent : Agent
{
    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log(actions.DiscreteActions[0]);
        //Debug.Log(actions.ContinuousActions[0]);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(10);
    }
}
