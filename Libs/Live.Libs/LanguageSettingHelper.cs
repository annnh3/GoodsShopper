using System;
using System.Collections.Generic;
using System.Linq;

namespace Live.Libs
{
    /// <summary>
    /// 語系相關設定小幫手
    /// </summary>
    public class LanguageSettingHelper
    {
        /// <summary>
        /// 語系總和轉換語系陣列
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static IEnumerable<LanguageType> GetLangType(int rules)
        {
            var langTypePowList = Enum.GetValues(typeof(LanguageType))
                .Cast<LanguageType>()
                .Select(s => Convert.ToInt32(Math.Pow(2, (int)s)))
                .ToList();

            //大於總和 或 小於none 就return
            if (rules > langTypePowList.Sum() || rules < Convert.ToInt32(Math.Pow(2, (int)LanguageType.None)))
            {
                return new List<LanguageType>();
            }

            var result = new List<LanguageType>();

            foreach (var r in langTypePowList)
            {
                //二進制計算
                var res = rules & r;

                if (res != 0)
                {
                    result.Add((LanguageType)Convert.ToInt32(Math.Log(r, 2)));
                }
            }

            return result;
        }


        /// <summary>
        /// 勾選對應語系算出總值
        /// </summary>
        /// <returns></returns>
        public static int GetLangCorrespondSum(bool enableTw, bool enableCn, bool enableEn, bool enableVi, bool enableTh, bool enableWd, bool enableId)
        {
            var sum = 0;

            int GetSum(LanguageType langType) => Convert.ToInt32(Math.Pow(2, (int)langType));

            if (enableTw)
            {
                sum += GetSum(LanguageType.Tw);
            }

            if (enableCn)
            {
                sum += GetSum(LanguageType.Cn);
            }

            if (enableEn)
            {
                sum += GetSum(LanguageType.En);
            }

            if (enableVi)
            {
                sum += GetSum(LanguageType.Vi);
            }

            if (enableTh)
            {
                sum += GetSum(LanguageType.Th);
            }

            if (enableWd)
            {
                sum += GetSum(LanguageType.Wd);
            } 
            
            if (enableId)
            {
                sum += GetSum(LanguageType.Id);
            }

            return sum;
        }

    }
}