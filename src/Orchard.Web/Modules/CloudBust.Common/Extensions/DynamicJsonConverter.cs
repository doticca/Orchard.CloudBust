using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace CloudBust.Common.Extensions {
    public class DynamicJsonConverter : JavaScriptConverter {
        public override IEnumerable<Type> SupportedTypes => new ReadOnlyCollection<Type>(new List<Type>(new[] {typeof(object)}));

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
            throw new NotImplementedException();
        }

        #region Nested type: DynamicJsonObject

        private sealed class DynamicJsonObject : DynamicObject {
            private readonly IDictionary<string, object> _dictionary;

            public DynamicJsonObject(IDictionary<string, object> dictionary) {
                _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            }

            public override string ToString() {
                var sb = new StringBuilder("{");
                ToString(sb);
                return sb.ToString();
            }

            private void ToString(StringBuilder sb) {
                var firstInDictionary = true;
                foreach (var pair in _dictionary) {
                    if (!firstInDictionary)
                        sb.Append(",");
                    firstInDictionary = false;
                    var value = pair.Value;
                    var name = pair.Key;
                    if (value is string) {
                        sb.AppendFormat("{0}:\"{1}\"", name, value);
                    }
                    else if (value is IDictionary<string, object> objects) {
                        new DynamicJsonObject(objects).ToString(sb);
                    }
                    else if (value is ArrayList list) {
                        sb.Append(name + ":[");
                        var firstInArray = true;
                        foreach (var arrayValue in list) {
                            if (!firstInArray)
                                sb.Append(",");
                            firstInArray = false;
                            if (arrayValue is IDictionary<string, object> dictionary)
                                new DynamicJsonObject(dictionary).ToString(sb);
                            else if (arrayValue is string)
                                sb.AppendFormat("\"{0}\"", arrayValue);
                            else
                                sb.AppendFormat("{0}", arrayValue);
                        }

                        sb.Append("]");
                    }
                    else {
                        sb.AppendFormat("{0}:{1}", name, value);
                    }
                }

                sb.Append("}");
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result) {
                if (!_dictionary.TryGetValue(binder.Name, out result)) return true;

                if (result is IDictionary<string, object> dictionary) {
                    result = new DynamicJsonObject(dictionary);
                    return true;
                }

                if (result is ArrayList arrayList && arrayList.Count > 0) result = arrayList[0] is IDictionary<string, object> ? new List<object>(arrayList.Cast<IDictionary<string, object>>().Select(x => new DynamicJsonObject(x))) : new List<object>(arrayList.Cast<object>());

                return true;
            }
        }

        #endregion
    }
}