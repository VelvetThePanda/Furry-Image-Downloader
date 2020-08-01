using System;
using System.Collections.Generic;
using System.Text;

namespace MFCD
{
    public abstract class SearchQueryBase
    {
        public string Tags { get; set; }
        public string BlackList { get; set; }
        public int PageLimit { get; set; }

        public virtual void ParseBlacklist()
        {
            var sb = new StringBuilder();
            foreach(var word in BlackList.Split(' '))
            {
                if (!word.Contains('-'))
                {
                    sb.Append($"-{word} ");
                }
                else
                {
                    sb.Append(word);
                }
            }

            BlackList = sb.ToString();
        }

    }
}
