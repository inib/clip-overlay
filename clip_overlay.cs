using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CPHInline
{
    public bool Execute()
    {
        // inputs
        string username = CPH.TryGetArg("twitchUser", out string u) ? u : null;
        int count = CPH.TryGetArg("count", out int c) ? c : 50; // how many to fetch before random pick

        if (string.IsNullOrWhiteSpace(username))
        {
            CPH.LogError("Missing arg 'twitchUser'.");
            return false;
        }

        // fetch only featured clips for the user
        var clips = CPH.GetClipsForUser(username, count, true); // isFeatured = true
        if (clips == null || clips.Count == 0)
        {
            CPH.LogInfo($"No featured clips for '{username}'.");
            return false;
        }

        var rng = new Random();
        var clip = clips[rng.Next(clips.Count)];

        // expose values to subsequent sub-actions if you want
        CPH.SetArgument("clipId", clip.Id);
        CPH.SetArgument("clipUrl", clip.Url);
        CPH.SetArgument("clipEmbedUrl", clip.EmbedUrl);
        CPH.SetArgument("clipTitle", clip.Title);
        CPH.SetArgument("clipCreator", clip.CreatorName);
        CPH.SetArgument("clipBroadcaster", clip.BroadcasterName);
        CPH.SetArgument("clipDuration", clip.Duration); // seconds if available

        // broadcast to overlays via WebSocket "General.Custom"
        var payload = new
        {
            topic = "featuredClip",
            clip = new {
                slug = clip.Id,
                url = clip.Url,
                embedUrl = clip.EmbedUrl,
                title = clip.Title,
                creator = clip.CreatorName,
                broadcaster = clip.BroadcasterName,
                duration = clip.Duration // may be null on some versions
            }
        };

        string json = JsonSerializer.Serialize(payload);
        CPH.WebsocketBroadcastJson(json);

        return true;
    }
}
