# Anti(Sticker|Emote)Bot
AntiStickerBot deletes all messages with stickers, AntiEmoteBot deletes all messages that have too many emotes.

To ignore a channel simply deny the bot read access to that channel.

## Docker deployment
Use these envvars:

### AntiStickerBot
- BOT_TOKEN
- STICKER_IGNORED_ROLES: semicolon separated list of role IDs that will be ignored

### AntiEmoteBot
- BOT_TOKEN
- EMOTE_IGNORED_ROLES: semicolon separated list of role IDs that will be ignored
- MAX_EMOTES: the maximum amount of emotes in a single message that will be allowed
