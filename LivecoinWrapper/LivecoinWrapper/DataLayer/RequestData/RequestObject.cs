﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using LivecoinWrapper.Helper;
using static LivecoinWrapper.Helper.Enums;
using static LivecoinWrapper.Helper.Enums.RequestType;

namespace LivecoinWrapper.DataLayer.RequestData
{
    public abstract class RequestObject
    {
        private readonly string apiSec;

        private const string urlSegmentExchange = "/exchange/";
        private const string urlSegmentPayment  = "/payment/";
        private const string urlSegmentInfo     = "/info/";

        internal SortedDictionary<string, string> arguments;

        internal string Url { get; private set; }
        internal string Sign { get; private set; }

        public RequestObject() { }

        public RequestObject(string apiSec)
        {
            this.apiSec = apiSec;
        }

        protected void GenerateRequest(RequestType type, string method)
        {
            if (type == exchange_GET) Url = new StringBuilder(urlSegmentExchange)
                    .AppendFormat("{0}?{1}", method, arguments.ToKeyValueString()).ToString();

            if (type == exchangeAuth_GET)
            {
                Url = new StringBuilder(urlSegmentExchange)
                        .AppendFormat("{0}?{1}", method, arguments.ToKeyValueString()).ToString();
                CreateSignature();
            }

            if (type == exchangeAuth_POST)
            {
                Url = new StringBuilder(urlSegmentExchange).Append(method).ToString();
                CreateSignature();
            }

            if (type == payment_GET)
            {
                Url = new StringBuilder(urlSegmentPayment)
                        .AppendFormat("{0}?{1}", method, arguments.ToKeyValueString()).ToString();
                CreateSignature();
            }
            if (type == payment_POST) { /*todo*/ }

            if (type == info_GET) Url = new StringBuilder(urlSegmentInfo).Append(method).ToString();            
        }

        private void CreateSignature()
        {
            var encryptor    = new HMACSHA256(Encoding.ASCII.GetBytes(apiSec));
            byte[] postBytes = Encoding.ASCII.GetBytes(arguments.ToKeyValueString());

            Sign = BitConverter.ToString(encryptor.ComputeHash(postBytes)).Replace("-", "").ToUpper();
        }
    }
}
