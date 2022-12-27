using Khakaton.DataHandler2Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khakaton
{
    public class DataDTO
    {
        public giftDTO[] gifts { get; set; }
        public snowAreaDTO[] snowAreas { get; set; }
        public childDTO[] children { get; set; }
    }
    public class giftDTO
    {
        public int id { get; set; }
        public int weight { get; set; }
        public int volume { get; set; }
        public string type { get; set; }
        public int price { get; set; }
    }

    public class snowAreaDTO
    {
        public int r { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class childDTO
    {
        public int x { get; set; }
        public int y { get; set; }
        public int age { get; set; }
        public string gender
        {
            set
            {
                switch (value)
                {
                    case "male":
                        Gender = Gender.Male;
                        break;
                    case "female":
                        Gender = Gender.Female;
                        break;
                }
            }
        }

        public Gender Gender { get; private set; }

        public bool visited = false;

        public snowAreaDTO snowAreaDTO;

        public int entryX { get; set; }
        public int entryY { get; set; }
    }

    public class moveDTO
    {
        public moveDTO()
        {

        }

        public moveDTO(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class resultDTO
    {
        public string mapID { get; set; }
        public List<moveDTO> moves { get; set; }
        public List<List<int>> stackOfBags { get; set; }
    }

    public class resultResponseDTO
    {
        public bool success { get; set; }
        public string error { get; set; }
        public string roundId { get; set; }
    }
}
