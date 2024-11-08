# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

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

