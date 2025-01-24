# NetCordSampler
A Discord bot that generates and displays various example code snippets of NetCord's C# Discord library. The goal of this concept bot is to be an informational midpoint between the existing NetCord tutorials/guides and the NetCord api documentation pages. 

### Proof Of Concept Project
> Currently, this project is solely a proof of concept. The Initial focal point to start should cover examples that are the most essential (commonly used). Following that should be examples that cover the most frequently asked questions topics (based on NetCord's support channel on Discord).

### Features
> Features that only cover the proof of concept stage and is subject to change. Feature ideas should be added via Github issues in this repo.
- **Quick Builder**: One click sample generation using the standard example content. Includes all available combination options including informational code comments in the sample output.
- **Standard Builder**: Select which properties/objects (recursively) to be included in the sample generation using the standard example content.
- **Custom Builder**: Similar to the standard builder, but with the ability to customize the content (Discord modal for content entry). This functionality will be expanded on further as a larger "Build a starter project" feature; (custom builder) to be written to fit such specification.
- **Output Options**: Code block or file.
- Pictures alongside the general information of the various Discord components/etc that you can select for samples.
- Samples that cover both Classic Syntax and Fluent Syntax.

### NetCord.Rest (Category)
> ### **SubCategory:** Message
> - **Object:** *NetCord.Rest.MessageProperties*
> ### **Overview**
> This group/category covers the creation side of message objects that are built and sent to Discord. As of now, the "Read" side of message data will be part another group/category and interaction objects will be covered in the Interaction group/category.  
> ### **Note**
> The list below is currently incomplete for this initial concept, but will eventually be expanded on and revised.
```Bash
Embed (Object: EmbedProperties)
├─ Title
├─ Description
├─ Url
├─ Timestamp
├─ Color
├─ Image
├─ Thumbnail
├─ Footer (Object: EmbedFooterProperties)
│	├─ Text
│	└─ IconUrl
│
├─ Author (Object: EmbedFooterProperties Object)
│	├─ Name
│	├─ Url
│	└─ IconUrl
│
└─ Field (Object: EmbedFooterProperties Object)
    ├─ Name
    ├─ Value
    └─ Inline

Components
├─ ActionRow (Object: ActionRowProperties)
│	├─ Button (Object: ButtonProperties)
│	│	├─ CustomId
│	│	├─ Label
│	│	├─ Emoji
│	│	└─ Style
│	├─ LinkButton (Object: LinkButtonProperties)
│	│	├─ Url
│	│	├─ Label
│	│	└─ Emoji
│	│
│	└─ PremiumButton (Object: PremiumButtonProperties)
│		└─ SkuId
│
├─ Menus
│	├─ String (Object: StringMenuProperties)
│	│	├─ Default
│	│	├─ Emoji
│	│	└─ Description
│	│
│	├─ User (Object: UserMenuProperties)
│	│	├─ 
│	│	└─ 
│	│
│	├─ Role (Object: RoleMenuProperties)
│	│	├─ 
│	│	└─ 
│	│
│	├─ Channel (Object: ChannelMenuProperties)
│	│	├─ 
│	│	└─ 
│	│
│	├─ Mentionable (Object: MentionableMenuProperties)
│	│	├─ 
│	│	└─ 
│	│
│	└─ Modal (Object: ModalProperties)
│		└─ TextInput
│
└─ Attachments (Object: AttachmentProperties)
    ├─ Standard
    ├─ Base64Encoded
    ├─ QuotedPrintable
    └─ GoogleCloud
```

### Interactions Objects
> This group/category covers the usage topics of interactions that occur after the creation of the interaction object. The focal point aside from the sample itself, should be pictures of the Discord component being selected. Another possible option would be to render a picture of what the code would look like on Discord (to go with the generated code block); though possibly overkill.  
>   
> ***Note***: This part is currently a big TODO as it is not the focus of the first half of the concept. 
```Bash
Components
├─ Button
├─ 
├─ 
├─ 
└─ 

Commands
├─ Slash
└─ Text
```

### Future Coverage
> Incomplete and possibly inaccurate list of other future sample coverage. Expected to be heavily revised and possibly moved elsewhere (roadmap possibly).
```Bash
Events
└─ Message
    ├─ Created
    ├─ Updated
    ├─ Deleted
    ├─ BulkDeleted
    ├─ ReactionAdded
    ├─ ReactionRemoved
    ├─ ReactionRemovedAll
    └─ ReactionRemovedEmoji

Message
├─ Deferral
├─ Followup
├─ Modify
└─ Send

Voice
├─ Send
└─ Receiving

Webhook
├─ Information
├─ Client
├─ Sending
└─ Receiving
```
