using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Khakaton.DataHandler2Step
{
    public class Data2Processer
    {
        Data2DTO data;
        const int budget = 100000;
        public Data2DTO GetData(string rawData)
        {
            data = JsonSerializer.Deserialize<Data2DTO>(rawData);
            return data;
        }

        public List<PresentingGiftDTO> PresentGifts()
        {
            List<PresentingGiftDTO> result = new List<PresentingGiftDTO>();
            foreach (var child in data.children)
            {

            }
            return result;
        }
    }
}
