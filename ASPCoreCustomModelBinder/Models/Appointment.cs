using ASPCoreCustomModelBinder.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreCustomModelBinder.Models
{
    public class Appointment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        // No need to do this when registering the model binder for all DateTime properties
        //[ModelBinder(BinderType = typeof(SplitDateTimeModelBinder))]
        // Also no need to do this when registering the JsonConverter for all DateTime properties
        //[JsonConverter(typeof(SplitDateTimeJsonConverter))]
        public DateTime AppointmentDate { get; set; }
    }
}
