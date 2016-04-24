using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DEngine
{
    public class GameManager : MonoBehaviour
    {
        public PlayerController HumanPrefab;
        public PlayerController RobotPrefab;

        /*
        public SpawnPoint CurrentHumanSpawnPoint
        {
        }
        public SpawnPoint CurrentRobotSpawnPoint = null;
        public PlayerController Human = null;
        public PlayerController Robot = null;
        */

        private SpawnPoint m_currentHumanSpawnPoint;
        private SpawnPoint m_currentRobotSpawnPoint;
        private PlayerController m_human;
        private PlayerController m_robot;

        public SpawnPoint CurrentHumanSpawnPoint
        {
            get { return m_currentHumanSpawnPoint; }
            private set { m_currentHumanSpawnPoint = value; }
        }


        public SpawnPoint CurrentRobotSpawnPoint
        {
            get { return m_currentRobotSpawnPoint; }
            private set { m_currentRobotSpawnPoint = value; }
        }


        public PlayerController Human
        {
            get { return m_human; }
            private set { m_human = value; }
        }


        public PlayerController Robot
        {
            get { return m_robot; }
            private set { m_robot = value; }
        }



        private int s_HumanLayer;
        private int s_RobotLayer;



        private static GameManager s_Instance;
        public static GameManager Instance
        {
            get
            {
                if (s_Instance == null)
                    InstantiateGameManager();

                return s_Instance;
            }
        }

        private static void InstantiateGameManager()
        {
            GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameManager")).GetComponent<GameManager>();
        }

        private void Awake()
        {
            s_Instance = this;

            s_HumanLayer = LayerMask.NameToLayer("HumanPlayer");
            s_RobotLayer = LayerMask.NameToLayer("RobotPlayer");
        }

        private void Start()
        {
            foreach (var player in FindObjectsOfType<PlayerController>())
            {
                if (player.gameObject.layer == s_HumanLayer)
                    Human = player;
                else if (player.gameObject.layer == s_RobotLayer)
                    Robot = player;
            }

            foreach (var spawnPoint in FindObjectsOfType<SpawnPoint>())
            {
                if (spawnPoint.LevelStart)
                {
                    switch (spawnPoint.Type)
                    {
                        case SpawnPoint.SpawnPointType.HUMAN:
                            CurrentHumanSpawnPoint = spawnPoint;
                            break;
                        case SpawnPoint.SpawnPointType.ROBOT:
                            CurrentRobotSpawnPoint = spawnPoint;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (Human == null && CurrentHumanSpawnPoint != null)
            {
                Human = GameObject.Instantiate<PlayerController>(HumanPrefab);
                Human.transform.position = CurrentHumanSpawnPoint.transform.position;
                Human.name = "Human";
            }

            if (Robot == null && CurrentRobotSpawnPoint != null)
            {
                Robot = GameObject.Instantiate<PlayerController>(RobotPrefab);
                Robot.transform.position = CurrentRobotSpawnPoint.transform.position;
                Robot.name = "Robot";
            }
        }

        public void SetCurrentSpawnPoint(SpawnPoint _spawn)
        {
            switch (_spawn.Type)
            {
                case SpawnPoint.SpawnPointType.HUMAN:
                    CurrentHumanSpawnPoint = _spawn;
                    break;
                case SpawnPoint.SpawnPointType.ROBOT:
                    CurrentRobotSpawnPoint = _spawn;
                    break;
                default:
                    break;
            }
        }
    }
}