using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PopoAgent : Agent
{

    [SerializeField]
    public Main mainScript;

    public override void OnEpisodeBegin()
    {
        if(mainScript.TRAINING){
            mainScript.RestartGame();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent positions
        sensor.AddObservation(this.transform.localPosition);

        // foreach(GameObject fruit in mainScript.fruits) {
        //     sensor.AddObservation(fruit.transform.localPosition);
        // }
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        mainScript.character.Move(actionBuffers.ContinuousActions[0]);
    }

}
