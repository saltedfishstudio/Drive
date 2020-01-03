using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace SaltedFishStudio.RoadKill.Manager
{
    using UI;
    
    public class GameManager : MonoBehaviour
    {
        // Public Properties
        public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public int m_ShellRandomRange = 20;
        public int m_ShellForce = 25;
        public int m_ShellWaveCount = 10;
        public float m_ShellDelay = 0.1f;
        public TankManager[] m_Tanks;               // A collection of managers for enabling and disabling different aspects of the tanks.

        public MainMenuCanvas mainMenuCanvas;
        public GameCanvas gameCanvas;
        public EndCanvas endCanvas;

        private Movement m_Player1Movement;
        private Crush m_Player1Shooting;
        private Health m_Player1Health;

        private void OnEnable()
        {
            BindMainMenuCanvas();
            BindGameCanvas();
            BindEndCanvas();
        }

        void BindMainMenuCanvas()
        {
            mainMenuCanvas.playButton.onClick.RemoveAllListeners();
            mainMenuCanvas.exitButton.onClick.RemoveAllListeners();
            
            mainMenuCanvas.playButton.onClick.AddListener(StartRound);
            mainMenuCanvas.exitButton.onClick.AddListener(Application.Quit);
        }

        void BindGameCanvas()
        {
            
        }

        void BindEndCanvas()
        {
            endCanvas.mainMenuButton.onClick.RemoveAllListeners();
            endCanvas.mainMenuButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });
        }

        // Start
        // Just go to main menu.
        private void Start()
        {
#if !UNITY_EDITOR
            if (Screen.fullScreen)
                Screen.fullScreen = false;
#endif

            GoToMainMenu();
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            // If any of our UI Labels have not been bound, do nothing.
            if (
                // m_SpeedLabel == null || 
                m_Tanks.Length == 0 || m_Player1Movement == null || m_Player1Health == null)
                return;

            // Player is dead..
            if (m_Player1Health.m_Dead)
                EndRound();

            // Update UI label text.
            // m_SpeedLabel.text = m_Player1Movement.m_Speed.ToString();

            // Update UI label text.
            var kills = m_Tanks.Length;
            foreach (var tank in m_Tanks)
                if (tank.m_Instance.activeSelf)
                    kills--;
            // m_KillsLabel.text = kills.ToString();

            // Update UI label text.
            var fireCount = m_Player1Shooting.m_FireCount;
            // m_ShotsLabel.text = fireCount.ToString();

            // Update UI label text.
            var hitCount = m_Player1Shooting.m_HitCount;
            if (fireCount == 0)
                fireCount = 1; // Avoid div by 0.
            var percent = (int)(((float)hitCount / (float)fireCount) * 100);
            // m_AccuracyLabel.text = percent.ToString();
        }

        void SetScreenEnableState(ICanvas canvas, bool state)
        {
            if (state)
            {
                canvas.Enable();
            }
            else
            {
                canvas.Disable();
            }
        }

        IEnumerator TransitionScreens(ICanvas from, ICanvas to)
        {
            from.Disable();

            to.Enable();

            yield return null;
            yield return null;
            yield return null;
        }

        private void SpawnAllTanks()
        {
            // For all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                var ran = Random.Range(0, 180);
                var rot = Quaternion.Euler(0, ran, 0);

                // ... create them, set their player number and references needed for control.
                m_Tanks[i].m_Instance =
                    Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, rot) as GameObject;
                m_Tanks[i].m_Instance.transform.localRotation = rot;
                m_Tanks[i].m_PlayerNumber = i + 1;
                m_Tanks[i].Setup();
            }

            var instance = m_Tanks[0].m_Instance;
            m_Player1Movement = instance.GetComponent<Movement>();
            m_Player1Shooting = instance.GetComponent<Crush>();
            m_Player1Health = instance.GetComponent<Health>();
        }

        private void SetCameraTargets()
        {
            // Create a collection of transforms the same size as the number of tanks.
            Transform[] targets = new Transform[1];

            // Just add the first tank to the transform.
            targets[0] = m_Tanks[0].m_Instance.transform;

            // These are the targets the camera should follow.
            m_CameraControl.m_Targets = targets;
        }

        private void GoToMainMenu()
        {
            SetScreenEnableState(mainMenuCanvas, true);
            SetScreenEnableState(gameCanvas, false);
            SetScreenEnableState(endCanvas, false);
        }

        private void StartRound()
        {
            SpawnAllTanks();
            SetCameraTargets();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            m_CameraControl.SetStartPositionAndSize();

            // As soon as the round begins playing let the players control the tanks.
            EnableTankControl();

            StartCoroutine(TransitionScreens(mainMenuCanvas, gameCanvas));
        }

        private void EndRound()
        {
            // Stop tanks from moving.
            DisableTankControl();

            SetScreenEnableState(mainMenuCanvas, false);
            SetScreenEnableState(gameCanvas, false);
            SetScreenEnableState(endCanvas, true);
        }

        private void EnableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].EnableControl();
            }
        }


        private void DisableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].DisableControl();
            }
        }
    }
}