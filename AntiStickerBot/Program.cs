using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.Entities;
using Foxite.Common.Collections;

[return: NotNullIfNotNull("defaultValue")]
T ParseEnvvarOrDefault<T>(string name, T defaultValue, Func<string, T> parse) {
	string? envvar = Environment.GetEnvironmentVariable(name);
	if (envvar != null) {
		return parse(envvar);
	} else {
		return defaultValue;
	}
}

bool deleteStickers            = ParseEnvvarOrDefault("DELETE_STICKERS",          true,                 bool.Parse);
int maxEmotes                  = ParseEnvvarOrDefault("MAX_EMOTES",               0,                    int.Parse);
ulong[] stickerIgnoredRoles    = ParseEnvvarOrDefault("STICKER_IGNORED_ROLES",    Array.Empty<ulong>(), envvar => envvar.Split(";").Select(ulong.Parse).ToArray());
ulong[] stickerIgnoredChannels = ParseEnvvarOrDefault("STICKER_IGNORED_CHANNELS", Array.Empty<ulong>(), envvar => envvar.Split(";").Select(ulong.Parse).ToArray());
ulong[] emoteIgnoredRoles      = ParseEnvvarOrDefault("EMOTE_IGNORED_ROLES",      Array.Empty<ulong>(), envvar => envvar.Split(";").Select(ulong.Parse).ToArray());
ulong[] emoteIgnoredChannels   = ParseEnvvarOrDefault("EMOTE_IGNORED_CHANNELS",   Array.Empty<ulong>(), envvar => envvar.Split(";").Select(ulong.Parse).ToArray());

bool IsIgnored(DiscordUser user, DiscordChannel channel, bool emotes) {
	ulong[] ignoredChannels = emotes ? emoteIgnoredChannels : stickerIgnoredChannels;
	ulong[] ignoredRoles = emotes ? emoteIgnoredRoles : stickerIgnoredRoles;
	return user is not DiscordMember member || ignoredChannels.Contains(channel.Id) || member.Roles.Select(role => role.Id).Intersect(ignoredRoles).Any();
}

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN"),
});

if (deleteStickers) {
	discord.MessageCreated += (_, ea) => {
		if (!IsIgnored(ea.Author, ea.Channel, false) && ea.Message.Stickers.Count > 0) {
			return ea.Message.DeleteAsync();
		} else {
			return Task.CompletedTask;
		}
	};
}

if (maxEmotes > 0) {
	var emoteRegex = new Regex(@"(<a?)?:\w+:(\d{18}>)?");
	
	IEnumerable<string> emojis = ((IReadOnlyDictionary<string, string>) typeof(DiscordEmoji).GetProperty("DiscordNameLookup", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!).Keys;
	var emojiTree = new StringTree();
	foreach (string emoji in emojis) {
		emojiTree.Add(emoji);
	}

	discord.MessageCreated += (_, ea) => {
		if (IsIgnored(ea.Author, ea.Channel, true)) {
			return Task.CompletedTask;
		}

		ReadOnlySpan<char> span = ea.Message.Content.AsSpan();
		int count = emoteRegex.Matches(ea.Message.Content).Count;
		if (count > maxEmotes) {
			return ea.Message.DeleteAsync();
		}

		for (int i = 0; i < ea.Message.Content.Length;) {
			int length = emojiTree.ContainsPrefix(span[i..]);
			if (length != -1) {
				i += length;
				count++;

				if (count > maxEmotes) {
					return ea.Message.DeleteAsync();
				}
			} else {
				i++;
			}
		}

		return Task.CompletedTask;
	};
}

await discord.ConnectAsync();

await Task.Delay(-1);
