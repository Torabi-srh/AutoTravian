using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTravian.Core.Util
{
    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal partial class JsonParser
    {
        [JsonProperty("response")]
        internal Response Response { get; set; }
    }

    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal partial class Response
    {
        [JsonProperty("error")]
        internal bool Error { get; set; }

        [JsonProperty("errorMsg")]
        internal object ErrorMsg { get; set; }

        [JsonProperty("data")]
        internal Data Data { get; set; }
    }

    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal partial class Data
    {
        [JsonProperty("html")]
        internal string Html { get; set; }

        [JsonProperty("list")]
        internal List List { get; set; }

        [JsonProperty("sort")]
        internal string Sort { get; set; }

        [JsonProperty("direction")]
        internal string Direction { get; set; }

        [JsonProperty("distributed")]
        internal List<long> Distributed { get; set; }

        [JsonProperty("resources")]
        internal List<long> Resources { get; set; }

        [JsonProperty("functionToCall")]
        internal string FunctionToCall { get; set; }

        [JsonProperty("options", NullValueHandling = NullValueHandling.Include)]
        internal Options Options { get; set; } = new Options { };
    }

    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal partial class Options
    {
        [JsonProperty("dialogOptions", NullValueHandling = NullValueHandling.Include)]
        internal DialogOptions DialogOptions { get; set; }

        [JsonProperty("html", NullValueHandling = NullValueHandling.Include)]
        internal string Html { get; set; }
    }

    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal partial class DialogOptions
    {
        [JsonProperty("infoIcon")]
        internal Uri InfoIcon { get; set; }

        [JsonProperty("saveOnUnload")]
        internal bool SaveOnUnload { get; set; }

        [JsonProperty("draggable")]
        internal bool Draggable { get; set; }

        [JsonProperty("buttonOk")]
        internal bool ButtonOk { get; set; }

        [JsonProperty("context")]
        internal string Context { get; set; }
    }

    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal partial class List
    {
        [JsonProperty("troops")]
        internal Dictionary<string, long> Troops { get; set; }

        [JsonProperty("directions")]
        internal Directions Directions { get; set; }

        [JsonProperty("slots")]
        internal Dictionary<string, Slot> Slots { get; set; }
    }

    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal partial class Directions
    {
        [JsonProperty("village")]
        internal string Village { get; set; }

        [JsonProperty("ew")]
        internal string Ew { get; set; }

        [JsonProperty("distance")]
        internal string Distance { get; set; }

        [JsonProperty("troops")]
        internal string Troops { get; set; }

        [JsonProperty("lastRaid")]
        internal string LastRaid { get; set; }
    }

    [System.Reflection.ObfuscationAttribute(Feature = "renaming", ApplyToMembers = true)]
    internal partial class Slot
    {
        [JsonProperty("troops")]
        internal Dictionary<string, long> Troops { get; set; }
    }

    internal partial class JsonParser
    {
        internal static JsonParser FromJson(string json) => JsonConvert.DeserializeObject<JsonParser>(json, Converter.Settings);
    }

    internal static class Serialize
    {
        internal static string ToJson(this JsonParser self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        internal static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            MissingMemberHandling = MissingMemberHandling.Ignore,
        };
    }
}
