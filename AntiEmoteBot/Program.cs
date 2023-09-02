using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using AntiStickerBot;
using DSharpPlus;
using DSharpPlus.Entities;

[return: NotNullIfNotNull("defaultValue")]
T ParseEnvvarOrDefault<T>(string name, T defaultValue, Func<string, T> parse) {
	string? envvar = Environment.GetEnvironmentVariable(name);
	if (envvar != null) {
		return parse(envvar);
	} else {
		return defaultValue;
	}
}

int maxEmotes = ParseEnvvarOrDefault("MAX_EMOTES", 0, int.Parse);
ulong[] ignoredRoles = ParseEnvvarOrDefault("EMOTE_IGNORED_ROLES", Array.Empty<ulong>(), envvar => envvar.Split(";").Select(ulong.Parse).ToArray());

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN"),
});

var emoteRegex = new Regex(@"(<a?)?:\w+:(\d{18}>)?");

IEnumerable<string> emojis = ((IReadOnlyDictionary<string, string>) typeof(DiscordEmoji).GetProperty("DiscordNameLookup", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!).Keys;
var emojiTree = new StringTree();
foreach (string emoji in emojis) {
	emojiTree.Add(emoji);
}

Task FilterEmojis(DiscordMessage message) {
	if (message.Author is DiscordMember author && author.Roles.Select(role => role.Id).Intersect(ignoredRoles).Any()) {
		return Task.CompletedTask;
	}

	ReadOnlySpan<char> span = message.Content.AsSpan();
	int count = emoteRegex.Matches(message.Content).Count;
	if (count > maxEmotes) {
		return message.DeleteAsync();
	}

	for (int i = 0; i < message.Content.Length;) {
		int length = emojiTree.ContainsPrefix(span[i..]);
		if (length != -1) {
			i += length;
			count++;

			if (count > maxEmotes) {
				return message.DeleteAsync();
			}
		} else {
			i++;
		}
	}

	return Task.CompletedTask;
}

discord.MessageCreated += (_, ea) => FilterEmojis(ea.Message);
discord.MessageUpdated += (_, ea) => FilterEmojis(ea.Message);

await discord.ConnectAsync();

await Task.Delay(-1);
