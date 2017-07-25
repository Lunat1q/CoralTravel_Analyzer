using System;
using System.Collections.Generic;
using CoralTravelAnalyzer.CoralTravelApi.Proto.HotelPriceOptions;
using TiqUtils.Utils;
using TiQWpfUtils.AbstractClasses;

namespace CoralTravelAnalyzer.Classes
{
    public class HotelOptionEntry : Notified
    {
        public string RoomType { get; set; }
        public string RoomImageUrl { get; set; }
        public string MealType { get; set; }
        public string BeginDateString => BeginDate.ToString("dd.MM.yyyy");
        private DateTime BeginDate { get; set; }
        public int DaysTotal { get; set; }
        public double TotalPrice { get; set; }
        public bool NoFlight { get; set; }
        public double PricePerDay => TotalPrice / DaysTotal;

        private PriceOption _baseEntry;


        public static List<HotelOptionEntry> GenerateFromResultOptions(ResultOptions resultOptions, bool noFlight = false)
        {
            var resultList = new List<HotelOptionEntry>();
            if (resultOptions == null) return resultList;
            foreach (var hotelOption in resultOptions.List)
            {
                foreach (var room in hotelOption.Rooms)
                {
                    foreach (var option in room.Options)
                    {
                        try
                        {
                            var item = new HotelOptionEntry
                            {
                                _baseEntry = option,
                                BeginDate = DateTime.Parse(option.BeginDate),
                                RoomType = room.Name,
                                RoomImageUrl = room.ImageUrl,
                                MealType = option.Name,
                                TotalPrice = option.Price,
                                DaysTotal = hotelOption.Nights,
                                NoFlight = noFlight
                            };
                            resultList.Add(item);
                        }
                        catch (Exception e)
                        {
                            Logging.ErrorLog(e.Message);
                        }
                    }
                }
            }


            return resultList;
        }

        
    }
}
