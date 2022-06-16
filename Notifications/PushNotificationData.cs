using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
// using logapp.Services;
using logapp.Utilities;
using static logapp.Utilities.Helpers;

namespace logapp.Entities
{
    public record PushNotificationData
    {
        [JsonIgnore]
        public string BodyText { get; set; } = null;

        [JsonIgnore]
        public string BodyTextJsonData { get; set; } = null;

        [JsonPropertyName("body")]
        public string BodyContentFormattedText
        {
            get
            {
                if (BodyTextJsonData is null)
                    return BodyText;
                return BuildStringContent(BodyText, BodyTextJsonData);
            }
        }

        [JsonIgnore]
        public string NotificationToken { get; set; }

        [JsonPropertyName("id")]
        public long? NotificationId { get; set; }

        [JsonPropertyName("title")]
        public string TitleText { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, string> Data { get; set; } = null;

        [JsonIgnore]
        public Dictionary<string, string> MergedData
        {
            get
            {
                var d = new Dictionary<string, string>();
                if (ClickModuleName is not null)
                    d.Add("module_name", ClickModuleName);
                if (ClickAppModuleId is not null)
                    d.Add("module_id", ClickAppModuleId);
                if (ClickModuleItemId is not null)
                    d.Add("module_item_id", ClickModuleItemId);
                if (ClickUserNumber is not null)
                    d.Add("user_number", ClickUserNumber);
                if (ClickWebviewUrl is not null)
                    d.Add("webview_url", ClickWebviewUrl);
                d.Add("is_video_notification", IsVideoNotification.ToString().ToLower());
                d.Add("video_force_full_screen", VideoForceFullScreen.ToString().ToLower());
                d.Add("no_db_store", ParentAppNoDbStore.ToString().ToLower());
                d.Add("external_browser", ClickWebviewExternal.ToString());
                if (Data is null) return d;
                foreach (var item in Data)
                {
                    d.Add(item.Key, item.Value);
                }
                return d;
            }
        }

        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; } = null;

        [JsonIgnore]
        public string AndroidChannelId { get; set; } = null;

        [JsonPropertyName("module_name")]
        public string ClickModuleName { get; set; } = null;

        [JsonPropertyName("module_id")]
        public string ClickAppModuleId { get; set; } = null;

        [JsonPropertyName("webview_url")]
        public string ClickWebviewUrl { get; set; } = null;

        [JsonPropertyName("external_browser")]
        public bool ClickWebviewExternal { get; set; } = false;

        [JsonPropertyName("module_item_id")]
        public string ClickModuleItemId { get; set; } = null;

        [JsonPropertyName("user_number")]
        public string ClickUserNumber { get; set; } = null;

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonIgnore]
        public long LoginStatId { get; set; }

        [JsonIgnore]
        public bool ParentAppNoDbStore { get; set; } = false;

        [JsonPropertyName("is_video_notification")]
        public bool IsVideoNotification { get; set; } = false;

        [JsonPropertyName("video_force_full_screen")]
        public bool VideoForceFullScreen { get; set; } = false;

        /// <summary>
        /// <para>Content string must be formed as example: "Hi {{name}}, your email is {{email}}."</para>
        /// 
        /// Data json must be { "name": "John", "email": "johndoe@apple.com" }
        /// </summary>
        public static string BuildStringContent(string content, string jsonData)
        {
            IDictionary<string, string> data = JsonSerializer.Deserialize<IDictionary<string, string>>(jsonData);
            var strFormatter = new StringFormatter(content);
            foreach (var param in data)
            {
                strFormatter.Add($"{{{{{param.Key}}}}}", param.Value); // translates to => .Add("{{Key}}", Value)
            }
            return strFormatter.ToString();
        }
    }
}