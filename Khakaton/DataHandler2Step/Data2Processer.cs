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

        private void FilterAndReorderGifts()
        {
            List<Gift2DTO> giftsAbove100Dollar = new();

            for (int i = 0; i < data.gifts.Length; i++)
            {
                if (data.gifts[i].type == "books")
                    continue;

                if (data.gifts[i].type == "radio_controlled_toys")
                    giftsAbove100Dollar.Add(data.gifts[i]);
                else if (data.gifts[i].price <= 121)
                    giftsAbove100Dollar.Add(data.gifts[i]);
            }

            data.gifts = giftsAbove100Dollar.OrderByDescending(g => g.price).ToArray();
        }

        private void CalculateTotal(List<PresentingGiftDTO> result)
        {
            int total = 0;

            foreach(var res in result)
            {
                total += data.gifts.First(g => g.id == res.giftID).price;
            }

            Console.WriteLine("Total: " + total);
        }

        public List<PresentingGiftDTO> PresentGifts()
        {
            FilterAndReorderGifts();

            List<PresentingGiftDTO> result = new List<PresentingGiftDTO>();
            
            for (int i = 0; i < data.children.Length; i++)
            {
                PresentingGiftDTO gift = new();

                gift.childID = data.children[i].id;
                gift.giftID = GetGiftId(data.children[i]);

                result.Add(gift);
            }

            CalculateTotal(result);

            return result;
        }

        private List<int> givenGifts = new();

        private int GetGiftId(Child2DTO child)
        {
            for (int i = 0; i < data.gifts.Length; i++)
            {
                if (IsGiftGiven(data.gifts[i].id))
                    continue;

                //if (!IsBudgetOk(data.gifts[i].price))
                //    continue;

                if (!IsGiftAgeCorrect(data.gifts[i], child))
                    continue;

                if (child.gender == "male")
                {
                    if (IsMaleOrAnyGift(data.gifts[i]))
                    {
                        givenGifts.Add(data.gifts[i].id);

                        return data.gifts[i].id;
                    }           
                }
                else
                {
                    if (IsFemaleOrAnyGift(data.gifts[i]))
                    {
                        givenGifts.Add(data.gifts[i].id);

                        return data.gifts[i].id;
                    }
                }
            }

            throw new Exception("Its impossible");
        }

        private bool IsGiftGiven(int giftId) => givenGifts.Contains(giftId);

        private bool IsBudgetOk(int newPrice)
        {
            int currentBudget = 0;

            foreach (int giftId in givenGifts)
            {
                currentBudget += data.gifts.First(g => g.id == giftId).price;
            }

            return currentBudget + newPrice <= budget;
        }

        private bool IsMaleOrAnyGift(Gift2DTO gift) =>
            GiftsData.data[gift.type].gender == Gender.Male || GiftsData.data[gift.type].gender == Gender.Any;

        private bool IsFemaleOrAnyGift(Gift2DTO gift) =>
            GiftsData.data[gift.type].gender == Gender.Female || GiftsData.data[gift.type].gender == Gender.Any;

        private bool IsGiftAgeCorrect(Gift2DTO gift, Child2DTO child) =>
            child.age >= GiftsData.data[gift.type].minAge && child.age <= GiftsData.data[gift.type].maxAge;
    }
}
