using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zirpl.CalcEngine
{
    /// <summary>
    /// Expression based on an object's properties.
    /// </summary>
    class BindingExpression : Expression
    {
        CalculationEngine _ce;
        List<BindingInfo> _bindingPath;
        CultureInfo _ci;

        // ** ctor
        internal BindingExpression(CalculationEngine engine, List<BindingInfo> bindingPath, CultureInfo ci)
        {
            _ce = engine;
            _bindingPath = bindingPath;
            _ci = ci;
        }

        // ** object model
        public override object Evaluate()
        {
            return GetValue(_ce.DataContext);
        }

        // ** implementation
        object GetValue(object obj)
        {
            const BindingFlags bf =
                BindingFlags.IgnoreCase |
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.Static;

            var initialObj = obj;
            var prevObj = obj;
	        for (int index = 0; index < _bindingPath.Count; index++)
	        {
		        var bi = _bindingPath[index];

				if (obj == null && _ce.InValidation)
				{
					var propertyInfo = prevObj.GetType().GetProperty(bi.Name, bf);
					obj = CreateInstance(propertyInfo.PropertyType);
				}

				if (obj == null)
				{
					var s = GetBindingPath(_bindingPath, index);
				
					throw new CalcEngineBindingException($"Binding path invalid (value is null): {s}", prevObj, initialObj);
				}

				var type = obj.GetType();
				// get property
				if (bi.PropertyInfo == null)
		        {
			        bi.PropertyInfo = type.GetProperty(bi.Name, bf);
		        }

		        var isGenericList = type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
				
				if (!isGenericList && bi.PropertyInfo == null)
		        {
					var s = GetBindingPath(_bindingPath, index);

					throw new ArgumentException($"'{bi.Name}' is not valid property of {type.Name} in path '{s}'");
		        }

		        prevObj = obj;
				// get object
				obj = bi.PropertyInfo.GetValue(obj, null);
				if (obj == null && _ce.InValidation)
				{
					var propertyInfo = prevObj.GetType().GetProperty(bi.Name, bf);
					obj = CreateInstance(propertyInfo.PropertyType);
				}
				// handle indexers (lists and dictionaries)
				if (bi.Parms != null && bi.Parms.Count > 0)
		        {
			        // get indexer property (always called "Item")
			        if (bi.PropertyInfoItem == null)
			        {
				        bi.PropertyInfoItem = obj.GetType().GetProperty("Item", bf);
			        }

			        // get indexer parameters
			        var pip = bi.PropertyInfoItem.GetIndexParameters();
			        var list = new List<object>();
			        for (int i = 0; i < pip.Length; i++)
			        {
				        var pv = bi.Parms[i].Evaluate();
				        pv = Convert.ChangeType(pv, pip[i].ParameterType, _ci);
				        list.Add(pv);
			        }

			        if (_ce.InValidation)
			        {
						//(bi.PropertyInfoItem as RuntimePropertyInfo);

						obj = CreateInstance(bi.PropertyInfoItem.PropertyType);
			        }
			        else
			        {
			            var dic = (dynamic)obj;
			            Type dicType = dic.GetType();
			            var propertyInfos = dicType.GetProperties();

			            obj = bi.PropertyInfoItem.GetValue(obj, list.ToArray());


					}

					// get value
				}
	        }

	        // all done
            return obj;
        }

	    private string GetBindingPath(List<BindingInfo> bindingPath, int index)
	    {
			var before = _bindingPath.Take(index).Select(info => info.Name).ToList();
			var beforePath = _bindingPath[index];
			var after = _bindingPath.Skip(index + 1).Select(info => info.Name).ToList();

			return string.Join(".", before) + (before.Count > 0 ? "." : "") + "[" + beforePath.Name + "]" + (after.Count > 0 ? "." : "") + string.Join(".", after);
		}

	    private object CreateInstance(Type propertyType)
	    {
		    if(HasPublicParameterlessContructor(propertyType))
				return Activator.CreateInstance(propertyType);
		    return null;
	    }

		public static bool HasPublicParameterlessContructor(Type t)
		{
			return t.GetConstructors().Any(constructorInfo => constructorInfo.GetParameters().Length == 0);
		}
	}
}
