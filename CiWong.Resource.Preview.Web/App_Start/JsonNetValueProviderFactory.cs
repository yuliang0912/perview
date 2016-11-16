using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace CiWong.Resource.Preview.Web
{
    public class JsonNetValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null) throw new ArgumentNullException("controllerContext");

            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            string bodyText;
            using (var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream))
            {
                bodyText = reader.ReadToEnd();
            }
            if (string.IsNullOrEmpty(bodyText)) return null;

            return new JObjectValueProvider(bodyText.StartsWith("[")
                ? JArray.Parse(bodyText) as JContainer
                : JObject.Parse(bodyText) as JContainer);
        }
    }

    public class JObjectValueProvider : IValueProvider
    {
        private readonly JContainer jcontainer;

        public JObjectValueProvider(JContainer jcontainer)
        {
            this.jcontainer = jcontainer;
        }

        public bool ContainsPrefix(string prefix)
        {
            return this.jcontainer.SelectToken(prefix) != null;
        }

        public ValueProviderResult GetValue(string key)
        {
            var jtoken = this.jcontainer.SelectToken(key);
            if (jtoken == null || jtoken.Type == JTokenType.Object) return null;

            return new JsonNetValueProviderResult(jtoken);
        }
    }

    public class JsonNetValueProviderResult : ValueProviderResult
    {
        private readonly JToken jtoken;
        public JsonNetValueProviderResult(JToken jtoken)
            : base(jtoken.ToObject<object>(), jtoken.ToString(), CultureInfo.CurrentCulture)
        {
            this.jtoken = jtoken;
        }

        public override object ConvertTo(Type type, CultureInfo culture)
        {
            var jsonSerializer = new Newtonsoft.Json.JsonSerializer();

            return jtoken.ToObject(type, jsonSerializer);
        }
    }

    public class JsonNetModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            // need to skip properties that aren't part of the request, else we might hit a StackOverflowException
            //lswweb 修改使支持DataMemberAttribute标识的属性名称
            var dataMemberAttribute = propertyDescriptor.Attributes.OfType<Attribute>()
                .FirstOrDefault(item => item is DataMemberAttribute) as DataMemberAttribute;

            var fullPropertyKey = CreateSubPropertyName(bindingContext.ModelName, dataMemberAttribute == null ? propertyDescriptor.Name : dataMemberAttribute.Name);
            if (!bindingContext.ValueProvider.ContainsPrefix(fullPropertyKey))
            {
                return;
            }

            // call into the property's model binder
            var propertyBinder = Binders.GetBinder(propertyDescriptor.PropertyType);
            var originalPropertyValue = propertyDescriptor.GetValue(bindingContext.Model);
            var propertyMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
            propertyMetadata.Model = originalPropertyValue;
            var innerBindingContext = new ModelBindingContext()
            {
                ModelMetadata = propertyMetadata,
                ModelName = fullPropertyKey,
                ModelState = bindingContext.ModelState,
                ValueProvider = bindingContext.ValueProvider
            };
            var newPropertyValue = this.GetPropertyValue(controllerContext, innerBindingContext, propertyDescriptor, propertyBinder);
            propertyMetadata.Model = newPropertyValue;

            // validation
            var modelState = bindingContext.ModelState[fullPropertyKey];
            if (modelState == null || modelState.Errors.Count == 0)
            {
                if (OnPropertyValidating(controllerContext, bindingContext, propertyDescriptor, newPropertyValue))
                {
                    SetProperty(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);
                    OnPropertyValidated(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);
                }
            }
            else
            {
                SetProperty(controllerContext, bindingContext, propertyDescriptor, newPropertyValue);

                // Convert FormatExceptions (type conversion failures) into InvalidValue messages
                foreach (var error in modelState.Errors.Where(err => String.IsNullOrEmpty(err.ErrorMessage) && err.Exception != null).ToList())
                {
                    for (var exception = error.Exception; exception != null; exception = exception.InnerException)
                    {
                        if (!(exception is FormatException)) continue;

                        var displayName = propertyMetadata.GetDisplayName();
                        var errorMessageTemplate = GetValueInvalidResource(controllerContext);
                        var errorMessage = String.Format(CultureInfo.CurrentCulture, errorMessageTemplate, modelState.Value.AttemptedValue, displayName);
                        modelState.Errors.Remove(error);
                        modelState.Errors.Add(errorMessage);
                        break;
                    }
                }
            }
        }

        private static string GetValueInvalidResource(ControllerContext controllerContext)
        {
            return "属性验证失败";
        }
    }
}