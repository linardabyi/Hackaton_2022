using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khakaton
{
    public class Data2DTO
    {
        public Gift2DTO[] gifts { get; set; }
        public Child2DTO[] children { get; set; }
    }
    public class Gift2DTO
    {
        public int id { get; set; }
        public string type { get; set; }
        public int price { get; set; }
    }

    public class Child2DTO
    {
        public int id { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
    }

    public class PresentingGiftDTO
    {
        public int giftID { get; set; }
        public int childID { get; set; }
    }

    public class Result2DTO
    {
        public string mapID { get; set; }
        public List<PresentingGiftDTO> presentingGifts { get; set; }
    }
}
