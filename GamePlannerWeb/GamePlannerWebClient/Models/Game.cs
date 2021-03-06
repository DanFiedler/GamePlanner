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

    public partial class Game
    {
        /// <summary>
        /// Initializes a new instance of the Game class.
        /// </summary>
        public Game() { }

        /// <summary>
        /// Initializes a new instance of the Game class.
        /// </summary>
        public Game(int? id = default(int?), string name = default(string), int? minPlayer = default(int?), int? maxPlayer = default(int?))
        {
            ID = id;
            Name = name;
            MinPlayer = minPlayer;
            MaxPlayer = maxPlayer;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ID")]
        public int? ID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MinPlayer")]
        public int? MinPlayer { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MaxPlayer")]
        public int? MaxPlayer { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.Name != null)
            {
                if (this.Name.Length > 450)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Name", 450);
                }
                if (this.Name.Length < 0)
                {
                    throw new ValidationException(ValidationRules.MinLength, "Name", 0);
                }
            }
        }
    }
}
