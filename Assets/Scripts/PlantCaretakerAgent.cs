using Google.Protobuf.WellKnownTypes;
using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlantCaretakerAgent : Agent
{
    public int duration;

    public GameObject PlantSimulator;
    private PlantSimulatorController SimRef;
    PlantBehavior testPlant = new PlantBehavior();

    public override void Initialize()
    {
        SimRef = PlantSimulator.GetComponent<PlantSimulatorController>();
    }

    private void NuturePlant(ActionSegment<int> act)
    {
        var action = act[0];

        switch (action)
        {
            case 1:
                SimRef.Water(ref testPlant);
                break;
            case 2:
                SimRef.Repot(ref testPlant);
                break;
        }
    }

    private int dayCount = 0;

    public override void OnActionReceived(ActionBuffers actions)
    {
        // take an action
        NuturePlant(actions.DiscreteActions);

        // move on to the next day
        float healthDiff = SimRef.Tick(ref testPlant);

        // increment day count
        dayCount += 1;

        // check if the plant is healthier after the agents actions 
        if(healthDiff > 0) 
            AddReward(1f); // reward if plant health improved
        else
            AddReward(-1f); // punish if plant health decressed


        if (testPlant.state.currSurfaceHealth <= 0.0f)
        {
            Debug.Log("we died");
            AddReward(-10f);
            EndEpisode();
        }
        else if (dayCount >= duration)
        {
            Debug.Log("we made it");
            AddReward(10f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(10);
    }

    public override void OnEpisodeBegin()
    {
        SimRef.ResetSimulator();

        testPlant = new PlantBehavior();

        dayCount = 0;
    }
}
