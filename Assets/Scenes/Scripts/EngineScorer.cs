using System;
using System.Collections.Generic;

public class EngineScorer
{
    private int sumTickCount = 25; // 0.5 sec on unity
    private float thresholdEngineShaftRPMDiffSumTick = 1000;

    private Queue<float> engineShaftRPMDiffQueue = new Queue<float>();

    private float previousEngineShaftSpeed;

    public EngineScorer()
    {
        for (int i = 0; i < sumTickCount; i++)
        {
            engineShaftRPMDiffQueue.Enqueue(0);
        }
    }

    // safe less than 1f
    public float CalcEngineShaftRPMScore(float engineShaftRPM)
    {
        engineShaftRPMDiffQueue.Enqueue(Math.Abs(engineShaftRPM - previousEngineShaftSpeed));
        engineShaftRPMDiffQueue.Dequeue();
        previousEngineShaftSpeed = engineShaftRPM;

        float diffSumTick = 0;
        foreach (float diff in engineShaftRPMDiffQueue)
        {
            diffSumTick += diff;
        }
        return diffSumTick / thresholdEngineShaftRPMDiffSumTick;
    }
}
