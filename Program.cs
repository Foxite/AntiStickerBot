using DSharpPlus;

var discord = new DiscordClient(new DiscordConfiguration() {
	Token = Environment.GetEnvironmentVariable("BOT_TOKEN")
});

discord.MessageCreated += async (_, eventArgs) => {
	if (eventArgs.Message.Stickers.Count > 0) {
		await eventArgs.Message.DeleteAsync();
	}
};

await discord.ConnectAsync();

await Task.Delay(-1);
