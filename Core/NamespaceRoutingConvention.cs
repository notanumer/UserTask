using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text;

namespace Core
{
    public class NamespaceRoutingConvention : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var template = new StringBuilder();
            template.Append("api/account");

            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel = new AttributeRouteModel()
                {
                    Template = template.ToString()
                };
            }
        }
    }
}
