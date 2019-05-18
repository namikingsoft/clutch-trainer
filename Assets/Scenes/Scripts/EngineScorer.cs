using System;
using System.Collections.Generic;

public class EngineScorer
{
    private int sumTickCount = 10; // 0.2 sec on unity
    private float thresholdEngineShaftRPMDiffSumTick = 300f;

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
    public float CalcEngineShaftRPMScore(int gear, float clutchRate, float engineShaftRPM)
    {
        engineShaftRPMDiffQueue.Enqueue(Math.Abs(engineShaftRPM - previousEngineShaftSpeed));
        engineShaftRPMDiffQueue.Dequeue();
        previousEngineShaftSpeed = engineShaftRPM;

        float diffSumTick = 0;
        foreach (float diff in engineShaftRPMDiffQueue)
        {
            diffSumTick += diff;
        }
        float score = diffSumTick / thresholdEngineShaftRPMDiffSumTick;
        return gear > 0 && clutchRate > 0.1f ? score : score / 2.5f;
    }
}
