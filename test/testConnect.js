'use strict';
var io = require('socket.io-client');

describe('connect', () => {
    it('logIn', done => {
        let socket = io('http://xrealtime.leanapp.cn', {
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
                userId: 'junwu',
                socketId: socketId
            }, data => {
                console.log(data);
                done();
            });
        });
    });
});