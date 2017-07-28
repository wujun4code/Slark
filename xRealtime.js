'use strict';
var xConnectionService = require('./connection');
var xMessageService = require('./message');

class xRealtime {
    constructor(io) {
        this.connection = new xConnectionService(io);
        this.message = new xMessageService(this.connection);
    }

    start() {
        this.connection.start(this.message);
    }
}

module.exports = xRealtime;