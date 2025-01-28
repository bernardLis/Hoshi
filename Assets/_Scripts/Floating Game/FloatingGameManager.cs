using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hoshi.Core;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace Hoshi.Floating_Game
{
    public class FloatingGameManager : Singleton<FloatingGameManager>
    {
        const string _ussCommonSongTitleLetter = "common__song-title-letter";

        [SerializeField] Light2D _globalLight;

        [SerializeField] PlayerStats _playerStats;

        [SerializeField] CinemachineCamera _floatingGameCamera;
        [SerializeField] FloatingPlayerController _floatingPlayerControllerPrefab;
        FloatingPlayerController _floatingPlayerController;
        [SerializeField] GameObject[] _walls;

        [Header("Audio")]
        [SerializeField] Sound _doubleJumpUnlockedSound;

        [SerializeField] AudioSource _tadabadaSource;

        [SerializeField] AudioSource _floatingGameSource;

        AudioManager _audioManager;

        PlayerController _playerController;

        BoxCollider2D _boxCollider2D;

        VisualElement _root;
        Label _artistsLabel;
        VisualElement _songTitleContainerTop;
        VisualElement _songTitleContainerBottom;
        readonly List<Label> _titleLabels = new();
        VisualElement _doubleJumpContainer;

        bool _isFloatingGameStarted;
        public event Action OnFloatingGameStarted;
        public event Action OnFloatingGame10SecondsUntilFinished;

        public event Action<Vector3> OnFloatingGameFinished;

        void Start()
        {
            _audioManager = AudioManager.Instance;

            _root = PlatformerManager.Instance.GetComponent<UIDocument>().rootVisualElement;
            _artistsLabel = _root.Q<Label>("artistsLabel");
            _songTitleContainerTop = _root.Q<VisualElement>("songTitleContainerTop");
            _songTitleContainerBottom = _root.Q<VisualElement>("songTitleContainerBottom");
            _doubleJumpContainer = _root.Q<VisualElement>("doubleJumpContainer");

            _boxCollider2D = GetComponent<BoxCollider2D>();

            _playerStats.MaxAirJumps = 0;
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

            DOTween.To(x => _globalLight.intensity = x, _globalLight.intensity, 0f, 5f)
                .SetEase(Ease.InOutSine);

            _tadabadaSource.Stop();
            _floatingGameSource.Play();

            StartCoroutine(FloatingGameSetupCoroutine());
            StartCoroutine(FinishGameCoroutine());
        }

        IEnumerator FinishGameCoroutine()
        {
            yield return new WaitForSeconds(175f);
            OnFloatingGame10SecondsUntilFinished?.Invoke();
            DOTween.To(x => _globalLight.intensity = x, _globalLight.intensity, 1f, 10f)
                .SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(10f);

            _floatingPlayerController.DisableMovement();
            _floatingGameCamera.transform.DOMoveY(1.2f, 4f);
            _floatingPlayerController.transform.DOMoveY(-4.5f, 4f);
            yield return new WaitForSeconds(4f);
            _floatingGameCamera.Priority = -1;
            _playerController.transform.position = _floatingPlayerController.transform.position;
            _floatingPlayerController.gameObject.SetActive(false);
            _playerController.gameObject.SetActive(true);

            GetComponent<MusicFrequencyManager>().Stop();

            _playerStats.MaxAirJumps = 1;
            DisplayDoubleJumpText();

            _walls[1].SetActive(false);

            OnFloatingGameFinished?.Invoke(_playerController.transform.position);


            yield return null;
        }

        void DisplayDoubleJumpText()
        {
            _doubleJumpContainer.style.opacity = 0;
            _doubleJumpContainer.style.display = DisplayStyle.Flex;
            DOTween.To(x => _doubleJumpContainer.style.opacity = x,
                _doubleJumpContainer.style.opacity.value, 1, 0.5f).SetDelay(2f).OnComplete(() =>
            {
                _audioManager.CreateSound().WithSound(_doubleJumpUnlockedSound).Play();
            });

            DOTween.To(x => _doubleJumpContainer.style.opacity = x,
                    _doubleJumpContainer.style.opacity.value, 0, 0.5f)
                .SetDelay(5f).OnComplete(() => _doubleJumpContainer.style.display = DisplayStyle.None);
        }

        IEnumerator FloatingGameSetupCoroutine()
        {
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
            _floatingPlayerController =
                Instantiate(_floatingPlayerControllerPrefab, _playerController.transform.position, Quaternion.identity);
            _floatingPlayerController.Initialize(_playerController.GetComponent<Rigidbody2D>().linearVelocity);

            _playerController.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);

            OnFloatingGameStarted?.Invoke();
        }
    }
}