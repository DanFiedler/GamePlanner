﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace GamePlannerWeb.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class EventRegistration
    {
        /// <summary>
        /// Initializes a new instance of the EventRegistration class.
        /// </summary>
        public EventRegistration() { }

        /// <summary>
        /// Initializes a new instance of the EventRegistration class.
        /// </summary>
        public EventRegistration(int? id = default(int?), EventModel eventProperty = default(EventModel), User user = default(User), IList<Preference> preferences = default(IList<Preference>), double? satisfaction = default(double?), GameAssignment assignment = default(GameAssignment))
        {
            ID = id;
            EventProperty = eventProperty;
            User = user;
            Preferences = preferences;
            Satisfaction = satisfaction;
            Assignment = assignment;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ID")]
        public int? ID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Event")]
        public EventModel EventProperty { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "User")]
        public User User { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Preferences")]
        public IList<Preference> Preferences { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Satisfaction")]
        public double? Satisfaction { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Assignment")]
        public GameAssignment Assignment { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.Preferences != null)
            {
                foreach (var element in this.Preferences)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
            if (this.Assignment != null)
            {
                this.Assignment.Validate();
            }
        }
    }
}
