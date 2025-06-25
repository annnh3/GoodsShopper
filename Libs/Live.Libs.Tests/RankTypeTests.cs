using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Live.Libs.Tests
{
    [TestClass]
    public class RankTypeTests
    {
        [TestMethod]
        public void 聊天顯示會員排名戳記排序測試()
        {
            var ranks = new[]
            {
                RankType.WeekFirst,
                RankType.MonthFifth,
            };

            var sort = ranks.ChatRankSymbolSort().ToArray();

            Assert.AreEqual(RankType.MonthFifth, sort[0]);
            Assert.AreEqual(RankType.WeekFirst, sort[1]);
        }

        [TestMethod]
        public void 聊天顯示會員排名戳記排序資料null測試()
        {
            RankType[] ranks = default;

            var sort = ranks.ChatRankSymbolSort().ToArray();

            Assert.IsFalse(sort.Any());
        }
    }
}