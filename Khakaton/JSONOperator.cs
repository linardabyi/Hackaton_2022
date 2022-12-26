using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khakaton
{
    public class JSONOperator
    {
        public string mapId;
        public string map2Id;
        public string mapFileName => $"map_{mapId}.json";

        public string map2FileName => $"map_{map2Id}.json";
        public string dataMovesFileName => $"moves_{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.json";
        public string dataPresentingGiftsFileName => $"presentGifts_{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.json";
        public string responseStartProcessFileName => $"startProcess_{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.json";
        public string responseStart2ProcessFileName => $"start2Process_{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}.json";

        public JSONOperator()
        {

        }

        public bool GetMapData(string mapId, out string jsonMap)
        {
            this.mapId = mapId;
            if (File.Exists(mapFileName))
            {
                using StreamReader streamreader = new StreamReader(mapFileName);
                jsonMap = streamreader.ReadToEnd();
                return true;
            }
            jsonMap = string.Empty;
            return false;
        }

        public void SaveMap(string jsonMap, string mapId)
        {
            this.mapId = mapId;
            using StreamWriter file = new(mapFileName);
            file.Write(jsonMap);
        }

        public void SaveMap2(string jsonMap, string mapId)
        {
            this.map2Id = mapId;
            using StreamWriter file = new(map2FileName);
            file.Write(jsonMap);
        }

        public void SaveMoves(string moves)
        {
            using StreamWriter file = new(dataMovesFileName);
            file.Write(moves);
        }

        public void SavePresentGifts(string gifts)
        {
            using StreamWriter file = new(dataPresentingGiftsFileName);
            file.Write(gifts);
        }

        public void SaveStartProcess(string startProcess)
        {
            using StreamWriter file = new(responseStartProcessFileName);
            file.Write(startProcess);
        }

        public void SaveStart2Process(string startProcess)
        {
            using StreamWriter file = new(responseStart2ProcessFileName);
            file.Write(startProcess);
        }
    }
}
