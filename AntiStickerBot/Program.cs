using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.Entities;
using Foxite.Common.Collections;

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN"),
});

string? envVarIgnoredChannels = Environment.GetEnvironmentVariable("IGNORED_CHANNELS");
ulong[] ignoredChannels = envVarIgnoredChannels == null ? Array.Empty<ulong>() : envVarIgnoredChannels.Split(";").Select(ulong.Parse).ToArray();

IEnumerable<string> emojis = ((IReadOnlyDictionary<string, string>) typeof(DiscordEmoji).GetProperty("DiscordNameLookup", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!).Keys;
var emojiTree = new StringTree();
foreach (string emoji in emojis) {
	emojiTree.Add(emoji);
}

var emoteRegex = new Regex(@"(<a?)?:\w+:(\d{18}>)?");
string? maxEmotesEnvvar = Environment.GetEnvironmentVariable("MAX_EMOTES");
int maxEmotes = maxEmotesEnvvar == null ? -1 : int.Parse(maxEmotesEnvvar);

string? envVarEmoteIgnoredRoles = Environment.GetEnvironmentVariable("EMOTE_IGNORED_ROLES");
ulong[] emoteIgnoredRoles = envVarEmoteIgnoredRoles == null ? Array.Empty<ulong>() : envVarEmoteIgnoredRoles.Split(";").Select(ulong.Parse).ToArray();

string? envVarEmoteIgnoredChannels = Environment.GetEnvironmentVariable("EMOTE_IGNORED_CHANNELS");
ulong[] emoteIgnoredChannels = envVarEmoteIgnoredChannels == null ? Array.Empty<ulong>() : envVarEmoteIgnoredChannels.Split(";").Select(ulong.Parse).ToArray();

discord.MessageCreated += (_, eventArgs) => {
	if (eventArgs.Author is not DiscordMember author || ignoredChannels.Contains(eventArgs.Channel.Id)) {
		return Task.CompletedTask;
	}
	
	if (eventArgs.Message.Stickers.Count > 0) {
		return eventArgs.Message.DeleteAsync();
	} else if (maxEmotes != -1 && !author.Roles.Select(role => role.Id).Intersect(emoteIgnoredRoles).Any() && !emoteIgnoredChannels.Contains(eventArgs.Channel.Id)) {
		ReadOnlySpan<char> span = eventArgs.Message.Content.AsSpan();
		int count = emoteRegex.Matches(eventArgs.Message.Content).Count;
		if (count > maxEmotes) {
			return eventArgs.Message.DeleteAsync();
		}
		for (int i = 0; i < eventArgs.Message.Content.Length;) {
			int length = emojiTree.ContainsPrefix(span.Slice(i));
			if (length != -1) {
				i += length;
				count++;
				
				if (count > maxEmotes) {
					return eventArgs.Message.DeleteAsync();
				}
			} else {
				i++;
			}
		}
	}

	return Task.CompletedTask;
};

await discord.ConnectAsync();

await Task.Delay(-1);
