using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Razor;

namespace CZWA.Web
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            // nothing here
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            //var area = context.ActionContext.ActionDescriptor.RouteValues.FirstOrDefault(rc => rc.Key == "action");
            //var additionalLocations = new LinkedList<string>();
            //if (area.Value != null)
            //{
            //    additionalLocations.AddLast($"/CZWA.Views/" + "{1}/{0}.cshtml");
            //}
            //return viewLocations.Concat(additionalLocations);
            return new string[] { "/CZWA.Views/{1}/{0}.cshtml", "/CZWA.Views/Shared/{0}.cshtml" };
        }
    }

    public class ModuleViewLocationExpander : IViewLocationExpander
    {
        private const string _moduleKey = "module";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            //var aaa = typeof(SimpleViewComponent).GetTypeInfo().Assembly;
            if (context.Values.ContainsKey(_moduleKey))
            {
                var module = context.Values[_moduleKey];
                if (!string.IsNullOrWhiteSpace(module))
                {
                    var moduleViewLocations = new string[]
                    {
                       "CZWA.Controllers/Views/{1}/{0}.cshtml",
                       "CZWA.Controllers/Views/Shared/{0}.cshtml"
                    };

                    viewLocations = moduleViewLocations.Concat(viewLocations);
                }
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var controller = context.ActionContext.ActionDescriptor.DisplayName;
            var moduleName = controller.Split('.')[2];
            if (moduleName != "WebHost")
            {
                context.Values[_moduleKey] = moduleName;
            }
        }
    }
}
