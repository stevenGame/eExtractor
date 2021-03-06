﻿using eExtractor.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eExtractor.Test
{
    [TestClass]
    public class OffsetParserTest
    {
        class Message
        {
            public string Code { get; set; }
            public string TimeStamp { get; set; }
            public string SenderName { get; set; }

            // AFH Service Extension
            public string FailedPanel { get; set; }

            // Extensions
            public string NewSerial { get; set; }
            public string OriginalSerial { get; set; }
            public string CardData { get; set; }
            public byte[] CardDataEncrypted { get; set; }
            public string Others { get; set; }
            public string Status { get; set; }
            public string CardReader { get; set; }
            public string Suspicious { get; set; }
            public bool IsValid { get; set; }
            public string Exception { get; set; }
            public string DateEx { get; set; }
            public string LogText { get; set; }
            public int ExceptionId { get; set; }
            public string CardType { get; set; }
        }
        [TestMethod]
        public void TestOffsetParse()
        {
            string text = "P99912:29:12 10/26/17 P5V1_211";
            var extractor = OffsetExtractor.Build<Message>(new List<OffsetRule> {
                new OffsetRule{ Field = "Code", Offset = 1, Length = 3 },
                new OffsetRule{ Field = "TimeStamp", Offset = 4, Length = 17 },
                new OffsetRule{ Field = "SenderName", Offset = 22, Length = 8 },
             
            });
            var message = extractor.Parse(text);
            Assert.AreEqual("999", message.Code);
            Assert.AreEqual("12:29:12 10/26/17", message.TimeStamp);
            Assert.AreEqual("P5V1_211", message.SenderName);
        }
    }
}
