using System.Collections.Generic;
using System.Linq;

namespace Live.Libs
{
    /// <summary>
    /// 週與月排名類別
    /// </summary>
    public enum RankType
    {
        /// <summary>
        /// 週第1名
        /// </summary>
        WeekFirst = 1,

        /// <summary>
        /// 週第2名
        /// </summary>
        WeekSecond = 2,

        /// <summary>
        /// 週第3名
        /// </summary>
        WeekThird = 3,

        /// <summary>
        /// 週第4名
        /// </summary>
        WeekFourth = 4,

        /// <summary>
        /// 週第5名
        /// </summary>
        WeekFifth = 5,

        /// <summary>
        /// 週第6名
        /// </summary>
        WeekSixth = 6,

        /// <summary>
        /// 週第7名
        /// </summary>
        WeekSeventh = 7,

        /// <summary>
        /// 週第8名
        /// </summary>
        WeekEighth = 8,

        /// <summary>
        /// 週第9名
        /// </summary>
        WeekNinth = 9,

        /// <summary>
        /// 週第10名
        /// </summary>
        WeekTenth = 10,


        /// <summary>
        /// 月第1名
        /// </summary>
        MonthFirst = 11,

        /// <summary>
        /// 月第2名
        /// </summary>
        MonthSecond = 12,

        /// <summary>
        /// 月第3名
        /// </summary>
        MonthThird = 13,

        /// <summary>
        /// 月第4名
        /// </summary>
        MonthFourth = 14,

        /// <summary>
        /// 月第5名
        /// </summary>
        MonthFifth = 15,

        /// <summary>
        /// 月第6名
        /// </summary>
        MonthSixth = 16,

        /// <summary>
        /// 月第7名
        /// </summary>
        MonthSeventh = 17,

        /// <summary>
        /// 月第8名
        /// </summary>
        MonthEighth = 18,

        /// <summary>
        /// 月第9名
        /// </summary>
        MonthNinth = 19,

        /// <summary>
        /// 月第10名
        /// </summary>
        MonthTenth = 20
    }

    /// <summary>
    /// RankType Extension
    /// </summary>
    public static class RankTypeExtension
    {
        /// <summary>
        /// 聊天顯示會員排名戳記排序
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<RankType> ChatRankSymbolSort(this IEnumerable<RankType> types)
        {
            var dic = new Dictionary<RankType, int>
            {
                {RankType.MonthFirst, 1},
                {RankType.MonthSecond, 2},
                {RankType.MonthThird, 3},
                {RankType.MonthFourth, 4},
                {RankType.MonthFifth, 5},
                {RankType.MonthSixth, 6},
                {RankType.MonthSeventh, 7},
                {RankType.MonthEighth, 8},
                {RankType.MonthNinth, 9},
                {RankType.MonthTenth, 10},
                {RankType.WeekFirst, 11},
                {RankType.WeekSecond, 12},
                {RankType.WeekThird, 13},
                {RankType.WeekFourth, 14},
                {RankType.WeekFifth, 15},
                {RankType.WeekSixth, 16},
                {RankType.WeekSeventh, 17},
                {RankType.WeekEighth, 18},
                {RankType.WeekNinth, 19},
                {RankType.WeekTenth, 20},
            };

            if (types == null || types.Any(p => !dic.ContainsKey(p)))
            {
                return new RankType[] { };
            }

            var sortTypes = types.ToArray();

            // swap
            for (var i = 0; i < sortTypes.Count(); i++)
            {
                for (var j = i + 1; j < sortTypes.Count(); j++)
                {
                    if (dic[sortTypes[i]] > dic[sortTypes[j]])
                    {
                        (sortTypes[i], sortTypes[j]) = (sortTypes[j], sortTypes[i]);
                    }
                }
            }

            return sortTypes;
        }
    }
}