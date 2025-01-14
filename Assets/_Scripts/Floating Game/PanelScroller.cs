using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hoshi
{
    public class PanelScroller : MonoBehaviour
    {
        [SerializeField] GameObject _panelPrefab;
        [SerializeField] float _speed;

        List<PanelController> _panels;

        IEnumerator _scrollCoroutine;

        void Start()
        {
            InstantiatePanels();
            //FloatingGameManager.Instance.OnFloatingGameStarted += StartScrolling;
            StartScrolling();
        }

        void InstantiatePanels()
        {
            _panels = new();
            for (int i = -1; i < 2; i++)
            {
                PanelController panel = Instantiate(_panelPrefab, transform).GetComponent<PanelController>();
                panel.Initialize();
                panel.transform.localPosition = new(1.9f, -15f * i, 0);
                _panels.Add(panel);
            }
        }

        void StartScrolling()
        {
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
                    panel.transform.Translate(Vector3.down * _speed * Time.deltaTime);
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

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}