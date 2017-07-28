'use strict';
var rxjs = require('rxjs');
var utils = require('./utils');
var RxAVPush = require('rx-lean-js-core').RxAVPush;

class xMessageService {

    constructor(connection) {
        this.connection = connection;
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

    start(socket) {
        this.startDirectMessage(socket).subscribe(directData => {
            let message = directData.message;
            let data = message.data;
            let ack = directData.ack;

            let sessionToken = data.sessionToken;
            let targetIds = data.targetIds || [];
            let directSent = [];
            let pushSent = [];
            targetIds.forEach(tid => {

                let socket = this.connection.querySocket(tid);
                if (socket != undefined) {
                    // send message on socket
                    let directData = message.directData();
                    socket.emit('message', directData);
                    directSent.push(tid);
                } else {
                    // offline message to push
                    let pushData = message.pushData();
                    RxAVPush.sendTo(tid, pushData);
                    pushSent.push(tid);
                }
            });
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