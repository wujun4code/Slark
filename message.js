'use strict';
var rxjs = require('rxjs');
var utils = require('./utils');
var rxLeanCloud = require('rx-lean-js-core');
var RxAVPush = rxLeanCloud.RxAVPush;

class xMessageService {

    constructor(connection, group) {
        this.connection = connection;
        if (group != undefined) {
            this.group = group;
        }
    }

    wrapMessage(sessionToken, data) {
        if (this.connection.tokens.has(sessionToken)) {
            data.from = this.connection.tokens.get(sessionToken).userId;
        }
        delete data.sessionToken;
        let message = new xMessage(data);
        return message;
    }

    startDirectMessage(socket) {

        let rxDirectMessage = new rxjs.Observable(observer => {
            socket.on('message/direct', (data, ack) => {
                console.log('message/direct', data);
                if (!Object.prototype.hasOwnProperty.call(data, 'sessionToken')) {
                    return;
                }
                // if (!Object.prototype.hasOwnProperty.call(data, 'socketId')) {
                //     return;
                // }
                if (!Object.prototype.hasOwnProperty.call(data, 'targetIds')) {
                    return;
                }
                let sessionToken = data.sessionToken;
                let message = this.wrapMessage(sessionToken, data);

                observer.next({
                    message: message,
                    ack: ack
                });
            });
        });

        return rxDirectMessage;
    }

    startGroupMessage(socket) {
        let rxGroupMessage = new rxjs.Observable(observer => {
            socket.on('message/group', (data, ack) => {
                console.log('message/group', data);
                if (!Object.prototype.hasOwnProperty.call(data, 'sessionToken')) {
                    return;
                }
                // if (!Object.prototype.hasOwnProperty.call(data, 'socketId')) {
                //     return;
                // }
                if (!Object.prototype.hasOwnProperty.call(data, 'groupId')) {
                    return;
                }
                let sessionToken = data.sessionToken;
                let message = this.wrapMessage(sessionToken, data);

                observer.next({
                    message: message,
                    ack: ack
                });
            });
        });
        return rxGroupMessage;
    }

    start(socket) {
        this.startDirectMessage(socket).subscribe(directData => {
            let message = directData.message;
            let data = message.data;
            let ack = directData.ack;

            let sessionToken = data.sessionToken;
            let targetIds = data.targetIds || [];

            let result = this.batchSend(targetIds, message);

            let directSent = result.d;
            let pushSent = result.p;

            ack({
                statusCode: 201,
                body: {
                    id: message.id,
                    sent: {
                        online: directSent,
                        offline: pushSent
                    },
                }
            });
        });

        if (this.group != undefined) {
            this.startGroupMessage(socket).subscribe(groupData => {
                let message = groupData.message;
                let data = message.data;
                let ack = groupData.ack;

                let groupId = message.groupId;

                this.group.query(groupId).subscribe(targetIds => {
                    let result = this.batchSend(targetIds, message);
                    let directSent = result.d;
                    let pushSent = result.p;

                    ack({
                        statusCode: 201,
                        body: {
                            id: message.id,
                            sent: {
                                online: directSent,
                                offline: pushSent
                            },
                        }
                    });
                });

            });
        }

    }
    batchSend(targetUserIds, message) {
        let directSent = [];
        let pushSent = [];
        targetUserIds.forEach(targetUserId => {
            let socket = this.connection.querySocket(targetUserId);
            if (socket != undefined) {
                // send message on socket
                let directData = message.directData();
                socket.emit('message', directData);
                directSent.push(targetUserId);
            } else {
                // offline message to push
                let pushData = message.pushData();
                RxAVPush.sendTo(targetUserId, pushData);
                pushSent.push(targetUserId);
            }
        });
        return {
            d: directSent,
            p: pushSent
        };
    }
}

class xMessage {
    constructor(data) {
        this.data = data;
        this.type = data.type;
        this.id = utils.newObjectId();
    }

    textMessage() {

    }

    pushData() {

        let title = '您有一条未读消息';
        let body = '';
        switch (this.type) {
            case 'text': {
                body = this.data.textContent;
                break;
            }

            case 'picture':
            case 'image': {
                break;
            }

            default: {
                break;
            }
        }

        let alert = {
            title: title,
            body: body
        };

        return {
            alert: alert,
            sound: 'default',
            id: this.id
        };
    }

    directData() {
        return {
            metaData: this.data,
            id: this.id
        };
    }

    send(from, to) {
    }

}

module.exports = xMessageService;