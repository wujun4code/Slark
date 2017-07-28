'use strict';
var io = require('socket.io-client');

describe('message', () => {
    let userId = 'junwu';
    it('send', done => {
        let socket = io('http://localhost:3000', {
            path: '/wechat'
        });
        socket.connect();
        socket.on('connect', () => {
            console.log('connected.');
        });
        socket.on('connected', data => {
            console.log('received', data);
            let socketId = data.socketId;
            console.log('socketId', socketId);
            socket.emit('logIn', {
                userId: userId,
                socketId: socketId
            }, data => {
                console.log(data);
                let sessionToken = data.body.sessionToken;
                socket.emit('message/direct', {
                    sessionToken: sessionToken,
                    targetIds: ['weichi'],
                    type: 'text',
                    textContent: '你是猴子么？',
                }, data => {
                    console.log('message sent', JSON.stringify(data));

                    done();
                });
            });
        });
    });
    it('receive', done => {
        let socket = io('http://localhost:3000', {
            path: '/wechat'
        });
        socket.connect();
        socket.on('connect', () => {
            console.log('connected.');
        });
        socket.on('connected', data => {
            console.log('received', data);
            let socketId = data.socketId;
            console.log('socketId', socketId);
            socket.emit('logIn', {
                userId: 'weichi',
                socketId: socketId
            }, data => {
                console.log(data);
                socket.on('message', data => {
                    console.log('receive message', data);
                    done();
                });
            });
        });
    });
});