using Google.Protobuf.WellKnownTypes;
using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlantCaretakerAgent : Agent
{
    // number of days to run an episode
    public int duration;

    // how often the plant should be repotted
    public int repottingFrequency;

    // to keep track of the number of ticks since the agent last repoted the plant
    private int ticksSinceLastRepot;


    public GameObject PlantSimulator;
    private PlantSimulatorController SimRef;
    PlantBehavior testPlant = new PlantBehavior();

    public override void Initialize()
    {
        SimRef = PlantSimulator.GetComponent<PlantSimulatorController>();
    }

    public override void OnEpisodeBegin()
    {
        SimRef.ResetSimulator();

        testPlant = new PlantBehavior();

        // Reset counters
        dayCount = 0;
        ticksSinceLastRepot = 0;
    }



    private void NuturePlant(ActionSegment<int> act)
    {
        var action = act[0];

        switch (action)
        {
            case 0:
                // Do nothing
                break;
            case 1:
                SimRef.Water(ref testPlant);
                break;
            case 2:
                float nutrients = testPlant.state.currSoilNutrients;

                if (nutrients > 0.7f)
                {
                    AddReward(-0.5f);
                }
                else
                {
                    AddReward(0.5f);
                }

                SimRef.Repot(ref testPlant);

                ticksSinceLastRepot = 0;

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

        AddReward(healthDiff * 10f);

        // small reward for maintaining surface health, test with and without it
        AddReward(testPlant.state.currSurfaceHealth * 0.01f);

        // Update counters
        dayCount += 1;
        ticksSinceLastRepot += 1;

        if (testPlant.state.currSurfaceHealth <= 0.0f)
        {
            Debug.Log("we died");
            AddReward(-10f);
            EndEpisode();
        }
        else if (dayCount >= duration)
        {
            Debug.Log("we made it");
            AddReward(testPlant.state.currSurfaceHealth * 10f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // we will give it the plants current health for it to obervb
        // this needs to be tested
        //sensor.AddObservation(10);
        sensor.AddObservation(testPlant.state.currSurfaceHealth);
        sensor.AddObservation(testPlant.state.currPlantHeight);

        sensor.AddObservation(testPlant.state.currSoilDryness);
        sensor.AddObservation(testPlant.state.currSoilNutrients);

        sensor.AddObservation(testPlant.state.currPlantWetness);
        sensor.AddObservation(testPlant.state.currStoredWater);

        //sensor.AddObservation(Mathf.Clamp01((float)ticksSinceLastRepot / Mathf.Max(1, repottingFrequency)));

        //var environment = SimRef.GetEnvironment();

        //sensor.AddObservation(environment.lightIntensity);
        //sensor.AddObservation(environment.bIndirectLight ? 1f : 0f);
        //sensor.AddObservation(environment.tempurature/100f);
        //sensor.AddObservation(environment.humidity);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;

        // for manual testing
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            actions[0] = 2;
        }
        else
            actions[0] = 0;
    }
}
