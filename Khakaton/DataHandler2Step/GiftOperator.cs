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
            data.Add("educational_games", new GiftInfo("educational_games", 6, 10, Gender.Any));
            data.Add("music_games", new GiftInfo("music_games", 2, 5, Gender.Any));
            data.Add("bath_toys", new GiftInfo("bath_toys", 0, 3, Gender.Any));
            data.Add("bike", new GiftInfo("bike", 7, 10, Gender.Male));
            data.Add("paints", new GiftInfo("paints", 3, 5, Gender.Any));
            data.Add("casket", new GiftInfo("casket", 6, 9, Gender.Female));
            data.Add("soccer_ball", new GiftInfo("soccer_ball", 6, 10, Gender.Male));
            data.Add("toy_kitchen", new GiftInfo("toy_kitchen", 5, 8, Gender.Female));
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
