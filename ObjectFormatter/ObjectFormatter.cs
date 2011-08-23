using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

namespace ObjectFormatting
{
    public static class ObjectFormatter
    {
        public static string TokenFormat(this string formatString, IDictionary<string, object> tokens)
        {
            return TokenFormat(formatString, null, tokens);
        }

        public static string TokenFormat(this string formatString, object source)
        {
            return TokenFormat(formatString, source, null);
        }

        public static string TokenFormat(this string formatString, object source, IDictionary<string, object> tokens)
        {
            var tokenList = new Dictionary<string, IndexValue>();
            var newFormat = new StringBuilder(formatString.Length * 3 / 2);

            var index = 0;

            if (tokens != null)
                foreach (var kvp in tokens)
                    tokenList.Add(kvp.Key, new IndexValue(index++, kvp.Value));

            var format = formatString.ToCharArray();

            var leftIndex = 0;
            var rightIndex = 0;
            var isInToken = false;

            while (rightIndex < format.Length)
            {
                var currentChar = format[rightIndex];
                rightIndex++;

                switch (currentChar)
                {
                    case '{':
                        if (format[rightIndex] == '{') 
                            rightIndex++;
                        
                        else
                        {
                            newFormat.Append(format, leftIndex, rightIndex - leftIndex);
                            leftIndex = rightIndex;
                            isInToken = true;
                        }
                        
                        break;

                    case '}':
                    case ':':
                        if (isInToken)
                        {
                            var token = GetStringSection(format, leftIndex, rightIndex);
                            if (!tokenList.ContainsKey(token))
                            {
                                if (source != null)
                                    tokenList.Add(token, new IndexValue(index++,  GetPropertyValueFromPath(source, token)));

                                else
                                    tokenList.Add(token, new IndexValue(index++,  null));
                            }
                            newFormat.Append(tokenList[token].Index);

                            leftIndex = rightIndex - 1;
                            isInToken = false;
                        }
                        break;
                }
            }

            newFormat.Append(format, leftIndex, rightIndex - leftIndex);
            return string.Format(newFormat.ToString(), tokenList.Select(tokenPair => tokenPair.Value.Value).ToArray());
        }

        private struct IndexValue
        {
            public int Index;
            public object Value;

            public IndexValue(int index, object value)
            {
                Index = index;
                Value = value;
            }
        }

        private static string GetStringSection(char[] format, int leftIndex, int rightIndex)
        {
            return new String(format, leftIndex, rightIndex - leftIndex - 1);
        }

        private static object GetPropertyValueFromPath(object target, string path)
        {
            var retVal = target;

            foreach (var sections in path
                .Split('.')
                .Select(property => property
                    .Replace("]", string.Empty)
                    .Split(new[] { '[' }, StringSplitOptions.RemoveEmptyEntries)
                )
            )
            {
                retVal = GetPropertyValue(retVal, sections[0], null);

                for (var i = 1; i < sections.Length; i++)
                    retVal = GetPropertyValue(retVal, retVal is string ? "Chars" : "Item", sections[i]);
            }

            return retVal;
        }

        private static object GetPropertyValue(object target, string name, string indexList)
        {
            var indexes = GetIndexArray(indexList);

            if (target.GetType().IsArray)
                return ((Array) target).GetValue(indexes.Select(index => (int)index).ToArray());

            var property = target.GetType().GetProperty(name, indexes.Select(index => index.GetType()).ToArray());
            if (property == null)
                throw new ArgumentException(string.Format("Unable to find the property {0} in type {1}.", name, target.GetType().FullName), "name");

            return property.GetValue(target, indexes);
        }

        private static object[] GetIndexArray(string indexes)
        {
            if (indexes == null) return new object[] { };

            var retVal = new List<object>();
            foreach (var index in indexes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (index.Contains('"'))
                    retVal.Add(index.Trim().Replace("\"", string.Empty));

                else
                    retVal.Add(int.Parse(index.Trim()));
            }

            return retVal.ToArray();
        }
    }
}
