using System;
using System.Collections.Generic;
using CoralTravelAnalyzer.CoralTravelApi.Proto.HotelPriceOptions;
using CoralTravelAnalyzer.FileDestinations;
using TiqUtils.Utils;
using TiQWpfUtils.AbstractClasses;

namespace CoralTravelAnalyzer.Classes
{
    public class HotelOptionEntry : Notified
    {
        [ExcelData("Nights", 75, ExcelDataAttribute.Align.Center)]
        public int DaysTotal => _baseEntry?.Nights ?? 1;

        [ExcelData("Begin date", 150, ExcelDataAttribute.Align.Center)]
        public DateTime BeginDate { get; set; }

        [ExcelData("Room type", 180, ExcelDataAttribute.Align.Center)]
        public string RoomType { get; set; }

        [ExcelData("Meal type", 180, ExcelDataAttribute.Align.Center)]
        public string MealType => _baseEntry?.Name.Trim() ?? "MealTypeUnknown";

        [ExcelData("Total Price", 150)]
        public double TotalPrice => Math.Round(_baseEntry?.Price ?? 0, 2);

        [ExcelData("Usd Price", 150)]
        public double UsdPrice => Math.Round(_baseEntry?.LocalPrice ?? 0, 2);

        [ExcelData("Price / day", 150)]
        public double PricePerDay => Math.Round(TotalPrice / DaysTotal, 2);

        [ExcelData("Without flight", 150)]
        public bool NoFlight { get; set; }


        public string BeginDateString => BeginDate.ToString("dd.MM.yyyy");
        public string RoomImageUrl { get; set; }

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
                                RoomType = room.Name.Trim(),
                                RoomImageUrl = room.ImageUrl,
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
