## AntiStickerBot
Deletes all stickers, and also messages with more than a certain amount of emotes.

### Deployment
Use of the Dockerfile is recommended.
The envvars:
- BOT_TOKEN
- DELETE_STICKERS: boolean
- MAX_EMOTES: integer, messages with more than this amount of emotes/emojis are deleted.
- STICKER_IGNORED_ROLES: list of role IDs (separated by semicolon) that are allowed to post stickers in all channels
- STICKER_IGNORED_CHANNELS: list of channel IDs (separated by semicolon) where everyone is allowed to post stickers
- EMOTE_IGNORED_ROLES: list of role IDs (separated by semicolon) that are allowed to exceed the emote limit
- EMOTE_IGNORED_CHANNELS: list of channel IDs (separated by semicolon) where everyone is allowed to exceed the emote limit
If you want to make a channel where everyone is allowed to both exceed the emote limit and post stickers, consider just denying the bot access to that channel.
