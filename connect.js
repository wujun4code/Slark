'use strict';
var rxjs = require('rxjs');
var RxAVPush = require('rx-lean-js-core').RxAVPush;
var Utils = require('./utils');
var xMessage = require('./message');

class xConnection {
    constructor(io) {
        this.io = io;
        this.userSet = new Map();
        this.socketSet = new Map();
    }

    start() {
        this.rxConnection = new rxjs.Observable(observer => {
            this.io.on('connection', socket => {
                console.log('a new client connected ', socket.id);
                observer.next(socket);
            });
        });
        this.rxConnection.subscribe(socket => {
            socket.emit('connected', {
                socketId: socket.id
            });

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

            let rxLogIn = new rxjs.Observable(observer => {
                socket.on('logIn', (data, ack) => {
                    if (!Object.prototype.hasOwnProperty.call(data, 'userId')) {
                        return;
                    }
                    if (!Object.prototype.hasOwnProperty.call(data, 'socketId')) {
                        return;
                    }
                    let userId = data.userId;
                    let socketId = data.socketId;
                    this.userSet.set(userId, socketId);
                    observer.next({
                        data: data,
                        ack: ack
                    });
                });
            });

            rxLogIn.subscribe(logInData => {
                console.log('logInData', logInData);
                let data = logInData.data;
                let ack = logInData.ack;
                let userId = data.userId;
                let socketId = data.socketId;
                ack({
                    statusCode: 200,
                    body: {
                        userId: userId,
                        socketId: socketId,
                    }
                });
            });

            let rxDirectMessage = new rxjs.Observable(observer => {

                socket.on('message/direct', (data, ack) => {
                    if (!Object.prototype.hasOwnProperty.call(data, 'userId')) {
                        return;
                    }
                    // if (!Object.prototype.hasOwnProperty.call(data, 'socketId')) {
                    //     return;
                    // }
                    if (!Object.prototype.hasOwnProperty.call(data, 'targetIds')) {
                        return;
                    }

                    let message = new xMessage(data);

                    observer.next({
                        message: message,
                        ack: ack
                    });
                });
            });

            rxDirectMessage.subscribe(directData => {
                let message = directData.message;
                let data = message.data;
                let ack = directData.ack;

                let userId = data.userId;
                let targetIds = data.targetIds || [];
                targetIds.forEach(tid => {
                    let socket = this.find(tid);
                    if (socket != undefined) {
                        // send message on socket
                        let directData = message.directData();
                        socket.emit('message', directData);
                        console.log('socket', tid, directData);
                    } else {
                        // offline message to push
                        let pushData = message.pushData();
                        RxAVPush.sendTo(tid, pushData);
                        console.log('push', tid, pushData);
                    }
                });

                ack({
                    statusCode: 201,
                    body: {
                        id: message.id,
                        sent: targetIds,
                    }
                });
            });
        });

        return this.rxConnection;
    }

    stop() {

    }

    find(userId) {
        let socketId = this.userSet.get(userId);
        return this.io.sockets.connected[socketId];
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

module.exports = xConnection;
// module.exports = {
//     connection: xConnection
// }


