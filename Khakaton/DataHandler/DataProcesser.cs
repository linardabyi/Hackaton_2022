using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Numerics;
using System.Net;
using System.Drawing;
using Khakaton.DataHandler2Step;
using System.Drawing.Design;

namespace Khakaton.DataHandler
{
    public class DataProcesser
    {
        const int maxWeight = 200;
        const int maxVolume = 100;
        const int budget = 50000;
        int currentSum = 0;
        List<giftDTO> warehouse = new List<giftDTO>();
        List<List<int>> bags = new List<List<int>>();
        List<moveDTO> moves = new List<moveDTO>();
        List<snowAreaDTO> snowAreas = new List<snowAreaDTO>();
        List<giftDTO> gifts = new List<giftDTO>();
        HashSet<int> givenGifts = new HashSet<int>();
        Graphics g;
        int startEntryX, startEntryY;
        Point StartPoint;
        int bagNumber = 0;

        public List<List<int>> Bags => bags;
        public List<moveDTO> Moves=> moves;

        public void SetGraphic(Graphics g)
        {
            this.g = g;
        }

        public DataDTO GetData(string rawData)
        {
            DataDTO result = JsonSerializer.Deserialize<DataDTO>(rawData);
            warehouse = result.gifts.ToList();
            PackBags();
            snowAreas = result.snowAreas.ToList();
            gifts = result.gifts.OrderBy(x => x.price).ToList();
            foreach (var child in result.children)
            {
                foreach (var snowArea in result.snowAreas)
                {
                    double d2 = Math.Sqrt(
                        Math.Pow(0 - snowArea.x, 2.0)
                        + Math.Pow(0 - snowArea.y, 2.0));
                    if (snowArea.r > d2)
                    {
                        CalculateStartEntry(snowArea.x, snowArea.y, snowArea.r);
                    }
                    double d = Math.Sqrt(
                        Math.Pow(child.x - snowArea.x, 2.0)
                        + Math.Pow(child.y - snowArea.y, 2.0));
                    if (snowArea.r >= d)
                    {
                        child.snowAreaDTO = snowArea;
                        double xv = (child.x - snowArea.x) * snowArea.r / d;
                        double yv = (child.y - snowArea.y) * snowArea.r / d;
                        if (xv > 0) xv = (int)xv + 1; else xv = (int)xv - 1;
                        if (yv > 0) yv = (int)yv + 1; else yv = (int)yv - 1;
                        child.entryX = snowArea.x + (int)xv;
                        child.entryY = snowArea.y + (int)yv;
                        if (child.entryX < 0)
                        {
                            child.entryX = 1;
                            double entryY = snowArea.y - Math.Sqrt(Math.Pow(snowArea.r, 2.0) - Math.Pow(snowArea.x - child.x, 2.0));
                            if (entryY < 0)
                                entryY = snowArea.y + Math.Sqrt(Math.Pow(snowArea.r, 2.0) - Math.Pow(snowArea.x - child.x, 2.0));
                            if (entryY > 10000)
                                throw new Exception("out of range");
                            child.entryY = (int)entryY;
                        }
                        if (child.entryX > 10000)
                        {
                            child.entryX = 9999;
                            double entryY = snowArea.y - Math.Sqrt(Math.Pow(snowArea.r, 2.0) - Math.Pow(snowArea.x - child.x, 2.0));
                            if (entryY < 0)
                                entryY = snowArea.y + Math.Sqrt(Math.Pow(snowArea.r, 2.0) - Math.Pow(snowArea.x - child.x, 2.0));
                            if (entryY > 10000)
                                throw new Exception("out of range");
                            child.entryY = (int)entryY;
                        }
                        if (child.entryY < 0)
                        {
                            child.entryY = 1;
                            double entryX = snowArea.x - Math.Sqrt(Math.Pow(snowArea.r, 2.0) - Math.Pow(snowArea.y - child.y, 2.0));
                            if (entryX < 0)
                                entryX = snowArea.x + Math.Sqrt(Math.Pow(snowArea.r, 2.0) - Math.Pow(snowArea.y - child.y, 2.0));
                            if (entryX > 10000)
                                throw new Exception("out of range");
                            child.entryX = (int)entryX;
                        }
                        if (child.entryY > 10000)
                        {
                            child.entryY = 9999;
                            double entryX = snowArea.x - Math.Sqrt(Math.Pow(snowArea.r, 2.0) - Math.Pow(snowArea.y - child.y, 2.0));
                            if (entryX < 0)
                                entryX = snowArea.x + Math.Sqrt(Math.Pow(snowArea.r, 2.0) - Math.Pow(snowArea.y - child.y, 2.0));
                            if (entryX > 10000)
                                throw new Exception("out of range");
                            child.entryX = (int)entryX;
                        }
                        break;
                    }
                }
            }


            return result;
        }

        public void CalculateStartEntry(int x, int y, int r)
        {
            double d = Math.Sqrt(x * x + y * y);
            double xv = (0 - x) * r / d;
            double yv = (0 - y) * r / d;
            if (xv > 0) xv = (int)xv + 1; else xv = (int)xv - 1;
            if (yv > 0) yv = (int)yv + 1; else yv = (int)yv - 1;
            startEntryX = x + (int)xv;
            startEntryY = y + (int)yv;
            if (startEntryX < 0)
            {
                startEntryX = 1;
                double entryY = y - Math.Sqrt(Math.Pow(r, 2.0) - Math.Pow(x - 0, 2.0));
                if (entryY < 0)
                    entryY = y + Math.Sqrt(Math.Pow(r, 2.0) - Math.Pow(x - 0, 2.0));
                if (entryY > 10000)
                    throw new Exception("out of range");
                startEntryY = (int)entryY;
            }
            if (startEntryX > 10000)
            {
                startEntryX = 9999;
                double entryY = y - Math.Sqrt(Math.Pow(r, 2.0) - Math.Pow(x - 0, 2.0));
                if (entryY < 0)
                    entryY = y + Math.Sqrt(Math.Pow(r, 2.0) - Math.Pow(x - 0, 2.0));
                if (entryY > 10000)
                    throw new Exception("out of range");
                startEntryY = (int)entryY;
            }
            if (startEntryY < 0)
            {
                startEntryY = 1;
                double entryX = x - Math.Sqrt(Math.Pow(r, 2.0) - Math.Pow(y - 0, 2.0));
                if (entryX < 0)
                    entryX = x + Math.Sqrt(Math.Pow(r, 2.0) - Math.Pow(y - 0, 2.0));
                if (entryX > 10000)
                    throw new Exception("out of range");
                startEntryX = (int)entryX;
            }
            if (startEntryY > 10000)
            {
                startEntryY = 9999;
                double entryX = x - Math.Sqrt(Math.Pow(r, 2.0) - Math.Pow(y - 0, 2.0));
                if (entryX < 0)
                    entryX = x + Math.Sqrt(Math.Pow(r, 2.0) - Math.Pow(y - 0, 2.0));
                if (entryX > 10000)
                    throw new Exception("out of range");
                startEntryX = (int)entryX;
            }
            startEntryX = 600;
            startEntryY = 800;
            StartPoint = new Point(startEntryX, startEntryY);
            
        }

        public List<Point> GetRoute(DataDTO dataDTO)
        {
            int currentWeight= 0;
            int currentVolume = 0;
            List<Point> route = new List<Point>();
            childDTO farChild = FindFarChildFromWarehouse(dataDTO);
            if (farChild == null)
            {
                return null;
            }
            var newBag = new List<int>();
            giftDTO giftForFirst = GetGift(farChild, 0, 0);
            
            if (giftForFirst == null)
            {
                throw new Exception("nu vse");
            }
            if (currentSum + giftForFirst.price > budget)
            {
                throw new Exception("dorogo");
            }
            currentWeight += giftForFirst.weight;
            currentVolume += giftForFirst.volume;
            currentSum += giftForFirst.price;
            newBag.Add(giftForFirst.id);
            Point farChildPont = new Point(farChild.x, farChild.y);
            if (farChild.snowAreaDTO== null)
            {
                route.Add(StartPoint);
                route.AddRange(GetSafeRouteFromPointToPoint(StartPoint, farChildPont));
            }
            else
            {
                route.Add(StartPoint);
                Point farChildEntryPoint = new Point(farChild.entryX, farChild.entryY);
                route.AddRange(GetSafeRouteFromPointToPoint(StartPoint, farChildEntryPoint));
                route.Add(farChildPont);
            }
            childDTO lastChild = farChild;
            Point lastPoint;
            
            while (true)
            {
                childDTO newChild = FindNearestChild(dataDTO, route.Last());
                if (newChild == null)
                    break;
                giftDTO gift = GetGift(newChild, currentWeight, currentVolume);
                if (gift == null || currentSum + gift.price > budget
                    || currentWeight + gift.weight > maxWeight
                    || currentVolume + gift.volume > maxVolume)
                {
                    break;
                }

                currentSum += gift.price;
                currentWeight += gift.weight;
                currentVolume += gift.volume;
                newChild.visited= true;
                newBag.Add(gift.id);

                Point endPoint = new Point(newChild.x, newChild.y);
                if ((lastChild == null || lastChild.snowAreaDTO == null) && newChild.snowAreaDTO == null)
                {
                    lastPoint = route.Last();
                    route.AddRange(GetSafeRouteFromPointToPoint(lastPoint, endPoint));
                }
                else if (lastChild != null && lastChild.snowAreaDTO != null
                    && newChild.snowAreaDTO == null)
                {
                    lastPoint = route.Last();
                    Point startPoint = new Point(lastChild.entryX, lastChild.entryY);
                    route.Add(startPoint);
                    route.AddRange(GetSafeRouteFromPointToPoint(startPoint, endPoint));
                }
                else if (lastChild != null && lastChild.snowAreaDTO == null
                    && newChild.snowAreaDTO != null)
                {
                    lastPoint = route.Last();
                    endPoint = new Point(newChild.entryX, newChild.entryY);
                    route.AddRange(GetSafeRouteFromPointToPoint(lastPoint, endPoint));
                    route.Add(new Point(newChild.x, newChild.y));
                }
                else
                {
                    route.Add(endPoint);
                }
                lastChild = newChild;
            }
            if (lastChild == null || lastChild.snowAreaDTO == null)
            {
                lastPoint = route.Last();
                route.AddRange(GetSafeRouteFromPointToPoint(lastPoint, StartPoint));
                route.Add(Point.Empty);
            }
            else
            {
                lastPoint = route.Last();
                Point startPoint = new Point(lastChild.entryX, lastChild.entryY);
                route.Add(startPoint);
                route.AddRange(GetSafeRouteFromPointToPoint(startPoint, StartPoint));
                route.Add(Point.Empty);
            }
            if (newBag.Count >0) { bags.Add(newBag); }
            moves.AddRange(route.Select(x => new moveDTO(x.X, x.Y)));
            return route;
        }

        public void ReplaceWithMoreExpensive()
        {
        }

        public resultResponseDTO ReadResultResponse(string resultResponse)
        {
            resultResponseDTO result = JsonSerializer.Deserialize<resultResponseDTO>(resultResponse);
            return result;
        }

        private List<Point> GetSafeRouteFromPointToPoint(Point start, Point end, bool debug = false)
        {
            if (debug)
            {
                PaintDebugLine(start, end);
                //System.Threading.Thread.Sleep(3000);
            }
            List<Point> result = new List<Point>();
            MathLineSegment segment = new MathLineSegment(start.X, start.Y, end.X, end.Y);
            snowAreas.Sort(delegate (snowAreaDTO circle1, snowAreaDTO circle2)
            {
                double d1 = Math.Pow(circle1.x - start.X, 2.0) + Math.Pow(circle1.y - start.Y, 2.0);
                double d2 = Math.Pow(circle2.x - start.X, 2.0) + Math.Pow(circle2.y - start.Y, 2.0);
                if (d1 > d2) { return 1; }
                if (d1 < d2) { return -1; }
                return 0;
            });
            foreach (var circle in snowAreas)
            {
                if (segment.IntersectCircle(circle.x, circle.y, circle.r))
                {
                    MathVector vectorToCircle = MathVector.GetVectorToAvoidCircle(circle.x, circle.y, circle.r,
                        start.X, start.Y,
                        end.X, end.Y);
                    if (vectorToCircle == null)
                        break;
                    Point newPoint = new Point((int)(start.X + vectorToCircle.x), (int)(start.Y + vectorToCircle.y));
                    if (newPoint.X == end.X && newPoint.Y == end.Y)
                        break;
                    result.AddRange(GetSafeRouteFromPointToPoint(start, newPoint, debug));
                    result.AddRange(GetSafeRouteFromPointToPoint(newPoint, end, debug));
                    return result;
                }
            }
            result.Add(end);
            return result;
        }

        private childDTO FindNearestChild(DataDTO dataDTO, Point point)
        {
            Point currentPoint = Point.Empty;
            double minS = 10000 * 10000;
            childDTO currentChild = null;
            foreach (var child in dataDTO.children)
            {
                if (!child.visited)
                {
                    double newDistance = DistanceBeetwenTwoPoint(point.X, point.Y, child.x, child.y);
                    if (newDistance < minS)
                    {
                        currentPoint = new Point(child.x, child.y);
                        minS = newDistance;
                        currentChild = child;
                    }
                }
            }
            return currentChild;
        }

        private void PaintDebugLine(Point start, Point end)
        {
            int scale = 11;
            Point scaleStart = new Point(start.X / scale, start.Y / scale);
            Point scaleEnd = new Point(end.X / scale, end.Y / scale);
            Pen p = new Pen(Color.Blue, 1);
            g.DrawLine(p, scaleStart, scaleEnd);
        }

        private childDTO FindFarChildFromWarehouse(DataDTO dataDTO)
        {
            childDTO currentChild = null;
            Point currentPoint = Point.Empty;
            double maxS = 0;
            foreach (var child in dataDTO.children)
            {
                if (!child.visited)
                {
                    double newDistance = DistanceFromWareHouse(child.x, child.y);
                    if (newDistance > maxS)
                    {
                        currentPoint = new Point(child.x, child.y);
                        maxS = newDistance;
                        currentChild = child;
                    }
                }
            }
            if (currentChild != null)
                currentChild.visited = true;
            return currentChild;
        }

        private double DistanceBeetwenTwoPoint(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        private double DistanceFromWareHouse(int X, int Y)
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        private void PackBags()
        {
            this.bags = new List<List<int>>();
            //List<giftDTO> gifts = warehouse.OrderBy(x => x.price).Take(1000).ToList();
            //List<List<int>> bags = new();

            //List<giftDTO> bag = new();

            //for (int i = 0; i < gifts.Count(); i++)
            //{
            //    if (CheckBagLimits(bag, gifts[i]))
            //    {
            //        bag.Add(gifts[i]);

            //        if (i != gifts.Count() - 1)
            //            continue;
            //    }

            //    List<int> giftsIds = bag.Select(b => b.id).ToList();
            //    bags.Add(giftsIds);

            //    bag = new()
            //    {
            //        gifts[i]
            //    };
            //}

            //this.bags = bags;
        }

        private bool CheckBagLimits(List<giftDTO> bag, giftDTO extraGift)
        {
            const int maximumWeight = 200;
            const int maximumVolume = 100;

            int weight = 0;
            int volume = 0;

            foreach (giftDTO gift in bag)
            {
                weight += gift.weight;
                volume += gift.volume;
            }

            weight += extraGift.weight;
            volume += extraGift.volume;

            if (weight <= maximumWeight && volume <= maximumVolume)
                return true;

            return false;
        }

        private int CollectGifts()
        {
            /*List<giftDTO> gifts = new List<giftDTO>();
            List <int> bag = new List<int>();
            int currentWeight = 0;
            int currentVolume = 0;
            int i = 0;
            while (currentWeight < maxWeight
                && currentVolume < maxVolume
                && warehouse.Count > 0
                && i < warehouse.Count)
            {
                if (currentWeight + warehouse[i].weight <= maxWeight
                    && currentVolume + warehouse[i].volume <= maxVolume)
                {
                    gifts.Add(warehouse[i]);
                    bag.Add(warehouse[i].id);
                    currentWeight = currentWeight + warehouse[i].weight;
                    currentVolume = currentVolume + warehouse[i].volume;
                    warehouse.RemoveAt(i);
                }
                else
                {
                    break;
                }
                i++;
            }
            if (bag.Count > 0) 
                bags.Add(bag);*/
            if (bagNumber < bags.Count)
                return bags[bagNumber++].Count;
            else return 0;

        }

        private giftDTO GetGift(childDTO child, int currentWeight, int currentVolume)
        {
            for (int i = 1998; i < gifts.Count; i++)
            {
                if (givenGifts.Contains(gifts[i].id))
                    continue;

                if (!IsGiftAgeCorrect(gifts[i], child))
                    continue;

                
                if (child.Gender == Gender.Male)
                {
                    if (IsMaleOrAnyGift(gifts[i]))
                    {
                        //if (gifts[i].weight + currentWeight > maxWeight)
                        //    continue;
                        //if (gifts[i].volume + currentVolume > maxVolume)
                        //    continue;
                        //if (gifts[i].price + currentSum > budget)
                        //    continue;

                        givenGifts.Add(gifts[i].id);

                        return gifts[i];
                    }
                }
                else
                {
                    if (IsFemaleOrAnyGift(gifts[i]))
                    {
                        //if (gifts[i].weight + currentWeight > maxWeight)
                        //    continue;
                        //if (gifts[i].volume + currentVolume > maxVolume)
                        //    continue;
                        //if (gifts[i].price + currentSum > budget)
                        //    continue;

                        givenGifts.Add(gifts[i].id);

                        return gifts[i];
                    }
                }
            }

            for (int i = 0; i < gifts.Count; i++)
            {
                if (givenGifts.Contains(gifts[i].id))
                    continue;

                if (!IsGiftAgeCorrect(gifts[i], child))
                    continue;

                if (child.Gender == Gender.Male)
                {
                    if (IsMaleOrAnyGift(gifts[i]))
                    {
                        //if (gifts[i].weight + currentWeight > maxWeight)
                        //    continue;
                        //if (gifts[i].volume + currentVolume > maxVolume)
                        //    continue;
                        //if (gifts[i].price + currentSum > budget)
                        //    continue;

                        givenGifts.Add(gifts[i].id);

                        return gifts[i];
                    }
                }
                else
                {
                    if (IsFemaleOrAnyGift(gifts[i]))
                    {
                        //if (gifts[i].weight + currentWeight > maxWeight)
                        //    continue;
                        //if (gifts[i].volume + currentVolume > maxVolume)
                        //    continue;
                        //if (gifts[i].price + currentSum > budget)
                        //    continue;

                        givenGifts.Add(gifts[i].id);

                        return gifts[i];
                    }
                }
            }

            throw new Exception("Its impossible");
        }

        private bool IsMaleOrAnyGift(giftDTO gift) =>
            GiftsData.data[gift.type].gender == Gender.Male || GiftsData.data[gift.type].gender == Gender.Any;

        private bool IsFemaleOrAnyGift(giftDTO gift) =>
            GiftsData.data[gift.type].gender == Gender.Female || GiftsData.data[gift.type].gender == Gender.Any;

        private bool IsGiftAgeCorrect(giftDTO gift, childDTO child) =>
            child.age >= GiftsData.data[gift.type].minAge && child.age <= GiftsData.data[gift.type].maxAge;
    }
}
