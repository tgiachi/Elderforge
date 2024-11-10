# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="0.1.0-alpha.1"></a>
## [0.1.0-alpha.1](https://www.github.com/tgiachi/Elderforge/releases/tag/v0.1.0-alpha.1) (2024-11-10)

### Features

* **.gitignore:** add *.db to ignore database files ([a6f0f38](https://www.github.com/tgiachi/Elderforge/commit/a6f0f380843a9f3897690f2295ea5d60a07eea25))
* **ChatMessageType.cs:** remove Motd enum value as it is no longer needed ([147824a](https://www.github.com/tgiachi/Elderforge/commit/147824ad4099ebdbe84635f90a9bf8bdfd5d7157))
* **DbEntityTypeData:** add DbEntityTypeData record to store entity type information ([02329ba](https://www.github.com/tgiachi/Elderforge/commit/02329ba24229862aab96d0e27e13cf14185b8445))
* **diagnosticService.cs:** add printCounter and printInterval fields to control ([67b7355](https://www.github.com/tgiachi/Elderforge/commit/67b73556f8d849013e346bada5acb4619cdbbbc3))
* **DirectoriesConfig.cs:** add Root property to improve access to root directory path ([d74001e](https://www.github.com/tgiachi/Elderforge/commit/d74001e1e0753f450e6e02cec09db773af191b7d))
* **Elderforge:** add ElderforgeConfig class to store server configuration settings ([a332907](https://www.github.com/tgiachi/Elderforge/commit/a3329071bcfc49d5f912a2fb3ada553b338b194a))
* **Elderforge.Core.Server.csproj:** add BCrypt.Net-Next package reference for password hashing ([0674a4a](https://www.github.com/tgiachi/Elderforge/commit/0674a4ac75d0315f002ff33d81fbfd10e51f1653))
* **Elderforge.Core.Server.csproj:** add Humanizer.Core package reference to project dependencies ([2706146](https://www.github.com/tgiachi/Elderforge/commit/27061467929a02f3abf50d80a7c880f3929e5925))
* **GameActionResult.cs:** add static Success property to GameActionResult class ([0699cfe](https://www.github.com/tgiachi/Elderforge/commit/0699cfe09d3f505a631d7c21bcf2900fbd053d3b))
* **GameActionResult.cs:** create a new class GameActionResult to represent the result of a game action ([2cbffac](https://www.github.com/tgiachi/Elderforge/commit/2cbffacb5e596f2ed3de7ba434f45920d1a9a2f7))
* **IDatabaseService.cs:** rename FindAsync method to QueryAsync for better clarity ([7562736](https://www.github.com/tgiachi/Elderforge/commit/756273622b417d1d72d9a5486143f129eeb71d18))
* **IGameAction.cs:** add parameter 'elapsedMilliseconds' to ExecuteAsync method for more precise action execution timing ([fecf8de](https://www.github.com/tgiachi/Elderforge/commit/fecf8dec2df35a2c2b50287b0c281d6de6d84d04))
* **ISchedulerService.cs:** add CurrentTick property to ISchedulerService interface for tracking current tick in scheduler service ([92988c6](https://www.github.com/tgiachi/Elderforge/commit/92988c60e4e5186ac75d3e1d27feffe4042b3caa))
* **NetworkClient:** add IObservable support to subscribe to specific network messages for real-time message handling ([01a4808](https://www.github.com/tgiachi/Elderforge/commit/01a4808142d75c94c5b084db7ee1afa712a3720a))
* **NetworkClient:** add SendMessage method to send network messages using NetPacketProcessor and NetDataWriter ([f9e8088](https://www.github.com/tgiachi/Elderforge/commit/f9e80887279b690fd3a95783d3e78070c7602d1e))
* **NetworkClient.cs:** add NetPacketProcessor to handle incoming network packets and process them accordingly ([8e97630](https://www.github.com/tgiachi/Elderforge/commit/8e97630f2d52c954da6d0b06d833167edb4485eb))
* **Scheduler:** add SchedulerServiceConfig, IGameAction interface, ISchedulerService interface, SchedulerService, MoveAction class, and SchedulerServiceTests to implement a flexible and efficient game action scheduling system ([91cff46](https://www.github.com/tgiachi/Elderforge/commit/91cff4642c96c7dc62ad0f62b43d2f09730d3ada))
* **sln:** add Elderforge.Network.Client project to solution ([5ab44f6](https://www.github.com/tgiachi/Elderforge/commit/5ab44f622e205a5110460642a1e1e42adf90c0fa))
* **Taskfile.yml:** add new task 'publish-unity' to publish Unity client project ([d36ff93](https://www.github.com/tgiachi/Elderforge/commit/d36ff932da0aac9a19dfc14d24ebd7e988fc7b2e))

<a name="0.1.0-alpha.0"></a>
## [0.1.0-alpha.0](https://www.github.com/tgiachi/Elderforge/releases/tag/v0.1.0-alpha.0) (2024-11-08)

### Features

* **bootstrap.lua:** add bootstrap script to log info messages during app initialization ([8dff389](https://www.github.com/tgiachi/Elderforge/commit/8dff389647a744c7585ad0636ce4387e4aae6cbd))
* **EngineStartedEvent.cs:** implement IElderforgeEvent interface for EngineStartedEvent ([18ec357](https://www.github.com/tgiachi/Elderforge/commit/18ec357eca13dd8aac6d0be2e326d8f84349302e))
* **IVersionService.cs:** add IVersionService interface to define a method for getting the version string ([8672a9f](https://www.github.com/tgiachi/Elderforge/commit/8672a9f1b4d46f72e983ee72c7871c3cca586b2d))
* **Program.cs:** register new ScriptModules (ContextVariableModule, VariableServiceModule) to enhance script functionality ([cca0c5b](https://www.github.com/tgiachi/Elderforge/commit/cca0c5b470c9281e8514db422d1f3dc40d6f89fc))
* **server:** add MotdObject class to handle Message of the Day with lines array ([174692e](https://www.github.com/tgiachi/Elderforge/commit/174692e5ad9f69ecbab91e0748bc12c6407c6bfe))
* **Variables:** add VariableService class to handle variables and their builders ([31369bf](https://www.github.com/tgiachi/Elderforge/commit/31369bf5d483ccd2558d363f0f58db2d017a41f1))

<a name="0.0.1"></a>
## [0.0.1](https://www.github.com/tgiachi/Elderforge/releases/tag/v0.0.1) (2024-11-08)

### Features

* add .dockerignore file to ignore unnecessary files and directories in Docker build ([78b26c7](https://www.github.com/tgiachi/Elderforge/commit/78b26c748d668470850f3269c12e1c440d27e90a))
* add .editorconfig file to enforce consistent coding style and formatting across the project ([82aacf9](https://www.github.com/tgiachi/Elderforge/commit/82aacf90f8d823beb1e3f814cd9cb52c3a02febf))
* **chat:** implement chat messaging system with ChatMessage and ChatMessageType classes ([97f76ff](https://www.github.com/tgiachi/Elderforge/commit/97f76ff043c445bad7791e82f3dba9f884b099f5))
* **ChatService:** implement dependency injection for INetworkServer and IEventBusService to enhance service architecture ([e852ca9](https://www.github.com/tgiachi/Elderforge/commit/e852ca9d3f29f5a7f40ec0a59b7cf4b5d4708a6b))
* **ClientConnectedEvent:** add new event class ClientConnectedEvent to handle client connection events ([8f2bbd3](https://www.github.com/tgiachi/Elderforge/commit/8f2bbd3dbb7b65ec70e095a9ddfefb33c0bd641b))
* **DirectoryType.cs:** add new enum value 'Logs' to DirectoryType for better categorization ([8fbef1c](https://www.github.com/tgiachi/Elderforge/commit/8fbef1cc29b7aa84348c6f77094665f3cf9a6902))
* **Dockerfile:** add Dockerfile for building and running the Elderforge.Server application to streamline deployment and ensure consistent environment setup ([e2f95e7](https://www.github.com/tgiachi/Elderforge/commit/e2f95e742bd8f1fff52765d5c3de34c40c3d670b))
* **IMessageDispatcherService:** add GetOutgoingMessagesChannel method to retrieve the outgoing messages channel ([03c9837](https://www.github.com/tgiachi/Elderforge/commit/03c9837d714027ac7c0b6fcee1bafed47835bc69))
* **network:** add Serilog for logging and implement INetworkPacket and INetworkMessageFactory interfaces for better message handling ([0a15b90](https://www.github.com/tgiachi/Elderforge/commit/0a15b90951ba12c453155c1583812db0f54f7d7f))
* **network:** implement Protobuf encoding and decoding for network messages to enhance message serialization ([62ea2d9](https://www.github.com/tgiachi/Elderforge/commit/62ea2d98a06c9511d9fe6d00018aa51f52040fb8))
* **network:** implement session network message and packet classes to handle session-based communication ([4abc785](https://www.github.com/tgiachi/Elderforge/commit/4abc785a752dd19f42c1f0ea832b3f05359e928d))
* **Network:** add INetworkMessageEncoderDecoder interface for combined encoding and decoding functionality ([84052ec](https://www.github.com/tgiachi/Elderforge/commit/84052ec3716cb173637b48f5ce8f9a30ffb874e6))
* **Network:** add MessageChannelService to handle incoming and outgoing messages ([f0d527d](https://www.github.com/tgiachi/Elderforge/commit/f0d527de3a464079169960c0cb7799a6d4b9b0a3))
* **Network:** add MessageParserWriterService for parsing and writing messages ([628287e](https://www.github.com/tgiachi/Elderforge/commit/628287e8844ce696b88df8fc059333aa47ba4b72))
* **NetworkMethodExtension.cs:** add RegisterNetworkMessage method to register network messages with their types ([696248c](https://www.github.com/tgiachi/Elderforge/commit/696248c609eebe797b96b3d5be3f544196cb8e0c))
* **NetworkServer.cs:** add IsRunning property to track server running status ([34f17f2](https://www.github.com/tgiachi/Elderforge/commit/34f17f2f32eeb1ec09a6b89d5f9892181f9d7c0a))
* **NetworkServer.cs:** add support for handling outgoing messages through channels ([e51b696](https://www.github.com/tgiachi/Elderforge/commit/e51b69615509ac7b0cacc3e3a89c941a066df241))
* **NetworkServer.cs, INetworkServer.cs, NetworkServerTests.cs:** add support for registering message listeners in the NetworkServer class to handle incoming network messages efficiently. This allows for better customization and extensibility of message handling logic. ([77a87be](https://www.github.com/tgiachi/Elderforge/commit/77a87be76766b0a5c3b72ae60c77df496867d2ba))
* **NetworkServerConfig.cs:** add NetworkServerConfig class to store server configuration ([4d8530d](https://www.github.com/tgiachi/Elderforge/commit/4d8530df2eadd5d0a4856c02d10f3f8dc1e382fb))
* **PingMessage.cs:** add Timestamp initialization to current UTC time in the PingMessage constructor ([a981539](https://www.github.com/tgiachi/Elderforge/commit/a981539f9850a8075ce97f4c0b3eb03462ba67a7))
* **Program.cs:** register IChatService in the dependency injection container to enable chat functionality ([8b4ffdd](https://www.github.com/tgiachi/Elderforge/commit/8b4ffddef36d47f4b5c9f2eaac310467e77ae2c6))
* **RegisterTypedListMethodEx.cs:** add a new extension method to register typed lists in the service collection ([4be89b4](https://www.github.com/tgiachi/Elderforge/commit/4be89b4f0676df29ea4c78d27bec588ae09629ed))
* **server:** add new script function attribute to define custom script functions with alias and help text ([5c9e8dc](https://www.github.com/tgiachi/Elderforge/commit/5c9e8dc5b7ba49d433934476e9e5534f243350f2))
* **server:** add Serilog.Sinks.File package to enable logging to a file ([553fa6e](https://www.github.com/tgiachi/Elderforge/commit/553fa6e77d362758cb8f15e4d81d7de76a28bf50))
* **sln:** add Elderforge.Core project to the solution ([c75ec32](https://www.github.com/tgiachi/Elderforge/commit/c75ec32f5486133b33bc5e6f5cf1fa0133fb20b0))
* **sln:** add Elderforge.Core.Server project to solution ([5f886a6](https://www.github.com/tgiachi/Elderforge/commit/5f886a66b1a51eec56d1f163be14151e9d4ac08c))
* **sln:** add Elderforge.Tests project to the solution for unit testing ([9ce130c](https://www.github.com/tgiachi/Elderforge/commit/9ce130cdd0d66d97cccf3b13ed047d85e444a8a4))
* **Taskfile:** add docker_build task for building Docker image of the server ([924cbb8](https://www.github.com/tgiachi/Elderforge/commit/924cbb88d9ca66bda4ca0c9ae5c2cc6aaaba6604))
* **Taskfile.yml:** add 'publish' task to build and publish the project in Release mode ([49546d3](https://www.github.com/tgiachi/Elderforge/commit/49546d3e5b5bba2a34a1674a424748f147ba86ed))

### Bug Fixes

* **ElderforgeServerOptions.cs:** change root-directory option from required to optional to enhance flexibility ([58394aa](https://www.github.com/tgiachi/Elderforge/commit/58394aa581604a60fac04e28981479b97eb4300b))

