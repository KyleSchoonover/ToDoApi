namespace Infrastructure.Models
{
    using System;
    using Newtonsoft.Json;

    public class Task
    {
        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonIgnore]
        public DateTime? Created { get; set; }

        [JsonIgnore]
        public DateTime? DateCompleted { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ListId { get; set; }

        [JsonIgnore]
        public DateTime? Modified { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}