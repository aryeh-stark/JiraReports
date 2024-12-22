using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JiraCmdLineTool.Common.Objects.Projects;

    public class Project
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }