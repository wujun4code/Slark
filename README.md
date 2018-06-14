# Slark - Game server engine written by C#.

Slark 是适配 LeanCloud Play 的游戏通讯协议的基于 C# 语言编写的服务端版本，尝试用 .NET Core 提供的基础组件来构建一个可以托管在云引擎的高性能的消息转发和消息广播的服务端。


## 已适配的功能

- [x] Play.CreateRoom
- [x] Play.RandomJoin


## 快速开始


### 服务端

拉取代码，用 Visual Studio（Visual Studio for Mac 也完全可以） 打开 src/Slark/Slarl.sln 项目，设置启动项目为：Slark.Server.ConsoleApp.NETCore

然后按 F5 直接启动，第一次启动会从 Nuget 下载相关依赖。

### 客户端
下载 Play SDK for Unity，按照文档初始化，额外需要设置服务器地址：

```cs
Play.SetRouteServer("http://localhost:5000/play/");
```


然后就可以按照文档里面的实例代码进行开发了。
