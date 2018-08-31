using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Agema.Json {
	/// <summary>
	///     Generic JSON.NET helper methods
	/// </summary>
	public static class JsonHelper {
		/// <summary>
		///     Converts JTOken to Dictionary
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T1">The type of the 1.</typeparam>
		/// <param name="jToken">The j token.</param>
		/// <returns>
		///     Dictionary
		/// </returns>
		public static Dictionary<T, T1> ToDictionary<T, T1>(this JToken jToken) {
			var dict = jToken.ToObject<Dictionary<T, T1>>();

			return dict;
		}

		/// <summary>
		///     Perform a deep Copy of the object, using Json as a serialisation method.
		/// </summary>
		/// <typeparam name="T">The type of object being copied.</typeparam>
		/// <param name="source">The object instance to copy.</param>
		/// <returns>The copied object.</returns>
		public static T CloneJson<T>(this T source) {
			// Don't serialize a null object, simply return the default for that object
			if (ReferenceEquals(source, null))
				return default(T);

			// initialize inner objects individually
			// for example in default constructor some list property initialized with some values,
			// but in 'source' these items are cleaned -
			// without ObjectCreationHandling.Replace default constructor values will be added to result
			var deserializeSettings = new JsonSerializerSettings {
				ObjectCreationHandling = ObjectCreationHandling.Replace
			};

			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
		}

		/// <summary>
		///     Serializes object to Json
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static string ToJson<T>(this T data) {
			return data.ToJson(Debugger.IsAttached
				? Formatting.Indented
				: Formatting.None);
		}

		/// <summary>
		///     Serializes object to json using the CamelCasePropertyNamesContractResolver
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static string ToJsonCamelCase(this object data) {
			var settings = new JsonSerializerSettings {
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Formatting = Debugger.IsAttached
					? Formatting.Indented
					: Formatting.None
			};

			var json = JsonConvert.SerializeObject(data, settings);

			return json;
		}

		/// <summary>
		///     Serializes object to json. Formatting None/Indented
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data">The data.</param>
		/// <param name="formatting">The formatting.</param>
		/// <returns></returns>
		public static string ToJson<T>(this T data, Formatting formatting) {
			if (data == null)
				return string.Empty;

			var json = JsonConvert.SerializeObject(data, formatting);

			return json;
		}
	}
}
