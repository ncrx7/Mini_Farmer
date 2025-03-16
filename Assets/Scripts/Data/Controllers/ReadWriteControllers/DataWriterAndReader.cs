using UnityEngine;
using System.IO;
using Data;
using Cysharp.Threading.Tasks;
using Data.Controllers;

namespace Data.Controllers
{
    public class DataWriterAndReader<T>
    {
        public string SaveDataDirectoryPath = "";
        public string SaveFileName = "";

        public DataWriterAndReader(string DataDirectoryPath, string DataFileName)
        {
            SaveDataDirectoryPath = DataDirectoryPath;
            SaveFileName = DataFileName;
        }

        public async UniTask<T> InitializeDataFile()
        {
            T playerData = default;
            string loadPath = Path.Combine(SaveDataDirectoryPath, SaveFileName);
            Debug.Log("load path : " + loadPath);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader fileReader = new StreamReader(stream))
                        {
                            dataToLoad = await fileReader.ReadToEndAsync();
                        }
                    }

                    playerData = JsonUtility.FromJson<T>(dataToLoad);
                    return playerData;
                }
                catch (System.Exception)
                {

                    throw;
                }
            }
            else
            {
                object data = GameDataManager.Instance.CreateNewGameDataObject();

                if (data is T convertedData)
                {
                    playerData = convertedData;
                }

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(loadPath));
                    Debug.Log("Creating Save File At: " + loadPath);

                    string dataToStore = JsonUtility.ToJson(playerData, true);

                    using (FileStream stream = new FileStream(loadPath, FileMode.Create))
                    {
                        using (StreamWriter fileWriter = new StreamWriter(stream))
                        {
                            await fileWriter.WriteAsync(dataToStore);
                        }
                    }
                }
                catch (System.Exception ex)
                {

                    Debug.LogError("GAME NOT SAVED" + loadPath + "" + ex.Message);
                }
                return playerData;
            }

        }

        public void UpdateDataFile(T playerStatData)
        {
            string savePath = Path.Combine(SaveDataDirectoryPath, SaveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("Updating Save File At: " + savePath);

                string dataToStore = JsonUtility.ToJson(playerStatData, true);

                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (System.Exception ex)
            {

                Debug.LogError("GAME NOT SAVED" + savePath + "" + ex.Message);
            }

        }
    }
}
