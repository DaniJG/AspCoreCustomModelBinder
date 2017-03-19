using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreCustomModelBinder.ModelBinding
{
    public class SplitDateTimeModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinder binder = new SplitDateTimeModelBinder(new SimpleTypeModelBinder(typeof(DateTime)));

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(DateTime) ? binder : null;
        }
        
    }
}
