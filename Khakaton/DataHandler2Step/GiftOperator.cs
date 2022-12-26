using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khakaton.DataHandler2Step
{
    public class GiftOperator
    {
        
    }

    public static class GiftsData
    {
        public static Dictionary<string, GiftInfo> data = new Dictionary<string, GiftInfo>();

        static GiftsData()
        {
            data.Add("constructors", new GiftInfo("constructors", 5, 10, Gender.Male));
            data.Add("dolls", new GiftInfo("dolls", 3, 5, Gender.Female));
            data.Add("radio_controlled_toys", new GiftInfo("radio_controlled_toys", 5, 10, Gender.Male));
            data.Add("toy_vehicles", new GiftInfo("toy_vehicles", 2, 5, Gender.Male));
            data.Add("board_games", new GiftInfo("board_games", 8, 10, Gender.Any));
            data.Add("outdoor_games", new GiftInfo("outdoor_games", 7, 10, Gender.Any));
            data.Add("playground", new GiftInfo("playground", 2, 5, Gender.Any));
            data.Add("soft_toys", new GiftInfo("soft_toys", 0, 3, Gender.Any));
            data.Add("computer_games", new GiftInfo("computer_games", 7, 10, Gender.Male));
            data.Add("sweets", new GiftInfo("sweets", 1, 4, Gender.Any));
            data.Add("books", new GiftInfo("books", 0, 10, Gender.Any));
            data.Add("pet", new GiftInfo("pet", 6, 10, Gender.Any));
            data.Add("clothes", new GiftInfo("clothes", 9, 10, Gender.Female));
        }
    }

    public class GiftInfo
    {
        public string name;
        public int minAge;
        public int maxAge;
        public Gender gender;
        public GiftInfo(string name, int minAge, int maxAge, Gender gender)
        {
            this.name = name;
            this.minAge = minAge;
            this.maxAge = maxAge;
            this.gender = gender;
        }
    }

    public enum Gender
    {
        Male,
        Female,
        Any
    }
}
