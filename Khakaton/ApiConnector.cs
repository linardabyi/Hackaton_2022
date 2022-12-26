using Microsoft.VisualBasic.Devices;
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
        string map_id = "faf7ef78-41b3-4a36-8423-688a61929c08";
        string map2_id = "a8e01288-28f8-45ee-9db4-f74fc4ff02c8";
        string getMapmethod => $"{baseUri}/json/map/{map_id}.json";
        string getMap2method => $"{baseUri}/json/map/{map2_id}.json";
        string postRouteMethod => $"{baseUri}/api/round";
        string postPresentingGiftsMethod => $"{baseUri}/api/round2";
        string apikey = "bf58cffd-39b2-44ce-91db-ba78ac5b2264";
        JSONOperator jSONOperator = new JSONOperator();
        public string GetChildAndGiftsMap()
        {
            jSONOperator.GetMapData(map2_id, out string mapData);
            if (mapData != string.Empty)
            {
                return mapData;
            }
            using HttpClient httpClient = new HttpClient();
            var msg = new HttpRequestMessage(HttpMethod.Get, getMap2method);
            msg.Headers.Add("X-API-Key", apikey);
            using HttpResponseMessage result = httpClient.Send(msg);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var contentTask = result.Content.ReadAsStringAsync();
                contentTask.Wait();
                mapData = contentTask.Result;
            }
            else
            {
                throw new Exception("something wdrong");
            }

            jSONOperator.SaveMap(mapData, map2_id);
            return mapData;
        }

        public string SendResult2(List<PresentingGiftDTO> presentingGifts)
        {
            string resultData = string.Empty;
            Result2DTO requestContent = new Result2DTO();
            requestContent.mapID = map2_id;
            requestContent.presentingGifts = presentingGifts;
            jSONOperator.SavePresentGifts(JsonSerializer.Serialize(requestContent));

            using HttpClient httpClient = new HttpClient();
            var msg = new HttpRequestMessage(HttpMethod.Post, postPresentingGiftsMethod);
            msg.Headers.Add("X-API-Key", apikey);
            msg.Content = JsonContent.Create(requestContent);

            using HttpResponseMessage result = httpClient.Send(msg);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var contentTask = result.Content.ReadAsStringAsync();
                contentTask.Wait();
                resultData = contentTask.Result;
            }

            jSONOperator.SaveStart2Process(resultData);
            return resultData;
        }

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
