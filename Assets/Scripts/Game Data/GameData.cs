using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GS.PongFootball
{
    [System.Serializable]
    public class SaveState
    {
        public int[] Balls = new int[10] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] Grounds = new int[7] { 2, 0, 0, 0, 0, 0, 0 };

        public int[] Puds = new int[5] { 1, 1, 2, 0, 0 };
        public int HighScore { get; set; }

        public int SelectedLanguage { get; set; }

        public int TotalCoin { get; set; }

    }

    public class GameData : MonoBehaviour
    {
        public static GameData Instance { get; private set; }
        public SaveState State { get => state; set => state = value; }

        public int CurrentlySelectedBallIndex = 0;
        public int CurrentlySelectedFieldIndex = 0;
        public int CurrentlySelectedPudIndex = 2;

        [Header("Logic")]

        [SerializeField] public string SaveFileName = "data.GS";
        private string saveFileName;
        [SerializeField] private bool loadOnStart = true;

        private SaveState state;
        private BinaryFormatter formatter;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            state = new SaveState();
            saveFileName = Application.persistentDataPath + "/" + SaveFileName;
            Debug.Log(saveFileName);
            formatter = new BinaryFormatter();
            Load();
        }

        public void Save()
        {
            //If there no previous state loaded, create a new one
            if (State == null)
            {
                State = new SaveState();
            }

            var file = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
            formatter.Serialize(file, State);
            file.Close();
        }

        public void Load()
        {
            // Open a physical file, on your disk to hold the save


            try
            {
                // If we found the file, open and read it
                var file = new FileStream(saveFileName, FileMode.Open, FileAccess.Read);
                State = (SaveState)formatter.Deserialize(file);
                file.Close();
                SelectedItemFinder();
            }
            catch
            {
                Debug.Log("No file found, creating new entry...");
                state.TotalCoin = 2500;
                UIManager.Instance.FirstTimeGameOn = true;
                Save();
            }
        }

        private void SelectedItemFinder()
        {
            CurrentlySelectedBallIndex = GetSelectedItemIndex(State.Balls);
            CurrentlySelectedFieldIndex = GetSelectedItemIndex(state.Grounds);
            CurrentlySelectedPudIndex = GetSelectedItemIndex(state.Puds);
        }

        private int GetSelectedItemIndex(int[] itemList)
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i] == 2)
                    return i;
            }

            return 0;
        }

    }
}