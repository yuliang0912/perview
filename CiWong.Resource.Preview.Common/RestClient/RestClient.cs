using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using RestSharp;
using CiWong.Resource.Preview.Common;

namespace CiWong.Resource.Preview
{
    #region RestClient扩展方法
    /// <summary>
    /// RestClient扩展方法
    /// </summary>
    public class RestClient
    {
        private readonly int userId;
        private readonly string baseUrl;

        public RestClient(int userId, string baseUrl = null)
        {
            this.userId = userId;
            this.baseUrl = baseUrl ?? SiteSettings.Instance.WebApi.Domain;// "http://192.168.1.61:8123"; 
        }

        public T ExecuteGet<T>(string resource, object body = null)
            where T : new()
        {
            return Execute<T>(resource, body);
        }

        public T ExecutePost<T>(string resource, object body = null)
            where T : new()
        {
            return Execute<T>(resource, body, Method.POST);
        }

        public T ExecuteDelete<T>(string resource, object body = null)
            where T : new()
        {
            return Execute<T>(resource, body, Method.DELETE);
        }

        public T ExecutePut<T>(string resource, object body = null)
            where T : new()
        {
            return Execute<T>(resource, body, Method.PUT);
        }

        public T Execute<T>(string resource, object body = null, Method method = Method.GET, IEnumerable<KeyValuePair<string, string>> headers = null)
            where T : new()
        {
            return GetResponse<T>(resource, body, method, headers).Data;
        }

        /// <summary>
        /// 执行API返回字符串
        /// </summary>
        /// <param name="resource">API地址</param>
        /// <param name="method">请求方式,默认GET</param>
        /// <param name="body">参数实体</param>
        /// <param name="headers"></param>
        /// <returns>结果字符串</returns>
        public string ExecuteContent(string resource, object body = null, Method method = Method.GET, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var client = new RestSharp.RestClient(this.baseUrl);

            var request = BuildRequest(resource, method, body, headers);

            return client.Execute(request).Content;

            //return GetResponse<ReturnResult>(resource, body, method, headers).Content;
        }

        private IRestResponse<T> GetResponse<T>(string resource, object body = null, Method method = Method.GET, IEnumerable<KeyValuePair<string, string>> headers = null)
            where T : new()
        {
            var client = new RestSharp.RestClient(this.baseUrl);
            client.AddHandler("application/json", new CustomJsonDeserializer());

            var request = BuildRequest(resource, method, body, headers);

            //var x = client.Execute(request);

            return client.Execute<T>(request);
        }

        private RestRequest BuildRequest(string resource, Method method = Method.GET, object body = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentNullException("resource", "请求资源url不能为空。");
            }

            var request = new CustomerRestRequest(method)
            {
                Resource = resource,
                RequestFormat = DataFormat.Json,
                DateFormat = "yyyy-MM-dd HH:mm:ss",
                JsonSerializer = new CustomJsonSerializer()
            };

            var effectiveHeaders = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Content-Type", "application/json"),
                new KeyValuePair<string, string>("Accept", "application/json"),
                new KeyValuePair<string, string>("_appid", "1")
            };

            if (userId > 0)
            {
                effectiveHeaders.Add(new KeyValuePair<string, string>("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.userId + ":"))));
                effectiveHeaders.Add(new KeyValuePair<string, string>("_userid", this.userId.ToString(CultureInfo.InvariantCulture)));
                effectiveHeaders.Add(new KeyValuePair<string, string>("_username", this.userId.ToString()));
            }

            if (headers != null && headers.Any())
            {
                effectiveHeaders.AddRange(headers);
            }

            foreach (var pair in effectiveHeaders)
            {
                request.AddHeader(pair.Key, pair.Value);
            }

            if (body == null)
            {
                return request;
            }

            //RequestBody只能工作在Post和Put模式
            if (method == Method.POST || method == Method.PUT)
            {
                request.AddObject(body);
            }
            else
            {
                request.AddObject(body);
            }

            return request;
        }
    }

    #endregion

    /// <summary>
    /// 注，自定义的RestRequest
    /// 原RestRequest.AddObject中数组格式mvc无法识别
    /// </summary>
    public class CustomerRestRequest : RestRequest
    {
		public CustomerRestRequest(Method method)
			: base((RestSharp.Method)method) { }

        public new IRestRequest AddObject(object obj, params string[] whitelist)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (whitelist != null && whitelist.Contains(property.Name))
                {
                    continue;
                }

                var value = property.GetValue(obj, null);
                if (value == null)
                {
                    continue;
                }

                var propertyType = property.PropertyType;
                if (propertyType.IsArray)
                {
                    var elementType = propertyType.GetElementType();
                    string[] values;
                    if (((Array)value).Length > 0 && (elementType.IsPrimitive || elementType.IsValueType || elementType == typeof(string)))
                    {
                        // convert the array to an array of strings                                                                
                        values = (from object item in ((Array)value) select item.ToString()).ToArray<string>();
                    }
                    else
                    {
                        // try to cast it                                                                
                        values = (string[])value;
                    }

                    foreach (var item in values)
                    {
                        this.AddParameter(property.Name, item, ParameterType.GetOrPost);
                    }
                }
                else
                {
                    this.AddParameter(property.Name, value);
                }
            }
            return this;
        }
    }

    #region 自定义反序列化结果类

    public class CustomJsonDeserializer : RestSharp.Deserializers.IDeserializer
    {
        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            return JSONHelper.Decode<T>(response.Content);
        }
    }

    #endregion

    #region 自定义序列化结果类

    public class CustomJsonSerializer : RestSharp.Serializers.ISerializer
    {
        public CustomJsonSerializer()
        {
            ContentType = "application/json";
        }
        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            return JSONHelper.Encode(obj);
        }
    }

    #endregion

	public enum Method
	{
		GET = 0,
		POST = 1,
		PUT = 2,
		DELETE = 3,
		HEAD = 4,
		OPTIONS = 5,
		PATCH = 6,
	}
}