using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HallEventManager
{
    [Serializable]
    public class Save
    {
        private Manager manager;
        private Hall hall;

        public Save(Manager manager, Hall hall)
        {
            this.manager = manager;
            this.hall = hall;
        }

        public void SaveData(string path)
        {
            using Stream stream = File.Open(path, FileMode.OpenOrCreate);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
        }

        public static Save LoadSave(string path)
        {
            if (File.Exists(path))
            {
                using Stream stream = File.Open(path, FileMode.OpenOrCreate);
                {
                    var formatter = new BinaryFormatter();
                    return (Save)formatter.Deserialize(stream);
                }
            }

            return null;
        }

        public Manager GetManager()
        {
            return manager;
        }

        public Hall GetHall()
        {
            return hall;
        }
    }
}