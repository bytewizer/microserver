using System;
using System.Collections;
using System.Text;

#if !(MF_FRAMEWORK_VERSION_V4_2 && MF_FRAMEWORK_VERSION_V4_3)
using System.Globalization;
#endif

/*
 * Copyright 2014 by Mario Vernari, Cet Electronics
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace MicroServer.Serializers.Json
{
    public static class JsonHelpers
    {
        /// <summary>
        /// Define the whitespace-equivalent characters
        /// </summary>
        private const string WhiteSpace = " \u0009\u000A\u000D";


        /// <summary>
        /// Parse a JSON-string to a specific DOM
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static JToken Parse(string text)
        {
            var rd = new JSonReader(text);
            var ctx = new JsonParserContext(rd)
                .ConsumeWhiteSpace()
                .First(true, JsonHelpers.ConsumeObject, JsonHelpers.ConsumeArray);

            return (JToken)ctx.Result;
        }


        /// <summary>
        /// Serialize the specific DOM to a JSON-string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Serialize(JToken source)
        {
            var sb = new StringBuilder();
            source.Serialize(sb);
            return sb.ToString();
        }


        /// <summary>
        /// Consume an arbitrary number of whitespace characters
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeWhiteSpace(
            this JsonParserContext ctx
            )
        {
            JSonReader src = ctx.Begin();

            for (int p = src.Position, len = src.Text.Length; p < len; p++)
            {
                if (WhiteSpace.IndexOf(src.Text[p]) < 0)
                {
                    src.Position = p;
                    break;
                }
            }

            return ctx;
        }


        /// <summary>
        /// Consume any character (at least one) in the specified set
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="charset"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeAnyChar(
            this JsonParserContext ctx,
            string charset,
            bool throws
            )
        {
            JSonReader src = ctx.Begin();

            char c;
            if (charset.IndexOf(c = src.Text[src.Position]) >= 0)
            {
                src.Position++;
                ctx.SetResult(c);
            }
            else if (throws)
            {
                throw new JsonParseException("Expected char not found.");
            }

            return ctx;
        }


        /// <summary>
        /// Consume all the characters in the specified sequence
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="sequence"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeAllChars(
            this JsonParserContext ctx,
            string sequence,
            bool throws
            )
        {
            JSonReader src = ctx.Begin();

            for (int q = 0, p = src.Position; q < sequence.Length; q++, p++)
            {
                if (src.Text[p] != sequence[q])
                {
                    if (throws)
                    {
                        throw new JsonParseException("Expected char not found.");
                    }

                    return ctx;
                }
            }

            src.Position += sequence.Length;
            ctx.SetResult(sequence);
            return ctx;
        }


        /// <summary>
        /// Consume a JSON object
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeObject(
            this JsonParserContext ctx,
            bool throws
            )
        {
            if (ctx.ConsumeWhiteSpace().ConsumeAnyChar("{", throws).IsSucceeded)
            {
                var jctr = new JObject();
                bool shouldThrow = false;

                do
                {
                    ctx.Begin();
                    if (ctx.ConsumeString(shouldThrow).IsSucceeded)
                    {
                        var name = (string)(JValue)ctx.Result;
                        ctx.ConsumeWhiteSpace()
                            .ConsumeAnyChar(":", true)
                            .ConsumeValue(true);

                        jctr.Add(name, (JToken)ctx.Result);
                    }

                    shouldThrow = true;
                }
                while ((char)ctx.ConsumeWhiteSpace().ConsumeAnyChar(",}", true).Result == ',');

                ctx.SetResult(jctr);
            }

            return ctx;
        }


        /// <summary>
        /// Consume a JSON array
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeArray(
            this JsonParserContext ctx,
            bool throws
            )
        {
            if (ctx.ConsumeWhiteSpace().ConsumeAnyChar("[", throws).IsSucceeded)
            {
                var jctr = new JArray();
                bool shouldThrow = false;

                do
                {
                    ctx.Begin();
                    if (ctx.ConsumeValue(shouldThrow).IsSucceeded)
                        jctr.Add((JToken)ctx.Result);
                    shouldThrow = true;
                }
                while ((char)ctx.ConsumeWhiteSpace().ConsumeAnyChar(",]", true).Result == ',');

                ctx.SetResult(jctr);
            }

            return ctx;
        }


        /// <summary>
        /// Consume any suitable JSON value token
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeValue(
            this JsonParserContext ctx,
            bool throws
            )
        {
            return ctx.First(
                throws,
                JsonHelpers.ConsumeString,
                JsonHelpers.ConsumeNumber,
                JsonHelpers.ConsumeObject,
                JsonHelpers.ConsumeArray,
                JsonHelpers.ConsumeBoolean,
                JsonHelpers.ConsumeNull
                );
        }


        /// <summary>
        /// Consume a JSON numeric token
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeNumber(
            this JsonParserContext ctx,
            bool throws
            )
        {
            const string Leading = "-0123456789";
            const string Allowed = Leading + ".Ee+";

            if (ctx.ConsumeWhiteSpace().ConsumeAnyChar(Leading, throws).IsSucceeded)
            {
                JSonReader src = ctx.Begin();

                for (int p = src.Position, len = src.Text.Length; p < len; p++)
                {
                    if (Allowed.IndexOf(src.Text[p]) < 0)
                    {
                        ctx.SetResult(
                            new JValue { BoxedValue = double.Parse(src.Text.Substring(src.Position - 1, p - src.Position + 1)) }
                            );

                        src.Position = p;
                        break;
                    }
                }
            }

            return ctx;
        }


        /// <summary>
        /// Consume a JSON string token
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeString(
            this JsonParserContext ctx,
            bool throws
            )
        {
            if (ctx.ConsumeWhiteSpace().ConsumeAnyChar("\"", throws).IsSucceeded)
            {
                JSonReader src = ctx.Begin();

                for (int p = src.Position, len = src.Text.Length; p < len; p++)
                {
                    if ((src.Text[p]) == '"')
                    {
                        ctx.SetResult(
                            new JValue { BoxedValue = src.Text.Substring(src.Position, p - src.Position) }
                            );

                        src.Position = p + 1;
                        break;
                    }
                }
            }

            return ctx;
        }


        /// <summary>
        /// Consume a JSON boolean token
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeBoolean(
            this JsonParserContext ctx,
            bool throws
            )
        {
            if (ctx.ConsumeWhiteSpace().ConsumeAnyChar("ft", throws).IsSucceeded)
            {
                bool flag = (char)ctx.Result == 't';
                ctx.ConsumeAllChars(flag ? "rue" : "alse", true);
                ctx.SetResult(
                    new JValue { BoxedValue = flag }
                    );
            }

            return ctx;
        }


        /// <summary>
        /// Consume the JSON "null" token
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        private static JsonParserContext ConsumeNull(
            this JsonParserContext ctx,
            bool throws
            )
        {
            if (ctx.ConsumeWhiteSpace().ConsumeAnyChar("n", throws).IsSucceeded)
            {
                ctx.ConsumeAllChars("ull", true);
                ctx.SetResult(
                    new JValue { BoxedValue = null }
                    );
            }

            return ctx;
        }


        /// <summary>
        /// Yield the consumption of the current context to a series of possible parsers
        /// The control will pass to the first one able to manage the source.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <param name="funcs"></param>
        /// <returns></returns>
        private static JsonParserContext First(
            this JsonParserContext ctx,
            bool throws,
            params JsonParseDelegate[] funcs
            )
        {
            for (int i = 0; i < funcs.Length; i++)
            {
                if (funcs[i](ctx, false).IsSucceeded)
                {
                    return ctx;
                }
            }

            if (throws)
            {
                throw new JsonParseException("Unmatched handler for context.");
            }
            else
            {
                return ctx;
            }
        }


        /// <summary>
        /// Delegate used for the "smart" matching pattern selection
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="throws"></param>
        /// <returns></returns>
        internal delegate JsonParserContext JsonParseDelegate(JsonParserContext ctx, bool throws);

    }


    /// <summary>
    /// Trivial string reader
    /// </summary>
    internal class JSonReader
    {
        public JSonReader(string text)
        {
            this.Text = text;
        }

        public readonly string Text;
        public int Position;

    }


    /// <summary>
    /// Context data used by the parser
    /// </summary>
    internal class JsonParserContext
    {
        public JsonParserContext(JSonReader source)
        {
            this._source = source;
        }

        private readonly JSonReader _source;
        public bool IsSucceeded { get; private set; }
        public object Result { get; private set; }


        public JSonReader Begin()
        {
            this.Result = null;
            this.IsSucceeded = false;
            return this._source;
        }

        public void SetResult(object result)
        {
            this.Result = result;
            this.IsSucceeded = true;
        }
    }


    /// <summary>
    /// JSON parser specific exception thrown in case of error
    /// </summary>
    public class JsonParseException
        : Exception
    {
        public JsonParseException(string message)
            : base(message)
        {
        }
    }


    /// <summary>
    /// Abstract base for any elemento of the JSON DOM
    /// </summary>
    public abstract class JToken
    {
        protected JToken() { }
        internal abstract void Serialize(StringBuilder sb);


        #region String conversion operators

        public static implicit operator JToken(string value)
        {
            return new JValue { BoxedValue = value };
        }

        public static implicit operator string(JToken value)
        {
            var jval = value as JValue;

            if (jval != null)
            {
                if (jval.BoxedValue is string == false)
                    throw new InvalidCastException();

                return (string)jval.BoxedValue;
            }

            throw new InvalidCastException();
        }

        #endregion


        #region Numeric conversion operator

        public static implicit operator JToken(double value)
        {
            return new JValue { BoxedValue = value };
        }

        public static implicit operator JToken(int value)
        {
            return new JValue { BoxedValue = (double)value };
        }

        public static implicit operator double(JToken value)
        {
            var jval = value as JValue;

            if (jval != null)
            {
                if (jval.BoxedValue is double == false)
                    throw new InvalidCastException();

                return (double)jval.BoxedValue;
            }

            throw new InvalidCastException();
        }

        #endregion


        #region Boolean conversion operator

        public static implicit operator JToken(bool value)
        {
            return new JValue { BoxedValue = value };
        }

        public static implicit operator bool(JToken value)
        {
            var jval = value as JValue;

            if (jval != null)
            {
                if (jval.BoxedValue is bool == false)
                    throw new InvalidCastException();

                return (bool)jval.BoxedValue;
            }

            throw new InvalidCastException();
        }

        #endregion

    }


    /// <summary>
    /// Represent a valued-node of the JSON DOM
    /// </summary>
    public class JValue
        : JToken
    {
        internal object BoxedValue;

        public object RawValue { get { return this.BoxedValue; } }

        internal override void Serialize(StringBuilder sb)
        {
            if (this.BoxedValue == null)
            {
                sb.Append("null");
            }
            else if (this.BoxedValue is bool)
            {
                sb.Append((bool)this.BoxedValue ? "true" : "false");
            }
            else if (this.BoxedValue is double)
            {
#if (MF_FRAMEWORK_VERSION_V4_2 || MF_FRAMEWORK_VERSION_V4_3)
                sb.Append(((double)this.BoxedValue).ToString());
#else
                sb.Append(((double)this.BoxedValue).ToString(CultureInfo.InvariantCulture));
#endif
            }
            else if (this.BoxedValue is string)
            {
                sb.Append('"');
                sb.Append((string)this.BoxedValue);
                sb.Append('"');
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }


    /// <summary>
    /// Represent a key-value pair instance used for the JSON object bag
    /// </summary>
    public class JProperty
        : JToken
    {
        public JProperty(
            string name,
            JToken value
            )
        {
            this.Name = name;
            this.Value = value;
        }


        public readonly string Name;
        public JToken Value;


        internal override void Serialize(StringBuilder sb)
        {
            sb.Append('"');
            sb.Append(this.Name);
            if (this.Value != null)
            {
                sb.Append("\":");
                this.Value.Serialize(sb);
            }
            else
            {
                sb.Append("\":null");
            }
        }

    }


    /// <summary>
    /// Represent a JSON object
    /// </summary>
    public class JObject
        : JToken, IEnumerable
    {
        private const int ChunkSize = 8;
        private static readonly JProperty[] Empty = new JProperty[0];

        private JProperty[] _items = Empty;
        private int _itemCount;


        /// <summary>
        /// Add a keyed-value to the object's bag
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <remarks>You cannot use a key that is already existent in the bag</remarks>
        public void Add(string name, JToken value)
        {
            if (this.IndexOf(name) > 0)
                throw new ArgumentException("Duplicate key.");

            if (this._itemCount >= this._items.Length)
            {
                var old = this._items;
                Array.Copy(
                    old,
                    this._items = new JProperty[this._itemCount + ChunkSize],
                    this._itemCount
                    );
            }

            this._items[this._itemCount++] = new JProperty(name, value);
        }


        /// <summary>
        /// Get an existent keyed-value from the bag.
        /// When set, the value replaces the existent entry, or is added if not present.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JToken this[string name]
        {
            get
            {
                return this._items[this.IndexOf(name)].Value;
            }
            set
            {
                int index;
                if ((index = this.IndexOf(name)) >= 0)
                {
                    this._items[index].Value = value;
                }
                else
                {
                    this.Add(name, value);
                }
            }
        }


        private int IndexOf(string name)
        {
            for (int i = 0; i < this._itemCount; i++)
            {
                if (this._items[i].Name == name)
                    return i;
            }

            return -1;
        }


        internal override void Serialize(StringBuilder sb)
        {
            sb.Append('{');

            for (int i = 0; i < this._itemCount; i++)
            {
                if (i > 0) sb.Append(',');

                this._items[i].Serialize(sb);
            }

            sb.Append('}');
        }


        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < this._itemCount; i++)
            {
                yield return this._items[i];
            }
        }

    }


    /// <summary>
    /// Represent a JSON array
    /// </summary>
    public class JArray
        : JToken, IEnumerable
    {
        private ArrayList _items = new ArrayList();


        /// <summary>
        /// Add an object at the end of the collection
        /// </summary>
        /// <param name="item"></param>
        public void Add(JToken item)
        {
            this.Insert(
                this._items.Count,
                item
                );
        }


        /// <summary>
        /// Insert an object at the specified position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="item"></param>
        public void Insert(
            int position,
            JToken item
            )
        {
            this._items.Insert(
                position,
                item
                );
        }


        /// <summary>
        /// Get the value stored at the specified position.
        /// When set, the value replaces the existent entry.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public JToken this[int index]
        {
            get { return (JToken)this._items[index]; }
            set { this._items[index] = value; }
        }


        /// <summary>
        /// Remove the entry at the specified position
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            this._items.RemoveAt(index);
        }


        public IEnumerator GetEnumerator()
        {
            return this._items.GetEnumerator();
        }


        internal override void Serialize(StringBuilder sb)
        {
            sb.Append('{');
            for (int i = 0, count = this._items.Count; i < count; i++)
            {
                if (i > 0) sb.Append(',');

                JToken item;
                if ((item = (JToken)this._items[i]) != null)
                {
                    item.Serialize(sb);
                }
                else
                {
                    sb.Append("null");
                }
            }
            sb.Append('}');
        }

    }
}
