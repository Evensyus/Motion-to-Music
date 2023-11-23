using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public double[] frequencies = new double[4];

    //{ 440.0f, 554.37f, 659.26f, 80.0f};
    //public double frequency = 440.0;

    private double increment0; //波每帧移动的距离，由频率决定
    private double increment1; //波每帧移动的距离，由频率决定
    private double increment2; //波每帧移动的距离，由频率决定
    private double incrementHeavyBeat; //主Beat，波每帧移动的距离

    private double phase0; //波上的实际位置
    private double phase1; //波上的实际位置
    private double phase2; //波上的实际位置
    private double phaseHeavyBeat;//主Beat，波上的实际位置

    private double sampling_frequency = 48000.0; //采样频率

    //public float gain;//增益，振荡器的实际功率或音量
    public float gain = 0.3f;
    public float volume = 0.1f;

    public bool beatJudge = true;

    private float heavyBeatTimer = 0.0f;
    public float timeToNextBeat = 1.0f; //节拍间隔

    //public float amplitude = 1f; //正弦振幅
    //public float frequency = 1f; // 正弦频率
    //private float accumulatedTime = 0f; //累计时间
    //float sinusoidalValue;

    private void Start()
    {
        frequencies[0] = 0f;
        frequencies[1] = 0f;
        frequencies[2] = 0f;
        frequencies[3] = 80.0f;
    }

    private void Update()
    {

        //节拍器，默认一拍0.2秒（可调整），通过调整timeToNextBeat可以调整节拍间隔， 0.2(最好不要)<timeToNextBeat
        heavyBeatTimer += Time.deltaTime;

        if (heavyBeatTimer >= 0.2f && heavyBeatTimer < timeToNextBeat)
        {
            frequencies[3] = 0.0f;
        }
        if (heavyBeatTimer >= timeToNextBeat)
        {
            frequencies[3] = 80.0f;
            heavyBeatTimer = 0.0f;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            frequencies[0] = 440.0f;
            frequencies[1] = 554.37f;
            frequencies[2] = 659.26f;
            //frequencies[3] = 50.0f;
            //gain = volume;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            //gain = 0;
            frequencies[0] = 0f;
            frequencies[1] = 0f;
            frequencies[2] = 0f;
            //frequencies[3] = 0.0f;
        }

        /*
        accumulatedTime += Time.deltaTime; //累积时间
        sinusoidalValue = amplitude * Mathf.Sin(2 * Mathf.PI * frequency * accumulatedTime); //使用正弦函数计算出值
        if (accumulatedTime >= 1 / frequency) //如果时间达到一周期则重置时间
        {
            accumulatedTime = 0;
        }
        */

         
    }

    
    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment0 = frequencies[0] * 2.0 * Mathf.PI / sampling_frequency;
        increment1 = frequencies[1] * 2.0 * Mathf.PI / sampling_frequency;
        increment2 = frequencies[2] * 2.0 * Mathf.PI / sampling_frequency;
        incrementHeavyBeat = frequencies[3] * 2.0 * Mathf.PI / sampling_frequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase0 += increment0;
            phase1 += increment1;
            phase2 += increment2;
            phaseHeavyBeat += incrementHeavyBeat;

            data[i] = (float)(gain * (Mathf.Sin((float)phase0) + Mathf.Sin((float)phase1) + Mathf.Sin((float)phase2)) / 3);
            data[i] += (float)(2 * gain * Mathf.Sin((float)phaseHeavyBeat));
            

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }

            if (phase0 > (Mathf.PI * 2))
            {
                phase0 = 0.0;
            }

            if (phase1 > (Mathf.PI * 2))
            {
                phase1 = 0.0;
            }

            if (phase2 > (Mathf.PI * 2))
            {
                phase2 = 0.0;
            }

            if (phaseHeavyBeat > (Mathf.PI))
            {
                phaseHeavyBeat = 0.0;
            }
        }
    }

    /*单声波执行
    private void OnAudioFilterRead(float[] data, int channels)
    {
        foreach (double currentFrequency in frequencies)
        {
            increment = currentFrequency * 2.0 * Mathf.PI / sampling_frequency;

            for (int i = 0; i < data.Length; i += channels)
            {
                phase += increment;

                //phase2 += increment * 2;

                data[i] = (float)(gain * Mathf.Sin((float)phase));

                if (channels == 2)
                {
                    data[i + 1] = data[i];
                }

                if (phase > (Mathf.PI * 2))
                {
                    phase = 0.0;
                }
            }

        }
    }
    */

    public static float sawWave(float input)
    {
        return (((input + 0.5f) % 1) - 0.5f) * 2;
    }

    public static float squareWave(float input)
    {
        return input < 0.5 ? 1 : -1;
    }
}
