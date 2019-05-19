using System;
using System.Collections.Generic;

public class EngineScorer
{
    private int sumTickCount = 10; // 0.2 sec on unity
    private float thresholdEngineShaftRPMDiffSumTick = 300f;
    private float beCalcMinEngineShaftRPM = 1200f; // startRPM + alpha

    private Queue<float> engineShaftRPMDiffQueue = new Queue<float>();

    private float previousEngineShaftRPM;

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
        engineShaftRPMDiffQueue.Enqueue(Math.Abs(engineShaftRPM - previousEngineShaftRPM));
        engineShaftRPMDiffQueue.Dequeue();
        previousEngineShaftRPM = engineShaftRPM;

        if (engineShaftRPM < beCalcMinEngineShaftRPM) return 0;

        float diffSumTick = 0;
        foreach (float diff in engineShaftRPMDiffQueue)
        {
            diffSumTick += diff;
        }
        float score = diffSumTick / thresholdEngineShaftRPMDiffSumTick;
        return gear > 0 && clutchRate > 0.1f ? score : score / 2.5f;
    }
}
