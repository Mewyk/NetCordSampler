using NetCord.Rest;
using NetCord;
using NetCordSampler.CodeSamples.Attributes;
using System.Collections.Immutable;

namespace NetCordSampler.CodeSamples.Collections;

public enum ApplicationCommandOptionChoice
{
    [SamplerData(typeof(ApplicationCommandOptionChoiceValueType), "Type of value of the choice.")]
    ValueType,
    [SamplerData(typeof(double), "Numeric value for the choice.")]
    ValueNumeric,
    [SamplerData(typeof(string), "Name of the choice (1-100 characters).")]
    Name,
    [SamplerData(typeof(string), "String value for the choice.")]
    ValueString,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Name` (1-100 characters each).")]
    NameLocalizations
}

public enum GuildOnboardingOptions
{
    [SamplerData(typeof(bool), "Whether onboarding is enabled in the guild.")]
    Enabled,
    [SamplerData(typeof(GuildOnboardingMode), "Current mode of onboarding.")]
    Mode,
    [SamplerData(typeof(IEnumerable<GuildOnboardingPromptProperties>), "Prompts shown during onboarding and in customize community.")]
    Prompts,
    [SamplerData(typeof(IEnumerable<ulong>), "Channel ids that members get opted into automatically.")]
    DefaultChannelIds
}

public enum CurrentGuildUserOptions
{
    [SamplerData(typeof(string), "New nickname, empty to remove nickname.")]
    Nickname
}

public enum SlashCommandProperties
{
    [SamplerData(typeof(IEnumerable<ApplicationCommandOptionProperties>), "Parameters for the command (max 25).")]
    Options,
    [SamplerData(typeof(string), "Description of the command (1-100 characters).")]
    Description,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Description` (1-100 characters each).")]
    DescriptionLocalizations
}

public enum GuildOnboardingPromptOption
{
    [SamplerData(typeof(string), "Description of the option.")]
    Description,
    [SamplerData(typeof(IReadOnlyList<ulong>), "Ids for roles assigned to an user when the option is selected.")]
    RoleIds,
    [SamplerData(typeof(Emoji), "Emoji of the option.")]
    Emoji,
    [SamplerData(typeof(string), "Title of the option.")]
    Title,
    [SamplerData(typeof(IReadOnlyList<ulong>), "Ids for channels an user is added to when the option is selected.")]
    ChannelIds
}

public enum EmbedFieldProperties
{
    [SamplerData(typeof(string), "Value of the field.")]
    Value,
    [SamplerData(typeof(string), "Name of the field.")]
    Name,
    [SamplerData(typeof(bool), "Whether or not the field should display inline.")]
    Inline
}

public enum GuildUserInfo
{
    [SamplerData(typeof(GuildUserJoinSourceType), "Specifies how the `User` joined the guild.")]
    JoinSourceType,
    [SamplerData(typeof(GuildUser), "The `GuildUser` object representing the user.")]
    User,
    [SamplerData(typeof(ulong), "The ID of the user that invited the `User` to the guild.")]
    InviterId,
    [SamplerData(typeof(string), "The code of the invite the `User` joined from.")]
    SourceInviteCode
}

public enum ApplicationCommandGuildPermissionProperties
{
    [SamplerData(typeof(bool), "`true` to allow, `false`, to disallow.")]
    Permission,
    [SamplerData(typeof(ulong), "ID of the role, user, or channel the permission is for. 'GuildId - 1' for all channels.")]
    Id,
    [SamplerData(typeof(ApplicationCommandGuildPermissionType), "Type of the permission.")]
    Type
}

public enum DMChannelProperties
{
    [SamplerData(typeof(ulong), "The recipient to open a DM channel with.")]
    UserId
}

public enum RestGuild
{
    [SamplerData(typeof(string), "The `RestGuild`'s description, shown in the 'Discovery' tab.")]
    Description,
    [SamplerData(typeof(string), "The `RestGuild`'s splash hash.")]
    SplashHash,
    [SamplerData(typeof(ContentFilter), "The `RestGuild`'s `NetCord.ContentFilter`.")]
    ContentFilter,
    [SamplerData(typeof(VerificationLevel), "The `NetCord.VerificationLevel` required for the `RestGuild`.")]
    VerificationLevel,
    [SamplerData(typeof(int), "The maximum amount of users in a stage video channel.")]
    MaxStageVideoChannelUsers,
    [SamplerData(typeof(MfaLevel), "The `RestGuild`'s required Multi-Factor Authentication level.")]
    MfaLevel,
    [SamplerData(typeof(Permissions), "Total permissions for the user in the `RestGuild` (excludes overwrites and implicit permissions).")]
    Permissions,
    [SamplerData(typeof(bool), "`true` if the `GuildWidget` is enabled.")]
    WidgetEnabled,
    [SamplerData(typeof(ulong), "The ID of the `RestGuild`'s owner.")]
    OwnerId,
    [SamplerData(typeof(int), "The number of boosts the `RestGuild` currently has.")]
    PremiumSubscriptionCount,
    [SamplerData(typeof(ImmutableDictionary<ulong, GuildEmoji>), "A dictionary of `GuildEmoji` objects, indexed by their IDs, representing the `RestGuild`'s custom emojis.")]
    Emojis,
    [SamplerData(typeof(string), "The `RestGuild`'s vanity invite URL code.")]
    VanityUrlCode,
    [SamplerData(typeof(ulong), "The `RestGuild`'s ID.")]
    Id,
    [SamplerData(typeof(int), "The maximum number of `Presence`s for the `RestGuild`. Always `null` with the exception of the largest guilds.")]
    MaxPresences,
    [SamplerData(typeof(int), "The maximum amount of users in a video channel.")]
    MaxVideoChannelUsers,
    [SamplerData(typeof(int), "The approximate number of `GuildUser`s in the `RestGuild`.")]
    ApproximateUserCount,
    [SamplerData(typeof(Role), "The guild's base role, applied to all users.")]
    EveryoneRole,
    [SamplerData(typeof(SystemChannelFlags), "Represents the `RestGuild`'s current system channels settings.")]
    SystemChannelFlags,
    [SamplerData(typeof(int), "How long in seconds to wait before moving users to the AFK channel.")]
    AfkTimeout,
    [SamplerData(typeof(string), "The name of the `RestGuild`. Must be between 2 and 100 characters. Leading and trailing whitespace are trimmed.")]
    Name,
    [SamplerData(typeof(string), "The `RestGuild`'s discovery splash hash.")]
    DiscoverySplashHash,
    [SamplerData(typeof(ulong), "ID of the `RestGuild`'s AFK channel.")]
    AfkChannelId,
    [SamplerData(typeof(string), "The `RestGuild`'s banner hash.")]
    BannerHash,
    [SamplerData(typeof(string), "The `RestGuild`'s icon hash.")]
    IconHash,
    [SamplerData(typeof(bool), "`true` if the user is the owner of the `RestGuild`.")]
    IsOwner,
    [SamplerData(typeof(ulong), "The ID of the channel where `RestGuild` notices such as welcome messages and boost events are posted.")]
    SystemChannelId,
    [SamplerData(typeof(GuildWelcomeScreen), "The welcome screen shown to new members, returned in an invite's `RestGuild` object.")]
    WelcomeScreen,
    [SamplerData(typeof(NsfwLevel), "The `RestGuild`'s set NSFW level.")]
    NsfwLevel,
    [SamplerData(typeof(int), "The `RestGuild`'s current server boost level.")]
    PremiumTier,
    [SamplerData(typeof(IReadOnlyList<string>), "A list of `RestGuild` feature strings, representing what features are currently enabled.")]
    Features,
    [SamplerData(typeof(bool), "Whether the `RestGuild` has a set banner.")]
    HasBanner,
    [SamplerData(typeof(bool), "Whether the `RestGuild` has an icon set.")]
    HasIcon,
    [SamplerData(typeof(ulong), "The `RestGuild` creator's application ID, if it was created by a bot.")]
    ApplicationId,
    [SamplerData(typeof(ImmutableDictionary<ulong, Role>), "A dictionary of `Role` objects indexed by their IDs, representing the `RestGuild`'s roles.")]
    Roles,
    [SamplerData(typeof(ulong), "The ID of the channel that the `GuildWidget` will generate an invite to, or `null` if set to no invite.")]
    WidgetChannelId,
    [SamplerData(typeof(int), "The maximum number of `GuildUser`s for the `RestGuild`.")]
    MaxUsers,
    [SamplerData(typeof(ulong), "The ID of the channel where admins and moderators of community guilds receive notices from Discord.")]
    PublicUpdatesChannelId,
    [SamplerData(typeof(int), "Approximate number of non-offline `GuildUser`s in the `RestGuild`.")]
    ApproximatePresenceCount,
    [SamplerData(typeof(bool), "Whether the `RestGuild` has the boost progress bar enabled.")]
    PremiumProgressBarEnabled,
    [SamplerData(typeof(string), "The preferred locale of a community `RestGuild`, used for the 'Discovery' tab and in notices from Discord, also sent in interactions. Defaults to `en-US</c>.")]
    PreferredLocale,
    [SamplerData(typeof(ulong), "The ID of the channel where admins and moderators of community guilds receive safety alerts from Discord.")]
    SafetyAlertsChannelId,
    [SamplerData(typeof(DefaultMessageNotificationLevel), "The `RestGuild`'s `NetCord.DefaultMessageNotificationLevel`.")]
    DefaultMessageNotificationLevel,
    [SamplerData(typeof(bool), "Whether the `RestGuild` has a set splash.")]
    HasSplash,
    [SamplerData(typeof(ulong), "The ID of the channel where community guilds can display their rules and/or guidelines.")]
    RulesChannelId,
    [SamplerData(typeof(bool), "Whether the `RestGuild` has a set discovery splash.")]
    HasDiscoverySplash,
    [SamplerData(typeof(ImmutableDictionary<ulong, GuildSticker>), "A dictionary of `GuildSticker` objects indexed by their IDs, representing the `RestGuild`'s custom stickers.")]
    Stickers
}

public enum FollowAnnouncementGuildChannelProperties
{
    [SamplerData(typeof(ulong), "ID of target channel.")]
    WebhookChannelId
}

public enum ApplicationCommandOptionProperties
{
    [SamplerData(typeof(IEnumerable<ApplicationCommandOptionProperties>), "Parameters for the option (max 25).")]
    Options,
    [SamplerData(typeof(string), "Description of the option (1-100 characters).")]
    Description,
    [SamplerData(typeof(bool), "If autocomplete interactions are enabled for the option.")]
    Autocomplete,
    [SamplerData(typeof(string), "Name of the option (1-32 characters).")]
    Name,
    [SamplerData(typeof(double), "The minimum value permitted.")]
    MinValue,
    [SamplerData(typeof(IEnumerable<ChannelType>), "If the option is a channel type, the channels shown will be restricted to these types.")]
    ChannelTypes,
    [SamplerData(typeof(int), "The minimum allowed length (0-6000).")]
    MinLength,
    [SamplerData(typeof(bool), "If the parameter is required or optional, default `false`.")]
    Required,
    [SamplerData(typeof(IEnumerable<ApplicationCommandOptionChoiceProperties>), "Choices for the user to pick from (max 25).")]
    Choices,
    [SamplerData(typeof(ApplicationCommandOptionType), "Type of the option.")]
    Type,
    [SamplerData(typeof(double), "The maximum value permitted.")]
    MaxValue,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Description` (1-100 characters each).")]
    DescriptionLocalizations,
    [SamplerData(typeof(int), "The maximum allowed length (0-6000).")]
    MaxLength,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Name` (1-32 characters each).")]
    NameLocalizations
}

public enum HttpInteractionValidator
{
    [SamplerData(typeof(ReadOnlySpan<byte>), "Map from an ASCII char to its hex value, e.g. arr['b'] == 11. 0xFF means it's not a hex digit.")]
    CharToHexLookup
}

public enum EmbedAuthorProperties
{
    [SamplerData(typeof(string), "Url of the author.")]
    Url,
    [SamplerData(typeof(string), "Url of the author icon.")]
    IconUrl,
    [SamplerData(typeof(string), "Name of the author.")]
    Name
}

public enum RestAuditLogEntry
{
    [SamplerData(typeof(User), "User that made the changes.")]
    User,
    [SamplerData(typeof(RestAuditLogEntryData), "Data of objects referenced in the audit log.")]
    Data
}

public enum CurrentUserVoiceStateOptions
{
    [SamplerData(typeof(DateTimeOffset), "Sets the user's request to speak.")]
    RequestToSpeakTimestamp,
    [SamplerData(typeof(ulong), "The ID of the channel the user is currently in.")]
    ChannelId,
    [SamplerData(typeof(bool), "Toggles the user's suppress state.")]
    Suppress
}

public enum ApplicationCommandProperties
{
    [SamplerData(typeof(bool), "Indicates whether the command is enabled by default when the app is added to a guild.")]
    DefaultPermission,
    [SamplerData(typeof(IEnumerable<InteractionContextType>), "Interaction context(s) where the command can be used.")]
    Contexts,
    [SamplerData(typeof(bool), "Indicates whether the command is age-restricted.")]
    Nsfw,
    [SamplerData(typeof(string), "Name of the command (1-32 characters).")]
    Name,
    [SamplerData(typeof(IEnumerable<ApplicationIntegrationType>), "Installation context(s) where the command is available.")]
    IntegrationTypes,
    [SamplerData(typeof(Permissions), "Default required permissions to use the command.")]
    DefaultGuildUserPermissions,
    [SamplerData(typeof(bool), "Indicates whether the command is available in DMs with the app.")]
    DMPermission,
    [SamplerData(typeof(ApplicationCommandType), "Type of the command.")]
    Type,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Name` (1-32 characters each).")]
    NameLocalizations
}

public enum GuildOnboardingPromptOptionProperties
{
    [SamplerData(typeof(string), "Description of the option.")]
    Description,
    [SamplerData(typeof(IEnumerable<ulong>), "IDs for roles assigned to a member when the option is selected.")]
    RoleIds,
    [SamplerData(typeof(ulong), "ID of the option.")]
    Id,
    [SamplerData(typeof(string), "Title of the option.")]
    Title,
    [SamplerData(typeof(string), "Emoji name of the option.")]
    EmojiName,
    [SamplerData(typeof(ulong), "Emoji ID of the option.")]
    EmojiId,
    [SamplerData(typeof(bool), "Whether the emoji is animated.")]
    EmojiAnimated,
    [SamplerData(typeof(IEnumerable<ulong>), "IDs for channels a member is added to when the option is selected.")]
    ChannelIds
}

public enum AuthorizationInformation
{
    [SamplerData(typeof(User), "The user who has authorized, if the user has authorized with the 'identify' scope.")]
    User,
    [SamplerData(typeof(DateTimeOffset), "When the access token expires.")]
    ExpiresAt,
    [SamplerData(typeof(Application), "The current application.")]
    Application,
    [SamplerData(typeof(IReadOnlyList<string>), "The scopes the user has authorized the application for.")]
    Scopes
}

public enum EmbedProperties
{
    [SamplerData(typeof(string), "Description of the embed.")]
    Description,
    [SamplerData(typeof(string), "Url of the embed.")]
    Url,
    [SamplerData(typeof(string), "Title of the embed.")]
    Title,
    [SamplerData(typeof(DateTimeOffset), "Timestamp of the embed.")]
    Timestamp,
    [SamplerData(typeof(EmbedFooterProperties), "Footer of the embed.")]
    Footer,
    [SamplerData(typeof(EmbedImageProperties), "Image of the embed.")]
    Image,
    [SamplerData(typeof(Color), "Color of the embed.")]
    Color,
    [SamplerData(typeof(IEnumerable<EmbedFieldProperties>), "Fields of the embed.")]
    Fields,
    [SamplerData(typeof(EmbedThumbnailProperties), "Thumbnail of the embed.")]
    Thumbnail,
    [SamplerData(typeof(EmbedAuthorProperties), "Author of the embed.")]
    Author
}

public enum ApplicationCommandOptions
{
    [SamplerData(typeof(IEnumerable<ApplicationCommandOptionProperties>), "Parameters for the command (max 25).")]
    Options,
    [SamplerData(typeof(string), "Description of the command (1-100 characters).")]
    Description,
    [SamplerData(typeof(bool), "Indicates whether the command is enabled by default when the app is added to a guild.")]
    DefaultPermission,
    [SamplerData(typeof(IEnumerable<InteractionContextType>), "Interaction context(s) where the command can be used.")]
    Contexts,
    [SamplerData(typeof(bool), "Indicates whether the command is age-restricted.")]
    Nsfw,
    [SamplerData(typeof(string), "Name of the command (1-32 characters).")]
    Name,
    [SamplerData(typeof(IEnumerable<ApplicationIntegrationType>), "Installation context(s) where the command is available.")]
    IntegrationTypes,
    [SamplerData(typeof(Permissions), "Default required permissions to use the command.")]
    DefaultGuildUserPermissions,
    [SamplerData(typeof(bool), "Indicates whether the command is available in DMs with the app.")]
    DMPermission,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Description` (1-100 characters each).")]
    DescriptionLocalizations,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Name` (1-32 characters each).")]
    NameLocalizations
}

public enum GuildOnboardingPromptProperties
{
    [SamplerData(typeof(IEnumerable<GuildOnboardingPromptOptionProperties>), "Options available within the prompt.")]
    Options,
    [SamplerData(typeof(ulong), "ID of the prompt.")]
    Id,
    [SamplerData(typeof(string), "Title of the prompt.")]
    Title,
    [SamplerData(typeof(bool), "Indicates whether the prompt is present in the onboarding flow. If false, the prompt will only appear in the Channels & Roles tab.")]
    InOnboarding,
    [SamplerData(typeof(bool), "Indicates whether users are limited to selecting one option for the prompt.")]
    SingleSelect,
    [SamplerData(typeof(bool), "Indicates whether the prompt is required before a user completes the onboarding flow.")]
    Required,
    [SamplerData(typeof(GuildOnboardingPromptType), "Type of the prompt.")]
    Type
}

public enum ChannelPositionProperties
{
    [SamplerData(typeof(bool), "Syncs the permission overwrites with the new parent, if moving to a new category.")]
    LockPermissions,
    [SamplerData(typeof(ulong), "Channel ID.")]
    Id,
    [SamplerData(typeof(ulong), "The new parent ID for the channel that is moved.")]
    ParentId,
    [SamplerData(typeof(int), "Sorting position of the channel.")]
    Position
}

public enum GuildOnboardingPrompt
{
    [SamplerData(typeof(IReadOnlyList<GuildOnboardingPromptOption>), "Options available within the prompt.")]
    Options,
    [SamplerData(typeof(string), "Title of the prompt.")]
    Title,
    [SamplerData(typeof(bool), "Indicates whether the prompt is present in the onboarding flow. If false, the prompt will only appear in the Channels & Roles tab.")]
    InOnboarding,
    [SamplerData(typeof(bool), "Indicates whether users are limited to selecting one option for the prompt.")]
    SingleSelect,
    [SamplerData(typeof(bool), "Indicates whether the prompt is required before a user completes the onboarding flow.")]
    Required,
    [SamplerData(typeof(GuildOnboardingPromptType), "Type of prompt.")]
    Type
}

public enum GuildStickerProperties
{
    [SamplerData(typeof(StickerFormat), "Sticker format.")]
    Format,
    [SamplerData(typeof(IEnumerable<string>), "Sticker autocomplete/suggestion tags (max 200 characters long when comma joined).")]
    Tags,
    [SamplerData(typeof(AttachmentProperties), "Sticker attachment.")]
    Attachment
}

public enum RestAuditLogEntryData
{
    [SamplerData(typeof(IReadOnlyDictionary<ulong, Webhook>), "List of webhooks referenced in the audit log.")]
    Webhooks,
    [SamplerData(typeof(IReadOnlyDictionary<ulong, Integration>), "List of integration objects.")]
    Integrations,
    [SamplerData(typeof(IReadOnlyDictionary<ulong, AutoModerationRule>), "List of auto moderation rules referenced in the audit log.")]
    AutoModerationRules,
    [SamplerData(typeof(IReadOnlyDictionary<ulong, GuildScheduledEvent>), "List of guild scheduled events referenced in the audit log.")]
    GuildScheduledEvents,
    [SamplerData(typeof(IReadOnlyDictionary<ulong, GuildThread>), "List of threads referenced in the audit log")]
    Threads,
    [SamplerData(typeof(IReadOnlyDictionary<ulong, ApplicationCommand>), "List of application commands referenced in the audit log.")]
    ApplicationCommands,
    [SamplerData(typeof(IReadOnlyDictionary<ulong, User>), "List of users referenced in the audit log.")]
    Users
}

public enum ApplicationCommandOptionChoiceProperties
{
    [SamplerData(typeof(ApplicationCommandOptionChoiceValueType), "Type of value.")]
    ValueType,
    [SamplerData(typeof(double), "Numeric value for the choice (max 100 characters).")]
    NumericValue,
    [SamplerData(typeof(string), "String value for the choice (max 100 characters).")]
    StringValue,
    [SamplerData(typeof(string), "Name of the choice (1-100 characters).")]
    Name,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Name` (1-100 characters each).")]
    NameLocalizations
}

public enum ApplicationCommandGuildPermissions
{
    [SamplerData(typeof(ulong), "ID of the command.")]
    CommandId,
    [SamplerData(typeof(IReadOnlyDictionary<ulong, ApplicationCommandPermission>), "Permissions for the command in the guild (max 100).")]
    Permissions,
    [SamplerData(typeof(ulong), "ID of the application the command belongs to.")]
    ApplicationId,
    [SamplerData(typeof(ulong), "ID of the guild.")]
    GuildId
}

public enum InteractionCallback
{
    [SamplerData(typeof(InteractionCallback), "ACK a ping interaction.")]
    Pong,
    [SamplerData(typeof(InteractionCallback), "For components, ACK an interaction and modify the original message later; the user does not see a loading state.")]
    DeferredModifyMessage
}

public enum GuildOnboarding
{
    [SamplerData(typeof(bool), "Whether onboarding is enabled in the guild.")]
    Enabled,
    [SamplerData(typeof(GuildOnboardingMode), "Current mode of onboarding.")]
    Mode,
    [SamplerData(typeof(IReadOnlyList<GuildOnboardingPrompt>), "Prompts shown during onboarding and in customize community.")]
    Prompts,
    [SamplerData(typeof(IReadOnlyList<ulong>), "Channel Ids that users get opted into automatically.")]
    DefaultChannelIds,
    [SamplerData(typeof(ulong), "ID of the guild this onboarding is part of.")]
    GuildId
}

public enum RestMessage
{
    [SamplerData(typeof(bool), "Whether the message was a Text-To-Speech message.")]
    IsTts,
    [SamplerData(typeof(InteractionResolvedData), "Data for `User`s, `GuildUser`s, `IGuildChannel`s, and `Role`s in the message's auto-populated select `Menu`s.")]
    ResolvedData,
    [SamplerData(typeof(IReadOnlyList<Embed>), "A list of `Embed` objects containing any embedded content present in the message.")]
    Embeds,
    [SamplerData(typeof(string), "Used for validating that a message was sent.")]
    Nonce,
    [SamplerData(typeof(RestMessage), "The message associated with the `MessageReference`.")]
    ReferencedMessage,
    [SamplerData(typeof(IReadOnlyList<MessageReaction>), "A list of `MessageReaction` objects containing all reactions to the message.")]
    Reactions,
    [SamplerData(typeof(MessageInteraction), "Sent if the message is a response to an `IInteraction`.")]
    Interaction,
    [SamplerData(typeof(ulong), "The ID of the message.")]
    Id,
    [SamplerData(typeof(IReadOnlyList<ulong>), "A list of IDs corresponding to roles specifically mentioned in the message.")]
    MentionedRoleIds,
    [SamplerData(typeof(IReadOnlyList<IComponent>), "A list of `IComponent` objects, contains components like `Button`s, `ActionRow`s, or other interactive components if any are present.")]
    Components,
    [SamplerData(typeof(bool), "Whether this message is pinned in a channel.")]
    IsPinned,
    [SamplerData(typeof(string), "The text contents of the message.")]
    Content,
    [SamplerData(typeof(IReadOnlyList<User>), "A list of `User` objects indexed by their IDs, containing users specifically mentioned in the message.")]
    MentionedUsers,
    [SamplerData(typeof(ulong), "If the message is an `IInteraction` response/followup or an application-owned `Webhook`, the ID of the `NetCord.Application`.")]
    ApplicationId,
    [SamplerData(typeof(RoleSubscriptionData), "Data of the role subscription purchase or renewal that prompted this `MessageType.RoleSubscriptionPurchase` message.")]
    RoleSubscriptionData,
    [SamplerData(typeof(DateTimeOffset), "When the message was edited (or null if never).")]
    EditedAt,
    [SamplerData(typeof(MessageInteractionMetadata), "Sent if the message is sent as a result of an `IInteraction`.")]
    InteractionMetadata,
    [SamplerData(typeof(ulong), "The ID of the channel the message was sent in.")]
    ChannelId,
    [SamplerData(typeof(MessageFlags), "A `MessageFlags` object indicating the message's applied flags.")]
    Flags,
    [SamplerData(typeof(MessageType), "The type of the message.")]
    Type,
    [SamplerData(typeof(IReadOnlyList<Attachment>), "A list of `Attachment` objects indexed by their IDs, containing any files attached in the message.")]
    Attachments,
    [SamplerData(typeof(MessageReference), "Contains data showing the source of a crosspost, channel follow add, pin, or message reply.")]
    MessageReference,
    [SamplerData(typeof(MessageActivity), "Sent with Rich Presence-related chat embeds.")]
    Activity,
    [SamplerData(typeof(Application), "Sent with Rich Presence-related chat embeds.")]
    Application,
    [SamplerData(typeof(User), "The `User` object of the message's author.")]
    Author,
    [SamplerData(typeof(int), "A generally increasing integer (there may be gaps or duplicates) that represents the approximate position of the message in a `GuildThread`.     It can be used to estimate the relative position of the message in a thread in tandem with the `GuildThread.TotalMessageSent` property of the parent thread.")]
    Position,
    [SamplerData(typeof(ulong), "If the message was generated by a `Webhook`, this is the webhook's ID.")]
    WebhookId,
    [SamplerData(typeof(IReadOnlyList<GuildChannelMention>), "A list of `GuildChannelMention` objects indexed by their IDs, containing channels specifically mentioned in the message.")]
    MentionedChannels,
    [SamplerData(typeof(IReadOnlyList<MessageSticker>), "Contains stickers contained in the message, if any.")]
    Stickers,
    [SamplerData(typeof(GuildThread), "The `GuildThread` that was started from this message.")]
    StartedThread,
    [SamplerData(typeof(IReadOnlyList<MessageSnapshot>), "A list of messages associated with the message reference.")]
    MessageSnapshots,
    [SamplerData(typeof(bool), "Whether the message mentions @everyone.")]
    MentionEveryone
}

public enum AttachmentProperties
{
    [SamplerData(typeof(string), "Description for the file (max 1024 characters for attachments sent by message, max 200 characters for attachments used for sticker creation).")]
    Description,
    [SamplerData(typeof(string), "Name of the upload.")]
    UploadedFileName,
    [SamplerData(typeof(string), "Title of the attachment.")]
    Title,
    [SamplerData(typeof(string), "Name of the file (max 1024 characters for attachments sent by message, 2-30 characters for attachments used for sticker creation).")]
    FileName
}

public enum ImageProperties
{
    [SamplerData(typeof(bool), "Whether `Data` is in Base64 format.")]
    IsBase64,
    [SamplerData(typeof(ReadOnlyMemory<byte>), "The data of the image.")]
    Data,
    [SamplerData(typeof(ImageFormat), "The format of the image.")]
    Format,
    [SamplerData(typeof(ImageProperties), "An empty `ImageProperties` instance.")]
    Empty
}

public enum ApplicationCommandOption
{
    [SamplerData(typeof(IReadOnlyList<ApplicationCommandOption>), "Parameters for the option (max 25).")]
    Options,
    [SamplerData(typeof(string), "Description of the option (1-100 characters).")]
    Description,
    [SamplerData(typeof(bool), "If autocomplete interactions are enabled for the option.")]
    Autocomplete,
    [SamplerData(typeof(string), "Name of the option (1-32 characters).")]
    Name,
    [SamplerData(typeof(double), "The minimum value permitted.")]
    MinValue,
    [SamplerData(typeof(IReadOnlyList<ChannelType>), "If the option is a channel type, the channels shown will be restricted to these types.")]
    ChannelTypes,
    [SamplerData(typeof(int), "The minimum allowed length (0-6000).")]
    MinLength,
    [SamplerData(typeof(bool), "If the parameter is required or optional.")]
    Required,
    [SamplerData(typeof(IReadOnlyList<ApplicationCommandOptionChoice>), "Choices for the user to pick from (max 25).")]
    Choices,
    [SamplerData(typeof(ApplicationCommandOptionType), "Type of the option.")]
    Type,
    [SamplerData(typeof(double), "The maximum value permitted.")]
    MaxValue,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Description` (1-100 characters each).")]
    DescriptionLocalizations,
    [SamplerData(typeof(int), "The maximum allowed length (0-6000).")]
    MaxLength,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Name` (1-32 characters each).")]
    NameLocalizations
}

public enum IPaginationProperties
{
    [SamplerData(typeof(string), "The starting point for pagination.")]
    From,
    [SamplerData(typeof(PaginationDirection), "The direction of pagination.")]
    Direction,
    [SamplerData(typeof(int), "The number of items to download at once.")]
    Limit
}

public enum EmbedThumbnailProperties
{
    [SamplerData(typeof(string), "Url of the thumbnail.")]
    Url
}

public enum EmbedFooterProperties
{
    [SamplerData(typeof(string), "Url of the footer icon.")]
    IconUrl,
    [SamplerData(typeof(string), "Text of the footer.")]
    Text
}

public enum Sku
{
    [SamplerData(typeof(string), "System-generated URL slug based on the SKU's name.")]
    Slug,
    [SamplerData(typeof(string), "Customer-facing name of your premium offering.")]
    Name,
    [SamplerData(typeof(ulong), "ID of the parent application.")]
    ApplicationId,
    [SamplerData(typeof(SkuFlags), "Flags of the SKU.")]
    Flags,
    [SamplerData(typeof(SkuType), "Type of the SKU.")]
    Type
}

public enum Connection
{
    [SamplerData(typeof(bool), "Whether friend sync is enabled for this connection.")]
    FriendSync,
    [SamplerData(typeof(IReadOnlyList<Integration>), "A list of server integrations.")]
    Integrations,
    [SamplerData(typeof(string), "The ID of the connection account.")]
    Id,
    [SamplerData(typeof(string), "The username of the connection account.")]
    Name,
    [SamplerData(typeof(ConnectionVisibility), "Visibility of this connection.")]
    Visibility,
    [SamplerData(typeof(bool), "Whether the connection is revoked.")]
    Revoked,
    [SamplerData(typeof(ConnectionType), "The service of this connection.")]
    Type,
    [SamplerData(typeof(bool), "Whether this connection has a corresponding third party OAuth2 token.")]
    TwoWayLink,
    [SamplerData(typeof(bool), "Whether the connection is verified.")]
    Verified,
    [SamplerData(typeof(bool), "Whether activities related to this connection will be shown in presence updates.")]
    ShowActivity
}

public enum ApplicationCommand
{
    [SamplerData(typeof(IReadOnlyList<ApplicationCommandOption>), "Parameters for the command (max 25).")]
    Options,
    [SamplerData(typeof(string), "Description of the command (1-100 characters).")]
    Description,
    [SamplerData(typeof(bool), "Indicates whether the command is enabled by default when the app is added to a guild.")]
    DefaultPermission,
    [SamplerData(typeof(IReadOnlyList<InteractionContextType>), "Interaction context(s) where the command can be used, only for globally-scoped commands.")]
    Contexts,
    [SamplerData(typeof(ulong), "Autoincrementing version identifier updated during substantial record changes.")]
    Version,
    [SamplerData(typeof(bool), "Indicates whether the command is age-restricted.")]
    Nsfw,
    [SamplerData(typeof(string), "Name of the command (1-32 characters).")]
    Name,
    [SamplerData(typeof(IReadOnlyList<ApplicationIntegrationType>), "Installation context(s) where the command is available, only for globally-scoped commands.")]
    IntegrationTypes,
    [SamplerData(typeof(Permissions), "Default required permissions to use the command.")]
    DefaultGuildUserPermissions,
    [SamplerData(typeof(ulong), "ID of the parent application.")]
    ApplicationId,
    [SamplerData(typeof(bool), "Indicates whether the command is available in DMs with the app.")]
    DMPermission,
    [SamplerData(typeof(ApplicationCommandType), "Type of the command.")]
    Type,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Description` (1-100 characters each).")]
    DescriptionLocalizations,
    [SamplerData(typeof(IReadOnlyDictionary<string, string>), "Localizations of `Name` (1-32 characters each).")]
    NameLocalizations
}

public enum RestClient
{
    [SamplerData(typeof(IToken), "The token of the `RestClient`.")]
    Token
}

public enum AllowedMentionsProperties
{
    [SamplerData(typeof(bool), "Allows reply mention.")]
    ReplyMention,
    [SamplerData(typeof(IEnumerable<ulong>), "Allowed roles, `null` for all.")]
    AllowedRoles,
    [SamplerData(typeof(bool), "Allows `@everyone` and `@here`.")]
    Everyone,
    [SamplerData(typeof(IEnumerable<ulong>), "Allowed users, `null` for all.")]
    AllowedUsers
}

public enum EmbedImageProperties
{
    [SamplerData(typeof(string), "Url of the image.")]
    Url
}

public enum NonceProperties
{
    [SamplerData(typeof(bool), "If `true`, the nonce will be checked for uniqueness in the past few minutes. If another message was created by the same author with the same nonce, that message will be returned and no new message will be created.")]
    Unique
}

public enum RestObjects
{
    ApplicationCommandOptionChoice,
    GuildOnboardingOptions,
    CurrentGuildUserOptions,
    SlashCommandProperties,
    GuildOnboardingPromptOption,
    EmbedFieldProperties,
    GuildUserInfo,
    ApplicationCommandGuildPermissionProperties,
    DMChannelProperties,
    RestGuild,
    FollowAnnouncementGuildChannelProperties,
    ApplicationCommandOptionProperties,
    HttpInteractionValidator,
    EmbedAuthorProperties,
    RestAuditLogEntry,
    CurrentUserVoiceStateOptions,
    ApplicationCommandProperties,
    GuildOnboardingPromptOptionProperties,
    AuthorizationInformation,
    EmbedProperties,
    ApplicationCommandOptions,
    GuildOnboardingPromptProperties,
    ChannelPositionProperties,
    GuildOnboardingPrompt,
    GuildStickerProperties,
    RestAuditLogEntryData,
    ApplicationCommandOptionChoiceProperties,
    ApplicationCommandGuildPermissions,
    InteractionCallback,
    GuildOnboarding,
    RestMessage,
    AttachmentProperties,
    ImageProperties,
    ApplicationCommandOption,
    IPaginationProperties,
    EmbedThumbnailProperties,
    EmbedFooterProperties,
    Sku,
    Connection,
    ApplicationCommand,
    RestClient,
    AllowedMentionsProperties,
    EmbedImageProperties,
    NonceProperties
}
