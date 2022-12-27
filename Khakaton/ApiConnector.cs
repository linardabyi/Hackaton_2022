using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Khakaton
{
    public class ApiConnector
    {
        string baseUri => @"https://datsanta.dats.team";
        string map_id = "dd6ed651-8ed6-4aeb-bcbc-d8a51c8383cc";
        string getMapmethod => $"{baseUri}/json/map/{map_id}.json";
        string postRouteMethod => $"{baseUri}/api/round";
        string apikey = "bf58cffd-39b2-44ce-91db-ba78ac5b2264";
        string postPresentingGiftsMethod => $"{baseUri}/api/round2";
        JSONOperator jSONOperator = new JSONOperator();
        public string GetMapData()
        {
            jSONOperator.GetMapData(map_id, out string mapData);
            if (mapData != string.Empty)
            {
                return mapData;
            }
            using HttpClient httpClient = new HttpClient();
            var msg = new HttpRequestMessage(HttpMethod.Get, getMapmethod);
            msg.Headers.Add("X-API-Key", apikey);
            using HttpResponseMessage result = httpClient.Send(msg);
            if (result.StatusCode== HttpStatusCode.OK)
            {
                var contentTask = result.Content.ReadAsStringAsync();
                contentTask.Wait();
                mapData = contentTask.Result;
            }
            else
            {
                throw new Exception("something wdrong");
            }
            
            jSONOperator.SaveMap(mapData, map_id);
            return mapData;
        }

        public string SendResult(List<moveDTO> moves, List<List<int>> stackOfBags)
        {
            string resultData = string.Empty;
            resultDTO requestContent = new resultDTO();
            requestContent.mapID = map_id;
            requestContent.stackOfBags = stackOfBags;
            requestContent.moves = moves;
            requestContent.moves.RemoveAt(moves.Count - 1);
            requestContent.moves.Reverse();
            jSONOperator.SaveMoves(JsonSerializer.Serialize(requestContent));

            using HttpClient httpClient = new HttpClient();
            var msg = new HttpRequestMessage(HttpMethod.Post, postRouteMethod);
            msg.Headers.Add("X-API-Key", apikey);
            msg.Content = JsonContent.Create(requestContent);
            
            using HttpResponseMessage result = httpClient.Send(msg);
            if (result.StatusCode== HttpStatusCode.OK)
            {
                var contentTask = result.Content.ReadAsStringAsync();
                contentTask.Wait();
                resultData = contentTask.Result;
            }
            
            jSONOperator.SaveStartProcess(resultData);
            return resultData;
        }

    }
}
