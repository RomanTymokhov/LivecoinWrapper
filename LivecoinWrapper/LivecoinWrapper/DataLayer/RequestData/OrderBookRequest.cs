﻿using System.Collections.Generic;

using static LivecoinWrapper.Helper.Enums.RequestType;

namespace LivecoinWrapper.DataLayer.RequestData
{
    public class OrderBookRequest : RequestObject
    {
        public OrderBookRequest(string pairId, bool groupByPrice, ushort? depth) : base()
        {
            arguments = new Dictionary<string, string>
            {
                ["currencyPair"] = pairId,
                ["groupByPrice"] = groupByPrice.ToString()
            };

            if (depth != null) arguments.Add("depth", depth.ToString());

            GenerateRequest(exchange, "order_book");
        }
    }
}
