﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Geta.Commerce.Payments.Verifone.HostedPages.Mvc
{
    public class VerifoneModelBinder : DefaultModelBinder
    {
        protected override System.ComponentModel.PropertyDescriptorCollection GetModelProperties(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var toReturn = base.GetModelProperties(controllerContext, bindingContext);

            List<PropertyDescriptor> additional = new List<PropertyDescriptor>();

            //now look for any aliasable properties in here
            foreach (var p in this.GetTypeDescriptor(controllerContext, bindingContext).GetProperties().Cast<PropertyDescriptor>())
            {
                foreach (var attr in p.Attributes.OfType<BindAliasAttribute>())
                {
                    additional.Add(new AliasPropertyDescriptor(attr.Alias, p));

                    if (bindingContext.PropertyMetadata.ContainsKey(p.Name))
                        bindingContext.PropertyMetadata.Add(attr.Alias, bindingContext.PropertyMetadata[p.Name]);
                }
            }

            return new PropertyDescriptorCollection(toReturn.Cast<PropertyDescriptor>().Concat(additional).ToArray());
        }
    }
}