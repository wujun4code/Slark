# xRealtime

## 开发目的
开发一个自由好用的实时的服务端，当然首先是想模仿一下微信的服务端。

## 基础框架

服务端选取 socket.io 作为实时通信的框架，加上 rxjs 风格改造事件的回调模型而变成 rx 风格。

## 协议格式

### 打开连接 - connect

```js
  socket.on('connect', () => {
      console.log('connected.');
  });
  socket.on('connected', data => {
     // socketId 在后续登录的时候需要用到，服务端会用这个 socketId 与 userId 进行映射
     var sokectId = data.socketId;
     // sokectId 是一个随机的字符串。例如：0JqMRoLQv8q3HFw8AAAG
  });
```

### 登录 - logIn

```js
 socket.emit('logIn', {
     userId: 'junwu',
     socketId: socketId
   }, data => {
      console.log(data);
      // data 格式为：{ userId: 'junwu', socketId: '0JqMRoLQv8q3HFw8AAAG',sessionToken:'6e5003c1bcb61ada8483655292918095' }
});
```

注意：sessionToken 是之后所有操作必须上传的参数，他标识当前客户端所包含的 socket id 以及 user id，暂无过期时间。

### 直传消息 - message/direct

```js
socket.emit('message/direct', {
    sessionToken:'6e5003c1bcb61ada8483655292918095',// 登录操作返回的 sessionToken
    targetIds: ['weichi'],// 接收消息的目标 user id 数组
    type: 'text',// 消息类型为文本消息
    textContent: '你是猴子么？',//文本内容
}, data => {
    console.log('message sent', data);
    // data 格式为：
    //{ 
    //    statusCode: 201,// 消息已经被服务端收到，以 201 代表 created 告知给发送端
    //    body: { 
    //        id: 'z31o1GIlxn', // 消息全局唯一的 id
    //        sent: [ 'weichi' ] // 消息的接收者 id 数组
    //    }
    //}
});
```

### 对话消息 - message/conversation

注意：区别与直传消息，对话消息的接收者是对话，对话中的成员都会接收到这一条消息，前提是这些成员都已经通过调用[加入对话](#加入对话)加入了该对话

```js
socket.emit('message/conversation', {
    sessionToken:'6e5003c1bcb61ada8483655292918095',// 登录操作返回的 sessionToken
    conversationId: '123456',// 对话 id
    ignoreIds: ['xyz'],// 指定这条消息不被哪些用户收到，这些用户可能在对话里面但是这条消息想屏蔽掉这些用户
    type: 'text',
    content: '你是猴子么？',
}, data => {
    console.log('message sent', data);
    // data 格式为：
    //{ 
    //    statusCode: 201,// 消息已经被服务端收到，以 201 代表 created 告知给发送端
    //    body: { 
    //        id: 'z31o1GIlxn', // 消息全局唯一的 id
    //        sent:{ online:[], offline:["weichi"] } // 消息的发送结果，online 数组代表接收者在线，offline 数组代表接收者离线
    //    }
    //}
});
```

### 创建对话

#### ~~sokcet 协议~~ （暂未实现） 

```js
socket.emit('conversation/add', {
    sessionToken:'6e5003c1bcb61ada8483655292918095',// 登录操作返回的 sessionToken
    protected: ['name','members','admin'],// 对话的保护字段，这些字段只有 admin 的成员可以修改 默认是 name,members,admin 这三个字段
    admin: ['xyz','abc'],// 指定管理员 
    members: ['uyi','lkl'],// 普通成员
    name: '吃货小分队',
    atr1: 'value',// 其他自定义字段
}, data => {
    console.log('message sent', data);
    // data 格式为：
    //{ 
    //    statusCode: 201,// 消息已经被服务端收到，以 201 代表 created 告知给发送端
    //    body: { 
    //        id: 'z31o1GIlxn', // 消息全局唯一的 id
    //        sent: [ 'weichi' ] // 消息的接收者 id 数组
    //    }
    //}
});
```
#### http(s) 协议

```js
let u = undefined;
RxAVUser.logIn('junwu', 'leancloud').flatMap(user => {
    u = user;
    let newGroup = new RxAVObject(groupSrc.groupProperties.className);
    newGroup.set('name', 'mochaTest');
    return newGroup.save();
}).flatMap(groupObj => {
    let user_group = new RxAVObject(groupSrc.userGroupRelationProperties.className);
    user_group.set(groupSrc.userGroupRelationProperties.user, u);
    user_group.set(groupSrc.userGroupRelationProperties.group, groupObj);
    return user_group.save();
}).subscribe(joined => {
    console.log('joined', joined);
});

```
