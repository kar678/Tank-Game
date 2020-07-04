using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System;

namespace Cogwheel.Save
{
    public static class SaveSystem
    {
        #region PlayerSaveing
        public static void SavePlayer (PlayerManager playerManager, int saveSlot)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/PlayerSave-" + saveSlot;
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(playerManager);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerData LoadPlayer (int saveSlot)
        {
            string path = Application.persistentDataPath + "/PlayerSave-" + saveSlot;

            if(File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save File Not Found in " + path);
                return null;
            }
        }
        #endregion

        public static bool CheckFileExsits(int saveSlot)
        {
            string path = Application.persistentDataPath + "/PlayerSave-" + saveSlot;

            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetFileLastWriteTime(int saveSlot)
        {
            string path = Application.persistentDataPath + "/PlayerSave-" + saveSlot;
            return File.GetLastWriteTime(path).ToString("HH:mm dd MMMM, yyyy");
        }

        public static void DeleteSaveFile(int saveSlot)
        {
            string path = Application.persistentDataPath + "/PlayerSave-" + saveSlot;
            File.Delete(path);
        }
    }

    [XmlRoot("SystemSettings")]
    public static class SaveSystemXml
    {
        #region SystemSettingsSaving
        public static void SaveSettings(SystemSettings systemSettings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SystemSettingsData));
            string path = Application.dataPath + "/SystemSettings.xml";
            FileStream stream = new FileStream(path, FileMode.Create);

            SystemSettingsData data = new SystemSettingsData();
            data.Fullscreen = systemSettings.fullscreen;
            data.Resolution = systemSettings.resolution;
            data.VsyncCount = systemSettings.vsyncCount;
            data.GraphicsPreset = systemSettings.graphicsPreset;
            data.MasterVolume = systemSettings.masterVolume;
            data.EffectsVolume = systemSettings.effectsVolume;

            serializer.Serialize(stream, data);
            stream.Close();
        }

        public static SystemSettingsData LoadSettings()
        {
            string path = Application.dataPath + "/SystemSettings.xml";

            if(File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SystemSettingsData));
                FileStream stream = new FileStream(path, FileMode.Open);

                SystemSettingsData data = serializer.Deserialize(stream) as SystemSettingsData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("System Settings file not found");
                return null;
            }
        }
        #endregion

        public static bool CheckFileExsits()
        {
            string path = Application.dataPath + "/SystemSettings.xml";

            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}