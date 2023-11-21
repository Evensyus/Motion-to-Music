using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Au : MonoBehaviour
{
    public double frequency = 440.0;
    private double increment; //波每帧移动的距离，由频率决定
    private double phase; //波上的实际位置
    private double sampling_frequency = 48000.0; //采样频率

    //private double phase2;

    public float gain;//增益，振荡器的实际功率或音量

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;

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


