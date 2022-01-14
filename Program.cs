using DSharpPlus;

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN")
});

string? envVarIgnoredChannels = Environment.GetEnvironmentVariable("IGNORED_CHANNELS");
ulong[] ignoredChannels = envVarIgnoredChannels == null ? Array.Empty<ulong>() : envVarIgnoredChannels.Split(";").Select(ulong.Parse).ToArray();

discord.MessageCreated += async (_, eventArgs) => {
	if (eventArgs.Message.Stickers.Count > 0 && !ignoredChannels.Contains(eventArgs.Channel.Id)) {
		await eventArgs.Message.DeleteAsync();
	}
};

await discord.ConnectAsync();

await Task.Delay(-1);
