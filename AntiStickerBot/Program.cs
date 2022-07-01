using System.Diagnostics.CodeAnalysis;
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

ulong[] ignoredRoles = ParseEnvvarOrDefault("STICKER_IGNORED_ROLES", Array.Empty<ulong>(), envvar => envvar.Split(";").Select(ulong.Parse).ToArray());

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN"),
});

discord.MessageCreated += (_, ea) => {
	if (!(ea.Author is DiscordMember user && user.Roles.Select(role => role.Id).Intersect(ignoredRoles).Any()) && ea.Message.Stickers.Count > 0) {
		return ea.Message.DeleteAsync();
	} else {
		return Task.CompletedTask;
	}
};

await discord.ConnectAsync();

await Task.Delay(-1);
