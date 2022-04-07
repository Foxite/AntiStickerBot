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

discord.MessageCreated += (_, eventArgs) => {
	if (ignoredChannels.Contains(eventArgs.Channel.Id)) {
		return Task.CompletedTask;
	}
	
	if (eventArgs.Message.Stickers.Count > 0) {
		return eventArgs.Message.DeleteAsync();
	} else if (maxEmotes != -1) {
		ReadOnlySpan<char> span = eventArgs.Message.Content.AsSpan();
		int count = emoteRegex.Matches(eventArgs.Message.Content).Count;
		for (int i = 0; i < eventArgs.Message.Content.Length;) {
			if (count > maxEmotes) {
				return eventArgs.Message.DeleteAsync();
			}
			int length = emojiTree.ContainsPrefix(span.Slice(i));
			if (length != -1) {
				i += length;
				count++;
			} else {
				i++;
			}
		}
	}

	return Task.CompletedTask;
};

await discord.ConnectAsync();

await Task.Delay(-1);
