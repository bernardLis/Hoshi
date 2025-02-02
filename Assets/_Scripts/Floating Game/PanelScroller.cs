using System;
using System.Collections;
using System.Collections.Generic;
using Hoshi.Core;
using UnityEngine;

namespace Hoshi.Floating_Game
{
    public class PanelScroller : MonoBehaviour
    {
        [SerializeField] GameObject _panelPrefab;
        [SerializeField] float _speed;


        List<PanelController> _panels;

        IEnumerator _scrollCoroutine;

        [SerializeField] List<ColorList> _colorLists = new();


        void Start()
        {
            InstantiatePanels();

            FloatingGameManager.Instance.OnFloatingGameStarted += StartScrolling;
            MusicFrequencyManager.Instance.OnFrequencyBandUpdate += InstanceOnOnFrequencyBandUpdate;
        }

        void InstanceOnOnFrequencyBandUpdate(float[] audioBandBuffer, float amplitudeBuffer, float smootherBuffer)
        {
            _speed = smootherBuffer * 20;
        }

        void InstantiatePanels()
        {
            _panels = new();
            for (int i = -1; i < 2; i++)
            {
                PanelController panel = Instantiate(_panelPrefab, transform).GetComponent<PanelController>();
                panel.Initialize(_colorLists[i + 1]);
                panel.transform.localPosition = new(1.9f, -15f * i, 0);
                panel.gameObject.SetActive(false);
                _panels.Add(panel);
            }
        }

        void StartScrolling()
        {
            foreach (PanelController panel in _panels)
            {
                panel.gameObject.SetActive(true);
                panel.Run();
            }

            _scrollCoroutine = ScrollingCoroutine();
            StartCoroutine(_scrollCoroutine);
        }

        IEnumerator ScrollingCoroutine()
        {
            while (true)
            {
                if (this == null) yield break;

                foreach (PanelController panel in _panels)
                {
                    panel.transform.Translate(Vector3.down * (_speed * Time.deltaTime));
                    if (panel.transform.position.y < -17f)
                    {
                        float highest = -17;
                        PanelController highestPanel = panel;

                        foreach (PanelController otherPanels in _panels)
                        {
                            if (otherPanels == panel) continue;
                            if (otherPanels.transform.position.y > highest)
                            {
                                highest = otherPanels.transform.position.y;
                                highestPanel = otherPanels;
                            }
                        }

                        panel.transform.position = new(panel.transform.position.x,
                            highestPanel.transform.position.y + 15f, panel.transform.position.z);
                    }
                }

                yield return null;
            }
        }
    }

    [Serializable]
    public struct ColorList
    {
        public List<Color> Colors;
    }
}