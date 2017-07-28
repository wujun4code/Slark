'use strict';
var rxjs = require('rxjs');
var utils = require('./utils');
var xMessageService = require('./message');

class xConnectionService {
    constructor(io) {
        this.io = io;
        this.tokens = new Map();
    }

    start(messageService) {
        let result = undefined;

        this.startConnection().subscribe(socket => {
            result = socket;
            // 告知客户端与服务端长连接创建成功
            socket.emit('connected', {
                socketId: socket.id
            });

            this.startDisconnecting(socket);

            this.startLogIn(socket).subscribe(logInData => {
                
                let data = logInData.data;
                let ack = logInData.ack;
                let userId = data.userId;
                let socketId = data.socketId;
                let sessionToken = utils.newToken();

                
                messageService.start(result);

                this.tokens.set(sessionToken, {
                    userId: userId,
                    socketId: socketId
                });

                ack({
                    statusCode: 200,
                    body: {
                        userId: userId,
                        socketId: socketId,
                        sessionToken: sessionToken
                    }
                });
            });
        })



        // this.rxConnection.subscribe(socket => {
        //     socket.emit('connected', {
        //         socketId: socket.id
        //     });

        //     let rxDisconnect = new rxjs.Observable(observer => {
        //         socket.on('disconnecting', reason => {
        //             // let rooms = Object.keys(socket.rooms);
        //             observer.next({
        //                 socketId: socket.id,
        //                 reason: reason
        //             });
        //         });
        //     });

        //     rxDisconnect.subscribe(disconnectData => {
        //         let socketId = disconnectData.socketId;
        //         console.log('disconnected', socketId);
        //     });
        // });

        // this.startLogIn();

        // this.rxLogIn.subscribe(logInData => {

        // });

    }

    startConnection() {
        let rxConnection = new rxjs.Observable(observer => {
            this.io.on('connection', socket => {
                console.log('a new client connected ', socket.id);
                observer.next(socket);
            });
        });
        return rxConnection;
    }

    startDisconnecting(socket) {
        let rxDisconnect = new rxjs.Observable(observer => {
            socket.on('disconnecting', reason => {
                // let rooms = Object.keys(socket.rooms);
                observer.next({
                    socketId: socket.id,
                    reason: reason
                });
            });
        });
        rxDisconnect.subscribe(disconnectData => {
            let socketId = disconnectData.socketId;
            console.log('disconnected', socketId);
        });
    }

    startDisconnected() {

    }

    startLogIn(socket) {
        let rxLogIn = new rxjs.Observable(observer => {
            socket.on('logIn', (data, ack) => {
                if (!Object.prototype.hasOwnProperty.call(data, 'userId')) {
                    return;
                }
                if (!Object.prototype.hasOwnProperty.call(data, 'socketId')) {
                    return;
                }
                observer.next({
                    data: data,
                    ack: ack,
                    socket: socket
                });
            });
        });
        return rxLogIn;
    }

    stop() {

    }

    querySocket(userId) {
        let result = undefined;
        this.tokens.forEach((value, key) => {
            
            if (Object.prototype.hasOwnProperty.call(value, 'userId')) {
                let vUserId = value.userId;
              
                if (vUserId == userId) {
                    let socketId = value.socketId;
                    
                    let socket = this.io.sockets.connected[socketId];
                    
                    result = socket;
                }
            }
        });
        return result;
    }

    join(appId, roomId, socketClient) {
        let nsp = io.of(`/${appId}`);
        let rxJoin = new rxjs.Observable(observer => {
            socketClient.join(roomId, () => {
                observer.next({
                    appId: appId,
                    roomId: roomId,
                    success: true
                });
            });
        });
        return rxJoin;
    }
}

module.exports = xConnectionService;
// module.exports = {
//     connection: xConnection
// }


