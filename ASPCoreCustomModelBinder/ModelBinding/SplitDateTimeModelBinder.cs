using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreCustomModelBinder.ModelBinding
{
    public class SplitDateTimeModelBinder: IModelBinder
    {
        private readonly IModelBinder fallbackBinder;
        public SplitDateTimeModelBinder(IModelBinder fallbackBinder)
        {
            this.fallbackBinder = fallbackBinder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Make sure both Date and Time values are found in the Value Providers
            // NOTE: You might not want to enforce both parts
            var datePartName = $"{bindingContext.ModelName}.Date";
            var timePartName = $"{bindingContext.ModelName}.Time";
            var datePartValues = bindingContext.ValueProvider.GetValue(datePartName);
            var timePartValues = bindingContext.ValueProvider.GetValue(timePartName);

            // Fallback to the default binder when a part is missing
            if (datePartValues.Length == 0 || timePartValues.Length == 0) return fallbackBinder.BindModelAsync(bindingContext);

            // When not wrapping the SimpleTypeModelBinder, you want to return an empty result here
            //if (datePartValues.Length == 0 || timePartValues.Length == 0) return Task.CompletedTask;

            // Parse Date and Time
            // TODO: You might want a stronger/smarter handling of locales, formats and cultures
            DateTime.TryParseExact(
                datePartValues.FirstValue, 
                "d", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var parsedDateValue);            
            DateTime.TryParseExact(
                timePartValues.FirstValue,
                "t",
                CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal,
                out var parsedTimeValue);
            
            // Combine into single DateTime which is the end result
            var result = new DateTime(parsedDateValue.Year,
                            parsedDateValue.Month,
                            parsedDateValue.Day,
                            parsedTimeValue.Hour,
                            parsedTimeValue.Minute,
                            parsedTimeValue.Second);            
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, result, $"{datePartValues.FirstValue} {timePartValues.FirstValue}");
            bindingContext.Result = ModelBindingResult.Success(result);        
            return Task.CompletedTask;
        }
    }
}
