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
      // data 格式为：{ userId: 'junwu', socketId: '0JqMRoLQv8q3HFw8AAAG' }
});
```

### 直传消息 - message/direct

```js
socket.emit('message/direct', {
    userId: userId,
    targetIds: ['weichi'],
    type: 'text',
    content: '你是猴子么？',
}, data => {
    console.log('message sent', data);
    // data 格式为：
    //{ 
    //    statusCode: 201,// 消息已经在服务端有记录，告知发送端已经新建了一条对应的消息
    //    body: { 
    //        id: 'z31o1GIlxn', // 消息全局唯一的 id
    //        sent: [ 'weichi' ] // 消息的接收者 id 数组
    //    }
    //}
});
```