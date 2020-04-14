﻿/***

   Copyright (C) 2020. rollrat. All Rights Reserved.
   
   Author: Custom Crawler Developer

***/

using MasterDevs.ChromeDevTools;
using MasterDevs.ChromeDevTools.Protocol.Chrome.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomCrawler
{
    /// <summary>
    /// CustomCrawlerDynamicsRequestInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomCrawlerDynamicsRequestInfo : Window
    {
        public CustomCrawlerDynamicsRequestInfo(RequestWillBeSentEvent request, ResponseReceivedEvent response)
        {
            InitializeComponent();

            var builder = new StringBuilder();

            builder.Append($"Request Type: {request.Type.ToString()}\r\n");
            builder.Append($"Request Body:\r\n");
            builder.Append(JsonConvert.SerializeObject(request.Request, Formatting.Indented));
            builder.Append($"\r\n");
            builder.Append($"\r\n");
            builder.Append($"==============================================================================================================================\r\n");
            builder.Append($"Response Raw:\r\n");

            var result = CustomCrawlerDynamics.ss.SendAsync(new GetResponseBodyCommand
            {
                RequestId = request.RequestId
            }).Result;

            string body;

            if (result.Result.Base64Encoded)
                result.Result.Body.TryParseBase64(out body);
            else
                body = result.Result.Body;

            try
            {
                body = JsonConvert.SerializeObject(JToken.Parse(body), Formatting.Indented);
            }
            catch { }

            builder.Append(body);

            builder.Append($"\r\n");
            builder.Append($"==============================================================================================================================\r\n");
            builder.Append($"Response Body:\r\n");
            builder.Append(JsonConvert.SerializeObject(response.Response, Formatting.Indented));
            builder.Append($"\r\n");
            builder.Append($"==============================================================================================================================\r\n");
            builder.Append($"Request Initiator:\r\n");

            var stack = request.Initiator.Stack;
            while (stack != null)
            {
                if (!string.IsNullOrEmpty(stack.Description))
                    builder.Append("Description: " + stack.Description + "\r\n");

                foreach (var frame in stack.CallFrames)
                    builder.Append($"{frame.Url}:<{frame.FunctionName}>:{frame.LineNumber + 1}:{frame.ColumnNumber + 1}\r\n");

                stack = stack.Parent;
            }

            Info.Text = builder.ToString();
        }
    }
}
