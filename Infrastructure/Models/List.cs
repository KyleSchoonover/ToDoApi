namespace Infrastructure.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class List
    {
        [JsonIgnore]
        public DateTime? Created { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonIgnore]
        public DateTime? Modified { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tasks")]
        public List<Task> Tasks { get; set; }
    }
}