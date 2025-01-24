using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hoshi.Core;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hoshi
{
    public class FloatingGameManager : Singleton<FloatingGameManager>
    {
        const string _ussCommonSongTitleLetter = "common__song-title-letter";

        BoxCollider2D _boxCollider2D;

        [SerializeField] CinemachineCamera _floatingGameCamera;
        [SerializeField] FloatingPlayerController _floatingPlayerControllerPrefab;
        [SerializeField] GameObject[] _walls;

        [Header("Audio")]
        [SerializeField] AudioSource _tadabadaSource;

        PlayerController _playerController;

        VisualElement _root;
        Label _artistsLabel;
        VisualElement _songTitleContainerTop;
        VisualElement _songTitleContainerBottom;
        readonly List<Label> _titleLabels = new();

        bool _isFloatingGameStarted;
        public event Action OnFloatingGameStarted;

        void Start()
        {
            _root = PlatformerManager.Instance.GetComponent<UIDocument>().rootVisualElement;
            _artistsLabel = _root.Q<Label>("artistsLabel");
            _songTitleContainerTop = _root.Q<VisualElement>("songTitleContainerTop");
            _songTitleContainerBottom = _root.Q<VisualElement>("songTitleContainerBottom");

            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                if (playerController.transform.position.x > _boxCollider2D.bounds.max.x)
                {
                    _playerController = playerController;
                    StartSetup();
                }
            }
        }

        void StartSetup()
        {
            if (_isFloatingGameStarted) return;
            _isFloatingGameStarted = true;

            GetComponent<MusicFrequencyManager>().Initialize();

            _tadabadaSource.Stop();
            GetComponent<AudioSource>().Play();

            // StartCoroutine(SyncAudioCoroutine());
            StartCoroutine(FloatingGameSetupCoroutine());
        }

        // IEnumerator SyncAudioCoroutine()
        // {
        //     // while (_tadabadaSource.isPlaying)
        //     // {
        //     //     yield return null;
        //     // }
        //
        // }

        IEnumerator FloatingGameSetupCoroutine()
        {
            foreach (GameObject wall in _walls) wall.SetActive(true);
            _walls[^1].SetActive(false); // south
            _floatingGameCamera.Priority = 1;
            yield return new WaitForSeconds(2f);

            _artistsLabel.style.display = DisplayStyle.Flex;

            yield return new WaitForSeconds(4f);

            _artistsLabel.style.display = DisplayStyle.None;
            yield return PrintSongTitle();
            yield return new WaitForSeconds(2f);


            yield return StartFloatingGame();
        }

        IEnumerator PrintSongTitle()
        {
            _songTitleContainerTop.Clear();
            _songTitleContainerBottom.Clear();
            _titleLabels.Clear();

            string title = "PERSEFONA";

            _songTitleContainerTop.style.display = DisplayStyle.Flex;
            _songTitleContainerBottom.style.display = DisplayStyle.Flex;
            VisualElement currentContainer = _songTitleContainerTop;
            foreach (char c in title)
            {
                if (c.ToString() == "F") currentContainer = _songTitleContainerBottom;
                Label l = new();
                l.AddToClassList(_ussCommonSongTitleLetter);
                l.text = c.ToString();
                _titleLabels.Add(l);
                currentContainer.Add(l);
                yield return new WaitForSeconds(0.4f);
            }

            for (int i = 0; i < title.Length; i++)
            {
                _titleLabels[i].text = "";
                yield return new WaitForSeconds(0.4f);
            }

            _songTitleContainerTop.style.display = DisplayStyle.None;
            _songTitleContainerBottom.style.display = DisplayStyle.None;

        }

        IEnumerator StartFloatingGame()
        {
            _playerController.StartFloatingGame();
            yield return new WaitForSeconds(0.2f);
            _floatingGameCamera.transform.DOMoveY(3, 1f);
            FloatingPlayerController floatingPlayerController =
                Instantiate(_floatingPlayerControllerPrefab, _playerController.transform.position, Quaternion.identity);
            floatingPlayerController.Initialize(_playerController.GetComponent<Rigidbody2D>().linearVelocity);

            _playerController.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);

            OnFloatingGameStarted?.Invoke();
        }
    }
}