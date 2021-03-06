﻿using CoralTravelAnalyzer.CoralTravelApi.Proto.HotelPriceOptions;

namespace CoralTravelAnalyzer.CoralTravelApi
{
    public class PriceOptionsSearch : ApiBase<HotelOptionsResult>
    {
        private string _baseUrl =
                "CoralTravel/HotelOptions/HotelOptionsList?sortAscending=true&sortBy=undefined&pageSize={12}&" +
                "currentPage={13}&pageCount={14}&search=&hotelEeId={0}&countryEeId={1}&areaFromEeId={2}&startDate={3}&" +
                "endDate=undefined&adults={4}&children={5}&childAge1=0&childAge2=0&childAge3=0&nightsFrom={6}&" +
                "nightsTo={7}&nights={8}&minPrice=0&maxPrice={9}&searchType={15}&showDiscount={10}&infiniteMaxPrice={11}" +
                "&saveFilter=true&otherResults=true&allotmentStatus=undefined";

        private string _hotelEeId;
        private string _countryEeId;
        private string _areaFromEeId;
        private string _startDate;
        private string _pageCount;
        private string _adults;
        private string _children;
        private string _nightsFrom;
        private string _nightsTo;
        private string _nights;
        private string _maxPrice;
        private string _showDicount;
        private string _infiniteMaxPrice;
        private string _pageSize;
        private string _currentPage;
        private string _searchType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters">
        /// hotelEeId,
        /// countryEeId,
        /// areaFromEeId -  2671 - Moscow,
        /// startDate,
        /// adults,
        /// children,
        /// nightsFrom,
        /// nightsTo,
        /// nights,
        /// maxPrice,
        /// showDicount - default false ????,
        /// infiniteMaxPrice - default true,
        /// pageSize - default 20,
        /// currentPage - default 1,
        /// pageCount - default 1,
        /// searchType - Tour or Hotel
        /// </param>
        public override void SetRequestParameters(params string[] parameters)
        {
            if (parameters == null) return;
            _hotelEeId = parameters[0]; // hotel search lookup result
            _countryEeId = parameters[1]; // hotel search lookup result
            _areaFromEeId = parameters[2]; // 2671 - Moscow
            _startDate = parameters[3]; // user input
            _adults = parameters[4]; //user input, default 2
            _children = parameters[5]; //user input, can be 0
            _nightsFrom = parameters[6]; //user input
            _nightsTo = parameters[7]; //user input
            _nights = parameters[8]; //user input
            _maxPrice = parameters[9]; //default 500000
            _showDicount = parameters[10]; //default false ????
            _infiniteMaxPrice = parameters[11]; //default true
            _pageSize = parameters[12]; //default 20
            _currentPage = parameters[13]; //default 1
            _pageCount = parameters[14]; //default 1
            _searchType = parameters[15]; // Hotel - Tour
        }

        protected override string GetRequestUrl()
        {
            return string.Format(_baseUrl, _hotelEeId, _countryEeId, _areaFromEeId, _startDate, _adults, _children,
                _nightsFrom, _nightsTo, _nights, _maxPrice, _showDicount, _infiniteMaxPrice, _pageSize,
                _currentPage, _pageCount, _searchType);
        }
    }
}
