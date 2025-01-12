using System;
using System.Collections;
using UnityEngine;

namespace Hoshi.Core
{
    public class MusicFrequencyManager : Singleton<MusicFrequencyManager>
    {
        AudioManager _audioManager;

        readonly float _bufferDecreaseValue = 0.005f;
        readonly float _bufferDecreaseRate = 1.2f;

        readonly float[] _spectrumData = new float[512];
        readonly float[] _frequencyBands = new float[8];
        readonly float[] _bandBuffer = new float[8];
        readonly float[] _bufferDecrease = new float[8];

        readonly float[] _frequencyBandHighest = new float[8];
        readonly float[] _audioBand = new float[8];
        readonly float[] _audioBandBuffer = new float[8];

        float _amplitudeHighest;
        float _amplitudeBuffer;

        public event Action<float[], float> OnFrequencyBandUpdate;

        public void Initialize()
        {
            _audioManager = AudioManager.Instance;
            AudioProfile();
            StartCoroutine(UpdateFrequenciesCoroutine());
        }

        void AudioProfile()
        {
            _amplitudeHighest = 5;
            for (int i = 0; i < 8; i++)
            {
                _frequencyBandHighest[i] = 5;
            }
        }

        IEnumerator UpdateFrequenciesCoroutine()
        {
            while (true)
            {
                if (this == null) yield break;
                _audioManager.GetMusicSpectrumData(_spectrumData);
                UpdateFrequencyBands();
                UpdateBandBuffers();
                CreateAudioBands();
                UpdateAmplitude();
                OnFrequencyBandUpdate?.Invoke(_audioBandBuffer, _amplitudeBuffer);

                yield return new WaitForSeconds(0.01f);
            }
        }

        void UpdateAmplitude()
        {
            float currentAmplitude = 0;
            float currentAmplitudeBuffer = 0;

            for (int i = 0; i < _audioBand.Length; i++)
            {
                currentAmplitude += _audioBand[i];
                currentAmplitudeBuffer += _audioBandBuffer[i];
            }

            if (currentAmplitude > _amplitudeHighest)
                _amplitudeHighest = currentAmplitude;

            _amplitudeBuffer = currentAmplitudeBuffer / _amplitudeHighest;
        }

        void CreateAudioBands()
        {
            for (int i = 0; i < _frequencyBands.Length; i++)
            {
                if (_frequencyBands[i] > _frequencyBandHighest[i])
                    _frequencyBandHighest[i] = _frequencyBands[i];

                _audioBand[i] = _frequencyBands[i] / _frequencyBandHighest[i];
                _audioBandBuffer[i] = _bandBuffer[i] / _frequencyBandHighest[i];
            }
        }

        void UpdateFrequencyBands()
        {
            int count = 0;
            for (int i = 0; i < _frequencyBands.Length; i++)
            {
                float average = 0;
                int sampleCount = (int)Math.Pow(2, i) * 2;
                for (int j = 0; j < sampleCount; j++)
                {
                    average += _spectrumData[count] * (count + 1);
                    count++;
                }

                average /= count;
                _frequencyBands[i] = average * 10;
            }
        }

        void UpdateBandBuffers()
        {
            for (int i = 0; i < _frequencyBands.Length; i++)
            {
                if (_frequencyBands[i] > _bandBuffer[i])
                {
                    _bandBuffer[i] = _frequencyBands[i];
                    _bufferDecrease[i] = _bufferDecreaseValue;
                }

                if (_frequencyBands[i] < _bandBuffer[i])
                {
                    _bandBuffer[i] -= _bufferDecrease[i];
                    _bufferDecrease[i] *= _bufferDecreaseRate;
                }
            }
        }
    }
}