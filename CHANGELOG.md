# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="0.1.0-alpha.3"></a>
## [0.1.0-alpha.3](https://www.github.com/tgiachi/Elderforge/releases/tag/v0.1.0-alpha.3) (2024-11-13)

### Features

* add new SimpleMaterial.mat with material properties for a basic material ([b248e2d](https://www.github.com/tgiachi/Elderforge/commit/b248e2ddcf17d716a2dd88d926897bacbcc5792b))
* add world generation and serialization services to enhance game world management ([a0670b5](https://www.github.com/tgiachi/Elderforge/commit/a0670b5f045e0ad31d896cd0bf133e1e65cbaed0))
* added pid to ignore ([09b3722](https://www.github.com/tgiachi/Elderforge/commit/09b3722780fcc980ab8bdedc6ffbbea8e7eebc92))
* login works! ([a281b0d](https://www.github.com/tgiachi/Elderforge/commit/a281b0dcca3f15de2476e0766869a8a6cca80029))
* **client:** Added unity project ([0b7abec](https://www.github.com/tgiachi/Elderforge/commit/0b7abecf8c7d75a2e80702373ef3d19bb0c07c0b))
* **LoginScene:** adjust anchored position in LoginScene to improve UI layout ([733fd0f](https://www.github.com/tgiachi/Elderforge/commit/733fd0fac16702c57ad6455d3de70c0db4abc832))
* **MapGenerationService.cs:** add support for event bus integration to listen for EngineStartedEvent and generate map on event trigger ([8646a04](https://www.github.com/tgiachi/Elderforge/commit/8646a044b07274ea38bd9a2e8bdb0196e07f6e47))
* **Vector3Int:** adjust operator method formatting for consistency ([04530cc](https://www.github.com/tgiachi/Elderforge/commit/04530cc25c64dfebff7563c9759ab9d0f87ba151))
* **workflows:** add .NET test workflow for CI to automate testing process on push to main branch ([2c3f705](https://www.github.com/tgiachi/Elderforge/commit/2c3f705f65d52ee998bf2b42c1a8a68c15a9857f))
* **WorldScene.unity:** add PlayerBody GameObject with CharacterController component and FPSKeyboardController script for player movement ([a377af9](https://www.github.com/tgiachi/Elderforge/commit/a377af95beaa0b290a50b131548cf3793d2f31e4))

### Bug Fixes

* **client:** removed unity project ([36d453b](https://www.github.com/tgiachi/Elderforge/commit/36d453b4bb423e6140d9b4dcf213aa523a0f90a1))

<a name="0.1.0-alpha.2"></a>
## [0.1.0-alpha.2](https://www.github.com/tgiachi/Elderforge/releases/tag/v0.1.0-alpha.2) (2024-11-12)

### Features

* **client:** add .gitignore and IDE configuration files for better project management ([d2fa945](https://www.github.com/tgiachi/Elderforge/commit/d2fa94581aec6bf98a28ff43a02f36889e1c9326))
* **client:** added default asset resources ([4d28b0e](https://www.github.com/tgiachi/Elderforge/commit/4d28b0ec0ac635a936f43c31f050234512019c9f))
* **client:** initialize Unity project with essential files and settings ([3a84779](https://www.github.com/tgiachi/Elderforge/commit/3a8477969048fcc6eae551ae2668c4b7a10f94f8))
* **ConnectToServerScript:** add connection status tracking and event pooling for connected state ([806271e](https://www.github.com/tgiachi/Elderforge/commit/806271ed4cddc3a104d2eeb660bc0049a726df1c))
* **ConnectToServerScript:** add script to connect to server with UI inputs for server and port ([601ad5c](https://www.github.com/tgiachi/Elderforge/commit/601ad5cc1605916743bc782dac8f6f373d09cc1a))
* **Elderforge.Client.Cmd:** add new project file for Elderforge.Client.Cmd to support command line interface functionality ([8f7a84e](https://www.github.com/tgiachi/Elderforge/commit/8f7a84ee40f5c12ab63f90d7a88c1f570d72cae6))
* **Elderforge.Entities:** add new project file for Elderforge.Entities with .NET 9.0 target framework and project reference to Elderforge.Core.Server ([a8a55d8](https://www.github.com/tgiachi/Elderforge/commit/a8a55d8487bfb55884aa4b7bd1953d01b938519b))
* **LoginScene:** add new login scene with background image and UI elements to enhance user experience ([52a31d9](https://www.github.com/tgiachi/Elderforge/commit/52a31d91943d037e4e44e0daf9b0af1b1a2d54a0))
* **LoginScene:** rename UI elements for clarity and consistency in naming conventions ([d0506bb](https://www.github.com/tgiachi/Elderforge/commit/d0506bbcb15ca636e6dd4b2b750fd812549ab954))
* **NetworkClient:** implement MessageReceived event to handle incoming messages more effectively ([edcf562](https://www.github.com/tgiachi/Elderforge/commit/edcf56211c1c1066f031514fd80a3de43ede7c71))
* **noise:** add implicit noise modules for various operations and types to enhance noise generation capabilities ([8d3c9c9](https://www.github.com/tgiachi/Elderforge/commit/8d3c9c95ba05fdfd45660b40b77b26f3a4cbce7a))
* **Program.cs:** add PongMessage registration to NetworkMessageFactory ([09ccdbd](https://www.github.com/tgiachi/Elderforge/commit/09ccdbd14023dd8cc660c7dbbcbe5f396c662a74))
* **SerializableExtension:** add extension methods for BlockEntity and ChunkEntity to support serialization ([1dd643b](https://www.github.com/tgiachi/Elderforge/commit/1dd643b28d22a85932e45ebb62a82bc8b4877105))
* **solution:** add Elderforge.Shared project to the solution for shared code management ([ca9940e](https://www.github.com/tgiachi/Elderforge/commit/ca9940e9d8a80599e723a2ece383b2fffd85abf4))
* **vscode:** add VSCode configuration files for better development experience ([6c99b3f](https://www.github.com/tgiachi/Elderforge/commit/6c99b3ff3b61c8dfe249bcc25820134a0da43866))

### Bug Fixes

* **ProjectVersion.txt:** update Unity Editor version to 2022.3.52f1 and revision hash ([b546d4b](https://www.github.com/tgiachi/Elderforge/commit/b546d4ba0914d5b6da51c0b347992dc6af0f7dba))

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

